using UnityEngine;

namespace Boss
{
    public partial class MandooTheBoss : MonoBehaviour
    {
        private enum MANDOO_STATE
        {
            FROZEN,
            NORMAL,
            MAD,
            DEAD
        }

        [SerializeField] private MANDOO_STATE _currentMandooState = MANDOO_STATE.FROZEN;

        // Coroutine
        private Coroutine _currentMoveCoroutine = null;
        private Coroutine _currentAttactCoroutine = null;

        // Unit
        private Unit _unit;
        public Unit Unit
        {
            get { return _unit; }
            set { _unit = value; }
        }

        [SerializeField]  private CircleCollider2D _mandooCollider;

        // Frozen Value
        public float maxFrozenValue = 30f;
        public float currentFrozenValue;

        // UI
        private BossUI _bossUI;


        private int _mandooBodyDashCount = 0;

        [SerializeField] private MandooTheBossHead _madMandooHead = null;


        #region MonsterMove ���� ���� �����
        // Monster Move ����
        private MonsterMove _monsterMove;

        // �⺻ �̼�
        public float randomMoveDuration = 1.0f;

        // ���� ����
        public float dashSpeed = 10.0f;     // ���� �ӵ�
        public float dashInterval = 5.0f;   // ���� ����
        public float dashDuration = 0.2f;     // ���� ���� �ð�

        // �̵� Ÿ��
        public Transform targetTransform;

        // Ÿ�̸�
        private float _currentTime = 0;

        public GameObject targetPositionMarker = null;
        #endregion MonsterMove ���� ���� �����


        public MandooAnimation mandooAnimation = null;

        [SerializeField] private MandooEffectAreaController _effectAreaController;
    }

}
