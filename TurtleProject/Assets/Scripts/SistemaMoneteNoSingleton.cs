using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SistemaMonete : MonoBehaviour
{
    public int perle = 0; // La quantità di perle iniziale
    public TextMeshProUGUI testoPerle; // Riferimento al Text UI per le perle

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


// Chiamare la funzione AggiungiPerle da un altro script
//SistemaMonete sistemaMonete = GetComponent<SistemaMonete>(); // Assegnare ad un oggetto
//sistemaMonete.AggiungiPerle(quantitaDaAggiungere);

// Chiamare la funzione SottraiPerle da un altro script
//sistemaMonete.SottraiPerle(quantitaDaSottrarre);

// Verificare la quantità attuale di perle da un altro script
//int quantitaPerle = sistemaMonete.perle;

// made with love from Assassin's script ♥