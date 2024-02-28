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
        //Se e quando l'oggetto viene distrutto, se questa ï¿½ l'istanza settata allora la rimuove
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
        MazeExploring       //Esplorazione del labirinto per liberare i granchi
    }
    // -------------------------------------------------------------------- //
    //Dichiarazione iniziale delle variabili
    private GameState currentState;

    private int currentPearls;
    private int reefHealth, pollution, biodiversity, oxygenLevel;
    private int pollutionChange, biodiversityChange, oxygenLevelChange, reefHealthChange;

    private GameObject[] corals;
    private Canvas canvas;
    
    private GameObject GameOverPanel, StatsPanel;
    private TextMeshProUGUI TitleText, CentralText, BottomText;
    private Button ReturnToMenuButton, WebsiteButton, SettingsButton;

    //private AudioManager audioManager;

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
        //audioManager = GetComponentInChildren<AudioManager>();
        //audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        // -------------------------------------------------------------------- //
        //Funzione temporanea per testare i parametri senza passare dal menù
        if (SceneManager.GetActiveScene().name == "GameScene")
            LoadGame();
        
    }
    public void LoadGame()
    {
        currentState = GameState.FreeRoaming;

        //TODO: temporaneo
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        GameOverPanel = canvas.transform.Find("GameOverPanel").gameObject;
        TitleText = GameOverPanel.transform.Find("TitleText").GetComponent<TextMeshProUGUI>();
        CentralText = GameOverPanel.transform.Find("CentralText").GetComponent<TextMeshProUGUI>();
        BottomText = GameOverPanel.transform.Find("BottomText").GetComponent<TextMeshProUGUI>();
        GameOverPanel.SetActive(false);
        TitleText.gameObject.SetActive(false);
        CentralText.gameObject.SetActive(false);
        BottomText.gameObject.SetActive(false);

        //TODO: cambia/sposta/rimuovi
        StatsPanel = canvas.transform.Find("InfoPanel").gameObject;
        StatsPanel.SetActive(false);

        ReturnToMenuButton = GameOverPanel.transform.Find("ReturnToMenuButton").GetComponent<Button>();
        WebsiteButton = GameOverPanel.transform.Find("WebsiteButton").GetComponent<Button>();
        ReturnToMenuButton.gameObject.SetActive(false);
        WebsiteButton.gameObject.SetActive(false);
        SettingsButton = canvas.transform.Find("SettingsButton").GetComponent<Button>();

        //SettingsButton.onClick.AddListener(OpenSettings);
        //ReturnToMenuButton.onClick.AddListener(LoadMenu);
        //WebsiteButton.onClick.AddListener(OpenURL);

        // -------------------------------------------------------------------- //
        //Impostazione dei valori iniziali di gioco
        corals = GameObject.FindGameObjectsWithTag("CoralSpot");
        setStartValues();
        // -------------------------------------------------------------------- //
        //Coroutine di fade out del loading screen
        StartCoroutine(FadeOutLoadingScreen());
        //Metodo che fa partire il ciclo di cambiamento dei parametri
        InvokeRepeating("tick", 0, 60);
    }

    public void tick()  //Funzione che viene chiamata una volta ogni minuto, e aggiorna i valori delle statistiche di gioco
    {
        updateValues();
        // -------------------------------------------------------------------- //
        //Controlla tutti gli oggetti con tag "Corallo", e li mette nell'array

        if (corals.Length != 0)
            Array.Clear(corals, 0, corals.Length);

        corals = GameObject.FindGameObjectsWithTag("CoralSpot");
    }
    // -------------------------------------------------------------------- //
    //Metodi per interagire con il GameState; usati dai minigiochi
    public void setGameState(GameState newState)
    {
        Debug.Log("Changed GameState to: " + newState);
        currentState = newState;
        if (newState == GameState.FreeRoaming)
        {
            MinigameInterface.endMinigame();
        }
        else
        {
            DialogueInterface.toggleDialoguePanel(false);
            MinigameInterface.startMinigame();
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
            Victory();
        }
        else if (reefHealth <= 0)
        {
            reefHealth = 0;
            GameOver();
        }

        //Aggiorna l'interfaccia
        BarsInterface.updateBars();
        BarsInterface.updateArrows();
    }
    //Coroutine di fade out del loading screen
    IEnumerator FadeOutLoadingScreen()
    {
        Image LoadingPanelImage = canvas.transform.Find("LoadingPanel").GetComponent<Image>();
        TextMeshProUGUI LoadingTextImage = canvas.transform.Find("LoadingPanel/LoadingText").GetComponent<TextMeshProUGUI>();
        float fadeAmount;
        Color nextPanelColor, nextTextColor;
        yield return new WaitForSeconds(1);
        while (LoadingPanelImage.color.a > 0)
        {
            fadeAmount = LoadingPanelImage.color.a - (Time.deltaTime * 0.5f);
            nextPanelColor = new Color(LoadingPanelImage.color.r, LoadingPanelImage.color.g, LoadingPanelImage.color.b, fadeAmount);
            nextTextColor = new Color(LoadingTextImage.color.r, LoadingTextImage.color.g, LoadingTextImage.color.b, fadeAmount);
            LoadingPanelImage.color = nextPanelColor;
            LoadingTextImage.color = nextTextColor;
            yield return new WaitForSeconds(0.01f);
        }
        LoadingPanelImage.gameObject.SetActive(false);
    }
    // -------------------------------------------------------------------- //
    //Metodi di GameOver e Victory
    public void GameOver()
    {
        TitleText.SetText("GAME OVER!");
        CentralText.SetText("Le condizioni dell'ambiente si sono deteriorate, e i coralli stanno iniziando a perdere colore... " +
            "Dovremo riprovare da un'altra parte.");
        GameOverPanel.SetActive(true);
        StartCoroutine(FadeIn(GameOverPanel.GetComponent<Image>()));
    }
    public void Victory()
    {
        TitleText.SetText("VITTORIA!");
        CentralText.SetText("Grazie ai tuoi sforzi, le condizioni dell'ambiente sono stabili e in grado di prosperare." +
            "Questa barriera corallina e' salva!");
        GameOverPanel.SetActive(true);
        StartCoroutine(FadeIn(GameOverPanel.GetComponent<Image>()));
    }
    // -------------------------------------------------------------------- //
    //Coroutine per la comparsa del pannello e delle scritte
    IEnumerator FadeIn(Image GameOverPanel)
    {
        float fadeAmount;
        Color nextFrameColor;
        while (GameOverPanel.color.a < 1)
        {
            fadeAmount = GameOverPanel.color.a + ( Time.deltaTime);
            nextFrameColor = new Color(GameOverPanel.color.r, GameOverPanel.color.g, GameOverPanel.color.b, fadeAmount);
            GameOverPanel.color = nextFrameColor;
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1);
        TitleText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        CentralText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        BottomText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        WebsiteButton.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        ReturnToMenuButton.gameObject.SetActive(true);
        

    }
    // -------------------------------------------------------------------- //
    //Listener per i bottoni della schermata finale (apri sito web e torna al menu')
    public void OpenURL()
    {
        //audioManager.PlaySFX(audioManager.selection);
        Application.OpenURL("https://coralreefrescueinitiative.org/");
    }
    public void LoadMenu()
    {
        CancelInvoke();
        //audioManager.PlaySFX(audioManager.selection);
        SceneManager.LoadScene("Simone_Menu_Iniziale");
    }

    public void OpenSettings()
    {
        //audioManager.PlaySFX(audioManager.selection);
        SceneManager.LoadScene("Simone_Impostazioni", LoadSceneMode.Additive);
    }

    public void StatsOpenAndClose(GameObject obj)
    {
        //audioManager.PlaySFX(audioManager.selection);
        obj.SetActive(!obj.activeSelf);
    }
    // -------------------------------------------------------------------- //
    //Metodi get e set per i parametri
    public void addPearls(int value)
    {
        currentPearls += value;
        BarsInterface.updatePearls();
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
        BarsInterface.updateBars();
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

        BarsInterface.updateArrows();
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
persone alla volta… */