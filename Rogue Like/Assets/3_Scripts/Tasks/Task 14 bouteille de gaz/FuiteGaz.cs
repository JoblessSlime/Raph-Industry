using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class FuiteGaz : MonoBehaviour
{
    public InputActionReference action_Interact;
    public GameObject TaskPanel;
    public GameObject ShowInput;

    public bool taskAlreadyDone = false;
    public bool playerIsInZone = false;
    public SpriteRenderer thisImage;
    public GameObject taskGameObject;
    public Sprite spriteFinished;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        thisImage = taskGameObject.GetComponent<SpriteRenderer>();
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
                Debug.Log("PlayingTask");
                PlayTask();
            }
        }
    }

    void PlayTask()
    {
        thisImage.sprite = spriteFinished;
        TaskFinished();
    }

    public void TaskFinished()
    {
        taskAlreadyDone = true;
        playerIsInZone = false;
        ShowInput.SetActive(false);
    }
}
