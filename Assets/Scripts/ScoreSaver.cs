using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSaver : MonoBehaviour
{
    // Start is called before the first frame update    
    private void OnDisable()
    {
        PlayerPrefs.SetInt("score", gameObject.GetComponent<GameController>().score);
    }
}
