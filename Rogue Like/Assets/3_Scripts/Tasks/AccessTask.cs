using UnityEngine;
using UnityEngine.InputSystem;

public class AccessTask : MonoBehaviour
{
    public InputActionReference action_Interact;
    public GameObject TaskPanel;

    private bool playerIsInZone = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsInZone = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsInZone = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerIsInZone)
        {
            if (action_Interact.action.WasPressedThisFrame())
            {
                Debug.Log("shouldWork");
                PlayTask();
            }
        }
    }

    void PlayTask()
    {
        TaskPanel.SetActive(true);
    }
}
