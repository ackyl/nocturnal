using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class IntroText : MonoBehaviour
{
    public GameObject overlayObject;
    private TextMeshProUGUI textComponent;
    private Image overlayImage;

    public void Awake()
    {
        textComponent = gameObject.GetComponent<TextMeshProUGUI>();
        overlayImage = overlayObject.GetComponent<Image>();
    }

    public void Start()
    {
        string lyrics = @"Grim night, gun shots
Burning bodies building roadblocks
Phone rang, time stops
Vision heavy they’ll be lights all
On me, get it, we should wash all of our sins
I like to take all you home in this winter and so we can be on their skin 

Killer killer whats your name?
Whats with the mirror it scream
They know who smiles through the highs
They know who bows to their king
We saw where the body goes
They won't admit that we did it
They wont admit they forget it
I get the credits, they livid
I'm in it";

        TypeText(textComponent, lyrics, 0.5f);
        StartCoroutine(LoadNextLevel());
    }

    private void TypeText(TextMeshProUGUI textComponent, string fullText, float delaySpeed)
    {
        // Clear the text first
        textComponent.text = "";

        // Use DOTween's callback functionality to "type" each character
        DOTween.To(() => 0, x => SetText(textComponent, fullText, x), fullText.Length, 25f).SetEase(Ease.Linear);
        // DOTween.To(
        //     () => 0,            // Getter: Current alpha value
        //     x => SetOverlayAlpha(x),               // Setter: Apply new alpha value
        //     1f,                                    // Target value (1 = fully visible)
        //     5f                                     // Duration in seconds
        // );

        overlayImage.DOFade(1f, 20f);
    }

    private void SetText(TextMeshProUGUI textComponent, string fullText, int characterCount)
    {
        // Reveal text up to the current character count
        textComponent.text = fullText.Substring(0, characterCount);
    }

    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(30f);
        LevelLoader.Instance.LoadNextLevel();
    }
}
