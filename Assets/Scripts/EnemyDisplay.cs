using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDisplay : MonoBehaviour
{
    List<GameObject> enemyBlips = new List<GameObject>();
    [SerializeField] GameObject enemyBlipPrefab;

    public void ShowEnemies()
    {
        foreach (GameObject enemyBlip in enemyBlips)
        {
            Destroy(enemyBlip);
        }
        enemyBlips = new List<GameObject>();
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject enemy in enemies)
        {
            GameObject newEnemyBlip = Instantiate(enemyBlipPrefab, new Vector3(enemy.transform.position.x, enemy.transform.position.y, -1), Quaternion.identity, transform);
            enemyBlips.Add(newEnemyBlip);
        }
    }

    
}
