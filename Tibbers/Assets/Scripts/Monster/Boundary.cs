using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary : MonoBehaviour
{
    public GameObject boundary = null;
    public void SpawnNewBoss(Vector3 position) {
        boundary.SetActive(true);
    }
}