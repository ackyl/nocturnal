using UnityEngine;
using Ink.Runtime;
using TMPro;
using DG.Tweening;

public class DialogueManager : MonoBehaviour
{
    public TextAsset inkJSON; // Assign your Ink JSON file here
    private Story story;
    public TextMeshProUGUI visitorChatUI;

    public TextMeshProUGUI playerResponseUI1;

    public TextMeshProUGUI playerResponseUI2;

    public TextMeshProUGUI characterName;

    public RectTransform documentUI;

    public CanvasGroup canvasGroup;

    private string _visitorChat;

    private string _playerResponse1;

    private string _playerResponse2;

    private bool chatFinished = false;

    private string currentCharacterName = "";

    private string dialogueState;

    private Vector3 originalDocumentUIPosition;

    private string protagonistName = "Victor";


    void Update()
    {
        // // Check if the left arrow key is pressed
        // if (Input.GetKeyDown(KeyCode.LeftArrow))
        // {
        //     MakeChoice(0);
        // }

        // // Check if the right arrow key is pressed
        // if (Input.GetKeyDown(KeyCode.RightArrow))
        // {
        //     MakeChoice(1);
        // }
    }

    void Start()
    {
        // for document stuff
        originalDocumentUIPosition = documentUI.anchoredPosition;
        canvasGroup.DOFade(0, 0).SetEase(Ease.InQuad);

        story = new Story(inkJSON.text);
        // story.variablesState["agent_name"] = "Agent Shadow";
        story.variablesState["protagonist_name"] = protagonistName;

        story.ObserveVariable("dialogue_state", (string varName, object newValue) =>
        {
            Debug.Log($"Variable '{varName}' updated to: {newValue}");

            dialogueState = newValue.ToString();

            if (newValue.ToString() == "get_document")
            {
                ShowDocument();
            }

            if (newValue.ToString() == "finish_document")
            {
                HideDocument();
            }
        });

        story.ObserveVariable("talking", (string varName, object newValue) =>
        {
            // UpdateCharacterName(newValue.ToString());
            if (newValue.ToString() == protagonistName)
            {
                characterName.color = Color.white;
            }
            else
            {
                characterName.color = Color.red;
            }
            TypeText(characterName, newValue.ToString(), 0f);

        });

        ContinueStory();
    }

    void ContinueStory()
    {
        if (story.canContinue)
        {
            NextVisitorDialogue();
        }
    }

    void NextVisitorDialogue()
    {
        _visitorChat = story.Continue();
        ClearChoiceUI();

        TypeText(visitorChatUI, _visitorChat).OnComplete(() =>
        {
            NextPlayerChoices();
        });

        foreach (string tag in story.currentTags)
        {
            Debug.Log(tag);
        }
    }

    public void NextPlayerChoices()
    {
        if (story.currentChoices.Count > 0)
        {
            // if text is X, dont show anything
            _playerResponse1 = story.currentChoices[0].text == "x" ? "" : story.currentChoices[0].text;
            _playerResponse2 = story.currentChoices.Count > 1 ? story.currentChoices[1].text : "";
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
        if (story.canContinue || story.currentChoices.Count > 0)
        {
            if (story.currentChoices.Count > choiceIndex)
            {
                story.ChooseChoiceIndex(choiceIndex);
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

    private void UpdateCharacterName(string name)
    {
        characterName.text = name;
    }

    private Tween TypeText(TextMeshProUGUI textComponent, string fullText, float speed = 1f, bool withSound = false)
    {
        textComponent.text = "";
        return DOTween.To(() => 0, x => SetText(textComponent, fullText, x), fullText.Length, speed).SetEase(Ease.Linear);
    }

    private void SetText(TextMeshProUGUI textComponent, string fullText, int characterCount)
    {
        textComponent.text = fullText.Substring(0, characterCount);
    }

    public void ShowDocument()
    {
        // Reset position and opacity
        documentUI.anchoredPosition = originalDocumentUIPosition + new Vector3(600, 0, 0); // Offset start position
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

        documentUI.DOAnchorPos(originalDocumentUIPosition, 1f).SetEase(Ease.OutQuad); // Move to original position
    }

    public void HideDocument()
    {
        // Disable interaction immediately
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        // Animate to hidden position and opacity
        documentUI.DOAnchorPos(originalDocumentUIPosition + new Vector3(600, 0, 0), 1f).SetEase(Ease.InQuad); // Move downward
        canvasGroup.DOFade(0, 0.5f).SetEase(Ease.InQuad);
    }
}
