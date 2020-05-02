using UnityEngine;

public class Powerup : MonoBehaviour {
    [SerializeField] private float speed = 4;
    [SerializeField] private int powerupID;
    [SerializeField] private AudioClip audioClip;

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
                switch (powerupID) {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.IncreaseSpeed();
                        break;
                    case 2:
                        player.ActivateShield();
                        break;
                }
            }
            AudioSource.PlayClipAtPoint(audioClip, new Vector3(0,0,-7));
            Destroy(gameObject);
        }
    }
}