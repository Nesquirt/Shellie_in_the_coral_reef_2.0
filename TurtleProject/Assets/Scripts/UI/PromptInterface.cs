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
        interactPrompt.gameObject.SetActive(false);

    }

    public static void togglePromptOn()
    {
        interactPrompt.gameObject.SetActive(true);
    }

    public static void togglePromptOff()
    {
        interactPrompt.gameObject.SetActive(false);
    }

    public static void setPromptText(string text)
    {
        interactPrompt.SetText(text);
    }
}
