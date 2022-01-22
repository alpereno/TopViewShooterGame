using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private GameObject gameOverUI;

    private void Start()
    {
        FindObjectOfType<Player>().onDeath += onGameOver;
    }

    void onGameOver() {
        Cursor.visible = true;
        StartCoroutine(fade(Color.clear, Color.black, 1));
        gameOverUI.SetActive(true);
    }

    IEnumerator fade(Color from, Color to, float time) {
        float speed = 1 / time;
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime * speed;
            fadeImage.color = Color.Lerp(from, to, percent);
            yield return null;
        }
    }

    public void startNewGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
