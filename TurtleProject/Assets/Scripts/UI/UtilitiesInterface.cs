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
        //audioManager.PlaySFX(audioManager.selection);
        SceneManager.LoadScene("Simone_Impostazioni", LoadSceneMode.Additive);
    }

    public void StatsOpenAndClose(GameObject obj)
    {
        //audioManager.PlaySFX(audioManager.selection);
        obj.SetActive(!obj.activeSelf);
    }
    
}
