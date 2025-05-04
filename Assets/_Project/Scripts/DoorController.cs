using System.Collections;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Ссылки")]
    public Transform door;           // сам объект двери

    [Header("Параметры открытия")]
    public Vector3 openOffset = new Vector3(2f, 0f, 0f);  // куда и сколько должна уехать дверь
    public float moveDuration = 1f;                      // время анимации

    private Vector3 closedPosition;
    private Vector3 openPosition;
    private Coroutine moveRoutine;

    private void Awake()
    {
        // Запомним исходное положение двери
        closedPosition = door.localPosition;
        openPosition   = closedPosition + openOffset;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        // Начинаем открывать
        StartMove(openPosition);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        // Начинаем закрывать
        StartMove(closedPosition);
    }

    private void StartMove(Vector3 targetLocalPos)
    {
        // Отменяем текущее движение (если было)
        if (moveRoutine != null) StopCoroutine(moveRoutine);
        moveRoutine = StartCoroutine(MoveDoor(targetLocalPos));
    }

    private IEnumerator MoveDoor(Vector3 targetLocalPos)
    {
        Vector3 startPos = door.localPosition;
        float elapsed = 0f;
        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / moveDuration);
            door.localPosition = Vector3.Lerp(startPos, targetLocalPos, t);
            yield return null;
        }
        door.localPosition = targetLocalPos;
    }
}
