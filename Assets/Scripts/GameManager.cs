using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    [SerializeField] private bool isGameOver;

    private void Update() {
        bool restartPressed;

#if (UNITY_ANDROID || UNITY_IOS)
        restartPressed = Input.GetMouseButtonDown(0);
#else
        restartPressed = Input.GetKeyDown(KeyCode.R);
#endif
        if (restartPressed && isGameOver) {
            SceneManager.LoadScene(1);
        }
    }

    public void GameOver() {
        isGameOver = true;
    }
}