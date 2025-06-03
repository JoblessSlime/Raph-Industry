using UnityEngine;

public class AmeliorationPanel : MonoBehaviour
{
    public Manager Manager;
    public GameObject Amelioration;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(Manager.planetsDone.Count == 0)
        {
            Amelioration.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
