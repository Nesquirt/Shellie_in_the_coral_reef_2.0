using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PromptInterface : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI interactPrompt;

    private void Awake()
    {
        togglePrompt(false);
        setPromptText("If you see this, something went wrong");

    }
    public bool isPromptActive()
    {
        return interactPrompt.IsActive();
    }
    public void togglePrompt(bool state)
    {
        interactPrompt.gameObject.SetActive(state);
    }

    public void setPromptText(string text)
    {
        interactPrompt.SetText(text);
    }
}
