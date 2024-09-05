using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog : MonoBehaviour
{
    //where 0 is transparent
    float transparent;
    //size should be 2 and bigger
    float size;

    public GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        //enough to see through to background
        transparent = 0.99f;
        //minimum size to view ship and block camera
        size = 2;
        ChangeTransparent(transparent);
        ChangeSize(gameController.visionRad);
    }

    // Update is called once per frame
    void Update()
    {
    }

    //changes transparency at runtime
    public void ChangeTransparent(float trans)
    {
        this.GetComponent<SpriteRenderer>().material.color = new Color(0, 0, 0, trans);
    }

    //changes size at runtime 
    public void ChangeSize(float rad)
    {
        size = rad * 4f;
        this.transform.localScale = new Vector3(size, size, 0);
    }
}
