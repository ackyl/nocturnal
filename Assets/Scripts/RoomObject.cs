using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomObject : MonoBehaviour
{
    public RoomObjectSO roomObjectSO;
    public Material anomalyMaterial;
    public Material[] anomalyMaterials;
    public Material defaultMaterial;

    private bool isPressed = false;
    private float pressTime = 0f;
    private float holdDuration = 1f;

    public bool isDistorted = false;

    void Update()
    {
        // if (isPressed)
        // {
        //     pressTime += Time.deltaTime;
        //     GameManager.instance.UpdateRemovalLoader(pressTime * 4);
        //     if (pressTime >= holdDuration)
        //     {
        //         OnCompletePress();
        //         isPressed = false;
        //         pressTime = 0f;
        //     }
        // }
    }

    void OnMouseDown()
    {
        // if (isDistorted)
        // {
        //     isPressed = true;
        //     pressTime = 0f;
        // }
        if (isDistorted)
        {
            GameManager.instance.StartAnomalyFixing();
        }
    }

    void OnMouseUp()
    {
        // isPressed = false;
        // pressTime = 0f;
        // GameManager.instance.UpdateRemovalLoader(0);
        bool fixSuccess = GameManager.instance.EndAnomalyFixing();

        if (fixSuccess)
        {
            OnCompletePress();
        }
        else
        {
            if (isDistorted)
            {
                SoundManager.instance.PlaySound(SoundManager.Sound.FailFix);
            }
        }
    }

    void OnCompletePress()
    {
        SoundManager.instance.PlaySound(SoundManager.Sound.AnomalyRemoved);
        gameObject.GetComponent<SpriteRenderer>().material = defaultMaterial;
        GameManager.instance.UpdateRemovalLoader(0);
        isDistorted = false;

        ScoreManager.instance.UpdateScore();
        // SoundManager.instance.StopSound(SoundManager.Sound.AnomalyTestSound);
    }

    void OnValidate()
    {
        if (roomObjectSO != null)
        {
            SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            BoxCollider2D boxCollider = gameObject.GetComponent<BoxCollider2D>();

            spriteRenderer.sprite = roomObjectSO.sprite;

            Vector2 spriteSize = spriteRenderer.sprite.bounds.size;

            boxCollider.size = spriteSize;
            boxCollider.offset = spriteRenderer.sprite.bounds.center;
        }
    }

    public void DistortObject(bool distortAll = false)
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        int randomInt = Random.Range(0, anomalyMaterials.Length);
        spriteRenderer.material = anomalyMaterials[randomInt];


        List<SoundManager.Sound> soundList = new List<SoundManager.Sound>
        {
            SoundManager.Sound.Anomaly1,
            SoundManager.Sound.Anomaly2,
            SoundManager.Sound.Anomaly3,
            SoundManager.Sound.Anomaly4,
            SoundManager.Sound.Anomaly5,
            SoundManager.Sound.Anomaly6,
            SoundManager.Sound.Anomaly7,
            SoundManager.Sound.Anomaly8,
            SoundManager.Sound.Anomaly9,
            SoundManager.Sound.Anomaly10,
            SoundManager.Sound.Anomaly11,
            SoundManager.Sound.Anomaly12
        };

        if (!distortAll)
        {
            SoundManager.instance.PlaySound(soundList[Random.Range(0, soundList.Count)]);
        }

        isDistorted = true;

        // Debug.Log("Anomaly Happened!");
    }
}
