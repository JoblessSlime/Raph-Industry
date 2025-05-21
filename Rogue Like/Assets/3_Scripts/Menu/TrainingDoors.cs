using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TrainingDoors : MonoBehaviour
{
    public InputActionReference action_Interact;

    public bool isOptionDoor;
    public bool isLevelDoor;
    public bool isQuitDoor;

    private bool playerIsInZone;
    public GameObject ShowInput;
    public GameObject OptionMenu;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsInZone = true;
            ShowInput.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
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
                if(isOptionDoor)
                {
                    Options();
                }
                else if (isLevelDoor)
                {
                    Level();
                }
                else if(isQuitDoor)
                {
                    Quit();
                }
            }
        }
    }

    void Options()
    {

    }

    void Level()
    {
        SceneManager.LoadScene("Test_Raph");
    }

    void Quit()
    {
        Application.Quit();
    }
}
