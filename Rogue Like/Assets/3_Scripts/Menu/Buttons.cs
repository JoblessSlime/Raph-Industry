using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public void LaunchGame()
    {
        SceneManager.LoadScene("ChangingPlanet");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
