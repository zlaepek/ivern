using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private CircleCollider2D _mandooCollider;

    // Frozen Value
    public float maxFrozenValue = 30f;
    public float currentFrozenValue;

    // UI
    private BossUI _bossUI;


    private int _mandooBodyDashCount = 0;

    [SerializeField] private MandooTheBossHead _madMandooHead = null;


    #region MonsterMove 관련 변수 선언부
    // Monster Move 선언
    private MonsterMove _monsterMove;

    // 기본 이속
    public float randomMoveDuration = 1.0f;

    // 돌진 관련
    public float dashSpeed = 10.0f;     // 돌진 속도
    public float dashInterval = 5.0f;   // 돌진 간격
    public float dashDuration = 0.2f;     // 돌진 지속 시간

    // 이동 타겟
    public Transform targetTransform;

    // 타이머
    private float _currentTime = 0;

    public GameObject targetPositionMarker = null;
    #endregion MonsterMove 관련 변수 선언부


    public MandooAnimation mandooAnimation = null;

    [SerializeField] private MandooEffectAreaController _effectAreaController;
}
