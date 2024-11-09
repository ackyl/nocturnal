using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class EndingText : MonoBehaviour
{
    // public GameObject overlayObject;
    private TextMeshProUGUI textComponent;
    public SpriteRenderer image1;
    public SpriteRenderer image2;

    public AudioSource screamSound;

    public GameObject typingSound;

    string[] monologueLines = {
    "They say time heals.",
    "But how do you heal when time... stops?",
    "Every second... the explosion never fades...",
    "I thought if I defused it...",
    "The guilt, the fear... it would not stop ticking.",
    "I see their faces, I see hear their screams.",
    "It's all I have left now...",
    "Their smiles... and the silence that follows.",
    "I must close my eyes.",
    "Because at this hour... They are all..."
    };


    public void Awake()
    {
        textComponent = gameObject.GetComponent<TextMeshProUGUI>();
        // overlayImage = overlayObject.GetComponent<Image>();
    }

    public void Start()
    {
        image2.DOFade(0, 0f);
        StartCoroutine(TypeEndingText());
    }

    IEnumerator TypeEndingText()
    {
        int index = 0;
        foreach (var monologueLine in monologueLines)
        {
            index++;
            if (index == 6)
            {
                image1.DOFade(0, 1f).OnComplete(() =>
                {
                    screamSound.Play();
                    image2.DOFade(1, 1f);
                    textComponent.transform.DOShakePosition(20f, 5, 5, 45, false, true);
                    image2.transform.DOShakePosition(4f, 3, 5, 30, false, true);
                });
            }
            TypeText(textComponent, monologueLine, true);
            yield return new WaitForSeconds(5f);
        }

        TypeText(textComponent, "here.", false);
        StartCoroutine(LoadMainMenu());
    }

    private void TypeText(TextMeshProUGUI textComponent, string fullText, bool withSound)
    {
        textComponent.text = "";
        DOTween.To(() => 0, x => SetText(textComponent, fullText, x), fullText.Length, 1.5f).SetEase(Ease.Linear);

        if (withSound)
        {

            StartCoroutine(PlayRandomTypingSoundCoroutine());
        }
    }

    private void SetText(TextMeshProUGUI textComponent, string fullText, int characterCount)
    {
        textComponent.text = fullText.Substring(0, characterCount);
    }

    private IEnumerator PlayRandomTypingSoundCoroutine()
    {
        float totalDuration = 1.5f;   // Total duration to play typing sounds
        float interval = 0.15f;      // Interval between sounds
        float elapsedTime = 0f;     // Track elapsed time

        AudioSource typingAudio = typingSound.GetComponent<AudioSource>();

        while (elapsedTime < totalDuration)
        {
            // Play the typing sound with random pitch
            typingAudio.pitch = Random.Range(0.8f, 1f);
            typingAudio.Play();

            // Wait for the interval
            yield return new WaitForSeconds(interval);

            // Increment elapsed time
            elapsedTime += interval;
        }
    }

    private void PlayRandomTypingSound()
    {
        AudioSource typingAudio = typingSound.GetComponent<AudioSource>();
        typingAudio.pitch = Random.Range(0.5f, 1f);
        typingAudio.Play();
    }

    IEnumerator LoadMainMenu()
    {
        yield return new WaitForSeconds(8f);
        LevelLoader.Instance.LoadMainMenu();
    }
}
