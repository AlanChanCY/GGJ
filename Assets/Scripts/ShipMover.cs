using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMover : MonoBehaviour
{
    private Vector3 velocity;
    private const float speedMultiplier = 0.5f;
    private const float maxVel = 3f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(velocity.magnitude > maxVel)
        {
            velocity = Vector3.zero;
        }
        gameObject.transform.position += velocity * Time.deltaTime;
        
        //if vector is zero dont change rotation 
        if (velocity != Vector3.zero)
        {
            // point towards moving direction
            gameObject.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg - 90);
        }
    }

    public void setVel(Vector3 vel)
    {
        velocity = vel * speedMultiplier;
    }
}
