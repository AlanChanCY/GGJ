using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotDeleter : MonoBehaviour
{
    public float lifetime;
    public float deathTime;
    // Start is called before the first frame update
    void Start()
    {
        deathTime = Time.time + lifetime;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > deathTime)
        {
            Destroy(gameObject);
        }
    }
}
