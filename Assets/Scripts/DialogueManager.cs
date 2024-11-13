using UnityEngine;
using System.Collections;
using Ink.Runtime;
using TMPro;
using DG.Tweening;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    // ---------------------------- //

    public TextAsset inkJSON;

    public TextMeshProUGUI visitorChatUI;

    public TextMeshProUGUI playerResponseUI1;

    public TextMeshProUGUI playerResponseUI2;

    public TextMeshProUGUI characterName;

    public RectTransform documentUI;

    public CanvasGroup canvasGroup;

    // ---------------------------- //

    private Story _story;

    private string _visitorChat;

    private string _playerResponse1;

    private string _playerResponse2;

    private Vector3 _originalDocumentUIPosition;

    private string _protagonistName = "Victor";

    private string _dialogueState;

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

    void Start()
    {
        // hide document on initial dialogue
        // record document initial position
        _originalDocumentUIPosition = documentUI.anchoredPosition;
        canvasGroup.DOFade(0, 0).SetEase(Ease.InQuad);

        // initiate _story and protagonist_name, visitor_name is inside the ink script
        _story = new Story(inkJSON.text);
        _story.variablesState["protagonist_name"] = _protagonistName;

        // handle dialogue state specific changes
        _story.ObserveVariable("dialogue_state", (string varName, object newValue) =>
        {
            Debug.Log($"Variable '{varName}' updated to: {newValue}");

            _dialogueState = newValue.ToString();

            if (newValue.ToString() == "get_document")
            {
                ShowDocument();
            }

            if (newValue.ToString() == "finish_document")
            {
                HideDocument();
            }
        });

        // update character name when talking value changes
        _story.ObserveVariable("talking", (string varName, object newValue) =>
        {
            if (newValue.ToString() == _protagonistName)
            {
                if (ColorUtility.TryParseHtmlString("#aee0ab", out Color color))
                {
                    characterName.color = color;
                }
            }
            else
            {
                if (ColorUtility.TryParseHtmlString("#fff2b3", out Color color))
                {
                    characterName.color = color;
                }
            }

            characterName.text = newValue.ToString();
        });
    }

    public void ContinueStory()
    {
        if (_story.canContinue)
        {
            NextVisitorDialogue();
        }
    }

    void NextVisitorDialogue()
    {
        _visitorChat = _story.Continue();
        ClearChoiceUI();

        TypeText(visitorChatUI, _visitorChat).OnComplete(() =>
        {
            NextPlayerChoices();
        });
    }

    public void NextPlayerChoices()
    {
        if (_story.currentChoices.Count > 0)
        {
            // if text is X, dont show anything. used for card choices.
            _playerResponse1 = _story.currentChoices[0].text == "x" ? "" : _story.currentChoices[0].text;
            _playerResponse2 = _story.currentChoices.Count > 1 ? _story.currentChoices[1].text : "";
        }
        else
        {
            _playerResponse1 = "";
            _playerResponse2 = "";
        }

        UpdateChoiceUI();
    }


    public void MakeChoice(int choiceIndex)
    {
        if (_story.canContinue || _story.currentChoices.Count > 0)
        {
            if (_story.currentChoices.Count > choiceIndex)
            {
                _story.ChooseChoiceIndex(choiceIndex);
                ContinueStory();
            }
        }
    }

    private void ClearChoiceUI()
    {
        playerResponseUI1.text = "";
        playerResponseUI2.text = "";
    }
    private void UpdateChoiceUI()
    {
        // visitorChatUI.text = _visitorChat;
        playerResponseUI1.text = _playerResponse1;
        playerResponseUI2.text = _playerResponse2;
    }

    private Tween TypeText(TextMeshProUGUI textComponent, string fullText, float speed = 0.03f, bool withSound = false)
    {
        textComponent.text = "";
        float totalDuration = fullText.Length * speed;

        // play talking sounds
        StartCoroutine(PlayRandomTypingSoundCoroutine(totalDuration));

        // need to do .OnComplete when this function called so return the tween
        return DOTween.To(() => 0, x => SetText(textComponent, fullText, x), fullText.Length, totalDuration).SetEase(Ease.Linear);
    }

    private void SetText(TextMeshProUGUI textComponent, string fullText, int characterCount)
    {
        textComponent.text = fullText.Substring(0, characterCount);
    }

    public void ShowDocument()
    {
        // Reset position and opacity
        documentUI.anchoredPosition = _originalDocumentUIPosition + new Vector3(600, 0, 0); // Offset start position
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        // Animate to visible position and opacity
        canvasGroup.DOFade(1, 0.5f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            // Enable interaction once fully visible
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        });

        documentUI.DOAnchorPos(_originalDocumentUIPosition, 1f).SetEase(Ease.OutQuad); // Move to original position
    }

    public void HideDocument()
    {
        // Disable interaction immediately
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        // Animate to hidden position and opacity
        documentUI.DOAnchorPos(_originalDocumentUIPosition + new Vector3(600, 0, 0), 1f).SetEase(Ease.InQuad); // Move downward
        canvasGroup.DOFade(0, 0.5f).SetEase(Ease.InQuad);
    }

    private IEnumerator PlayRandomTypingSoundCoroutine(float totalDuration)
    {
        float interval = 0.12f;
        float elapsedTime = 0f;

        AudioSource typingAudio = gameObject.GetComponent<AudioSource>();

        Debug.Log(typingAudio.volume);

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
}
