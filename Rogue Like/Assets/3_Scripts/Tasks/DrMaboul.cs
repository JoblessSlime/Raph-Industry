using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class DrMaboul : MonoBehaviour
{
    public GraphicRaycaster raycaster; // Assign this in the Inspector or get it from your Canvas
    public EventSystem eventSystem;    // Assign in Inspector
    public InputActionReference action_Interact;

    public CharacterInputs characterInputs;
    public AccessTask accessTask;
    public GameManager gameManager;
    public GameObject TaskPanel;

    private Vector2 basePosition = Vector2.zero;
    private Vector2 baseObjectPos = Vector2.zero;

    private bool isHolding = false;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        /*
        basePosition = Input.mousePosition;
        baseObjectPos = gameObject.transform.position;
        */
    }
    void Update()
    {
        /*
        Vector2 mousePos = Input.mousePosition;
        Vector2 newPos = mousePos - basePosition;

        PointerEventData pointerData = new PointerEventData(eventSystem)
        {
            position = baseObjectPos + newPos,
        };
        */

        PointerEventData pointerData = new PointerEventData(eventSystem)
        {
            position = Input.mousePosition,
        };

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerData, results);

        foreach (RaycastResult result in results)
        {
            GameObject hitObject = result.gameObject;

            // tags
            if (hitObject.CompareTag("Task"))
            {
                if (action_Interact.action.IsPressed())
                {
                    Debug.Log("Yes!");
                    hitObject.transform.position = pointerData.position;
                    isHolding = true;
                }
                else { isHolding = false; }
            }

            else if (hitObject.CompareTag("KillZone"))
            {
                // Die
                Debug.Log("Lose Task");
                Lose();
            }

            else if (hitObject.CompareTag("Finish") && isHolding)
            {
                // Win
                Debug.Log("Win Task");
                Win();
            }
        }
    }

    void Lose()
    {
        characterInputs.hp = 0;
    }

    void Win()
    {
        gameManager.taskNumberDone += 1;
        accessTask.TaskFinished();
        TaskPanel.SetActive(false);
    }
}
