using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CharacterInputs characterInputs;

    // timer
    public float timerLength;
    public TextMeshProUGUI timerText;
    private float time = 0;

    // tasks
    public int tasksNumberTotal;
    public int taskNumberDone;
    public TextMeshProUGUI taskText;

    // Door
    public GameObject door;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        timerText.text = ((int)(timerLength - time)).ToString();
        taskText.text = taskNumberDone.ToString() + "/" + tasksNumberTotal.ToString() + " tasks done";

        if (time >= timerLength)
        {
            characterInputs.hp = 0;
        }

        if (taskNumberDone == tasksNumberTotal)
        {
            NextRoom();
        }
    }

    private void NextRoom()
    {
        door.SetActive(false);
    }

    private void NextPlanet()
    {

    }
}
