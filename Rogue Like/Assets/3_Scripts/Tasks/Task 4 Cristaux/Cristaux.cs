using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using TMPro;

public class Cristaux : MonoBehaviour
{
    public GraphicRaycaster raycaster; // Assign this in the Inspector or get it from your Canvas
    public EventSystem eventSystem;    // Assign in Inspector
    public InputActionReference action_Interact;

    public CharacterInputs characterInputs;
    public AccessTask accessTask;
    public GameManager gameManager;
    public GameObject TaskPanel;

    public bool taskKills;

    private Vector2 basePosition = Vector2.zero;
    private Vector2 baseObjectPos = Vector2.zero;

    public bool isHolding = false;

    private GameObject carriedObject;

    // cristaux
    public int cristalNumbers;
    private int deposedCristals = 0;
    private bool alreadyHold1;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;

        basePosition = Input.mousePosition;
        baseObjectPos = gameObject.transform.position;

        gameManager = GameObject.FindFirstObjectByType<GameManager>();
    }
    void Update()
    {

        Vector2 mousePos = Input.mousePosition;
        Vector2 newPos = mousePos - basePosition;

        PointerEventData pointerData = new PointerEventData(eventSystem)
        {
            position = baseObjectPos + newPos,
        };

        gameObject.transform.position = pointerData.position;

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerData, results);

        alreadyHold1 = false;
        foreach (RaycastResult result in results)
        {
            GameObject hitObject = result.gameObject;

            // tags
            if (hitObject.CompareTag("HoldObject") && !alreadyHold1)
            {
                alreadyHold1 = true;
                if (action_Interact.action.IsPressed())
                {
                    Debug.Log("Yes!");
                    hitObject.transform.position = pointerData.position;
                    isHolding = true;
                    carriedObject = hitObject;
                }
                else { isHolding = false; }
            }

            else if (hitObject.CompareTag("KillZone"))
            {
                // Die
                Debug.Log("Lose Task");
                Lose();
            }

            else if (hitObject.CompareTag("Tri vert") && isHolding)
            {
                if (action_Interact.action.WasReleasedThisFrame())
                {
                    Debug.Log("verttt");
                    isHolding = false;
                    if (!carriedObject.GetComponent<ChangeColor>().isViolet)
                    {
                        deposedCristals++;
                        Destroy(carriedObject);
                        if (deposedCristals >= cristalNumbers)
                        {
                            Win();
                        }
                    }
                    else
                    {
                        Lose();
                    }
                }
            }

            else if (hitObject.CompareTag("Tri Violet") && isHolding)
            {
                if (action_Interact.action.WasReleasedThisFrame())
                {
                    Debug.Log("violettt");
                    isHolding = false;
                    if (carriedObject.GetComponent<ChangeColor>().isViolet)
                    {
                        deposedCristals++;
                        Destroy(carriedObject);
                        if (deposedCristals >= cristalNumbers)
                        {
                            Win();
                        }
                    }
                    else
                    {
                        Lose();
                    }
                }
            }
        }
    }

    void Lose()
    {
        if (taskKills)
        {
            characterInputs.hp = 0;
        }
        else
        {
            TaskPanel.SetActive(false);
        }
    }

    void Win()
    {
        gameManager.taskNumberDone += 1;
        accessTask.TaskFinished();
        TaskPanel.SetActive(false);
    }
}
