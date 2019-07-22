using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    [SerializeField]
    private GameObject boarPrefab, cannibalPrefab;

    public Transform[] cannibalSpawnPoints, boarSpawnPoints;

    [SerializeField]
    private int cannibalCount, boarCount;

    private int initialCannibalCount, initialBoarCount;

    public float spawnWaitTime = 10;
    void Awake()
    {
        MakeInstance();
    }

    void Start()
    {
        initialCannibalCount = cannibalCount;
        initialBoarCount = boarCount;
        SpawnEnemies();
        StartCoroutine("CheckToSpawnEnimies");
    }

    void MakeInstance()
    {
        if(instance==null)
        {
            instance = this;
        }
    }

    void SpawnEnemies()
    {
        SpawnBoars();
        SpawnCannibals();
    }

    void SpawnCannibals()
    {
        int index = 0;
        for(int i=0;i<cannibalCount;i++)
        {
            if (index >= cannibalSpawnPoints.Length)
            {
                index = 0;
            }
            Instantiate(cannibalPrefab, cannibalSpawnPoints[index].position, Quaternion.identity);
            index++;
        }

        cannibalCount = 0;
    }

    IEnumerator CheckToSpawnEnemies()
    {
        yield return new WaitForSeconds(spawnWaitTime);
        SpawnCannibals();
        SpawnBoars();
        StartCoroutine("CheckToSpawnEnemies");
    }
    void SpawnBoars()
    {
        int index = 0;
        for (int i = 0; i < boarCount; i++)
        {
            if (index >= boarSpawnPoints.Length)
            {
                index = 0;
            }
            Instantiate(boarPrefab, boarSpawnPoints[index].position, Quaternion.identity);
            index++;
        }

        boarCount = 0;
    }

    public void EnemyDied(bool cannibal)
    {
        if(cannibal)
        {
            cannibalCount++;
            if(cannibalCount>initialCannibalCount)
            {
                cannibalCount = initialCannibalCount;
            }
        }
        else
        {
            boarCount++;
            if (boarCount > initialBoarCount)
            {
                boarCount = initialBoarCount;
            }
        }
    }

    public void StopSpawning()
    {
        StopCoroutine("CheckToSpawnEnemies");
    }
}
