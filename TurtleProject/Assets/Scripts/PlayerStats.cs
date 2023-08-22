using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerStats : MonoBehaviour
{
    public int ringPoints;
    public TMP_Text ringText;
    // Start is called before the first frame update
    void Start()
    {
        ringPoints = 0;
    }

    private void OnTriggerEnter(Collider target)
    {
        ringPoints++;
        Debug.Log("Anello passato");
        ringText.text = "Anelli passati: " + ringPoints + "/8";
        Destroy(target.gameObject); //TODO: temporaneo

    }

}
