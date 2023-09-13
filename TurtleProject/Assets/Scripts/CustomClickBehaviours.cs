using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CustomClickBehaviours : MonoBehaviour
{
    public Button PillarCoralButton;
    public Button ConfirmButton, CancelButton;

    public CoralSO pillarCoral;

    TextMeshProUGUI coralTitle, coralDesc;
    private void Awake()
    {
        transform.Find("CoralChoicePanel").gameObject.SetActive(false);
        PillarCoralButton.onClick.AddListener(Pillar_onClick);
        coralTitle = transform.Find("CoralChoicePanel").transform.Find("DescriptionPanel").transform.Find("TitlePanel").transform.Find("CoralTitle").gameObject.GetComponent<TextMeshProUGUI>();
        coralDesc = transform.Find("CoralChoicePanel").transform.Find("DescriptionPanel").transform.Find("DescriptionPanel").transform.Find("CoralDesc").gameObject.GetComponent<TextMeshProUGUI>();

        //Svuota i testi all'avvio
        coralTitle.SetText("");
        coralDesc.SetText("");
    }

    public void Pillar_onClick()
    {
        
        coralTitle.SetText(pillarCoral.coralName);
        coralDesc.SetText(pillarCoral.coralDesc);
        Debug.Log("Premuto il tasto PillarCoral");
    }
}
