using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplatScript : MonoBehaviour
{
    //time in which object expires
    float dieTime;
    float size;
    // Start is called before the first frame update
    void Start()
    {
        dieTime = Time.time + 1;
        size = 0.1f;
        this.transform.localScale = new Vector3(size, size, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //increase counter per real time
        if (Time.time > dieTime)
        {
            Destroy(this.gameObject);
        }
        
        //increase size of sprite by frames
        size += 0.01f;
        this.transform.localScale = new Vector3(size, size, 0);
    }
}
