using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DayManager : MonoBehaviour
{
    public static DayManager instance;

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
        CodeManager.instance.GenerateDailyCode().OnComplete(() =>
        {
            DOVirtual.DelayedCall(1.25f, () =>
            {
                EncounterManager.instance.GenerateVisitor();
            });
        });
    }
}
