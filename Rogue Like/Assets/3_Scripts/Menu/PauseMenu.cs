using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    private bool pausePanelActive = false;
    public InputActionReference action_Pause;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        if (action_Pause.action.WasPressedThisFrame())
        {
            pausePanelActive = !pausePanelActive;
            pausePanel.SetActive(pausePanelActive);
        }
    }

    public void Relancer()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
