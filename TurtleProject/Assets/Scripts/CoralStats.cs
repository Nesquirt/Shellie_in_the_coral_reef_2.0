using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoralStats : MonoBehaviour
{
    [SerializeField] private int pollutionChange, oxygenLevelChange, biodiversityChange;
    public void Awake()
    {
        //Inizializzazione di variabili di default; vengono modificate nell'inspector
        pollutionChange = 0;
        oxygenLevelChange = 0;
        biodiversityChange = 0;
    }

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

}
