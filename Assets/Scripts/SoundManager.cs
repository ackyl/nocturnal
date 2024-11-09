using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public enum Sound
    {
        UIClick,
        Distortion,
        SwitchRoom,
        AnomalyRemoved,
        AnomalyTestSound,
        MainBGM,
        Anomaly1,
        Anomaly2,
        Anomaly3,
        Anomaly4,
        Anomaly5,
        Anomaly6,
        Anomaly7,
        Anomaly8,
        Anomaly9,
        Anomaly10,
        Anomaly11,
        Anomaly12,

        HowToPlay,
        FailFix,
    }


    // Singleton instance
    public static SoundManager instance;

    // Dictionary to store lists of AudioSource components linked to sounds
    private Dictionary<Sound, List<AudioSource>> soundDictionary;

    // Public array to set the sound sources in the Inspector
    public SoundAudioSource[] soundAudioSources;

    [System.Serializable]
    public class SoundAudioSource
    {
        public Sound sound;           // Enum for different sounds
        public AudioSource audioSource;  // Link to the base AudioSource used as a template
    }

    public GameObject dialogueGroup;

    void Awake()
    {
        // Set up the singleton instance
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // Optional: persists between scenes
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Initialize sound dictionary
        soundDictionary = new Dictionary<Sound, List<AudioSource>>();
        foreach (SoundAudioSource soundAudioSource in soundAudioSources)
        {
            soundDictionary[soundAudioSource.sound] = new List<AudioSource>();
        }
    }

    // Function to play a sound
    public void PlaySound(Sound sound)
    {
        if (soundDictionary.ContainsKey(sound))
        {
            // Create a new instance of AudioSource by cloning the base AudioSource
            AudioSource newSource = Instantiate(soundAudioSources[(int)sound].audioSource, transform);
            // newSource.pitch = Random.Range(0.5f, 0.7f);
            newSource.Play();

            // Add the new AudioSource instance to the list
            soundDictionary[sound].Add(newSource);

            // Optionally, clean up AudioSource after it finishes playing
            if (newSource.loop == false)
            {
                StartCoroutine(DestroyAudioSourceAfterPlay(newSource, sound));
            }
        }
        else
        {
            Debug.LogWarning("Sound " + sound + " not found in SoundManager.");
        }
    }

    // Coroutine to clean up AudioSource after it has finished playing
    private IEnumerator DestroyAudioSourceAfterPlay(AudioSource source, Sound sound)
    {
        yield return new WaitWhile(() => source.isPlaying);

        // Remove the AudioSource from the dictionary list and destroy it
        soundDictionary[sound].Remove(source);
        Destroy(source.gameObject);
    }

    // Function to stop a specific sound
    public void StopSound(Sound sound)
    {
        if (soundDictionary.ContainsKey(sound) && soundDictionary[sound].Count > 0)
        {
            // Stop the last playing instance (could be modified to stop a specific instance)
            AudioSource source = soundDictionary[sound][soundDictionary[sound].Count - 1];
            if (source.isPlaying)
            {
                source.Stop();
                soundDictionary[sound].Remove(source);
                Destroy(source.gameObject); // Clean up the AudioSource instance
            }
        }
        else
        {
            Debug.LogWarning("Sound " + sound + " not found or is not playing.");
        }
    }

    // Function to stop all instances of a specific sound
    public void StopAllSounds(Sound sound)
    {
        if (soundDictionary.ContainsKey(sound))
        {
            foreach (AudioSource source in soundDictionary[sound])
            {
                if (source.isPlaying)
                {
                    source.Stop();
                    Destroy(source.gameObject);
                }
            }

            // Clear the list after stopping and destroying all AudioSource instances
            soundDictionary[sound].Clear();
        }
    }

    // Optional: Stop all sounds globally
    public void StopAllSounds()
    {
        foreach (KeyValuePair<Sound, List<AudioSource>> entry in soundDictionary)
        {
            foreach (AudioSource source in entry.Value)
            {
                if (source.isPlaying)
                {
                    source.Stop();
                    Destroy(source.gameObject);
                }
            }
            entry.Value.Clear();
        }
    }
}
