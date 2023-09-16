using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CustomClickBehaviours : MonoBehaviour
{
    [SerializeField] private Button pillarCoralButton, fireCoralButton, softCoralButton, elkhornCoralButton; //Bottoni di selezione del corallo
    [SerializeField] private Button[] coralButtons;                                                          //Array di bottoni   
    [SerializeField] private Button ConfirmButton, CancelButton;                                             //Bottone di conferma e annulla
    [SerializeField] private CoralSO pillarCoral, fireCoral, softCoral, elkhornCoral;                        //Scriptable Objects contenenti le informazioni da mostrare su ogni corallo
    private CoralSO[] corals;                                                                                //Array di Scriptable Objects
    private int selectedCoral;                          
    [SerializeField] private GameObject gameDirector;
    private TextMeshProUGUI coralTitle, coralDesc, coralStats, coralCost;
    private void Awake()
    {
        selectedCoral = -1;
        coralButtons = new Button[4];
        coralButtons[0] = pillarCoralButton;
        coralButtons[1] = fireCoralButton;
        coralButtons[2] = softCoralButton;
        coralButtons[3] = elkhornCoralButton;

        corals = new CoralSO[4];
        corals[0] = pillarCoral;
        corals[1] = fireCoral;
        corals[2] = softCoral;
        corals[3] = elkhornCoral;

        //Se lasciato attivo, nasconde il pannello all'accensione
        transform.Find("CoralChoicePanel").gameObject.SetActive(false);

        /* Nota: questo for, per qualche motivo, non funziona bene. Per ora lascio la scrittura estesa
        for(int i = 0; i<coralButtons.Length;i++)
        {
            coralButtons[i].onClick.AddListener(delegate { CoralButton_onClick(i); });
        }
        */
        // STRUTTURA TEMPORANEA
        coralButtons[0].onClick.AddListener(delegate { CoralButton_onClick(0); });
        coralButtons[1].onClick.AddListener(delegate { CoralButton_onClick(1); });
        coralButtons[2].onClick.AddListener(delegate { CoralButton_onClick(2); });
        coralButtons[3].onClick.AddListener(delegate { CoralButton_onClick(3); });

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
        Debug.Log(index);
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
