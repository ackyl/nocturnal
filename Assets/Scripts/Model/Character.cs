using System;

[System.Serializable]
public class Character
{
    public int number;              // The character number
    public string id;               // Unique character ID
    public string fullName;         // Full name of the character
    public string identity;         // Civilian or agent
    public Document doc;            // Nested document details
}
