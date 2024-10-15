using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{

    public GameObject[] enemyPrefab;
    public Transform planeTransform;
    private Tutorial3D tutorial3;

    void Start()
    {
        tutorial3 = new Tutorial3D();
        tutorial3.Enable();
    }
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.PageUp))
        //if (Singleton.Instance.tutorial3.Player.Generate.triggered)
        if (tutorial3.Player.Generate.triggered)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        Vector3 randomPosition = GetRandomPositionOnPlane();
        int randomIndex = Random.Range(0, enemyPrefab.Length);
        Instantiate(enemyPrefab[randomIndex], randomPosition, Quaternion.identity);
    }

    Vector3 GetRandomPositionOnPlane()
    {
        Vector3 planeSize = new Vector3(10, 10, 10);
        //Vector3 planeSize = planeTransform.localScale;
        float halfWidth = planeSize.x / 2f;
        float halfLength = planeSize.z / 2f;

        float randomX = Random.Range(-halfWidth, halfWidth);
        float randomZ = Random.Range(-halfLength, halfLength);

        return new Vector3(randomX, 0, randomZ) + planeTransform.position;
    }
}