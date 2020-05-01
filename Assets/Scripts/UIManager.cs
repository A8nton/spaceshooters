using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    [SerializeField] private Text scoreText;
    [SerializeField] private Image livesImg;
    [SerializeField] private Sprite[] liveSprites;
    [SerializeField] private Text gameOverText;
    [SerializeField] private Text gameOverRestartText;
    private GameManager gameManager;

    private void Start() {
        scoreText.text = "Score: 0";
        gameOverText.gameObject.SetActive(false);
        gameOverRestartText.gameObject.SetActive(false);
        gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (gameManager == null) {
            Debug.LogError("GAME_MANAGER IS NULL");
        }

#if (!UNITY_ANDROID && !UNITY_IOS)
        FindObjectOfType<Joystick>().gameObject.SetActive(false);
        FindObjectOfType<FireButton>().gameObject.SetActive(false);
#endif
    }

    public void UpdateScore(int playerScore) {
        scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateLives(int currentLives) {
        livesImg.sprite = liveSprites[currentLives];

        if (currentLives == 0) {
            GameOverSequence();
        }
    }

    void GameOverSequence() {
#if (UNITY_ANDROID || UNITY_IOS)
        gameOverRestartText.text = "Touch to restart";
#else
        gameOverRestartText.text = "Press R to restart";
#endif
        gameManager.GameOver();
        gameOverRestartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
    }

    IEnumerator GameOverFlickerRoutine() {
        while (true) {
            gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.3f);
        }
    }
}