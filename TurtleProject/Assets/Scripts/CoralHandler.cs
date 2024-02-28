using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoralHandler : MonoBehaviour
{
    [SerializeField] private GameObject PillarCoral, FireCoral, SoftCoral, ElkhornCoral;
    private Vector3[] spawnPoints;
    private int growthCounter;
    private bool isGrowing;
    private UIManager UImanager;
    private void Awake()
    {
        UImanager = GameObject.Find("Canvas").GetComponent<UIManager>();

        isGrowing = false;
        spawnPoints = new Vector3[5];
        spawnPoints[0] = new Vector3(this.transform.position.x + 1.9f, this.transform.position.y + 5.7f, this.transform.position.z + 2.5f);
        spawnPoints[1] = new Vector3(this.transform.position.x + 7.6f, this.transform.position.y + 4.3f, this.transform.position.z - 6f);
        spawnPoints[2] = new Vector3(this.transform.position.x + 5f, this.transform.position.y + 2.2f, this.transform.position.z - 12.3f);
        spawnPoints[3] = new Vector3(this.transform.position.x - 4.3f, this.transform.position.y + 5.9f, this.transform.position.z + 11.7f);
        spawnPoints[4] = new Vector3(this.transform.position.x - 7.6f, this.transform.position.y + 4.2f, this.transform.position.z - 8);

        growthCounter = 0;
    }

    public void SpawnCorals(CoralSO coral)
    {
        isGrowing = true;
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            var newCoral = Instantiate(coral.getcoralPrefab(), spawnPoints[i], Quaternion.identity);
            newCoral.transform.parent = this.transform;
            GameDirector.Instance.modifyParameterChanges(coral.getPollutionChange(), coral.getBiodiversityChange(), coral.getOxygenLevelChange());
            StartCoroutine(CoralGrow());
        }
        IEnumerator CoralGrow()
        {
            while (growthCounter <= 1200)
            {
                foreach (Transform coral in transform)
                {
                    if (coral.tag == "Coral")
                    {
                        Vector3 scaleChange = new Vector3(0.002f, 0.002f, 0.002f);
                        coral.transform.localScale += scaleChange;
                    }
                }
                growthCounter++;
                yield return new WaitForSecondsRealtime(0.1f);
            }
        }
    }
        public void OnTriggerEnter(Collider collider)
        {
            if (isGrowing)
            {
                UImanager.promptInterface.togglePrompt(false);
                UImanager.coralChoiceInterface.toggleCoralChoicePanel(false);
                return;
            }
            if (collider.gameObject.tag == "Player" &&
                GameDirector.Instance.getGameState() == GameDirector.GameState.FreeRoaming)
            {
                GameDirector.Instance.currentCoralSpot = this.gameObject;
                UImanager.promptInterface.setPromptText("Premi E per piantare un corallo");
                UImanager.promptInterface.togglePrompt(true);
            }
        }
        public void OnTriggerStay(Collider collider)
        {
            if (collider.gameObject.tag == "Player" &&
                GameDirector.Instance.getGameState() == GameDirector.GameState.FreeRoaming
                && !isGrowing)
            {
                if (Input.GetKey(KeyCode.E))
                {
                    
                    UImanager.coralChoiceInterface.toggleCoralChoicePanel(true);
                    UImanager.promptInterface.togglePrompt(false);
                }
                if (Input.GetKey(KeyCode.Escape))
                {
                    UImanager.coralChoiceInterface.toggleCoralChoicePanel(false);
                    UImanager.promptInterface.togglePrompt(true);
                }
            }
            else if (isGrowing && UImanager.promptInterface.isPromptActive())
                UImanager.promptInterface.togglePrompt(false);

        }
        public void OnTriggerExit(Collider collider)
        {
            if (collider.gameObject.tag == "Player" &&
                GameDirector.Instance.getGameState() == GameDirector.GameState.FreeRoaming)
            {
                UImanager.promptInterface.togglePrompt(false);
                UImanager.coralChoiceInterface.toggleCoralChoicePanel(false);
            }
        }
}
