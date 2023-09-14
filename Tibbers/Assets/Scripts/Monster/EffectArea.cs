using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EffectArea : MonoBehaviour
{
    #region VARIABLES
    /* ICE VARIABLES */
    public GameObject iceArea = null;
    public GameObject fireArea = null;
    #endregion

    #region MANDOO (ICE, FIRE)
    /* ICE */
    public void SpawnIceArea(Transform thisTransform, float dashSpeed, float dashDuration)
    {

         GameObject spawnedIceArea = Instantiate(iceArea, thisTransform.position, Quaternion.Euler(thisTransform.GetComponent<Rigidbody2D>().velocity.normalized));

    }

    public IEnumerator RemoveIceArea(GameObject iceArea)
    {
        yield return new WaitForSeconds(2);
        Destroy(iceArea);

        yield break;
    }

    /* FIRE */
    public void SpawnFireArea(Transform centerTransform)
    {
        float radius = 5.0f;

        for (int i = 0; i < 6; i++)
        {
            float x = radius * Mathf.Sin(Mathf.Deg2Rad * i * 60);
            float y = radius * Mathf.Cos(Mathf.Deg2Rad * i * 60);
            Instantiate(fireArea, centerTransform.position + new Vector3(x, y), Quaternion.identity);
        }
        //TODO: ������ ��ġ�� ��ȯ

    }
    void FireArea()
    {
        //TODO: ���ΰ� �ε����� �� (���� �������� ���)
        // ���� �� �� ��� 
        // collider stay ������ Ÿ�̸� �����ؼ� deltatime����
        // ���� ���� ���ݾ� ���

        //TODO: �÷��̾ �ε����� �� (��Ʈ���� �ִ´�)
        // �׳� ������Ʈ������ ���� ���
    }
    #endregion
}
