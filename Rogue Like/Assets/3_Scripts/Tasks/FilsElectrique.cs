using DG.Tweening.Core.Easing;
using UnityEngine;
using UnityEngine.InputSystem;

public class FilsElectrique : MonoBehaviour
{
    public InputActionReference action_Interact;
    public GameObject ShowInput;
    public GameManager gameManager;
    public AccessTask accessTask;

    private bool isCarryingWire = false;
    public bool isInFinishZone = false;
    public bool taskAlreadyDone = false;
    public bool playerIsInZone = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.FindFirstObjectByType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !taskAlreadyDone)
        {
            playerIsInZone = true;
            ShowInput.SetActive(true);
        }

        else if (collision.CompareTag("WireFinishZone") && !taskAlreadyDone)
        {
            isInFinishZone = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !taskAlreadyDone)
        {
            playerIsInZone = false;
            ShowInput.SetActive(false);
        }

        else if (collision.CompareTag("WireFinishZone") && !taskAlreadyDone)
        {
            isInFinishZone = false;
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
        if (isCarryingWire)
        {
            this.gameObject.transform.position = GameObject.FindGameObjectsWithTag("Player")[0].transform.position;
            ShowInput.SetActive(false);

            if (isInFinishZone)
            {
                if (action_Interact.action.WasPressedThisFrame())
                {
                    TaskFinished();
                }
            }
        }
    }

    void PlayTask()
    {
        isCarryingWire = !isCarryingWire;
    }

    public void TaskFinished()
    {
        gameManager.taskNumberDone += 1;

        isCarryingWire = false;
        taskAlreadyDone = true;
        playerIsInZone = false;
        ShowInput.SetActive(false);

        accessTask.TaskFinished();
    }
}   
