using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Legumes : MonoBehaviour
{
    public InputActionReference action_Interact;
    public GameObject TaskPanel;
    public GameObject ShowInput;
    public int numberOfClicksNeeded;
    private int numberOfClicksDone;

    public bool taskAlreadyDone = false;
    public bool playerIsInZone = false;
    public GameObject taskGameObject;

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
                Debug.Log("PlayingTask");
                PlayTask();
            }
        }
    }

    void PlayTask()
    {
        ShowInput.transform.localScale = new Vector3(ShowInput.transform.localScale.x * 0.9f, ShowInput.transform.localScale.y * 0.9f, ShowInput.transform.localScale.z * 0.9f);
        numberOfClicksDone++;
        if(numberOfClicksDone > numberOfClicksNeeded)
        {
            TaskFinished();
        }
    }

    public void TaskFinished()
    {
        taskAlreadyDone = true;
        playerIsInZone = false;
        ShowInput.SetActive(false);
        taskGameObject.SetActive(false);
    }
}
