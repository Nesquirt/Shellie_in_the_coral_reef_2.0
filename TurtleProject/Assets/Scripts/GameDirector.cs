using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDirector : MonoBehaviour
{

    private int reefHealth, pollution, temperature, oxygenLevel;
    private int pollutionChange, temperatureChange, oxygenLevelChange;
    public enum gameState
    {
        Intro,
        FreeRoam,
        ObstacleRace
    }

    public gameState currentGameState;

    private void Awake()
    {
        currentGameState = gameState.Intro;
        reefHealth = 50;
        pollution = 50;
        temperature = 24;
        oxygenLevel = 50;
    }

    public void tick()
    {
        // -------------------------------------------------------------------- //
        //Calcolo del cambio dei parametri
        pollution += pollutionChange;       //Calcolo del cambio di inquinamento
        pollutionChange = +5;               //resetta il valore di cambio di inquinamento per il prossimo tick.

        temperature += temperatureChange;   //Calcolo del cambio di temperatura
        System.Random random = new System.Random();
        if (random.Next() % 2 == 0)         //resetta il valore di cambio di temperatura per il prossimo tick;                               
            temperatureChange = 0;          //ha un 50% di possibilità di aumentare la temperatura di un grado.
        else
            temperatureChange = 1;     

        oxygenLevel += oxygenLevelChange;   //Calcolo del cambio di livello di ossigeno
        oxygenLevelChange = -5;             //resetta il valore di cambio del livello di ossigeno per il prossimo tick.
        // -------------------------------------------------------------------- //
        //Calcolo del cambio di vita della barriera corallina
        if (reefHealth >= 0 && reefHealth <= 100)
        {
            reefHealth += ((pollution / 10) - 4) + (-Mathf.Abs(temperature-24)+5)+ ((oxygenLevel / 10) - 4);
        }
        else if (reefHealth >= 100)
            reefHealth = 100;
        else if(reefHealth<=0)
        {
            //TODO: gameOver
        }
        // -------------------------------------------------------------------- //
    }


}
