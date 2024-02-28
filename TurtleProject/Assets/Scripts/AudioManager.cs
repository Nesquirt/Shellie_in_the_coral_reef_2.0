using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class AudioManager : MonoBehaviour
{

    [SerializeField] private AudioMixerGroup Music, SFX;  //Riferimento al Group Music e Group SFX del Mixer  
    private List<AudioSource> audioSources; //Lista di AudioSource 
    private Dictionary<string, AudioClip> audioClips;//Key,Value = Nome della clip, ClipAudio
    private readonly float[] MusicVolume = {0.7f, 0.5f, 0.6f, 1, 0.4f};  //Volumi delle AudioClip di tipo "Music"
    private bool isInMiniGame; //Controllo se sono all'interno di un MiniGame
    private string miniGame_musicName; //Usata per settare l'AudioClip a seconda del MiniGame

    public void Awake()
    {
        audioSources = new();
        audioClips = new();
        isInMiniGame = false;
        miniGame_musicName = null;
        LoadAudioClips("Sounds");
    }
    //Disattivo il GameObject se la sua AudioSource non e' in riproduzione
    public void Update()
    {
        foreach (AudioSource source in audioSources)
        {
            if (!source.isPlaying)
            {
                source.gameObject.SetActive(false);
            }
        }
    }
    //Funzione per il salvataggio delle AudioClip (contenute in una cartella) all'interno del Dictonary  
    public void LoadAudioClips(String folderPath)
    {
        AudioClip[] temp = Resources.LoadAll<AudioClip>(folderPath);
        foreach (AudioClip clip in temp)
        {
            Debug.Log(clip.name);
            audioClips.Add(clip.name, clip);
        }
    }
    //Funzioni Music
    public void MenuMusic() 
    { 
        if(audioSources.Count == 0) Play("menu_Music", true, 0.8f); 
        else Restart();
    }
    public void GameMusic()
    {
        StartCoroutine(FadeTwoClips("menu_Music", 0, "freeRoaming_Music", MusicVolume[0], 5));
        Play("water_SFX", true, 0.5f);
        Play("bubble_SFX", true, 0.5f);
    }
    /*
     * La funzione viene chiamata all'inizio e alla fine di ogni MiniGame. Se isInMiniGame = true
     * ho appena cominciato un MiniGame, setto quindi il nome dell'AudioClip e avvio il fade con la
     * traccia dello stato FreeRoaming. Se isInMiniGame = false ho completato il MiniGame, avvio quindi
     * il fade opposto
    */
    public void MiniGame()
    {
        isInMiniGame = !isInMiniGame;

        float volumeMiniGame = 0f;
        float volumeFreeRoaming = isInMiniGame ? 0 : MusicVolume[0];

        if (isInMiniGame == true)
        {
            switch (GameDirector.Instance.getGameState())
            {
                case GameDirector.GameState.ObstacleCourse:
                    {
                        miniGame_musicName = "obstacleCourse_Music";
                        volumeMiniGame = MusicVolume[1];
                        break;
                    }
                case GameDirector.GameState.TrashCollecting:
                    {
                        Play("gridDrop_SFX", false, 1f);
                        miniGame_musicName = "trashCollecting_Music";
                        volumeMiniGame = MusicVolume[2];
                        break;
                    }
                case GameDirector.GameState.MazeExploring:
                    {
                        miniGame_musicName = "mazeExploring_Music";
                        volumeMiniGame = MusicVolume[3];
                        break;
                    }
                case GameDirector.GameState.SummoningRitual:
                    {
                        miniGame_musicName = "summoningRitual_Music";
                        volumeMiniGame = MusicVolume[4];
                        break;
                    }
            }
        }
        else if (miniGame_musicName != "summoningRitual_Music") Play("endMiniGame_SFX", false, 1f);

        StartCoroutine(FadeTwoClips(miniGame_musicName, volumeMiniGame, "freeRoaming_Music", volumeFreeRoaming, 7));
    }

    //Funzioni per la riproduzione audio (SFX)
    public void ButtonPressed() { Play("selection_SFX", false, 1f); }
    public void ShipHornOrGridClimb(float currentTime) { Play(currentTime > 0 ? "shipHorn_SFX" : "gridClimb_SFX", false, 1); }
    public void CrossRing(int targetNumber) { Play(targetNumber == 0 ? "raceStart_SFX" : "ringCross_SFX", false, targetNumber == 0 ? 1 : 0.6f); }
    public void RaceStart() { Play("raceStart_SFX", false, 2f); }
    public void KeyOrCage(Collider collider) { Play(collider.CompareTag("Chiave") ? "keyTaken_SFX" : "cageOpen_SFX", false, collider.CompareTag("Chiave") ? 0.6f : 0.7f); }
    /*
     * Dopo aver ottenuto (se esiste) l'AudioClip dal Dictionary tramite la key "clipName", la associo
     * all'AudioSource ottenuta dalla funzione GetAvailableSource(). Dopo aver settato i parametri di 
     * loop, output e volume, riproduco l'AudioClip.
    */
    public void Play(String clipName, bool loop, float volume)
    {
        if (!audioClips.ContainsKey(clipName))
        {
            Debug.LogWarning("Clip audio non trovata: " + clipName);
            return;
        }
        else
        {
            AudioClip audioClip = audioClips[clipName];
            AudioSource source = GetAvailableSource(clipName);
            if (source != null)
            {
                source.gameObject.SetActive(true);
                source.clip = audioClip;
                SetMixerGroup(source, clipName);
                source.loop = loop;
                source.volume = volume;
                source.Play();
            }
            else
            {
                Debug.LogWarning("Audiosource nullo!");
            }
        }
    }
    //Associa il corretto output all'AudioSource a seconda del nome dell'AudioClip 
    private void SetMixerGroup(AudioSource source, String clipName)
    {
        if (clipName.EndsWith("_SFX")) source.outputAudioMixerGroup = SFX;
        else source.outputAudioMixerGroup = Music;
    }
    /*
     * La funzione restituisce la prima AudioSource inattiva collegata ad un GameObject (child di AudioManager) gia' instanziato. 
     * Se tutte le AudioSource sono in riproduzione, crea un nuovo GameObject e restituisce la nuova AudioSource a lui collegata. 
    */
    private AudioSource GetAvailableSource(String clipName)
    {
        foreach (AudioSource source in audioSources)
        {
            if (!source.isPlaying)
            {
                source.gameObject.name = clipName;
                return source;
            }
        }
        // If no available source found, create a new one
        GameObject newAudioSource = new(clipName);
        newAudioSource.transform.parent = transform;
        AudioSource newSource = newAudioSource.AddComponent<AudioSource>();
        audioSources.Add(newSource);
        return newSource;
    }
    //La funzione restituisce null o l'AudioSource a cui è associata la clip passata come parametro
    private AudioSource GetAudioSourceFromClip(AudioClip clip)
    {
        foreach (AudioSource audioSource in audioSources)
        {
            if (audioSource.clip == clip)
            {
                return audioSource;
            }
        }
        return null;
    }
    /*
     * La funzione richiama il Fade per tutte le AudioSource che sono in riproduzione e avvia contemporaneamente
     * il fade per l'AudioSource che sarà associata all'AudioClip del menu
    */
    private void Restart()
    {
        foreach (AudioSource audioSource in audioSources)
        {
            if(audioSource.isPlaying) 
            {
                StartCoroutine(Fade(audioSource.name,0,3));
            }
        }
        StartCoroutine(Fade("menu_Music", 0.8f, 3));
    }
    public IEnumerator FadeTwoClips(String clip1Name, float targetVolume1, String clip2Name, float targetVolume2, float duration)
    {
        StartCoroutine(Fade(clip1Name, targetVolume1, duration));
        StartCoroutine(Fade(clip2Name, targetVolume2, duration));
        yield return new WaitForSeconds(duration);
    }
    public IEnumerator Fade(String clipName, float targetVolume, float duration)
    {
        if (!audioClips.ContainsKey(clipName))
        {
            Debug.LogWarning("Clip audio non trovata: " + clipName);
            yield break;
        }
        else
        {
            AudioClip audioClip = audioClips[clipName];
            AudioSource source = GetAudioSourceFromClip(audioClip);
            float startVolume;

            if (source == null || !source.isPlaying)
            {
                Play(audioClip.name, true, 0);
                startVolume = 0;
                source = GetAudioSourceFromClip(audioClip);
            }
            else
            {
                SetMixerGroup(source, clipName);
                startVolume = source.volume;
            }
            float startTime = Time.time;

            while (Time.time < startTime + duration)
            {
                source.volume = Mathf.Lerp(startVolume, targetVolume, (Time.time - startTime) / duration);
                yield return null;
            }

            source.volume = targetVolume;

            if (targetVolume == 0f)
            {
                source.Stop();
            }
        }
    }
}