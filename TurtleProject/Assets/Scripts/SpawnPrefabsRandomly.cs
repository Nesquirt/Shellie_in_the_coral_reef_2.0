using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
public class SpawnPrefabsRandomly : MonoBehaviour
{

    [SerializeField] private GameObject prefabToSpawn1;
    [SerializeField] private GameObject prefabToSpawn2;
    [SerializeField] private GameObject prefabToSpawn3;
    [SerializeField] private GameObject sostegno;
    [SerializeField] private GameObject rete;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float totalTime;

    private float spawnAreaWidth = 50f;
    private float spawnAreaLength = 50f;
    private int numerosacchettidaspawnare = 5;
    private float originepianox, originepianoz, totalTime2, currentTime, currentTime2;
    private float lunghezzapiano = 30f;
    private float larghezzapiano = 30f;
    private int oggettiNelPiano, a, b, rifiutiraccolti;
    private List<GameObject> spazzature = new List<GameObject>();
    private float upwardForce = 2f;
    private Image img;
    private Transform StefanoPrompt;
    private TextMeshProUGUI NPCName, dialogueText, rewardsText, testocontatore, testocronometro, testo5;
    //private Button confirmButton, cancelButton;
    private GameObject canvas;
    private bool run = false;

    private AudioManager audioManager;
    private bool playHorn = true;

    private void Awake()
    {


        canvas = GameObject.Find("Canvas");
        StefanoPrompt = canvas.transform.Find("Promptstefano");
        StefanoPrompt.gameObject.SetActive(false);
        NPCName = canvas.transform.Find("DialoguePanel/TitlePanel/NPCName").gameObject.GetComponent<TextMeshProUGUI>();
        testocronometro = canvas.transform.Find("Trashcronometro").gameObject.GetComponent<TextMeshProUGUI>();
        testocontatore = canvas.transform.Find("trashcontatore").gameObject.GetComponent<TextMeshProUGUI>();
        testo5 = canvas.transform.Find("trash5").gameObject.GetComponent<TextMeshProUGUI>();
        img = canvas.transform.Find("imgSpazzatura").gameObject.GetComponent<Image>();
        testocronometro.gameObject.SetActive(false);
        testocontatore.gameObject.SetActive(false);
        testo5.gameObject.SetActive(false);
        img.gameObject.SetActive(false);
        dialogueText = canvas.transform.Find("DialoguePanel/DialogueText").gameObject.GetComponent<TextMeshProUGUI>();
        canvas.transform.Find("DialoguePanel").gameObject.SetActive(false);
        //confirmButton = canvas.transform.Find("DialoguePanel/ConfirmButton").gameObject.GetComponent<Button>();
        //cancelButton = canvas.transform.Find("DialoguePanel/CancelButton").gameObject.GetComponent<Button>();
        totalTime2 = totalTime + 4f;

        //Aggiunge i listener ai bottoni di dialogo
        //confirmButton.onClick.RemoveAllListeners();
        //confirmButton.onClick.RemoveAllListeners();
        //confirmButton.onClick.AddListener(ConfirmButton_onClick);
        //cancelButton.onClick.AddListener(CancelButton_onClick);
        Transform tp = GetComponent<Transform>();
        originepianox = tp.position.x;
        originepianoz = tp.position.z;

        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

    }
    public void trashStartPrompt()
    {
        if (GameDirector.Instance.getGameState() != GameDirector.GameState.FreeRoaming)
            return;
        if (!canvas.transform.Find("DialoguePanel").gameObject.activeSelf)
            StefanoPrompt.gameObject.SetActive(true);
        if (Input.GetKey(KeyCode.E))
        {
            canvas.transform.Find("BarsPanel").gameObject.SetActive(false);
            StefanoPrompt.gameObject.SetActive(false);
            canvas.transform.Find("DialoguePanel").gameObject.SetActive(true);
            GameDirector.Instance.checkDialoguePanelButtons("TrashCollecting");
            NPCName.SetText("Peppe il pesce");
            dialogueText.SetText("Hey sembrerebbe che ci siano dei sacchetti della spazzatura\n " +
            "ad inquinare l'oceano! Ti va di aiutarmi a metterli tutti nella rete? \n" +
            "Attenta! hai solo un minuto di tempo prima che risalga la rete");
        }

    }
    public void PesceRossoTriggerExit()
    {
        if (canvas.transform.Find("DialoguePanel").gameObject.activeSelf)
            canvas.transform.Find("DialoguePanel").gameObject.SetActive(false);
        if (StefanoPrompt.gameObject.activeSelf)
            StefanoPrompt.gameObject.SetActive(false);
        if (!canvas.transform.Find("BarsPanel").gameObject.activeSelf)
            canvas.transform.Find("BarsPanel").gameObject.SetActive(true);
    }
    public void ConfirmButton_onClick()
    {
        canvas.transform.Find("DialoguePanel").gameObject.SetActive(false);
        StefanoPrompt.gameObject.SetActive(false);
        canvas.transform.Find("BarsPanel").gameObject.SetActive(true);
        audioManager.PlaySFX(audioManager.selection);
        startTrashGame();
    }
    public void CancelButton_onClick()
    {
        PesceRossoTriggerExit();
        audioManager.PlaySFX(audioManager.selection);
        StefanoPrompt.gameObject.SetActive(true);
    }

    void startTrashGame()
    {
        if (GameDirector.Instance.getGameState() != GameDirector.GameState.FreeRoaming)
        {
            GameDirector.Instance.setGameState(GameDirector.GameState.FreeRoaming);
            return;
        }
        GameDirector.Instance.setGameState(GameDirector.GameState.TrashCollecting);
        a = 0;
        b = 0;
        rifiutiraccolti = 0;
        sostegno.gameObject.SetActive(false);
        testocronometro.gameObject.SetActive(true);
        testocontatore.gameObject.SetActive(true);
        testo5.gameObject.SetActive(true);
        img.gameObject.SetActive(true);
        currentTime = totalTime;
        currentTime2 = totalTime2;
        run = true;
        audioManager.PlaySFX(audioManager.GridClimb);
        SpawnPrefabs();
    }
    void FixedUpdate()
    {

        if (run)
        {
            Debug.Log("oggetti nel paino" + oggettiNelPiano);
            Debug.Log("oggetti raccolti" + rifiutiraccolti);
            if (currentTime > 0)
            {
                testocronometro.text = Mathf.Round(currentTime).ToString();
                testocontatore.text = oggettiNelPiano.ToString();

                currentTime -= Time.deltaTime;
            }
            if (currentTime2 > 0)
            {
                testocontatore.text = oggettiNelPiano.ToString();
                currentTime2 -= Time.deltaTime;
            }

            if (currentTime < 10 && playHorn == true)
            {
                audioManager.PlaySFX(audioManager.ShipHorn);
                playHorn = false;
            }
            if (currentTime <= 0 && a == 0)
            {
                if (b == 0)
                {
                    rifiutiraccolti = oggettiNelPiano;
                    int earnedPearls = rifiutiraccolti * 5;
                    GameDirector.Instance.addPearls(earnedPearls);
                    //GameDirector.Instance.setGameState(GameDirector.GameState.FreeRoaming);
                    GameDirector.Instance.addPollution(-rifiutiraccolti * 5);
                    testocronometro.gameObject.SetActive(false);
                    testocontatore.gameObject.SetActive(false);
                    testo5.gameObject.SetActive(false);
                    img.gameObject.SetActive(false);
                    b = 1;

                    audioManager.PlaySFX(audioManager.GridClimb);

                }
                rb.AddForce(Vector3.up * upwardForce, ForceMode.Impulse);


                currentTime = 0;

            }

            if (currentTime2 <= 0)
            {

                foreach (GameObject spazzatura in spazzature)
                {
                    Destroy(spazzatura);
                }
                spazzature.Clear();


                a = 1;
                currentTime2 = 0;
                sostegno.gameObject.SetActive(true);
                canvas.transform.Find("VictoryPanel").gameObject.SetActive(true);
                rewardsText = canvas.transform.Find("VictoryPanel/RewardsPanel/RewardsText").GetComponent<TextMeshProUGUI>();
                rewardsText.SetText("Rifiuti raccolti: " + rifiutiraccolti + "\n" +
                                 "Perle guadagnate: " + rifiutiraccolti * 5 + "\n" +
                                 "Livello di ossigeno diminuito del " + rifiutiraccolti * 5 + "%");
                GameDirector.Instance.setGameState(GameDirector.GameState.FreeRoaming);
                run = false;



            }
            if (numerosacchettidaspawnare == oggettiNelPiano)
            {
                currentTime = 0;
                currentTime2 = 4f;
            }

        }



    }


    void SpawnPrefabs()
    {

        for (int i = 0; i < numerosacchettidaspawnare; i++) // Numero di prefabs da spawnare
        {
            Vector3 randomPosition = new Vector3(originepianox + lunghezzapiano, Random.Range(80f, 100f), Random.Range(originepianoz - spawnAreaLength / 2, originepianoz + spawnAreaLength / 2));
            Vector3 randomPosition1 = new Vector3(Random.Range(originepianox - spawnAreaLength / 2, originepianox + spawnAreaLength / 2), Random.Range(80f, 100f), originepianoz + lunghezzapiano);
            Vector3 randomPosition2 = new Vector3(originepianox - lunghezzapiano, Random.Range(80f, 100f), Random.Range(originepianoz - spawnAreaLength / 2, originepianoz + spawnAreaLength / 2));
            Vector3 randomPosition3 = new Vector3(Random.Range(originepianox - spawnAreaLength / 2, originepianox + spawnAreaLength / 2), Random.Range(80f, 100f), originepianoz - lunghezzapiano);

            if (i < 2)
            {
                if (i % 2 == 0)
                {
                    spazzature.Add(Instantiate(prefabToSpawn1, randomPosition, Quaternion.identity));
                }
                else
                {
                    spazzature.Add(Instantiate(prefabToSpawn1, randomPosition1, Quaternion.identity));
                }
            }
            else if (i >= 2 && i < 4)
            {
                if (i % 2 == 0)
                {
                    spazzature.Add(Instantiate(prefabToSpawn2, randomPosition2, Quaternion.identity));

                }
                else
                {
                    spazzature.Add(Instantiate(prefabToSpawn2, randomPosition3, Quaternion.identity));
                }

            }
            else
            {
                spazzature.Add(Instantiate(prefabToSpawn3, randomPosition, Quaternion.identity));
            }

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("spazzatura"))
        {
            oggettiNelPiano++;
            //Debug.Log("Oggetto entrato nel piano. Oggetti totali nel piano: " + oggettiNelPiano);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("spazzatura"))
        {
            oggettiNelPiano--;
            // Debug.Log("Oggetto uscito dal piano. Oggetti totali nel piano: " + oggettiNelPiano);
        }
    }



}






