using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EncounterManager : MonoBehaviour
{
    public static EncounterManager instance;

    // ---------------------------- //

    public RectTransform characterUI;

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

    public void GenerateVisitor()
    {
        // reset position
        characterUI.anchoredPosition = new Vector3(-500, -27, 0);

        // get into position
        characterUI.DOAnchorPos(new Vector3(0, -27, 0), 1f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            DialogueManager.instance.ContinueStory();
        });
    }

    public void HideCharacter()
    {
        // hide to right
        characterUI.DOAnchorPos(new Vector3(-500, -27, 0), 1f).SetEase(Ease.InQuad);
    }

    public void CardHover(RectTransform card)
    {
        card.DOAnchorPosY(card.anchoredPosition.y + 18, 0.2f).SetEase(Ease.Linear);
    }

    public void CardUnhover(RectTransform card)
    {
        card.DOAnchorPosY(24, 0.2f).SetEase(Ease.Linear);
    }
}
