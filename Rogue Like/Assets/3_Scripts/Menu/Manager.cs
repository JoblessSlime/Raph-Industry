using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Manager", menuName = "Scriptable Objects/Manager")]
public class Manager : ScriptableObject
{
    public int actualPlanet;
    public int NumberOfDaysPassed;
    public int NumberOfRooms;
    public int NumberOfRoomsPassed;
    public float InitialTime;
    public float TimePassed;
    public List<int> planetsDone;
}
