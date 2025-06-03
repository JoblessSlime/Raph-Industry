using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using TMPro;

public class Sandwhich : MonoBehaviour
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

    // sandwich
    public GameObject bread;
    public GameObject tomatoes;
    public GameObject cheese;

    public GameObject breadSpawner;
    public GameObject tomatoesSpawner;
    public GameObject cheeseSpawner;

    public GameObject sandwich;

    public List<GameObject> sandwich_composition;

    public TextMeshProUGUI recipeText;

    public bool carriesBread;
    public bool carriesTomatoes;
    public bool carriescheese;

    private int sandwichPart;

    private bool alreadyHold1 = false;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;


        basePosition = Input.mousePosition;
        baseObjectPos = Input.mousePosition;

        gameManager = GameObject.FindFirstObjectByType<GameManager>();

        int numberOfIngredients = Random.Range(3, 7);
        string text = "";
        for (int i = 0; i < numberOfIngredients; i++)
        {
            if ( i == 0 || i == numberOfIngredients - 1)
            {
                sandwich_composition.Add(bread);
            }
            else
            {
                if (Random.Range(0,2) == 0)
                {
                    sandwich_composition.Add(tomatoes);
                }
                else
                {
                    sandwich_composition.Add(cheese);
                }
            }

            text += '\n' + sandwich_composition[i].name;
        }

        recipeText.text = text;
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
            if (hitObject.CompareTag("Bread"))
            {
                Debug.Log("seeBread");
                if (action_Interact.action.IsPressed() && !hitObject.GetComponent<AlreadyDeposed>().alreadyDeposed && !alreadyHold1)
                {
                    alreadyHold1 = true;
                    Debug.Log("Yes!");
                    hitObject.transform.position = pointerData.position;
                    isHolding = true;
                    carriesBread = true;
                    carriedObject = hitObject;
                }
                else { isHolding = false; carriesBread = false; }
            }

            else if (hitObject.CompareTag("Tomatoes"))
            {
                if (action_Interact.action.IsPressed() && !hitObject.GetComponent<AlreadyDeposed>().alreadyDeposed && !alreadyHold1)
                {
                    alreadyHold1 = true;
                    Debug.Log("Yes!");
                    hitObject.transform.position = pointerData.position;
                    isHolding = true;
                    carriesTomatoes = true;
                    carriedObject = hitObject;
                }
                else { isHolding = false; carriesTomatoes = false; }
            }

            else if (hitObject.CompareTag("Cheese"))
            {
                if (action_Interact.action.IsPressed() && !hitObject.GetComponent<AlreadyDeposed>().alreadyDeposed && !alreadyHold1)
                {
                    alreadyHold1 = true;
                    Debug.Log("Yes!");
                    hitObject.transform.position = pointerData.position;
                    isHolding = true;
                    carriescheese = true;
                    carriedObject = hitObject;
                }
                else { isHolding = false; carriescheese = false; }
            }

            else if (hitObject.CompareTag("KillZone"))
            {
                // Die
                Debug.Log("Lose Task");
                Lose();
            }

            if (hitObject.CompareTag("Finish") && isHolding)
            {
                Debug.Log("seeFinish");
                if (action_Interact.action.WasReleasedThisFrame())
                {
                    isHolding = false;
                    if (carriesBread && sandwich_composition[sandwichPart] == bread)
                    {
                        if (sandwichPart == sandwich_composition.Count - 1)
                        {
                            Win();
                        }
                        else
                        {
                            bread.GetComponent<AlreadyDeposed>().alreadyDeposed = false;
                            GameObject newGO = Instantiate(bread, breadSpawner.transform);
                            newGO.transform.localPosition = Vector3.zero;
                            bread.GetComponent<AlreadyDeposed>().alreadyDeposed = true;
                            carriedObject.GetComponent<AlreadyDeposed>().alreadyDeposed = true;
                            carriedObject.transform.SetParent(sandwich.transform);
                        }
                    }
                    else if (carriescheese && sandwich_composition[sandwichPart] == cheese)
                    {
                        if (sandwichPart == sandwich_composition.Count)
                        {
                            Win();
                        }
                        else
                        {
                            cheese.GetComponent<AlreadyDeposed>().alreadyDeposed = false;
                            GameObject newGO = Instantiate(cheese, cheeseSpawner.transform);
                            newGO.transform.localPosition = Vector3.zero;
                            cheese.GetComponent<AlreadyDeposed>().alreadyDeposed = true;
                            carriedObject.GetComponent<AlreadyDeposed>().alreadyDeposed = true;
                            carriedObject.transform.SetParent(sandwich.transform);
                        }
                    }
                    else if (carriesTomatoes && sandwich_composition[sandwichPart] == tomatoes)
                    {
                        if (sandwichPart == sandwich_composition.Count)
                        {
                            Win();
                        }
                        else
                        {
                            tomatoes.GetComponent<AlreadyDeposed>().alreadyDeposed = false;
                            GameObject newGO = Instantiate(tomatoes, tomatoesSpawner.transform);
                            newGO.transform.localPosition = Vector3.zero;
                            tomatoes.GetComponent<AlreadyDeposed>().alreadyDeposed = true;
                            carriedObject.GetComponent<AlreadyDeposed>().alreadyDeposed = true;
                            carriedObject.transform.SetParent(sandwich.transform);
                        }
                    }
                    else
                    {
                        Lose();
                    }
                    sandwichPart++;
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
