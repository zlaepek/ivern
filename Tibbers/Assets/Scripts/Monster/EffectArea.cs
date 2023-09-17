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

    public float iceAreaSpawnFrequency = 20f;
    private float iceDuration = 5.0f;
    #endregion

    #region MANDOO (ICE, FIRE)
    /* ICE */
    public IEnumerator SpawnIceArea(Transform thisTransform, float dashSpeed, float dashDuration)
    {
        for (int i = 0; i < dashDuration * dashSpeed * iceAreaSpawnFrequency; i++) {
            GameObject spawnedIceArea = Instantiate(iceArea, thisTransform.position, thisTransform.rotation);
            Destroy(spawnedIceArea, iceDuration);
            yield return new WaitForSeconds(1 / (dashSpeed * iceAreaSpawnFrequency));
        }

        yield break;
    }

    /* FIRE */
    public void SpawnFireArea(Transform centerTransform)
    {
        float radius = 6.0f;

        for (int i = 0; i < 6; i++)
        {
            float x = radius * Mathf.Sin(Mathf.Deg2Rad * i * 60);
            float y = radius * Mathf.Cos(Mathf.Deg2Rad * i * 60);
            Instantiate(fireArea, centerTransform.position + new Vector3(x, y), Quaternion.identity);
        }
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
