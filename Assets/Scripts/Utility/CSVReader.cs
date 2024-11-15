using System.Collections.Generic;
using UnityEngine;

public class CSVReader : MonoBehaviour
{
    [SerializeField] private TextAsset csvFile; // Assign your CSV file in the Inspector

    void Start()
    {
        List<Document> documents = ParseCSV(csvFile.text);

        foreach (var doc in documents)
        {
            Debug.Log($"Email: {doc.email}");
        }
    }

    List<Document> ParseCSV(string csvText)
    {
        List<Document> documents = new List<Document>();
        string[] lines = csvText.Split('\n');

        if (lines.Length <= 1) return documents; // Skip empty or header-only CSV

        string[] headers = lines[0].Split(',');
        for (int i = 1; i < lines.Length; i++) // Skip header row
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue; // Skip empty lines

            string[] values = lines[i].Split(',');
            Document doc = new Document
            {
                email = GetValue(headers, values, "doc.email"),
            };
            documents.Add(doc);
        }
        return documents;
    }

    string GetValue(string[] headers, string[] values, string key)
    {
        int index = System.Array.IndexOf(headers, key);
        return index >= 0 && index < values.Length ? values[index] : string.Empty;
    }
}
