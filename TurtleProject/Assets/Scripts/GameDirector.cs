using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{

    private int reefHealth, pollution, biodiversity, oxygenLevel;

    private GameObject[] corals;
    public Slider reefHealthSlider, pollutionSlider, biodiversitySlider, oxygenLevelSlider;
    public enum gameState
    {
        Intro,
        FreeRoam,
        ObstacleRace,
        TrashGathering
    }

    public gameState currentGameState;

    private void Awake()
    {
        currentGameState = gameState.Intro;
        reefHealth = 50;
        pollution = 70;
        biodiversity = 24;
        oxygenLevel = 50;

        corals = GameObject.FindGameObjectsWithTag("Corallo");

        InvokeRepeating("tick", 0, 3);
    }

    public void tick()                                          //Funzione che viene chiamata una volta ogni minuto, e aggiorna i valori delle statistiche di gioco
    {
        
        //Controlla tutti gli oggetti con tag "Corallo", e li mette nell'array
        if(corals.Length != 0)
            Array.Clear(corals, 0, corals.Length);
        corals = GameObject.FindGameObjectsWithTag("Corallo");
        // -------------------------------------------------------------------- //
        //Calcolo del cambio dei parametri

        pollution += CalculatePollutionChange();               //Calcolo del cambio di inquinamento
        pollutionSlider.value = pollution;

        biodiversity += CalculateBiodiversityChange();         //Calcolo del cambio di biodiversità
        biodiversitySlider.value = biodiversity;

        oxygenLevel += CalculateOxygenLevelChange();           //Calcolo del cambio di livello di ossigeno
        oxygenLevelSlider.value = oxygenLevel;

        if (pollution < 0)
            pollution = 0;
        if (pollution > 100)
            pollution = 100;

        if (biodiversity < 0)
            biodiversity = 0;
        if (biodiversity > 100)
            biodiversity = 100;

        if (oxygenLevel < 0)
            oxygenLevel = 0;
        if (oxygenLevel > 100)
            oxygenLevel = 100;
        // -------------------------------------------------------------------- //
        //Calcolo del cambio di vita della barriera corallina
        if (reefHealth >= 0 && reefHealth <= 100)
        {
            reefHealth += ((pollution / 10) - 4) + (-Mathf.Abs(biodiversity-24)+5)+ ((oxygenLevel / 10) - 4);
            //
        }
        else if (reefHealth >= 100)
            reefHealth = 100;
        else if(reefHealth<=0)
        {
            reefHealth = 0;
            //TODO: gameOver
        }

        reefHealthSlider.value = reefHealth;
        // -------------------------------------------------------------------- //

        /*
        Debug.Log("Tick");
        Debug.Log("Biodiversity: " + biodiversity);
        Debug.Log("Pollution: " + pollution);
        Debug.Log("Oxygen Level: " + oxygenLevel);
        Debug.Log("Total Reef Health: " + reefHealth);
        */
    }

    public int CalculatePollutionChange()
    {
        int totalChange = +5;   //valore di default se non ci sono coralli
        foreach (GameObject coral in corals)
        {
            //totalChange += coral.GetComponent<CoralStats>().getPollutionChange();
        }
        return totalChange;
    }

    public int CalculateBiodiversityChange()
    {
        int totalChange = -3;   //valore di default se non ci sono coralli
        foreach (GameObject coral in corals)
        {
            //totalChange += coral.GetComponent<CoralStats>().getBiodiversityChange();
        }
        return totalChange;
    }

    public int CalculateOxygenLevelChange()
    {
        int totalChange = -4;   //valore di default se non ci sono coralli
        foreach (GameObject coral in corals)
        {
            //totalChange += coral.GetComponent<CoralStats>().getOxygenLevelChange();
        }
        return totalChange;
    }


}
