using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Text;

public class CodeManager : MonoBehaviour
{
    public static CodeManager instance;

    // ---------------------------- //

    public TextMeshProUGUI codeText;

    // ---------------------------- //

    private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789 ";
    private List<int> revealOrder;

    private string _processingText = "The air hung heavy with a haze of smoke as the fading warmth of July lingered, memories of the 11th hour slipping through like shadows, soft and fleeting, yet etched into the quiet amber of the sky.";
    private string _finalText = "The air hung heavy with a haze of <color=#cb3d65>smoke</color> as the fading warmth of <color=#cb3d65>July</color> lingered, memories of the <color=#cb3d65>11</color>th hour slipping through like shadows, soft and fleeting, yet etched into the quiet amber of the sky.";

    // ---------------------------- //

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Tween GenerateDailyCode()
    {
        StartCoroutine(PlayCodeSoundCoroutine(1.25f));
        return TypeTextWithGlitch(codeText, _processingText, _finalText, 2.4f);
    }


    private Tween TypeTextWithGlitch(TextMeshProUGUI textComponent, string fullText, string fullRichText, float totalDuration)
    {
        textComponent.text = "";

        // Initialize reveal order for randomization
        revealOrder = new List<int>();
        for (int i = 0; i < fullText.Length; i++)
        {
            revealOrder.Add(i);
        }

        // Shuffle the reveal order
        Shuffle(revealOrder);

        return DOTween.To(() => 0, x => SetTextWithGlitch(textComponent, fullText, x), fullText.Length, totalDuration)
                      .SetEase(Ease.OutExpo);
    }

    private void SetTextWithGlitch(TextMeshProUGUI textComponent, string fullText, float progress)
    {
        int visibleCharacters = Mathf.FloorToInt(progress); // Number of characters to display correctly
        StringBuilder currentText = new StringBuilder(new string(' ', fullText.Length)); // Create a placeholder with spaces

        // Reveal characters in the shuffled order
        for (int i = 0; i < visibleCharacters; i++)
        {
            int index = revealOrder[i];
            currentText[index] = fullText[index];
        }

        // Replace remaining characters with random ones
        for (int i = visibleCharacters; i < fullText.Length; i++)
        {
            int index = revealOrder[i];
            currentText[index] = chars[Random.Range(0, chars.Length)];
        }

        // update the ui
        if (progress == fullText.Length)
        {
            textComponent.text = _finalText;
        }
        else
        {
            textComponent.text = currentText.ToString();
        }
    }

    private void Shuffle<T>(List<T> list)
    {
        // Fisher-Yates shuffle algorithm
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    private IEnumerator PlayCodeSoundCoroutine(float totalDuration)
    {
        float interval = 0.125f;
        float elapsedTime = 0f;

        AudioSource typingAudio = gameObject.GetComponent<AudioSource>();

        while (elapsedTime < totalDuration)
        {
            // Wait for the interval
            yield return new WaitForSeconds(interval);

            // Play the typing sound with random pitch
            typingAudio.Play();

            // Increment elapsed time
            elapsedTime += interval;
        }
    }
}
