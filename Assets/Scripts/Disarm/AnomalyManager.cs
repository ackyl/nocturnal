using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnomalyManager : MonoBehaviour
{
    public static AnomalyManager instance;

    private float elapsedTime = 0;

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

    void Update()
    {
        if (GameManager.instance.currentGameState == GameManager.GameState.Playing)
        {
            elapsedTime += Time.deltaTime;

            int randomInt = Random.Range(4, 10);

            if (elapsedTime > randomInt)
            {
                TriggerAnomalyOnRandomObject();
                elapsedTime = 0;
            }
        }

    }

    public void TriggerAnomalyOnRandomObject()
    {
        GameObject[] roomObjects = GameObject.FindGameObjectsWithTag("RoomObject");

        if (roomObjects.Length > 0)
        {
            int randomInt = Random.Range(0, roomObjects.Length);
            RoomObject roomObjectInstance = roomObjects[randomInt].GetComponent<RoomObject>();

            if (roomObjectInstance.isDistorted == false)
            {
                roomObjectInstance.DistortObject();
            }
        }
    }

    public void DistortAllObjects()
    {
        GameObject[] roomObjects = GameObject.FindGameObjectsWithTag("RoomObject");

        SoundManager.instance.PlaySound(SoundManager.Sound.Anomaly10);
        SoundManager.instance.PlaySound(SoundManager.Sound.Anomaly7);
        SoundManager.instance.PlaySound(SoundManager.Sound.Anomaly6);
        SoundManager.instance.PlaySound(SoundManager.Sound.Anomaly2);

        if (roomObjects.Length > 0)
        {
            foreach (var roomObject in roomObjects)
            {
                RoomObject roomObjectInstance = roomObject.GetComponent<RoomObject>();
                roomObjectInstance.DistortObject(true);
            }
        }
    }
}
