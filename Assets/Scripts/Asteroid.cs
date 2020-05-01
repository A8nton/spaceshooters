
using UnityEngine;

public class Asteroid : MonoBehaviour {
    
    [SerializeField] private float rotationSpeed = 50;
    [SerializeField] private GameObject explosionPrefab;
    private SpawnManager spawnManager;

    private void Start() {
        spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
    }

    void Update() {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Laser")) {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            spawnManager.StartSpawning();
            Destroy(gameObject, 1f);
        }
    }
}
