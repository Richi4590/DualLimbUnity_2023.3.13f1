using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFader : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1.0f;

    private void Start()
    {
        // Ensure the fade image is initialized and transparent at the beginning
        if (fadeImage != null)
        {
            Color transparentColor = fadeImage.color;
            transparentColor.a = 1f;
            fadeImage.color = transparentColor;
        }

        Unfade(2.0f);
    }

    public async void Fade(float secondsToWaitUntilFade = 0.0f)
    {
        await Utilities.WaitForSecondsAsync(secondsToWaitUntilFade);

        if (fadeImage != null)
        {
            StartCoroutine(FadeToBlackCoroutine());
        }
    }

    public async void Unfade(float secondsToWaitUntilUnfade)
    {
        await Utilities.WaitForSecondsAsync(secondsToWaitUntilUnfade);

        if (fadeImage != null)
        {
            StartCoroutine(UnfadeCoroutine());
        }
    }

    private IEnumerator FadeToBlackCoroutine()
    {
        Color startColor = fadeImage.color;
        Color endColor = startColor;
        endColor.a = 1.0f;

        float startTime = Time.time;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            fadeImage.color = Color.Lerp(startColor, endColor, elapsedTime / fadeDuration);
            elapsedTime = Time.time - startTime;
            yield return null;
        }

        fadeImage.color = endColor; // Ensure the final color is set
    }

    private IEnumerator UnfadeCoroutine()
    {
        Color startColor = fadeImage.color;
        Color endColor = startColor;
        endColor.a = 0f;

        float startTime = Time.time;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            fadeImage.color = Color.Lerp(startColor, endColor, elapsedTime / fadeDuration);
            elapsedTime = Time.time - startTime;
            yield return null;
        }

        fadeImage.color = endColor; // Ensure the final color is set
    }
}
