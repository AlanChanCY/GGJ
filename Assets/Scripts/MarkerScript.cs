using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerScript : MonoBehaviour
{
    private AllyDataProvider data;
    private GameObject player;

    float dieTime;
    bool touched;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        data = gameObject.GetComponent<AllyDataProvider>();
        //for 10 seconds
        dieTime = 10;
        touched = false;
    }

    // Update is called once per frame
    void Update()
    {
        //using the same method as player getting pickups
        if(player)
        {
            foreach (AllyGroup a in data.gameController.GetComponent<GameController>().allyGroups)
            {
                foreach (GameObject g in a.allies)
                {
                    if ((g.transform.position - gameObject.transform.position).magnitude < 1)
                    {
                        touched = true;
                    }
                }
            }
        }
        //alternate way of ending marker if player switches targets before marker reached
        if (Time.time > dieTime)
        {
            touched = true;
        }
        dieTime = Time.time + 1;

        if (touched)
        {
            Destroy(this.gameObject);
        }
    }
}
