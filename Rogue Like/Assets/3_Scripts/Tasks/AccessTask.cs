using UnityEngine;
using UnityEngine.InputSystem;

public class AccessTask : MonoBehaviour
{
    public InputActionReference action_Interact;
    public GameObject TaskPanel;
    public GameObject ShowInput;

    public bool taskAlreadyDone = false;
    public bool playerIsInZone = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !taskAlreadyDone)
        {
            playerIsInZone = true;
            ShowInput.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !taskAlreadyDone)
        {
            playerIsInZone = false;
            ShowInput.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerIsInZone)
        {
            if (action_Interact.action.WasPressedThisFrame())
            {
                PlayTask();
            }
        }
    }

    void PlayTask()
    {
        TaskPanel.SetActive(true);
    }

    public void TaskFinished()
    {
        taskAlreadyDone = true;
        playerIsInZone = false;
        ShowInput.SetActive(false);
    }
}
