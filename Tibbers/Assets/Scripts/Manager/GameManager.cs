using UnityEngine;

using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    #region Static GameManager
    private static GameManager _instance = null;
    public static GameManager Instance
    {
        get
        {
            return _instance;

        }
    }
    #endregion

    #region Life Cycle (Initialize)
    private void Awake()
    {
        if (Instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    public GameObject FindAllObject(string _Name)
    {
        GameObject[] inactiveObjects = GameObject.FindObjectsOfType<GameObject>();
        GameObject Res = null;
        foreach (GameObject obj in inactiveObjects)
        {
            if (obj.name == "Canvas")
            {
                obj.SetActive(true);
            }
            List<GameObject> childObjects = GetChildObjects(obj);

            foreach(GameObject child in childObjects)
            {
                if (child.name == _Name)
                {
                    Res = obj;
                }
            }
            
        }

        return Res;
    }
    // �ڽ� ��ȸ�ϸ鼭 ã����

    public List<GameObject> GetChildObjects(GameObject _parentObject)
    {
        List<GameObject> childObjects = new List<GameObject>();

        // �θ� ������Ʈ�� �ڽ� ������ŭ �ݺ��Ͽ� �ڽ� ������Ʈ�� ����Ʈ�� �߰��մϴ�.
        int childCount = _parentObject.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform childTransform = _parentObject.transform.GetChild(i);
            GameObject childObject = childTransform.gameObject;
            childObjects.Add(childObject);
            GetChildObject(childObject, ref childObjects);
        }

        return childObjects;
    }

    public void GetChildObject(GameObject _parentObject, ref List<GameObject> _List)
    {
        int childCount = _parentObject.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform childTransform = _parentObject.transform.GetChild(i);
            GameObject childObject = childTransform.gameObject;
            _List.Add(childObject);
            GetChildObject(childObject, ref _List);
        }
    }

}
