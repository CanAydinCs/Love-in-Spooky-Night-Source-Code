using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAllay : MonoBehaviour
{
    public int allayNumber = 0;

    public GameObject[] spawnPoints;
    public GameObject friendPrefab;

    int spawnIndex = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject spawn = spawnPoints[spawnIndex];
            Vector3 spawnPosition = spawn.transform.position;
            spawnPosition.y += 5;
            Instantiate(friendPrefab, spawnPosition, Quaternion.identity);

            spawnIndex++;
            if(spawnIndex >= spawnPoints.Length)
            {
                spawnIndex = 0;
            }
        }
    }
}
