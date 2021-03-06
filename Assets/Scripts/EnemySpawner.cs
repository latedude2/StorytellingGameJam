using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    GameObject enemyPrefab;
    int enemyCount = 10;
    float mapRadius;
    float yModifier = 0.7f;
    float spawnInterval = 20f;

    // Start is called before the first frame update
    void Start()
    {
        mapRadius = 0.5f * transform.localScale.x;
        for(int i = 0; i < enemyCount; i++){
            SpawnEnemy();
        }
        InvokeRepeating(nameof(SpawnEnemy), spawnInterval, spawnInterval);
    }

    void SpawnEnemy(){
        //random bool basically
        if(Random.value > 0.5f)     
        {
            if(Random.value > 0.5f)     
            {
                //spawn south
                Instantiate(enemyPrefab, new Vector3(Random.Range(-mapRadius, mapRadius), -mapRadius*yModifier, -1), Quaternion.identity, transform);
            }
            else {
                //spawn north
                Instantiate(enemyPrefab, new Vector3(Random.Range(-mapRadius, mapRadius), mapRadius*yModifier, -1), Quaternion.identity, transform);
            }
        }
        else{
            if(Random.value > 0.5f)     
            {
                //spawn west
                Instantiate(enemyPrefab, new Vector3(-mapRadius,Random.Range(-mapRadius*yModifier, mapRadius*yModifier), -1), Quaternion.identity, transform);
            }
            else {
                //spawn east
                Instantiate(enemyPrefab, new Vector3(mapRadius,Random.Range(-mapRadius*yModifier, mapRadius*yModifier), -1), Quaternion.identity, transform);
            }

        }


    }
}
