using UnityEngine;

[CreateAssetMenu(fileName = "RoomObject", menuName = "ScriptableObjects/RoomObject", order = 1)]

public class RoomObjectSO : ScriptableObject
{
    public string objectName;

    public Sprite sprite;
}