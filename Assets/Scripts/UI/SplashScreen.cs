using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup logo1;
    [SerializeField] private CanvasGroup logo2;
    [SerializeField] private float fadeInDuration = 1.5f;
    [SerializeField] private float holdDuration = 1.5f;
    [SerializeField] private float fadeOutDuration = 1f;
    [SerializeField] private string nextScene = "MainMenu";

    private void Start()
    {
        logo1.alpha = 0f;
        logo2.alpha = 0f;
        StartCoroutine(PlaySplash());
    }

    private IEnumerator PlaySplash()
    {
        yield return Fade(logo1, 0f, 1f, fadeInDuration);
        yield return new WaitForSeconds(holdDuration);
        yield return Fade(logo1, 1f, 0f, fadeOutDuration);

        yield return Fade(logo2, 0f, 1f, fadeInDuration);
        yield return new WaitForSeconds(holdDuration);
        yield return Fade(logo2, 1f, 0f, fadeOutDuration);

        SceneManager.LoadScene(nextScene);
    }

    private IEnumerator Fade(CanvasGroup target, float from, float to, float duration)
    {
        float elapsed = 0f;
        target.alpha = from;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            target.alpha = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }

        target.alpha = to;
    }
}