using UnityEngine;
using UnityEngine.UI;

public class ChangeColor : MonoBehaviour
{
    public Image cristalImage;
    public Sprite spriteViolet;
    public Sprite spriteVert;
    public Color vert;
    public Color violet;
    public bool isViolet;

    private float timeToSwitch;
    private float timer;
    private float timerSwitching;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Random.Range(0,2) == 0)
        {
            isViolet = true;
        }
        else { isViolet = false; }

        if (isViolet)
        {
            cristalImage.sprite = spriteViolet;
            cristalImage.color = violet;
        }
        else
        {
            cristalImage.sprite = spriteVert;
            cristalImage.color = vert;
        }
        timeToSwitch = Random.Range(3.5f, 7f);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeToSwitch - 2f)
        {
            timerSwitching += Time.deltaTime;
            if (cristalImage.color == vert && timerSwitching > 0.25f)
            {
                timerSwitching = 0;
                cristalImage.color = violet;
            }
            else if (cristalImage.color == violet && timerSwitching > 0.25f)
            {
                timerSwitching = 0;
                cristalImage.color = vert;
            }
        }

        if (timer > timeToSwitch)
        {
            if (isViolet)
            {
                cristalImage.sprite = spriteVert;
                cristalImage.color = vert;
                isViolet = false;
            }
            else
            {
                cristalImage.sprite = spriteViolet;
                cristalImage.color = violet;
                isViolet = true;
            }
            timeToSwitch = Random.Range(2f, 5f);
            timer = 0;
            timerSwitching = 0;
        }
    }
}
