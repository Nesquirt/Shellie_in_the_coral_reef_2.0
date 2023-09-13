using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SistemaMonete : MonoBehaviour
{
    private static SistemaMonete _istanza;

    public static SistemaMonete Istanza
    {
        get
        {
            if (_istanza == null)
            {
                _istanza = FindObjectOfType<SistemaMonete>();

                if (_istanza == null)
                {
                    // Se l'istanza non esiste, crea un nuovo oggetto vuoto e aggiungi il sistema di monete ad esso
                    GameObject sistemaMoneteObject = new GameObject("SistemaMonete");
                    _istanza = sistemaMoneteObject.AddComponent<SistemaMonete>();
                }
            }
            return _istanza;
        }
    }

    public int perle = 0; // La quantità di perle iniziale
    public TextMeshProUGUI testoPerle; // Riferimento al Text UI per le perle

    private void Awake()
    {
        if (_istanza != null && _istanza != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _istanza = this;
        DontDestroyOnLoad(this.gameObject);
    }

    // Funzione per aggiungere perle
    public void AggiungiPerle(int quantita)
    {
        perle += quantita;
        AggiornaTestoPerle();
    }

    // Funzione per sottrarre perle
    public void SottraiPerle(int quantita)
    {
        perle -= quantita;
        AggiornaTestoPerle();
    }

    // Funzione per aggiornare il testo delle perle
    void AggiornaTestoPerle()
    {
        if (testoPerle != null)
        {
            testoPerle.text = "Perle: " + perle.ToString();
        }
    }
}




// SistemaMonete.Istanza.AggiungiPerle(quantitaDaAggiungere); //aggiungere perle da altri script
// SistemaMonete.Istanza.SottraiPerle(quantitaDaSottrarre);  // rimuovere perle da altri script
// int quantitaPerle = SistemaMonete.Istanza.perle;  //verificare quantità attuale di perle da altro script


// made with love from Assassin's script ♥




