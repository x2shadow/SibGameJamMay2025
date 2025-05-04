using System.Collections;
using UnityEngine;
using Cinemachine;

public class EndGameController : MonoBehaviour
{
    [Header("Ссылки")]
    public PlayerController player;
    public GameObject[] objectsToDisable;
    public Transform nuggetBox;
    public CanvasGroup thankYouCanvasGroup;
    public CinemachineVirtualCamera vcam;
    public AudioSource audioSource;

    [Header("Параметры полёта")]
    public float flyDuration = 1.5f;
    public float finalDistance = 4f;
    public float eyeHeightOffset = 1.5f;

    [Header("Вращение")]
    public float rotationSpeed = 90f;

    [Header("FOV-твики")]
    public float fovPeak = 120f;
    public float fovOriginal = 60f;
    public float fovUpDuration = 0.5f;
    public float fovDownDuration = 0.5f;

    [Header("Текст")]
    public float textFadeDuration = 1f;

    private bool sequenceStarted = false;

    public void StartEndSequence()
    {
        if (sequenceStarted) return;
        sequenceStarted = true;

        player.SetInputBlocked(true);
        foreach (var obj in objectsToDisable)
            obj.SetActive(false);

        thankYouCanvasGroup.alpha = 0f;
        thankYouCanvasGroup.gameObject.SetActive(false);

        audioSource.Play();

        StartCoroutine(EndSequenceRoutine());
    }

    private IEnumerator EndSequenceRoutine()
    {
        // 1) TVIK FOV и ждём
        yield return StartCoroutine(FOVTween());

        // 2) Полёт и одноразовое вращение
        yield return StartCoroutine(FlightTween());

        // 3) Запускаем бесконечное вращение отдельно
        StartCoroutine(KeepRotating());

        // 4) И только теперь — текст
        yield return StartCoroutine(FadeCanvasGroup(thankYouCanvasGroup, 0f, 1f, textFadeDuration));
    }

    private IEnumerator FOVTween()
    {
        float t = 0f;
        // вверх
        while (t < fovUpDuration)
        {
            t += Time.deltaTime;
            vcam.m_Lens.FieldOfView = Mathf.Lerp(60f, fovPeak, t / fovUpDuration);
            yield return null;
        }
        // вниз
        t = 0f;
        while (t < fovDownDuration)
        {
            t += Time.deltaTime;
            vcam.m_Lens.FieldOfView = Mathf.Lerp(fovPeak, fovOriginal, t / fovDownDuration);
            yield return null;
        }
        vcam.m_Lens.FieldOfView = fovOriginal;
    }

    private IEnumerator FlightTween()
    {
        Transform camT = player.playerCamera.transform;
        Vector3 eyeLevel = camT.position + Vector3.up * eyeHeightOffset;
        Vector3 targetPos = eyeLevel + camT.forward * finalDistance;
        Vector3 startPos = nuggetBox.position;

        float elapsed = 0f;
        while (elapsed < flyDuration)
        {
            elapsed += Time.deltaTime;
            float tt = Mathf.SmoothStep(0f, 1f, elapsed / flyDuration);
            nuggetBox.position = Vector3.Lerp(startPos, targetPos, tt);
            nuggetBox.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
            yield return null;
        }
        nuggetBox.position = targetPos;
    }

    private IEnumerator KeepRotating()
    {
        while (true)
        {
            nuggetBox.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
            yield return null;
        }
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float from, float to, float duration)
    {
        cg.gameObject.SetActive(true);
        float elapsed = 0f;
        cg.alpha = from;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            cg.alpha = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }
        cg.alpha = to;
    }
}
