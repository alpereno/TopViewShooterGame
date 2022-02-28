using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private RectTransform newWaveBanner;
    [SerializeField] private Text newWaveNumberText;
    [SerializeField] private Text newWaveEnemyCountText;

    Spawner spawner;

    private void Awake()
    {
        spawner = FindObjectOfType<Spawner>();
        spawner.onNewWave += onNewWave;
    }

    private void Start()
    {
        FindObjectOfType<Player>().onDeath += onGameOver;
    }

    void onNewWave(int waveNumber) {
        string[] numbers = { "One", "Two", "Three", "Four", "Five", "Six", "Seven"};
        newWaveNumberText.text = "-- Wave " + numbers[waveNumber - 1] + " --";
        string enemyCount = spawner.waves[waveNumber - 1].infinite ? "Infinite" : spawner.waves[waveNumber - 1].enemyCount + "";
        newWaveEnemyCountText.text = "Enemies: " + enemyCount;

        StopCoroutine("animateBanner");
        StartCoroutine(animateBanner());
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

    IEnumerator animateBanner() {
        //Banner will go down firstly then go back to up
        float delayTime = 1.5f;
        float speed = 3f;
        float animatePercent = 0;
        float endDelayTime = Time.time + 1 / speed + delayTime;
        int direction = -1;
        
        while (animatePercent >= 0){
            animatePercent -= Time.deltaTime * speed * direction;

            if (animatePercent >= 1)
            {
                animatePercent = 1;
                if (Time.time > endDelayTime)
                {
                    direction = 1;
                }
            }
            newWaveBanner.anchoredPosition = Vector2.up *Mathf.Lerp(400, 190, animatePercent);
            yield return null;
        }
    }

    public void startNewGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
