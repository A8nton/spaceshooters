using UnityEngine;

public class EnemyLaser : MonoBehaviour {
    [SerializeField] private float speed = 10;

    void Update() {
        transform.Translate(Vector3.down * (speed * Time.deltaTime));

        if (transform.position.y <= -6) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            Player player = other.transform.GetComponent<Player>();

            if (player != null) {
                player.Damage();
            }
        }

        if (other.CompareTag("Laser")) {
            Destroy(other.gameObject);
        }

        Destroy(gameObject);
    }
}