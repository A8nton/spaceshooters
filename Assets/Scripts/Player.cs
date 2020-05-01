using System.Collections;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour {
    private const float NormalSpeed = 4f;
    private const float IncreasedSpeed = 10f;
    [SerializeField] private float speed = NormalSpeed;
    [SerializeField] private GameObject LaserPrefab;
    [SerializeField] private float fireRate = 0.15f;
    private float _canFire = -1f;
    [SerializeField] private int health = 3;
    private SpawnManager spawnManager;
    private bool shootTriple;
    [SerializeField] private GameObject TripleLaserPrefab;
    [SerializeField] private bool isShieldActive;
    [SerializeField] private GameObject shieldVisualaser;
    [SerializeField] private int score;
    private UIManager uiManager;
    [SerializeField] private GameObject rightEngine;
    [SerializeField] private GameObject leftEngine;
    private AudioSource audio;
    [SerializeField] private AudioClip laserClip;

    void Start() {
        transform.position = new Vector3(0, 0, 0);
        spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (spawnManager == null) {
            Debug.LogError("SpawnManager Null");
        }

        audio = GetComponent<AudioSource>();
        if (audio == null) {
            Debug.LogError("Audio Source on Player is Null");
        }
        else {
            audio.clip = laserClip;
        }
    }

    void Update() {
        CalculateMovement();
    }

    private void CalculateMovement() {
        bool firePressed;
        
#if (UNITY_ANDROID || UNITY_IOS)
        firePressed = CrossPlatformInputManager.GetButtonDown("Fire");
#else
        firePressed = Input.GetKeyDown(KeyCode.Space);
#endif
        if (firePressed && Time.time > _canFire) {
            FireLaser();
        }

        float horizontalInput;
        float verticalInput;

#if (UNITY_ANDROID || UNITY_IOS)
        horizontalInput = CrossPlatformInputManager.GetAxis("Horizontal");
        verticalInput = CrossPlatformInputManager.GetAxis("Vertical");
#else
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
#endif

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * (speed * Time.deltaTime));

        var position = transform.position;
        position = new Vector3(position.x, Mathf.Clamp(position.y, -3.8f, 0), 0);
        transform.position = position;

        if (transform.position.x >= 10) {
            transform.position = new Vector3(-10, transform.position.y, 0);
        }
        else if (transform.position.x <= -10) {
            transform.position = new Vector3(10, transform.position.y, 0);
        }
    }

    private void FireLaser() {
        _canFire = (Time.time + fireRate);
        if (shootTriple) {
            Instantiate(TripleLaserPrefab, transform.position, Quaternion.identity);
        }
        else {
            Instantiate(LaserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }

        audio.Play();
    }

    public void Damage() {
        if (isShieldActive) {
            isShieldActive = false;
            shieldVisualaser.SetActive(false);
            return;
        }

        health--;

        if (health == 2) {
            rightEngine.SetActive(true);
        }
        
        if (health == 1) {
            leftEngine.SetActive(true);
        }

        uiManager.UpdateLives(health);
        
        if (health < 1) {
            spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive() {
        shootTriple = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    private IEnumerator TripleShotPowerDownRoutine() {
        yield return new WaitForSeconds(5.0f);
        shootTriple = false;
    }

    public void IncreaseSpeed() {
        speed = IncreasedSpeed;
        StartCoroutine(IncreaseSpeedRoutine());
    }

    private IEnumerator IncreaseSpeedRoutine() {
        yield return new WaitForSeconds(5.0f);
        speed = NormalSpeed;
    }

    public void ActivateShield() {
        isShieldActive = true;
        shieldVisualaser.SetActive(true);
    }

    public void UpdateScore(int scr) {
        score += scr;
        uiManager.UpdateScore(score);
    }
}