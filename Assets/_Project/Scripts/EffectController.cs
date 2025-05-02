using Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class EffectController : MonoBehaviour
{
    [Header("Screen Shake")]
    public CinemachineImpulseSource impulseSource;

    [Header("Red Flash")]
    public CanvasGroup redFlashGroup;
    public float flashDuration = 0.2f;

    [Header("Glitch")]
    public Behaviour glitchEffect; // например, Volume override или скрипт

    [Header("Brother Animator")]
    public Animator brotherAnimator;

    public void TriggerHitEffects()
    {
        StartCoroutine(PlayHitEffects());
    }

    /// <summary>Полная последовательность эффектов.</summary>
    public IEnumerator PlayHitEffects()
    {
        // 1) Screen Shake
        impulseSource.GenerateImpulse();
        Debug.Log("Screen Shake");

        // 2) Красный фильтр
        //yield return FadeCanvasGroup(redFlashGroup, 0f, 1f, flashDuration/2);
        //yield return FadeCanvasGroup(redFlashGroup, 1f, 0f, flashDuration/2);

        // 3) Glitch
        //glitchEffect.enabled = true;
        yield return new WaitForSeconds(0.3f);
        //glitchEffect.enabled = false;

        // 4) Cry animation
        //brotherAnimator.SetTrigger("Cry");
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float from, float to, float time)
    {
        float t = 0f;
        while (t < time)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(from, to, t/time);
            yield return null;
        }
        cg.alpha = to;
    }
}
