using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeOut : MonoBehaviour
{
    public CanvasGroup canvasGroup;

    [Tooltip("Скорость затухания (секунд)")]
    public float fadeDuration = 1f;

    public string nextSceneName;

    void OnTriggerEnter(Collider other)
    {
        StartCoroutine(FadeOutEffect());   
    }

    public void StartFadeOut()
    {
        StartCoroutine(FadeOutEffect());   
    }

    private IEnumerator FadeOutEffect()
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(timer / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1f;

        // Когда экран полностью чёрный — загружаем новую сцену
        if (!string.IsNullOrEmpty(nextSceneName))
            SceneManager.LoadScene(nextSceneName);
    }
}
