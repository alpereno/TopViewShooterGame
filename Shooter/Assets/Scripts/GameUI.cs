using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class GameUI : MonoBehaviour
{
    [Header("Game Over UI")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private GameObject gameOverUI;

    [Header("Wave UI")]
    [SerializeField] private RectTransform newWaveBanner;
    [SerializeField] private TMP_Text newWaveNumberText;
    [SerializeField] private TMP_Text newWaveEnemyCountText;

    [Header("Health UI")]
    [SerializeField] Slider healthSlider;
    [SerializeField] Image fillImage;
    [SerializeField] Color fullHealthColor;
    [SerializeField] Color zeroHealthColor;

    Spawner spawner;
    Player player;

    private void Awake()
    {
        spawner = FindObjectOfType<Spawner>();
        spawner.onNewWave += onNewWave;
        player = FindObjectOfType<Player>();
    }

    private void Start()
    {
        player.onDeath += onGameOver;
    }

    private void Update()
    {
        if (player != null)
        {
            setHealthUI(player.getHealth());
        }
    }

    void onNewWave(int waveNumber) {
        string[] numbers = { "One", "Two", "Three", "Four", "Five", "Six", "Seven"};
        newWaveNumberText.text = "--Wave " + numbers[waveNumber - 1] + "--";
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

    void setHealthUI(float health) {
        healthSlider.value = health;
        fillImage.color = Color.Lerp(zeroHealthColor, fullHealthColor, health / player.startingHealth);
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
