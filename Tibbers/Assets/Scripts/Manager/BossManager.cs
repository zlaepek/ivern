using UnityEngine;

public enum BossName
{
    Mandoo,
}

public class BossManager : MonoBehaviour
{
    #region Instance
    private static BossManager instance = null;

    public static BossManager Instance {
        get {
            if (instance == null) {
                //게임 인스턴스가 없다면 하나 생성해서 넣어준다.
                instance = new BossManager();
            }
            return instance;
        }
    }
    #endregion

    // UI
    public BossUI bossUI = null;

    // 보스
    public BossName currentBossName = BossName.Mandoo;

    public GameObject currentBoss = null;
    public GameObject mandooTheBossPrefab = null;
    
    // 
    /// 플레이어 이동 제한 테두리 생성

    

    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn() {
        bossUI.SpawnNewBoss(currentBossName);

        BossSpawnByName();
    }

    public void BossSpawnByName() {
        switch (currentBossName) {

            case BossName.Mandoo:
                currentBoss = Instantiate(mandooTheBossPrefab);
                break;

            default:

                break;
        }
    }
}

