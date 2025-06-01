using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class PlaceTasks : MonoBehaviour
{
    public List<GameObject> list_tasks;
    public List<GameObject> list_tasksSpecific;
    public List<GameObject> Tasks_Spawn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        List<int> indexTaken = new List<int>();
        indexTaken.Add(0);
        indexTaken.Add(100);
        indexTaken.Add(100);

        int whileBreakCondition = 0;

        int index = 0;
        for (int i = 0; i < 3; i++)
        {
            whileBreakCondition = 0;
            while (indexTaken.Contains(index) && whileBreakCondition < 15)
            {
                index = Random.Range(0, list_tasks.Count - 1);
                whileBreakCondition ++;
            }
            indexTaken[i] = index;

            if (list_tasksSpecific.Contains(list_tasks[index]))
            {
                list_tasks[i].SetActive(true);
            }
            else
            {
                Instantiate(list_tasks[index], Tasks_Spawn[i].transform);
            }
        }
    }
}
