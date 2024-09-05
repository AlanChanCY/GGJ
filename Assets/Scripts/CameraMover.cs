using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    private float speed = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float xVal = 0;
        float yVal = 0;
        if (Input.GetKey(KeyCode.A))
            xVal -= speed;
        if (Input.GetKey(KeyCode.D))
            xVal += speed;
        if (Input.GetKey(KeyCode.W))
            yVal += speed;
        if (Input.GetKey(KeyCode.S))
            yVal -= speed;

        gameObject.transform.position = new Vector3(gameObject.transform.position.x + xVal, gameObject.transform.position.y + yVal, gameObject.transform.position.z);
    }
}
