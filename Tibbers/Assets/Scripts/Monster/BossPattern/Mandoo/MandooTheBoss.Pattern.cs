using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    public partial class MandooTheBoss : MonoBehaviour
    {
        private void ResetCoroutine()
        {
            if (_currentMoveCoroutine != null)
            {
                StopCoroutine(_currentMoveCoroutine);
            }
            if (_currentAttactCoroutine != null)
            {
                StopCoroutine(_currentAttactCoroutine);
            }
            _madMandooHead?.ResetCoroutine();
        }


        // Frozen
        public void GetMelt(float value)
        {
            currentFrozenValue -= value;
            _bossUI.bossFrozenSlider.value = currentFrozenValue;
        }

        /// Mad
        // random body
        private void RandomBodyDash()
        {
            Vector3 targetDirection = _monsterMove.SetRandomDirection();

            if (_currentMoveCoroutine != null)
            {
                StopCoroutine(_currentMoveCoroutine);
            }
            _currentMoveCoroutine = StartCoroutine(_monsterMove.DashToTarget(transform, targetDirection, dashSpeed, dashDuration));

            _currentTime = 0;
            _mandooBodyDashCount = 0;
        }

        private void ShortBodyJumpToHead()
        {
            if (_currentMoveCoroutine != null)
            {
                StopCoroutine(_currentMoveCoroutine);
            }

            _currentMoveCoroutine = StartCoroutine(_monsterMove.JumpToTarget(_mandooCollider, targetTransform, transform, dashSpeed, dashDuration));
        }

        private IEnumerator MandooShotBullet(float delay, int number = 1)
        {
            mandooAnimation.StartThrow();

            Vector3 originDirection = targetTransform.position - transform.position;
            float startAngle = Mathf.Atan2(originDirection.y, originDirection.x) * Mathf.Rad2Deg - (number - 1) * 10.0f; // 시작 각도 조정

            for (int i = 0; i < number; i++)
            {
                // 각 총알에 대한 방향 계산
                float angle = startAngle + (20.0f * i); // 20.0f는 각 총알 간의 각도 차이입니다. 이 값을 조정하여 총알 간의 간격을 변경할 수 있습니다.
                Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);

                // 총알 발사
                BossBullet bullet = BossManager.Instance.ShotBullet(BossBulletType.mandooBullet, direction, transform).GetComponent<BossBullet>();
                bullet.InitialMoveInfo(direction, 8.0f);

            }
            yield return new WaitForSeconds(delay);
            mandooAnimation.EndThrow();
        }
    }
}