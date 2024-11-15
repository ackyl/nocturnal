using System;

[System.Serializable]
public class Document
{
    public string dob;              // Date of birth
    public string Id;               // Document ID
    public string nationality;      // Nationality
    public string phone;            // Phone number
    public string email;            // Email address
    public string checkInDate;      // Check-in date
    public string checkOutDate;     // Check-out date
    public string roomType;         // Room type (e.g., Deluxe)
    public string request;          // Special requests
    public string signature;        // Signature (optional)
    public string scribbles;        // Additional notes (optional)
}
