using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CustomClickBehaviours : MonoBehaviour
{
    private Button[] coralButtons;  
    private Button ConfirmButton;
    private TextMeshProUGUI coralTitle, coralDesc, coralStats, coralCost, currentPearls;

    [SerializeField] private CoralSO pillarCoral, fireCoral, softCoral, elkhornCoral;
    private CoralSO[] corals;                                                                                
    private int selectedCoral;
    
    private GameObject gameDirector;
    
    private void Awake()
    {
        // -------------------------------------------------------------------- //
        //Trova il gameDirector
        gameDirector = GameObject.Find("Director");
        //Trova tutti i bottoni dei coralli e li salva in un array
        selectedCoral = -1;
        coralButtons = new Button[4];
        coralButtons[0] = transform.Find("CoralChoicePanel/IconsPanel/PillarCoralButton").GetComponent<Button>();
        coralButtons[1] = transform.Find("CoralChoicePanel/IconsPanel/FireCoralButton").GetComponent<Button>();
        coralButtons[2] = transform.Find("CoralChoicePanel/IconsPanel/SoftCoralButton").GetComponent<Button>();
        coralButtons[3] = transform.Find("CoralChoicePanel/IconsPanel/ElkhornCoralButton").GetComponent<Button>();

        //Aggiunge i listener ai bottoni dei coralli
        coralButtons[0].onClick.AddListener(delegate { CoralButton_onClick(0); });
        coralButtons[1].onClick.AddListener(delegate { CoralButton_onClick(1); });
        coralButtons[2].onClick.AddListener(delegate { CoralButton_onClick(2); });
        coralButtons[3].onClick.AddListener(delegate { CoralButton_onClick(3); });

        //Trova il bottone di conferma, e aggiunge il listener
        ConfirmButton = transform.Find("CoralChoicePanel/ChoicePanel/ConfirmButton").GetComponent<Button>();
        ConfirmButton.onClick.AddListener(delegate { Confirm_onClick(); });
        //Nota: il bottone annulla ha il default listener, perch� deve solo disattivare il pannello

        //Inserisce gli scriptable objects con le info dei coralli in un array
        //TODO: vedi se riesci a ricavarli tramite resources.findobjectsoftypeall()
        corals = new CoralSO[4];
        corals[0] = pillarCoral;
        corals[1] = fireCoral;
        corals[2] = softCoral;
        corals[3] = elkhornCoral;

        //Prende i campi di testo del pannello
        coralTitle = transform.Find("CoralChoicePanel/InfoPanel/TitlePanel/CoralTitle").gameObject.GetComponent<TextMeshProUGUI>();
        coralDesc = transform.Find("CoralChoicePanel/InfoPanel/DescriptionPanel/CoralDesc").gameObject.GetComponent<TextMeshProUGUI>();
        coralStats = transform.Find("CoralChoicePanel/InfoPanel/StatsPanel/CoralStats").gameObject.GetComponent<TextMeshProUGUI>();
        coralCost = transform.Find("CoralChoicePanel/ChoicePanel/CostText").gameObject.GetComponent<TextMeshProUGUI>();
        currentPearls = transform.Find("CoralChoicePanel/ChoicePanel/AvailablePearlsText").gameObject.GetComponent<TextMeshProUGUI>();
        //Svuota i testi all'avvio
        coralTitle.SetText("");
        coralDesc.SetText("");
        coralStats.SetText("");
        coralCost.SetText("");
        currentPearls.SetText("Perle disponibili: " + GameDirector.Instance.getCurrentPearls());

        //Se lasciato attivo, nasconde il pannello all'accensione
        if (transform.Find("CoralChoicePanel").gameObject.activeSelf)
            transform.Find("CoralChoicePanel").gameObject.SetActive(false);

        /* Nota: questo for, per qualche motivo, non funziona bene. Per ora lascio la scrittura estesa
        for(int i = 0; i<coralButtons.Length;i++)
        {
            coralButtons[i].onClick.AddListener(delegate { CoralButton_onClick(i); });
        }
        */
    }

    public void CoralButton_onClick(int index)
    {
        coralTitle.SetText(corals[index].coralName);
        coralDesc.SetText(corals[index].coralDesc);
        coralStats.SetText("Caratteristiche:" +
                           "\nBiodiversit�: " + corals[index].getBiodiversityChange() +
                           "\nInquinamento: " + corals[index].getPollutionChange() +
                           "\nLivello di ossigeno: " + corals[index].getOxygenLevelChange());
        coralCost.SetText("Costo: " + corals[index].getCost() + " perle");

        selectedCoral = index;
    }
    public void Confirm_onClick()
    {
        Debug.Log("Selected Coral: " + corals[selectedCoral].coralName);
        currentPearls.SetText("Perle disponibili: " + GameDirector.Instance.getCurrentPearls());
        if (selectedCoral != -1 && GameDirector.Instance.getCurrentPearls() >= corals[selectedCoral].getCost())
        {
            GameDirector.Instance.addPearls(-corals[selectedCoral].getCost());
            GameDirector.Instance.currentCoralSpot.GetComponent<CoralHandler>().SpawnCorals(selectedCoral);
        }

    }
}
