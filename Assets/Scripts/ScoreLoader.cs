using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreLoader : MonoBehaviour
{
    public int score = 0;
    void OnEnable()
    {
        score = PlayerPrefs.GetInt("score");
        gameObject.GetComponent<Text>().text = "GameOver \n Score: " + score.ToString();
    }
}
