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
                //���� �ν��Ͻ��� ���ٸ� �ϳ� �����ؼ� �־��ش�.
                instance = new BossManager();
            }
            return instance;
        }
    }
    #endregion

    // UI
    public BossUI bossUI = null;

    // ����
    public BossName currentBossName = BossName.Mandoo;

    public GameObject currentBoss = null;
    public GameObject mandooTheBossPrefab = null;
    
    // 
    /// �÷��̾� �̵� ���� �׵θ� ����

    

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

