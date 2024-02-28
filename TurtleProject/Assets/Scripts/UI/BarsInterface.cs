using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BarsInterface : MonoBehaviour
{
    [SerializeField] private Slider reefHealthSlider, pollutionSlider, biodiversitySlider, oxygenLevelSlider;
    [SerializeField] private Image reefHealthArrow, pollutionArrow, biodiversityArrow, oxygenLevelArrow;  //TODO: aggiungi un'immagine per quando il cambiamento di parametri e' 0
    [SerializeField] private Sprite upArrow, downArrow;
    [SerializeField] private TextMeshProUGUI pearlsText;
    public void updatePearls()
    {
        pearlsText.SetText("Perle: " + GameDirector.Instance.getCurrentPearls());
    }
    
    public void updateBars()
    {
        pollutionSlider.value = GameDirector.Instance.getPollution();
        biodiversitySlider.value = GameDirector.Instance.getBiodiversity();
        oxygenLevelSlider.value = GameDirector.Instance.getOxygenLevel();

        reefHealthSlider.value = GameDirector.Instance.getReefHealth();
    }
    public void updateArrows()
    {
        if (GameDirector.Instance.getReefHealthChange() >= 0)
            reefHealthArrow.sprite = upArrow;
        else
            reefHealthArrow.sprite = downArrow;

        if (GameDirector.Instance.getPollutionChange() >= 0)
            pollutionArrow.sprite = downArrow;
        else
            pollutionArrow.sprite = upArrow;

        if (GameDirector.Instance.getBiodiversityChange() >= 0)
            biodiversityArrow.sprite = upArrow;
        else
            biodiversityArrow.sprite = downArrow;

        if (GameDirector.Instance.getOxygenChange() >= 0)
            oxygenLevelArrow.sprite = upArrow;
        else
            oxygenLevelArrow.sprite = downArrow;
    }
}