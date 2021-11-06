using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] GameObject enemy;
    [SerializeField] [Range(0.1f,30f)] float spawnTimer = 3f;
    [SerializeField] [Range(0,50)] int poolSize = 5;

    GameObject[] pool;
    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    void Awake()
    {
        PopulatePool();
    }

    void PopulatePool()
    {
        pool = new GameObject[poolSize];

        for (int i = 0; i < poolSize; i++)
        {
            pool[i] = Instantiate(enemy, transform);
            pool[i].SetActive(false);
        }
    }

    void EnableObjectInPool(){
       for (int i = 0; i < poolSize; i++)
        {
            if(!pool[i].activeInHierarchy){
                pool[i].SetActive(true);
                return;
            }
        } 
    }

    IEnumerator SpawnEnemy(){
        while (true){
            EnableObjectInPool();
            yield return new WaitForSeconds(spawnTimer);
        }
    }

}
