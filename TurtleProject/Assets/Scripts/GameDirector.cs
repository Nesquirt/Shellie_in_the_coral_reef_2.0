using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{

    private int reefHealth, pollution, biodiversity, oxygenLevel;
    private int pollutionChange, biodiversityChange, oxygenLevelChange, reefHealthChange;
    private GameObject[] corals;
    public Slider reefHealthSlider, pollutionSlider, biodiversitySlider, oxygenLevelSlider;
    public Image reefHealthArrow, pollutionArrow, biodiversityArrow, oxygenLevelArrow;
    //TODO: aggiungi un'immagine per quando il cambiamento di parametri è 0

    public Sprite upArrow, downArrow;
    
    public GameObject currentCoralSpot;
    private void Awake()
    {
        reefHealth = 50;
        pollution = 70;
        biodiversity = 24;
        oxygenLevel = 50;

        pollutionChange = +5;
        biodiversityChange = -4;
        oxygenLevelChange = -3;

        corals = GameObject.FindGameObjectsWithTag("CoralSpot");

        InvokeRepeating("tick", 0, 10);
    }

    public void tick()                                          //Funzione che viene chiamata una volta ogni minuto, e aggiorna i valori delle statistiche di gioco
    {
        // -------------------------------------------------------------------- //
        //Controlla tutti gli oggetti con tag "Corallo", e li mette nell'array

        if (corals.Length != 0)
            Array.Clear(corals, 0, corals.Length);

        corals = GameObject.FindGameObjectsWithTag("CoralSpot");
        // -------------------------------------------------------------------- //
        //Calcolo del cambio dei parametri

        //pollution += CalculatePollutionChange();               //Calcolo del cambio di inquinamento
        pollution += pollutionChange;
        pollutionSlider.value = pollution;

        //biodiversity += CalculateBiodiversityChange();         //Calcolo del cambio di biodiversità
        biodiversity += biodiversityChange;
        biodiversitySlider.value = biodiversity;

        //oxygenLevel += CalculateOxygenLevelChange();           //Calcolo del cambio di livello di ossigeno
        oxygenLevel += oxygenLevelChange;
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
            reefHealthChange = ((pollution / 10) - 4) + ((biodiversity / 10) - 4) + ((oxygenLevel / 10) - 4);
            reefHealth += reefHealthChange;
            if (reefHealthChange >= 0)
                reefHealthArrow.sprite = upArrow;
            else
                reefHealthArrow.sprite = downArrow;
            
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

    public void modifyPollutionChange(int value)
    {
        pollutionChange += value;
        if (pollutionChange >= 0)
            pollutionArrow.sprite = downArrow;
        else
            pollutionArrow.sprite = upArrow;
    }
    public void modifyBiodiversityChange(int value)
    {
        biodiversityChange += value;
        if (biodiversityChange >= 0)
            biodiversityArrow.sprite = upArrow;
        else
            biodiversityArrow.sprite = downArrow;
    }
    public void modifyOxygenLevelChange(int value)
    {
        oxygenLevelChange += value;
        if (oxygenLevelChange >= 0)
            oxygenLevelArrow.sprite = upArrow;
        else
            oxygenLevelArrow.sprite = downArrow;
    }

    /*
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
    */

}
