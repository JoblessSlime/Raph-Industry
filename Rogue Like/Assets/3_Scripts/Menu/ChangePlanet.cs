using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangePlanet : MonoBehaviour
{
    [Header("---------- Scenes ----------")]
    public List<string> scenes;

    public GameObject Planet1;
    public GameObject Planet2;
    public GameObject Planet3;
    public GameObject Planet4;
    public GameObject Planet5;
    public GameObject Vaisseau;
    public Manager manager;
    public float step;

    private GameObject prevPlanet;
    private GameObject nextPlanet;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        switch (manager.actualPlanet)
        {
            case 0:
                prevPlanet = Planet1;
                break;
            case 1:
                prevPlanet = Planet2;
                break;
            case 2:
                prevPlanet = Planet3;
                break;
            case 3:
                prevPlanet = Planet4;
                break;
            case 4: 
                prevPlanet = Planet5;
                break;
        }

        int i = Random.Range(0, 4);
        while (manager.planetsDone.Contains(i))
        {
            i = Random.Range(0, 4);
        }

        switch (i)
        {
            case 0:
                nextPlanet = Planet1;
                break;
            case 1:
                nextPlanet = Planet2;
                break;
            case 2:
                nextPlanet = Planet3;
                break;
            case 3:
                nextPlanet = Planet4;
                break;
            case 4:
                nextPlanet = Planet5;
                break;
        }

        manager.actualPlanet = i;

        Vaisseau.transform.position = prevPlanet.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vaisseau.transform.position = Vector3.MoveTowards(Vaisseau.transform.position, nextPlanet.transform.position, step);
        if (Vector3.Distance(Vaisseau.transform.position, nextPlanet.transform.position) < 0.001f)
        {
            int sceneNumber = Random.Range(0, scenes.Count);
            int buildIndex = 0;
            while (scenes[sceneNumber] == SceneManager.GetActiveScene().name || buildIndex < 0)
            {
                sceneNumber = Random.Range(0, scenes.Count);
                buildIndex = SceneUtility.GetBuildIndexByScenePath(scenes[sceneNumber]);
            }
            SceneManager.LoadScene(scenes[sceneNumber]);
            SceneManager.LoadScene(scenes[sceneNumber]);
        }
    }
}
