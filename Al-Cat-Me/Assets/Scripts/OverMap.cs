using UnityEngine;
using System.Collections.Generic;

public class OverMap : MonoBehaviour
{
    public GameController gameController;
    public Combat combatEncounter;
    public Shop shopEncounter;
    public Craft craftEncounter;
    public Surprise surpriseEncounter;
    List<Encounter> encounters;
    List<Encounter> selectableEncounters;
    int selectedEncounter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        selectedEncounter = -1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void selectEncounter(int encounter)
    {
        selectedEncounter = encounter;
        selectableEncounters[encounter].startEncounter();
    }

    public void finishEncounter()
    {
        selectableEncounters[selectedEncounter].endEncounter();
        selectedEncounter = -1;
    }
}
