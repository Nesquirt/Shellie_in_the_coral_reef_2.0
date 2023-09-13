using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CustomClickBehaviours : MonoBehaviour
{
    public Button PillarCoralButton;
    public Button ConfirmButton, CancelButton;

    TextMeshProUGUI coralTitle;
    private void Awake()
    {
        PillarCoralButton.onClick.AddListener(Pillar_onClick);
        coralTitle = transform.Find("CoralChoicePanel").transform.Find("DescriptionPanel").transform.Find("TitlePanel").transform.Find("CoralTitle").gameObject.GetComponent<TextMeshProUGUI>();
    }

    public void Pillar_onClick()
    {
        coralTitle.SetText("Test modifica titolo");
        Debug.Log("Premuto il tasto PillarCoral");
    }
}
