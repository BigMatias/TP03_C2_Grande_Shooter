using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//IA

public class UIGameResult : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerMovement playerMovement;
    
    [Header("Panel")]
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private CanvasGroup panelCanvasGroup;

    [Header("Stats")]
    [SerializeField] private TextMeshProUGUI finalScoreText;

    [Header("Botones")]
    [SerializeField] private Button retryButton;
    [SerializeField] private Button menuButton;

    [Header("Animación")]
    [SerializeField] private float fadeInDuration = 0.5f;
    
    private void Awake()
    {
        retryButton?.onClick.AddListener(OnRetry);
        menuButton?.onClick.AddListener(OnMenu);
    }
    
    private void Start()
    {
        playerMovement.onDie += HandleGameOver;

        resultPanel.SetActive(false);
    }

    private void OnDisable()
    {
        if (playerMovement != null)
            playerMovement.onDie -= HandleGameOver;

        retryButton?.onClick.RemoveListener(OnRetry);
        menuButton?.onClick.RemoveListener(OnMenu);
    }

    private void HandleGameOver()
    {
        resultPanel.SetActive(true);
        StartCoroutine(AnimateGameOverIn());
    }

    private IEnumerator AnimateGameOverIn()
    {
        yield return StartCoroutine(FadeIn());

        yield return StartCoroutine(CountUpScore(ScoreManager.Instance.Score, 0.8f));
    }

    private IEnumerator FadeIn()
    {
        if (panelCanvasGroup == null) yield break;

        panelCanvasGroup.alpha = 0f;
        float elapsed = 0f;
        while (elapsed < fadeInDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            panelCanvasGroup.alpha = elapsed / fadeInDuration;
            yield return null;
        }
        panelCanvasGroup.alpha = 1f;
    }

    private IEnumerator CountUpScore(int targetScore, float duration)
    {
        if (finalScoreText == null) yield break;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            int display = Mathf.RoundToInt(Mathf.Lerp(0, targetScore, elapsed / duration));
            finalScoreText.text = display.ToString("N0");
            yield return null;
        }
        finalScoreText.text = targetScore.ToString("N0");
    }


    private void OnRetry()
    {
        Time.timeScale = 1f;
        PoolManager.Instance.ReturnAllPools();
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }

    private void OnMenu()
    {
        Time.timeScale = 1f;
        PoolManager.Instance.ReturnAllPools();
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenuScene");
    }
}