using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class UtilitiesInterface : MonoBehaviour
{
    
    public void OpenSettings()
    {
        GameDirector.Instance.audioManager.ButtonPressed();
        SceneManager.LoadScene("Simone_Impostazioni", LoadSceneMode.Additive);
    }

    public void StatsOpenAndClose(GameObject obj)
    {
        GameDirector.Instance.audioManager.ButtonPressed();
        obj.SetActive(!obj.activeSelf);
    }
    
}
