using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public Manager manager;
    public GameObject ChangePlanetPanel;
    public GameObject AmeliorationPanel;
    public void LaunchGame()
    {
        SceneManager.LoadScene("ChangingPlanet");
    }

    public void Amelioration1()
    {
        manager.decelerationTimeAdded = 1f;
        manager.speedAdded = 1f;
        ChangePlanetPanel.SetActive(true);
        AmeliorationPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
