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
                GameObject newObj = new GameObject("Director"); //Se non la trova, crea il singleton
                newObj.AddComponent<GameDirector>();
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
            Instance = null;
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

    private Slider reefHealthSlider, pollutionSlider, biodiversitySlider, oxygenLevelSlider;
    private Image reefHealthArrow, pollutionArrow, biodiversityArrow, oxygenLevelArrow;  //TODO: aggiungi un'immagine per quando il cambiamento di parametri � 0
    private Sprite upArrow, downArrow;
    private GameObject GameOverPanel;
    private TextMeshProUGUI TitleText, CentralText, BottomText;
    private Button ReturnToMenuButton, WebsiteButton;

    public GameObject currentCoralSpot;  //Questa variabile comunica con CoralHandler per ricordare su quale roccia si sta piantando i coralli
    // -------------------------------------------------------------------- //
    private void Awake()
    {
        // -------------------------------------------------------------------- //
        //Inizializzazione del Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != null && Instance != this)
        {
            Destroy(gameObject); //Se esiste gi� un'altra istanza, distrugge questo oggetto per evitare duplicati dell'istanza
        }
        // -------------------------------------------------------------------- //

    }
    public void LoadGame()
    {
        if(SceneManager.GetActiveScene().name != "GameScene")
        {
            Debug.Log("Chiamata funzione LoadGame dal men�");
        }
        //Nella scena di gioco, prende dalla hierarchy tutti gli elementi dell'interfaccia
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        reefHealthSlider = canvas.transform.Find("BarsPanel/ReefHealthBar").GetComponent<Slider>();
        pollutionSlider = canvas.transform.Find("BarsPanel/PollutionBar").GetComponent<Slider>();
        biodiversitySlider = canvas.transform.Find("BarsPanel/BiodiversityBar").GetComponent<Slider>();
        oxygenLevelSlider = canvas.transform.Find("BarsPanel/OxygenLevelBar").GetComponent<Slider>();

        reefHealthArrow = canvas.transform.Find("BarsPanel/ReefHealthBar/ReefHealthArrow").GetComponent<Image>();
        pollutionArrow = canvas.transform.Find("BarsPanel/PollutionBar/PollutionArrow").GetComponent<Image>();
        biodiversityArrow = canvas.transform.Find("BarsPanel/BiodiversityBar/BiodiversityArrow").GetComponent<Image>();
        oxygenLevelArrow = canvas.transform.Find("BarsPanel/OxygenLevelBar/OxygenLevelArrow").GetComponent<Image>();

        GameOverPanel = canvas.transform.Find("GameOverPanel").gameObject;
        TitleText = GameOverPanel.transform.Find("TitleText").GetComponent<TextMeshProUGUI>();
        CentralText = GameOverPanel.transform.Find("CentralText").GetComponent<TextMeshProUGUI>();
        BottomText = GameOverPanel.transform.Find("BottomText").GetComponent<TextMeshProUGUI>();
        GameOverPanel.SetActive(false);
        TitleText.gameObject.SetActive(false);
        CentralText.gameObject.SetActive(false);
        BottomText.gameObject.SetActive(false);

        ReturnToMenuButton = GameOverPanel.transform.Find("ReturnToMenuButton").GetComponent<Button>();
        WebsiteButton = GameOverPanel.transform.Find("WebsiteButton").GetComponent<Button>();
        ReturnToMenuButton.gameObject.SetActive(false);
        WebsiteButton.gameObject.SetActive(false);

        ReturnToMenuButton.onClick.AddListener(LoadMenu);  //TODO: collega alla scena men�

        WebsiteButton.onClick.AddListener(OpenURL);

        upArrow = Resources.Load<Sprite>("Sprites/UpArrow");
        downArrow = Resources.Load<Sprite>("Sprites/DownArrow");
        // -------------------------------------------------------------------- //
        //Impostazione dei valori iniziali di gioco
        currentState = GameState.FreeRoaming;

        currentPearls = 0;

        reefHealth = 50;
        pollution = 20;
        biodiversity = 24;
        oxygenLevel = 50;

        pollutionChange = +5;
        biodiversityChange = -4;
        oxygenLevelChange = -3;

        corals = GameObject.FindGameObjectsWithTag("CoralSpot");

        //Metodo che fa partire il ciclo di cambiamento dei parametri
        InvokeRepeating("tick", 0, 30);
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
        Color32 PollutedWater = new Color32(114, 200, 186, 255);
        RenderSettings.fogColor = Color.LerpUnclamped(CleanWater, PollutedWater, pollutionPercentage);

        //biodiversity += CalculateBiodiversityChange();         //Calcolo del cambio di biodiversit�
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
        {
            reefHealth = 100;
            Victory();
        }
        else if (reefHealth <= 0)
        {
            reefHealth = 0;
            GameOver();
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
    //Metodi per modificare la velocit� dei parametri; usati dai coralli
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
        StartCoroutine(setGameStateCR(newState));
    }

    public GameState getGameState()
    {
        return currentState;
    }
    // -------------------------------------------------------------------- //
    //Coroutine per far attendere le chiamate di getGameState e setGameState
    IEnumerator setGameStateCR(GameState newState)
    {
        yield return new WaitForFixedUpdate();
        Debug.Log("Changed GameState to: " + newState);
        currentState = newState;
    }
    IEnumerator getGameStateCR()
    {
        yield return new WaitForFixedUpdate();
        yield return currentState;
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
            "Questa barriera corallina � salva!");
        GameOverPanel.SetActive(true);
        StartCoroutine(FadeIn(GameOverPanel.GetComponent<Image>()));
    }
    //Coroutine per la comparsa del pannello e delle scritte
    IEnumerator FadeIn(Image GameOverPanel)
    {
        float fadeAmount;
        Color nextFrameColor;
        while (GameOverPanel.color.a < 1)
        {
            fadeAmount = GameOverPanel.color.a + (2f * Time.deltaTime);
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
    //Listener per i bottoni della schermata finale (apri sito web e torna al men�)
    public void OpenURL()
    {
        Application.OpenURL("https://coralreefrescueinitiative.org/");
    }
    public void LoadMenu()
    {
        SceneManager.LoadScene("Simone_Menu_Iniziale");
    }
    
}
