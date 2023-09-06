using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Coral", menuName = "Coral/Plant")]
public class CoralStatsSO : ScriptableObject
{
    [SerializeField] private int pollutionChange, oxygenLevelChange, biodiversityChange, cost;
    /*
    public void Awake()
    {
        //Inizializzazione di variabili di default; vengono modificate nell'inspector
        pollutionChange = 0;
        oxygenLevelChange = 0;
        biodiversityChange = 0;
    }
    */
    public int getPollutionChange()
    {
        return pollutionChange;
    }

    public int getOxygenLevelChange()
    {
        return oxygenLevelChange;
    }

    public int getBiodiversityChange()
    {
        return biodiversityChange;
    }

    public int getCost()
    {
        return cost;
    }

}
