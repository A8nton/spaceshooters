using UnityEngine;

public class Enemy : MonoBehaviour {
    [SerializeField] private float speed = 4;
    private Player player;
    [SerializeField] private Animator animator;
    private AudioSource audio;
    [SerializeField] private AudioClip explosionClip;
    [SerializeField] private AudioClip laserClip;
    private bool isDead;
    [SerializeField] private GameObject enemyLaser;
    private float firePause;
    private bool bornToFire;
    private static readonly int OnEnemyDeath = Animator.StringToHash("OnEnemyDeath");

    private void Start() {
        player = GameObject.Find("Player").GetComponent<Player>();
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        if (audio == null) {
            Debug.LogError("Audio Source on Enemy is Null");
        }

        bornToFire = СheckIfBornToFire();
    }

    void Update() {
        transform.Translate(Vector3.down * (speed * Time.deltaTime));

        if (transform.position.y <= -6) {
            transform.position = SpawnPosition();
            bornToFire = СheckIfBornToFire();
        }

        if (Time.time > firePause && bornToFire && !isDead) {
            FireLaser();
        }
    }

    private bool СheckIfBornToFire() {
        return Random.Range(1, 5) == 1;
    }

    private void FireLaser() {
        firePause = (Time.time + 0.75f);
        Instantiate(enemyLaser, transform.position + new Vector3(0, -1.05f, 0), Quaternion.identity);

        audio.clip = laserClip;
        audio.Play();
    }

    private Vector3 SpawnPosition() {
        return new Vector3(Random.Range(-9f, 9f), 7, 0);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        audio.clip = explosionClip;
        if (other.CompareTag("Player") && !isDead) {
            isDead = true;

            if (player != null) {
                player.Damage();
            }

            animator.SetTrigger(OnEnemyDeath);
            speed = 0.5f;
            audio.Play();
            Destroy(gameObject, 2.5f);
        }

        if (other.CompareTag("Laser") && !isDead) {
            isDead = true;
            if (player != null) {
                player.UpdateScore(10);
            }

            animator.SetTrigger(OnEnemyDeath);
            speed = 0.5f;
            Destroy(other.gameObject);
            audio.Play();
            Destroy(gameObject, 2.5f);
        }
    }
}