using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PromptInterface : MonoBehaviour
{
    private static TextMeshProUGUI interactPrompt;

    private void Awake()
    {
        interactPrompt = GameObject.Find("Canvas/InteractPrompt").GetComponent<TextMeshProUGUI>();
        togglePrompt(false);
        setPromptText("If you see this, something went wrong");

    }
    public static void togglePrompt(bool state)
    {
        interactPrompt.gameObject.SetActive(state);
    }

    public static void setPromptText(string text)
    {
        interactPrompt.SetText(text);
    }
}
