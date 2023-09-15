using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CustomClickBehaviours : MonoBehaviour
{
    public Button pillarCoralButton, fireCoralButton;
    public Button[] coralButtons;
    public Button ConfirmButton, CancelButton;

    public CoralSO pillarCoral, fireCoral; //, fireCoral, ...
    private CoralSO[] corals;

    private int selectedCoral;

    public GameObject gameDirector;

    TextMeshProUGUI coralTitle, coralDesc, coralStats, coralCost;
    private void Awake()
    {
        selectedCoral = -1;
        coralButtons = new Button[4];
        coralButtons[0] = pillarCoralButton;
        coralButtons[1] = fireCoralButton;

        corals = new CoralSO[4];
        corals[0] = pillarCoral;
        corals[1] = fireCoral;
        
        

        //Se lasciato attivo, nasconde il pannello all'accensione
        transform.Find("CoralChoicePanel").gameObject.SetActive(false);

        /*
        for(int i = 0; i<2;i++)// coralButtons.Length;i++)
        {
            coralButtons[i].onClick.AddListener(delegate { CoralButton_onClick(i); });
        }
        */
        //STRUTTURA TEMPORANEA
        coralButtons[0].onClick.AddListener(delegate { CoralButton_onClick(0); });
        coralButtons[1].onClick.AddListener(delegate { CoralButton_onClick(1); });

        ConfirmButton.onClick.AddListener(delegate { Confirm_onClick(); });

        //Prende i campi di testo del pannello
        coralTitle = transform.Find("CoralChoicePanel/InfoPanel/TitlePanel/CoralTitle").gameObject.GetComponent<TextMeshProUGUI>();
        coralDesc = transform.Find("CoralChoicePanel/InfoPanel/DescriptionPanel/CoralDesc").gameObject.GetComponent<TextMeshProUGUI>();
        coralStats = transform.Find("CoralChoicePanel/InfoPanel/StatsPanel/CoralStats").gameObject.GetComponent<TextMeshProUGUI>();
        coralCost = transform.Find("CoralChoicePanel/ChoicePanel/CostText").gameObject.GetComponent<TextMeshProUGUI>();
        //Svuota i testi all'avvio
        coralTitle.SetText("");
        coralDesc.SetText("");
        coralStats.SetText("");
        coralCost.SetText("");
    }

    public void CoralButton_onClick(int index)
    {
        Debug.Log("Index: " + index);
        coralTitle.SetText(corals[index].coralName);
        coralDesc.SetText(corals[index].coralDesc);
        coralStats.SetText("Caratteristiche:" +
                           "\nBiodiversità: " + corals[index].getBiodiversityChange() +
                           "\nInquinamento: " + corals[index].getPollutionChange() +
                           "\nLivello di ossigeno: " + corals[index].getOxygenLevelChange());
        coralCost.SetText("Costo: " + corals[index].getCost() + " perle");

        selectedCoral = index;
    }
    public void Confirm_onClick()
    {
        if(selectedCoral != -1)
        {
            gameDirector.GetComponent<GameDirector>().currentCoralSpot.GetComponent<CoralHandler>().SpawnCorals(selectedCoral);
        }
            

        //TODO: impostare condizione di prezzo, e diminuzione delle perle
    }
}
