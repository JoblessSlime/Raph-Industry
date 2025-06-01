using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Incubation : MonoBehaviour
{
    public Image incubation_slider;
    public Image incubation_sliderCanvas2;
    public TextMeshProUGUI incubation_timer;
    public float incubation_time;
    private float time_incubating;
    private bool incubation_started = false;
    private bool hasIncubated;



    public CharacterInputs characterInputs;
    public AccessTask accessTask;
    public GameManager gameManager;
    public GameObject TaskPanel;

    public bool taskKills;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        incubation_timer.text = ((int)incubation_time).ToString();

        gameManager = GameObject.FindFirstObjectByType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(incubation_started)
        {
            time_incubating += Time.deltaTime;
            incubation_slider.fillAmount = Mathf.Clamp01(1 - (time_incubating / incubation_time));
            incubation_sliderCanvas2.fillAmount = Mathf.Clamp01(1 - (time_incubating / incubation_time));
            incubation_timer.text = ((int)(incubation_time - time_incubating)).ToString();
            if (time_incubating >= incubation_time)
            {
                hasIncubated = true;
            }
        }
    }

    public void LaunchIncubation()
    {
        Debug.Log("Incubation started");
        incubation_started = !incubation_started;
        if(hasIncubated)
        {
            Win();
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
        Cursor.visible = false;
    }

    void Win()
    {
        gameManager.taskNumberDone += 1;
        accessTask.TaskFinished();
        TaskPanel.SetActive(false);
        Cursor.visible = false;
    }
}
