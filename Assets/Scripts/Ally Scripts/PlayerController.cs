using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector3 target;

    // healing
    public float healRange;
    public float healRate;
    private float nextHeal;

    private const float deleteMarkerRange = 0.1f;

    private AllyDataProvider data;
    

    // Start is called before the first frame update
    void Start()
    {
        data = gameObject.GetComponent<AllyDataProvider>();
        data.activated = true;
        nextHeal = 0;
    }

    // Update is called once per frame

    private void Update()
    {
        if(Time.time > nextHeal)
        {
            foreach (AllyGroup a in data.gameController.GetComponent<GameController>().allyGroups)
            {
                foreach (GameObject g in a.allies)
                {
                    if ((g.transform.position - gameObject.transform.position).magnitude < healRange)
                    {
                        g.GetComponent<AllyHealth>().Heal();
                    }
                }                   
            }
            nextHeal += 1 / healRate;
        }       
    }
}
