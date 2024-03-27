using System.Collections;
using UnityEngine;

public class MandooTheBossHead : MonoBehaviour
{
    [SerializeField] private MandooTheBoss _mandooOrigin;

    private Coroutine _madMandooHeadCoroutine = null;


    public void ResetCoroutine()
    {
        if (_madMandooHeadCoroutine != null)
        {
            StopCoroutine(_madMandooHeadCoroutine);
        }
    }

    public void MadInit()
    {
        _madMandooHeadCoroutine = StartCoroutine(HeadInitialMove());
    }

    // head idle
    public void HeadMove()
    {
        // _monsterMove.FollowTarget(_unit.fCurMoveSpeed, transform, _mandooOrigin.targetTransform.position);
    }

    // 머리는 느린속도로 움직이기 때문에 360도로 총알을 (느린속도로) 발사하는게 재밋을것 같음
    private IEnumerator MandooHeadShotBullet(float delay, int number = 1)
    {
        Vector3 originDirection = _mandooOrigin.targetTransform.position - transform.position;
        float startAngle = Mathf.Atan2(originDirection.y, originDirection.x) * Mathf.Rad2Deg - (number - 1) * 10.0f; // 시작 각도 조정

        for (int i = 0; i < number; i++)
        {
            // 각 총알에 대한 방향 계산
            float angle = startAngle + (30.0f * i); // s각 총알 간의 각도 차이입니다. 이 값을 조정하여 총알 간의 간격을 변경할 수 있습니다.
            Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);

            // 총알 발사
            BossBullet bullet = BossManager.Instance.ShotBullet(BossBulletType.mandooBullet, direction, transform).GetComponent<BossBullet>();
            bullet.InitialMoveInfo(direction, 8.0f);

        }
        yield return new WaitForSeconds(delay);
    }

    private IEnumerator HeadInitialMove()
    {
        Vector3 direction = (_mandooOrigin.targetTransform.position - transform.position).normalized;
        var speed = 0.5f;
        GetComponent<Rigidbody2D>().velocity = direction * speed;
        yield break;
    }

    #region Trigger Enter
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("tag_Player"))
        {
            collision.GetComponent<Unit>().GetDamage(_mandooOrigin.Unit.m_stStat.fDamage_Base);
        }
    }
    #endregion
}
