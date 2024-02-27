using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BarsInterface : MonoBehaviour
{
    private static Slider reefHealthSlider, pollutionSlider, biodiversitySlider, oxygenLevelSlider;
    private static Image reefHealthArrow, pollutionArrow, biodiversityArrow, oxygenLevelArrow;  //TODO: aggiungi un'immagine per quando il cambiamento di parametri e' 0
    private static Sprite upArrow, downArrow;

    private void Awake()
    {
        GameObject canvas = GameObject.Find("Canvas");
        reefHealthSlider = canvas.transform.Find("BarsPanel/ReefHealthBar").GetComponent<Slider>();
        pollutionSlider = canvas.transform.Find("BarsPanel/PollutionBar").GetComponent<Slider>();
        biodiversitySlider = canvas.transform.Find("BarsPanel/BiodiversityBar").GetComponent<Slider>();
        oxygenLevelSlider = canvas.transform.Find("BarsPanel/OxygenLevelBar").GetComponent<Slider>();

        reefHealthArrow = canvas.transform.Find("BarsPanel/ReefHealthBar/ReefHealthArrow").GetComponent<Image>();
        pollutionArrow = canvas.transform.Find("BarsPanel/PollutionBar/PollutionArrow").GetComponent<Image>();
        biodiversityArrow = canvas.transform.Find("BarsPanel/BiodiversityBar/BiodiversityArrow").GetComponent<Image>();
        oxygenLevelArrow = canvas.transform.Find("BarsPanel/OxygenLevelBar/OxygenLevelArrow").GetComponent<Image>();

        upArrow = Resources.Load<Sprite>("Sprites/UpArrow");
        downArrow = Resources.Load<Sprite>("Sprites/DownArrow");
    }
    
    public static void updateBars()
    {
        int pollution = GameDirector.Instance.getPollution();
        int biodiversity = GameDirector.Instance.getBiodiversity();
        int oxygenLevel = GameDirector.Instance.getOxygenLevel();
        int reefHealth = GameDirector.Instance.getReefHealth();

        pollutionSlider.value = pollution;
        biodiversitySlider.value = GameDirector.Instance.getBiodiversity();
        oxygenLevelSlider.value = oxygenLevel;
        
        reefHealthSlider.value = reefHealth;
    }
    public static void updateArrows()
    {
        int pollutionChange = GameDirector.Instance.getPollutionChange();
        int biodiversityChange = GameDirector.Instance.getBiodiversityChange();
        int oxygenLevelChange = GameDirector.Instance.getOxygenChange();

        if (GameDirector.Instance.getReefHealthChange() >= 0)
            reefHealthArrow.sprite = upArrow;
        else
            reefHealthArrow.sprite = downArrow;

        if (pollutionChange >= 0)
            pollutionArrow.sprite = downArrow;
        else
            pollutionArrow.sprite = upArrow;

        if (biodiversityChange >= 0)
            biodiversityArrow.sprite = upArrow;
        else
            biodiversityArrow.sprite = downArrow;

        if (oxygenLevelChange >= 0)
            oxygenLevelArrow.sprite = upArrow;
        else
            oxygenLevelArrow.sprite = downArrow;
    }
}