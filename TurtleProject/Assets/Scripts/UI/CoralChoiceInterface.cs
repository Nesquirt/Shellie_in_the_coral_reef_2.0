using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CoralChoiceInterface : MonoBehaviour
{
    private Button[] coralButtons;  
    private Button ConfirmButton;
    private TextMeshProUGUI coralTitle, coralDesc, coralStats, coralCost, currentPearls;

    [SerializeField] private CoralSO pillarCoral, fireCoral, softCoral, elkhornCoral;
    private CoralSO[] corals;                                                                                
    private int selectedCoral;

    private BarsInterface barsInterface;
    
    private void Awake()
    {
        // -------------------------------------------------------------------- //
        //Trova tutti i bottoni dei coralli e li salva in un array
        selectedCoral = -1;
        coralButtons = new Button[4];
        coralButtons[0] = transform.Find("IconsPanel/PillarCoralButton").GetComponent<Button>();
        coralButtons[1] = transform.Find("IconsPanel/FireCoralButton").GetComponent<Button>();
        coralButtons[2] = transform.Find("IconsPanel/SoftCoralButton").GetComponent<Button>();
        coralButtons[3] = transform.Find("IconsPanel/ElkhornCoralButton").GetComponent<Button>();

        //Aggiunge i listener ai bottoni dei coralli
        coralButtons[0].onClick.AddListener(delegate { CoralButton_onClick(0); });
        coralButtons[1].onClick.AddListener(delegate { CoralButton_onClick(1); });
        coralButtons[2].onClick.AddListener(delegate { CoralButton_onClick(2); });
        coralButtons[3].onClick.AddListener(delegate { CoralButton_onClick(3); });

        //Trova il bottone di conferma, e aggiunge il listener
        ConfirmButton = transform.Find("ChoicePanel/ConfirmButton").GetComponent<Button>();
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
        coralTitle = transform.Find("InfoPanel/TitlePanel/CoralTitle").gameObject.GetComponent<TextMeshProUGUI>();
        coralDesc = transform.Find("InfoPanel/DescriptionPanel/CoralDesc").gameObject.GetComponent<TextMeshProUGUI>();
        coralStats = transform.Find("InfoPanel/StatsPanel/CoralStats").gameObject.GetComponent<TextMeshProUGUI>();
        coralCost = transform.Find("ChoicePanel/CostText").gameObject.GetComponent<TextMeshProUGUI>();
        currentPearls = transform.Find("ChoicePanel/AvailablePearlsText").gameObject.GetComponent<TextMeshProUGUI>();
        //Svuota i testi all'avvio
        coralTitle.SetText("");
        coralDesc.SetText("");
        coralStats.SetText("");
        coralCost.SetText("");
        currentPearls.SetText("Perle disponibili: " + GameDirector.Instance.getCurrentPearls());

        //Se lasciato attivo, nasconde il pannello all'accensione
        if (GameObject.Find("CoralChoicePanel").gameObject.activeSelf)
            GameObject.Find("CoralChoicePanel").gameObject.SetActive(false);

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
        Debug.Log("Selected Coral: " + corals[selectedCoral].getCoralName());
        currentPearls.SetText("Perle disponibili: " + GameDirector.Instance.getCurrentPearls());
        if (selectedCoral != -1 && GameDirector.Instance.getCurrentPearls() >= corals[selectedCoral].getCost())
        {
            GameDirector.Instance.addPearls(-(corals[selectedCoral].getCost()));
            GameDirector.Instance.currentCoralSpot.GetComponent<CoralHandler>().SpawnCorals(corals[selectedCoral]);
        }

    }

    public void toggleCoralChoicePanel(bool state)
    {
        transform.gameObject.SetActive(state);
    }
}
