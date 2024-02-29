using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class GameDirector : MonoBehaviour
{
    // -------------------------------------------------------------------- //
    //Creazione dell'istanza del Singleton
    private static GameDirector _instance;
    public static GameDirector Instance
    {
        get
        {
            CreateOrGetInstance();
            return _instance;
        }
        protected set { _instance = value; }
    }
    static void CreateOrGetInstance()
    {
        if (_instance == null)
        {
            _instance = FindObjectOfType<GameDirector>(); //Cerca nella scena se esiste un oggetto di tipo GameDirector
            if (_instance == null)
            {
                GameObject newObj = Instantiate<GameObject>(Resources.Load("Director") as GameObject);
                //GameObject newObj = new GameObject("Director"); //Se non la trova, crea il singleton
                _instance = newObj.GetComponent<GameDirector>();
                DontDestroyOnLoad(newObj);
            }
        }
    }
    private void OnDestroy()
    {
        //Se e quando l'oggetto viene distrutto, se questa � l'istanza settata allora la rimuove
        if (_instance == this)
        {
            _instance = null;
        }
    }
    protected GameDirector() { } //Costruttore protected per non permettere l'inizializzazione fuori dalla gerarchia
    // -------------------------------------------------------------------- //
    //Creazione dell'enum per gli stati del gioco
    public enum GameState
    {
        FreeRoaming,        //Non attualmente in un minigioco
        ObstacleCourse,     //Percorso ad ostacoli
        TrashCollecting,    //Raccolta della spazzatura nella rete
        MazeExploring,      //Esplorazione del labirinto per liberare i granchi
        SummoningRitual     //Rituale d'evocazione
    }
    // -------------------------------------------------------------------- //
    //Dichiarazione iniziale delle variabili
    private GameState currentState;

    private int currentPearls;
    private int reefHealth, pollution, biodiversity, oxygenLevel;
    private int pollutionChange, biodiversityChange, oxygenLevelChange, reefHealthChange;

    public AudioManager audioManager;
    private UIManager UImanager;

    public GameObject currentCoralSpot;  //Questa variabile comunica con CoralHandler per ricordare su quale roccia si sta piantando i coralli
    // -------------------------------------------------------------------- //
    private void Awake()
    {
        // -------------------------------------------------------------------- //
        //Inizializzazione del Singleton
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject); //Se esiste gia' un'altra istanza, distrugge questo oggetto per evitare duplicati dell'istanza
        }
        audioManager = GetComponentInChildren<AudioManager>();
        //audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        // -------------------------------------------------------------------- //
        //Funzione temporanea per testare i parametri senza passare dal men�
        if (SceneManager.GetActiveScene().name == "GameScene")
            LoadGame();
        
    }
    public void LoadGame()
    {
        currentState = GameState.FreeRoaming;

        UImanager = GameObject.Find("Canvas").GetComponent<UIManager>();
        // -------------------------------------------------------------------- //
        //Impostazione dei valori iniziali di gioco
        setStartValues();
        // -------------------------------------------------------------------- //
        //Coroutine di fade out del loading screen
        UImanager.gameOverInterface.fadeOutLoadingScreen();
        //Metodo che fa partire il ciclo di cambiamento dei parametri
        InvokeRepeating("tick", 0, 60);

        //TEMPORANEO; PER TESTING DELLA SCHERMATA DI GAMEOVER
        //gameOverInterface.GameOver(true);
    }

    public void tick()  //Funzione che viene chiamata una volta ogni minuto, e aggiorna i valori delle statistiche di gioco
    {
        updateValues();
    }
    // -------------------------------------------------------------------- //
    //Metodi per interagire con il GameState; usati dai minigiochi
    public void setGameState(GameState newState)
    {
        Debug.Log("Changed GameState to: " + newState);
        currentState = newState;
        if (newState == GameState.FreeRoaming || newState == GameState.SummoningRitual)
        {
            //Riattiva l'outline dei coralSpots
            UImanager.toggleOutline(true);
            UImanager.minigameInterface.endMinigame();
        }
        else
        {
            //Disattiva l'outline dei coralSpots mentre si è in un minigioco
            UImanager.toggleOutline(false);
            UImanager.dialogueInterface.toggleDialoguePanel(false);
            UImanager.minigameInterface.startMinigame();
        }

    }

    public GameState getGameState()
    {
        return currentState;
    }
    // -------------------------------------------------------------------- //
    //Funzioni di gestione dei parametri di gioco
    public void setStartValues()
    {
        currentPearls = 10;
        reefHealth = 50;
        pollution = 20;
        biodiversity = 60;
        oxygenLevel = 60;

        pollutionChange = +5;
        biodiversityChange = -5;
        oxygenLevelChange = -5;
    }
    public void updateValues()
    {
        pollution += pollutionChange;
        biodiversity += biodiversityChange;
        oxygenLevel += oxygenLevelChange;

        //Metodo per cambiare il colore della nebbia
        float pollutionPercentage = (float)pollution / 100;
        Color32 CleanWater = new Color32(114, 205, 231, 255);
        Color32 PollutedWater = new Color32(114, 200, 186, 255);
        RenderSettings.fogColor = Color.LerpUnclamped(CleanWater, PollutedWater, pollutionPercentage);

        //Standardizzazione valori
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

        //Vita della barriera corallina
        if (reefHealth >= 0 && reefHealth <= 100)
        {
            reefHealthChange = (4 - (pollution / 10)) + ((biodiversity / 10) - 4) + ((oxygenLevel / 10) - 4);
            reefHealth += reefHealthChange;
        }
        else if (reefHealth >= 100)
        {
            reefHealth = 100;
            UImanager.gameOverInterface.GameOver(true);
        }
        else if (reefHealth <= 0)
        {
            reefHealth = 0;
            UImanager.gameOverInterface.GameOver(false);
        }

        //Aggiorna l'interfaccia
        UImanager.barsInterface.updateBars();
        UImanager.barsInterface.updateArrows();
    }

    // -------------------------------------------------------------------- //
    //Metodi get e set per i parametri
    public void addPearls(int value)
    {
        currentPearls += value;
        UImanager.barsInterface.updatePearls();
    }
    public int getCurrentPearls()
    {
        return currentPearls;
    }
    public void addParameters(int sumPollution, int sumOxygen, int sumBio)
    {
        pollution += sumPollution;
        oxygenLevel += sumOxygen;
        biodiversity += sumBio;
        UImanager.barsInterface.updateBars();
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
    public int getReefHealth()
    {
        return reefHealth;
    }
    public int getReefHealthChange()
    {
        return reefHealthChange;
    }
    public void modifyParameterChanges(int sumPollutionChange, int sumBioChange, int sumOxygenChange)
    {
        pollutionChange += sumPollutionChange;
        biodiversityChange += sumBioChange;
        oxygenLevelChange += sumOxygenChange;

        UImanager.barsInterface.updateArrows();
    }
    public int getPollutionChange()
    {
        return pollutionChange;
    }
    public int getBiodiversityChange()
    {
        return biodiversityChange;
    }
    public int getOxygenChange()
    {
        return oxygenLevelChange;
    }

}


/*Dopo i Serial Killer.....
ora ci sono pure i Parallel Killer, assassini tecnologici che uccidono 8
persone alla volta� */