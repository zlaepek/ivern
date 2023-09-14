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
        //TODO: 정해진 위치에 소환

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
