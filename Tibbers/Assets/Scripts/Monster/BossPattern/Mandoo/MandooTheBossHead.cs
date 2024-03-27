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

    // �Ӹ��� �����ӵ��� �����̱� ������ 360���� �Ѿ��� (�����ӵ���) �߻��ϴ°� ������� ����
    private IEnumerator MandooHeadShotBullet(float delay, int number = 1)
    {
        Vector3 originDirection = _mandooOrigin.targetTransform.position - transform.position;
        float startAngle = Mathf.Atan2(originDirection.y, originDirection.x) * Mathf.Rad2Deg - (number - 1) * 10.0f; // ���� ���� ����

        for (int i = 0; i < number; i++)
        {
            // �� �Ѿ˿� ���� ���� ���
            float angle = startAngle + (30.0f * i); // s�� �Ѿ� ���� ���� �����Դϴ�. �� ���� �����Ͽ� �Ѿ� ���� ������ ������ �� �ֽ��ϴ�.
            Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);

            // �Ѿ� �߻�
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
