using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance { get; private set; }
    public Animator transition;

    public GameObject mainMenuAudio;
    public GameObject riserSound;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        transition.Play("Crossfade_End", 0, 0f);
    }

    public void LoadNextLevel()
    {
        Debug.Log("Loading next level...");
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void LoadMainMenu()
    {
        StartCoroutine(LoadLevel(0));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene(levelIndex);

        transition.ResetTrigger("Start");
    }

    public void FadeMainMenuAudio()
    {
        AudioSource mainMenuSound = mainMenuAudio.GetComponent<AudioSource>();
        AudioSource riserAudio = riserSound.GetComponent<AudioSource>();

        mainMenuSound.DOFade(0f, 1f);
        riserAudio.Play();
        SoundManager.instance.PlaySound(SoundManager.Sound.UIClick);
    }
}
