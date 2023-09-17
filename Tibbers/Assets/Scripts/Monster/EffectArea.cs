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
        //TODO: 만두가 부딪혔을 때 (얼음 게이지를 깐다)
        // 들어섰을 때 훅 까고 
        // collider stay 문에서 타이머 설정해서 deltatime으로
        // 몇초 마다 조금씩 까고

        //TODO: 플레이어가 부딪혔을 때 (도트뎀을 넣는다)
        // 그냥 업데이트문에서 냅다 깐다
    }
    #endregion
}
