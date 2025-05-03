using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider))]
public class BoxEndTrigger : MonoBehaviour
{
    public EndGameController endGameController;
    private bool inRange = false;
    public GameObject promptUI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(out _))
        {
            inRange = true;
            promptUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(out _))
        {
            inRange = false;
            promptUI.SetActive(false);
        }
    }

    private void Update()
    {
        if (inRange && endGameController.player.inputActions.Player.Interact.WasPerformedThisFrame())
        {
            endGameController.StartEndSequence();
            inRange = false;
            Collider collider = GetComponent<BoxCollider>();
            promptUI.SetActive(false);
            collider.enabled = false;
        }
    }
}
