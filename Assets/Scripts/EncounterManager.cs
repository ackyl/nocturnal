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

    public RectTransform[] cardUI;

    // ---------------------------- //

    private int _activeIndex;

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
        characterUI.DOAnchorPos(new Vector3(500, -27, 0), 1f).SetEase(Ease.InQuad);
    }

    public void CardClick(int index)
    {
        if (index == _activeIndex)
        {
            // when active one clicked again, deactivate it
            CardUnhover(_activeIndex);
            _activeIndex = 99;
        }
        else
        {
            _activeIndex = index;
            CardHover(_activeIndex);
            CardUnhover(_activeIndex == 0 ? 1 : 0);
        }
    }

    public void CardHover(int index)
    {
        RectTransform card = cardUI[index];
        card.DOAnchorPosY(42, 0.2f).SetEase(Ease.Linear);
    }

    public void CardUnhover(int index)
    {
        RectTransform card = cardUI[index];

        if (_activeIndex != index)
        {
            card.DOAnchorPosY(24, 0.2f).SetEase(Ease.Linear);
        }
    }
}
