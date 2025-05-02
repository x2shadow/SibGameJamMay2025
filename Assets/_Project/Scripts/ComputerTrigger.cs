using UnityEngine;
using UnityEngine.InputSystem;

public class ComputerTrigger : MonoBehaviour
{
    [Header("Ссылки")]
    InputActions inputActions;
    public SnakeGameManager snakeManager;
    public PlayerController player;
    public GameObject promptUI;
    public DialogueUI dialogueUI;
    public string afterWinText;
    public GameObject dialogueTrigger2;

    private bool inRange = false;
    private bool isWon = false;

    void Awake()
    {
        inputActions = player.inputActions;
    }

    private void OnEnable()
    {
        // Подписываемся на событие Enter у E
        inputActions.Player.Interact.performed += OnInteract;
        // Убедимся, что карта Player включена
        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.Interact.performed -= OnInteract;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isWon) return;

        if (other.GetComponent<PlayerController>() != null)
        {
            inRange = true;
            promptUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            inRange = false;
            promptUI.SetActive(false);
        }
    }

    private void OnInteract(InputAction.CallbackContext ctx)
    {
        if (isWon) return;
        if (!inRange) return;

        promptUI.SetActive(false);

        // Блокируем FPS-ввод
        inputActions.Player.Disable();
        player.SetInputBlocked(true);

        // Показываем Canvas змейки
        snakeManager.GetComponent<Canvas>().gameObject.SetActive(true);

        // Запускаем игру
        snakeManager.Init(inputActions, OnWin, OnLose);
    }

    private void OnWin()
    {
        EndSession();
        Debug.Log("Победа!");
        isWon = true;
        dialogueUI.ShowPlayerDialogue(afterWinText);
        if (dialogueTrigger2 != null) dialogueTrigger2.SetActive(true);
    }

    private void OnLose()
    {
        Debug.Log("Проигрыш! Рестарт");
        snakeManager.Init(inputActions, OnWin, OnLose);
    }

    private void EndSession()
    {
        // Скрываем Canvas
        snakeManager.GetComponent<Canvas>().gameObject.SetActive(false);

        // Возвращаем ввод FPS
        inputActions.Player.Enable();
        player.SetInputBlocked(false);
    }
}
