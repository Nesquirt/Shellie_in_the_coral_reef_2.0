using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameDirector : MonoBehaviour
{
    // -------------------------------------------------------------------- //
    //Creazione dell'istanza del Singleton
    private static GameDirector _instance;
    public static GameDirector Instance
    {
        get { CreateOrGetInstance();
            return _instance; }
        protected set { _instance = value; }
    }
    static void CreateOrGetInstance()
    {
        if(_instance == null)
        {
            _instance = FindObjectOfType<GameDirector>(); //Cerca nella scena se esiste un oggetto di tipo GameDirector
            if(_instance == null)
            {
                GameObject newObj = new GameObject("Director"); //Se non la trova, crea il singleton
                newObj.AddComponent<GameDirector>();
                _instance = newObj.GetComponent<GameDirector>();
                DontDestroyOnLoad(newObj);
            }
        }
    }
    private void OnDestroy()
    {
        //Se e quando l'oggetto viene distrutto, se questa è l'istanza settata allora la rimuove
        if(_instance == this)
        {
            Instance = null;
        }
    }
    // -------------------------------------------------------------------- //
    //Creazione dell'enum per gli stati del gioco
    public enum GameState
    {
        FreeRoaming,        //Non attualmente in un minigioco
        ObstacleCourse,     //Percorso ad ostacoli
        TrashCollecting,    //Raccolta della spazzatura nella rete
        MazeExploring       //Esplorazione del labirinto per liberare i granchi
    }
    // -------------------------------------------------------------------- //
    //Dichiarazione iniziale delle variabili
    private GameState currentState;

    private int currentPearls;
    private int reefHealth, pollution, biodiversity, oxygenLevel;
    private int pollutionChange, biodiversityChange, oxygenLevelChange, reefHealthChange;
    private GameObject[] corals;
    //TODO: rendi questi elementi dell'UI private e ricavali nell'awake con GameObject.Find
    public Slider reefHealthSlider, pollutionSlider, biodiversitySlider, oxygenLevelSlider;
    public Image reefHealthArrow, pollutionArrow, biodiversityArrow, oxygenLevelArrow;  //TODO: aggiungi un'immagine per quando il cambiamento di parametri è 0
    public Sprite upArrow, downArrow;
    
    public GameObject currentCoralSpot;  //Questa variabile comunica con CoralHandler per ricordare su quale roccia si sta piantando i coralli
    // -------------------------------------------------------------------- //
    private void Awake()
    {
        // -------------------------------------------------------------------- //
        //Inizializzazione del Singleton
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(Instance != null && Instance != this)
        {
            Destroy(gameObject); //Se esiste già un'altra istanza, distrugge questo oggetto per evitare duplicati dell'istanza
        }
        // -------------------------------------------------------------------- //
        //Impostazione dei valori iniziali di gioco
        currentState = GameState.FreeRoaming;

        currentPearls = 100;

        reefHealth = 50;
        pollution = 20;
        biodiversity = 24;
        oxygenLevel = 50;

        pollutionChange = +5;
        biodiversityChange = -4;
        oxygenLevelChange = -3;

        corals = GameObject.FindGameObjectsWithTag("CoralSpot");

        //Metodo che fa partire il ciclo di cambiamento dei parametri
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

        //Metodo per cambiare il colore della nebbia
        float pollutionPercentage = (float)pollution / 100;
        Color32 CleanWater = new Color32(114, 205, 231, 255);
        Debug.Log(CleanWater);
        Color32 PollutedWater = new Color32(114, 200, 186, 255);
        RenderSettings.fogColor = Color.LerpUnclamped(CleanWater, PollutedWater, pollutionPercentage);

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
            reefHealthChange = (4 - (pollution / 10)) + ((biodiversity / 10) - 4) + ((oxygenLevel / 10) - 4);
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

    // -------------------------------------------------------------------- //
    //Metodi per modificare la velocità dei parametri; usati dai coralli
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
    // -------------------------------------------------------------------- //
    //Metodi per modificare direttamente i parametri; usati dai minigiochi
    public void addPearls(int value)
    {
        currentPearls += value;
        GameObject.Find("Canvas/PearlsText").gameObject.GetComponent<TextMeshProUGUI>().SetText("Perle: " + getCurrentPearls());
    }
    public void addPollution(int value)
    {
        pollution += value;
        pollutionSlider.value = pollution;
    }
    public void addOxygenLevel(int value)
    {
        oxygenLevel += value;
        oxygenLevelSlider.value = oxygenLevel;
    }
    public void addBiodiversity(int value)
    {
        biodiversity += value;
        biodiversitySlider.value = biodiversity;
    }

    public int getCurrentPearls()
    {
        return currentPearls;
    }
    public int getPollution()
    {
        return pollution;
    }
    public int getOxygenLevel()
    {
        return oxygenLevel;
    }
    public int getBiodiversity()
    {
        return biodiversity;
    }
    // -------------------------------------------------------------------- //
    //Metodi per interagire con il GameState; usati dai minigiochi
    public void setGameState(GameState newState)
    {
        currentState = newState;
    }

    public GameState getGameState()
    {
        return currentState;
    }
    // -------------------------------------------------------------------- //

}
