using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    public partial class MandooTheBoss : MonoBehaviour
    {
        #region ������ ����Ŭ
        private void InitializeUnitValues()
        {
            _unit = GetComponent<Unit>();

            _unit.m_stStat.fDamage_Base = 10.0f;

            _unit.m_stStat.fHp_Base = 100.0f;
            _bossUI.SetMaxHP(_unit.m_stStat.fHp_Base);

            _unit.m_stStat.fMoveSpeed_Base = 0.2f;

            _unit.ResetHp();
        }

        private void Start()
        {
            // Get from BossManager
            targetTransform = BossManager.Instance.PlayerTransform;
            _bossUI = BossManager.Instance.BossUI;

            InitializeUnitValues();

            // Move Init
            _monsterMove = new MonsterMove();
            _monsterMove.Initialize(targetPositionMarker);

            _effectAreaController = transform.parent.GetComponentInChildren<MandooEffectAreaController>();

            FrozenInit();
        }

        private void FixedUpdate()
        {
            _currentTime += Time.fixedDeltaTime;
            _bossUI.UpdateHPSlider(_unit.m_stStat.fHp_Cur);
            switch (_currentMandooState)
            {
                case MANDOO_STATE.FROZEN:
                    FrozenPattern();
                    break;
                case MANDOO_STATE.NORMAL:
                    NormalPattern();
                    break;
                case MANDOO_STATE.MAD:
                    MadPattern();
                    break;
                case MANDOO_STATE.DEAD:
                    Dead();
                    break;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("tag_Player"))
            {
                collision.GetComponent<Unit>().GetDamage(_unit.m_stStat.fDamage_Base);
            }
        }
        #endregion ������ ����Ŭ

        /// Frozen
        private void FrozenInit()
        {
            // ����
            gameObject.tag = "tag_Enemy_Invincible";
            // Value
            currentFrozenValue = maxFrozenValue;
            _bossUI.InitFrozenSlider(0f, maxFrozenValue);

            _currentMandooState = MANDOO_STATE.FROZEN;

            _effectAreaController.SpawnFireArea(targetTransform, transform.parent);

            mandooAnimation.FrozenAnimation(true);
        }

        private void FrozenPattern()
        {
            if (_currentTime > dashInterval)
            {
                Vector3 targetDirection = _monsterMove.SetDashPosition(transform, targetTransform);

                if (_currentMoveCoroutine != null)
                {
                    StopCoroutine(_currentMoveCoroutine);
                }
                _currentMoveCoroutine = StartCoroutine(_monsterMove.DashToTarget(transform, targetDirection, dashSpeed, dashDuration));

                if (_currentAttactCoroutine != null)
                {
                    StopCoroutine(_currentAttactCoroutine);
                }
                _currentAttactCoroutine = StartCoroutine(_effectAreaController.SpawnIceArea(transform, dashSpeed, dashDuration));

                _currentTime = 0;
            }

            if (currentFrozenValue <= 0f)
            {
                FrozenEnd();
                NormalInit();
            }
        }

        private void FrozenEnd()
        {
            // ���� ����
            gameObject.tag = "tag_Enemy";

            ResetCoroutine();

            // �ִϸ��̼� ����
            mandooAnimation.FrozenAnimation(false);

            // ������ �����
            _effectAreaController.DestroyFireArea();
        }



        /// Mad
        // init
        private void MadInit()
        {
            _currentMandooState = MANDOO_STATE.MAD;

            mandooAnimation.StartBodyThrow();
            _madMandooHead.MadInit();

            StartCoroutine(mandooAnimation.StartMad(0.75f));
        }

        private void MadPattern()
        {
            // body
            if (_currentTime > dashInterval)
            {
                if (_mandooBodyDashCount > 3)
                {
                    RandomBodyDash();
                }
                else
                {
                    ShortBodyJumpToHead();
                }
            }

            // ���� �Ӹ��� ��� �����ϸ鼭 ����ٴ�
            _madMandooHead.HeadMove();

            // ���
            if (_unit.m_stStat.fHp_Cur == 0)
            {
                MadEnd();
                Dead();
            }

        }

        private void MadEnd()
        {
            ResetCoroutine();

            Destroy(_madMandooHead);
        }



        /// Normal
        private void NormalInit()
        {
            _currentMandooState = MANDOO_STATE.NORMAL;
        }

        private void NormalPattern()
        {
            _monsterMove.FollowTarget(_unit.fCurMoveSpeed, transform, targetTransform);

            _currentTime += Time.deltaTime;


            // źȯ
            if (_currentTime >= 3.0f)
            {
                _currentAttactCoroutine = StartCoroutine(MandooShotBullet(1f, 3));
                _currentTime = 0f;
            }

            // ���� ����
            // if (_unit.m_stStat.fHp_Cur <= 30)
            //if (_unit.m_stStat.fHp_Cur <= 80)
            //{
            //    NormalEnd();
            //    MadInit();
            //}

            // ���
            if (_unit.m_stStat.fHp_Cur == 0)
            {
                MadEnd();
                Dead();
            }
        }

        private void NormalEnd()
        {
            ResetCoroutine();
        }



        /// Dead
        private void Dead()
        {
            _currentMandooState = MANDOO_STATE.DEAD;
            BossManager.Instance.BossClear();
        }
    }
}