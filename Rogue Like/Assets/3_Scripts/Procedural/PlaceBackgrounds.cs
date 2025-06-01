using System.Collections.Generic;
using UnityEngine;

public class PlaceBackgrounds : MonoBehaviour
{
    public List<GameObject> planet_backgrounds;
    public Manager manager_scriptable;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        planet_backgrounds[manager_scriptable.actualPlanet].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
