using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Coral", menuName = "Coral", order = 1)]
public class CoralSO : ScriptableObject
{
    public int pollutionChange, oxygenLevelChange, biodiversityChange, cost;
    public string coralName, coralDesc;

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

    public void Print()
        {
            Debug.Log(pollutionChange + ": " + oxygenLevelChange + ": " + biodiversityChange + ": " + cost + "The card cost: ");
        }

}
