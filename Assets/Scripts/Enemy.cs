using UnityEngine;

public class Enemy : MonoBehaviour {
    [SerializeField] private float speed = 4;
    private Player player;
    [SerializeField] private Animator animator;
    private AudioSource audio;
    [SerializeField] private AudioClip explosionClip;
    private bool isAlreadyHit;

    private void Start() {
        player = GameObject.Find("Player").GetComponent<Player>();
        animator = GetComponent<Animator>();
        
        audio = GetComponent<AudioSource>();
        if (audio == null) {
            Debug.LogError("Audio Source on Enemy is Null");
        }
        else {
            audio.clip = explosionClip;
        }
    }

    void Update() {
        transform.Translate(Vector3.down * (speed * Time.deltaTime));

        if (transform.position.y <= -6) {
            transform.position = SpawnPosition();
        }
    }

    public Vector3 SpawnPosition() {
        return new Vector3(Random.Range(-9f, 9f), 7, 0);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && !isAlreadyHit) {
            isAlreadyHit = true;
            Player player = other.transform.GetComponent<Player>();

            if (player != null) {
                player.Damage();
            }
            animator.SetTrigger("OnEnemyDeath");
            speed = 0.5f;
            audio.Play();
            Destroy(gameObject, 2.5f);
        }

        if (other.CompareTag("Laser") && !isAlreadyHit) {
            isAlreadyHit = true;
            if (player != null) {
                player.UpdateScore(10);
            }

            animator.SetTrigger("OnEnemyDeath");
            speed = 0.5f;
            Destroy(other.gameObject);
            audio.Play();
            Destroy(gameObject, 2.5f);
        }
    }
}