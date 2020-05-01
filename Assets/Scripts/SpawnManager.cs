using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour {
    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] private GameObject enemyContainer;
    private bool spawn = true;

    [SerializeField] private GameObject[] powerups;

    public void StartSpawning() {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }
    
    IEnumerator SpawnEnemyRoutine() {
        yield return new WaitForSeconds(3f);
        
        while (spawn) {
            GameObject newEnemy =
                Instantiate(enemyPrefab, new Vector3(Random.Range(-8f, 8f), 7, 0), Quaternion.identity);
            newEnemy.transform.parent = enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }
    }

    private IEnumerator SpawnPowerupRoutine() {
        yield return new WaitForSeconds(3f);

        while (spawn) {
            yield return new WaitForSeconds(Random.Range(6f, 9f));
            Instantiate(powerups[Random.Range(0, 3)], new Vector3(Random.Range(-9f, 9f), 8, 0), Quaternion.identity);
        }
    }

    public void OnPlayerDeath() {
        spawn = false;
    }
}