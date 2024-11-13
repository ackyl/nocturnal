using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Highscore : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TextMeshProUGUI highScoreText = gameObject.GetComponent<TextMeshProUGUI>();

        int highScore = ES3.Load<int>("highScore", 0);
        highScoreText.text = $"Highscore: {highScore}";
    }
}
