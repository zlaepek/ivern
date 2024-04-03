using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 인게임 드랍되는 아이템들
public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }

    public enum eItemType
    {
        EXP_ball,
        
        Coin,

        Max,
    }

    public GameObject EXP_Ball;
    public GameObject Coin;


    private List<GameObject>[] ItemPool = new List<GameObject>[(int)eItemType.Max];

    private GameObject playerCharacter;
    private float fPlayer_Radius;

    private void Awake()
    {
        ItemPool[(int)eItemType.EXP_ball] = new List<GameObject>();
        ItemPool[(int)eItemType.Coin] = new List<GameObject>();

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerCharacter = GameObject.FindGameObjectWithTag("tag_Player");
        fPlayer_Radius = GetRadius(playerCharacter);
    }

    // Update is called once per frame
    void Update()
    {
        Check();
    }
    private void Check()
    {
        //foreach(List<GameObject> Iter_List in ItemPool)
        for(int i = 0; i < (int)eItemType.Max; ++i)
            {
            foreach (GameObject it in ItemPool[i])
            {
                if (it.activeSelf == false)
                {
                    continue;
                }

                Item TempItem = it.GetComponent<Item>();

                if (CheckSphereCollision(TempItem.transform.position, playerCharacter.transform.position, TempItem.Radius, fPlayer_Radius))
                {
                    TempItem.ItemEffect(i);

                    return;
                }
            }
        }
    }

    bool CheckSphereCollision(Vector2 _PosA, Vector2 _PosB, float _RadA, float _RadB)
    {
        float distance = Vector2.Distance(_PosA, _PosB);
        float sumOfRadi = _RadA + _RadB;

        return distance <= sumOfRadi;
    }

    private GameObject FindNoActiveItem(int _Type)
    {
        foreach (GameObject it in ItemPool[_Type])
        {
            if (it.activeSelf == false)
            {
                return it;
            }
        }
        return null;
    }

    public void DropItem(GameObject _object, Vector2 _vPoint, int _iType)
    {
        GameObject Item = FindNoActiveItem(_iType);

        if (!Item)
        {
            Item = GameObject.Instantiate(_object, _vPoint, Quaternion.identity);
            AddPool(Item, _iType);
        }
        else
        {
            Item.SetActive(true);
            Item.transform.position = _vPoint;
        }
        Item Temp_Item = Item.GetComponent<Item>();
        Temp_Item.Init(_iType);

        Debug.Log("\n ItemDropPos : " + _vPoint.x + ", " + _vPoint.y + " :: Type : " + Temp_Item.Type);
    }

    private void AddPool(GameObject _Item, int _Type)
    {
        ItemPool[_Type].Add(_Item);
    }

    private float GetRadius(GameObject _Object)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            return 0f;
        }

        Bounds bounds = spriteRenderer.bounds;
        //return Mathf.Sqrt((bounds.size.x * bounds.size.x * 0.25f) + (bounds.size.y * bounds.size.y * 0.25f));
        return (bounds.size.x + bounds.size.y) * 0.5f;
    }
}
