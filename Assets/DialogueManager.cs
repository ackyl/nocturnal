using UnityEngine;
using Ink.Runtime;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextAsset inkJSON; // Assign your Ink JSON file here
    private Story story;

    public TextMeshProUGUI visitorChatUI;

    public TextMeshProUGUI playerResponseUI1;

    public TextMeshProUGUI playerResponseUI2;

    private string _visitorChat;

    private string _playerResponse1;

    private string _playerResponse2;

    private bool chatFinished = false;

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
        story = new Story(inkJSON.text);
        story.variablesState["agent_name"] = "Agent Shadow";

        story.ObserveVariable("mission_status", (string varName, object newValue) =>
        {
            Debug.Log($"Variable '{varName}' updated to: {newValue}");

            if (newValue.ToString() == "triggered")
            {
                Debug.Log("Unity Function Called");
            }
        });

        ContinueStory();
    }

    void ContinueStory()
    {
        if (story.canContinue)
        {
            NextVisitorDialogue();
            NextPlayerChoices();
            UpdateAllDialogueUI();
        }
    }

    void NextVisitorDialogue()
    {
        _visitorChat = story.Continue();
    }

    public void NextPlayerChoices()
    {
        if (story.currentChoices.Count > 0)
        {
            _playerResponse1 = story.currentChoices[0].text;
            _playerResponse2 = story.currentChoices.Count > 1 ? story.currentChoices[1].text : "";
        }
        else
        {
            _playerResponse1 = "";
            _playerResponse2 = "";
        }
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

    private void UpdateAllDialogueUI()
    {
        visitorChatUI.text = _visitorChat;
        playerResponseUI1.text = _playerResponse1;
        playerResponseUI2.text = _playerResponse2;
    }
}
