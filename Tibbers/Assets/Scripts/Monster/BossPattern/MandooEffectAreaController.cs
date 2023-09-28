using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MandooEffectAreaController : MonoBehaviour
{
    #region VARIABLES
    private List<GameObject> fireAreaObjectList = new List<GameObject>();

    public float iceAreaSpawnFrequency = 20f;
    private float iceDuration = 5.0f;
    #endregion

    #region MANDOO (ICE, FIRE)
    /* ICE */
    public IEnumerator SpawnIceArea(Transform thisTransform, float dashSpeed, float dashDuration) {
        for (int i = 0; i < dashDuration * dashSpeed * iceAreaSpawnFrequency; i++) {
            GameObject spawnedIceArea = Instantiate(EffectAreaManager.instance.iceArea, thisTransform.position, thisTransform.rotation);
            spawnedIceArea.transform.parent = thisTransform.parent;
            Destroy(spawnedIceArea, iceDuration);
            yield return new WaitForSeconds(1 / (dashSpeed * iceAreaSpawnFrequency));
        }

        yield break;
    }

    /* FIRE */
    public void SpawnFireArea(Transform centerTransform, Transform parentTransform) {
        float radius = 6.0f;

        for (int i = 0; i < 6; i++) {
            float x = radius * Mathf.Sin(Mathf.Deg2Rad * i * 60);
            float y = radius * Mathf.Cos(Mathf.Deg2Rad * i * 60);
            fireAreaObjectList.Add(Instantiate(EffectAreaManager.instance.fireArea, centerTransform.position + new Vector3(x, y), Quaternion.identity));
            fireAreaObjectList[i].transform.parent = parentTransform;
        }
    }

    public void DestroyFireArea() {
        for (int i = 0; i < fireAreaObjectList.Count; i++) {
            Destroy(fireAreaObjectList[i]);
        }
    }
    #endregion
}
