using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndToStart : MonoBehaviour
{
    void Start()
    {
        Button btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(GoToStart);
    }
    public void GoToStart()
    {
        SceneManager.LoadScene("StartScreen", LoadSceneMode.Single);
    }
}
