using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDataProvider : MonoBehaviour
{
    public GameObject gameController;
    public ShipData shipData;
    public int enemyGroupId;

    public List<GameObject> shootTargets;

    // commander gets a target, the rest follow the commander
    public bool commander;
    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");   
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void FindShootTargets()
    {
        shootTargets = new List<GameObject>();
        // find out which targets are within range
        foreach (AllyGroup a in gameController.GetComponent<GameController>().allyGroups)
        {
            foreach (GameObject g in a.allies)
            {
                if ((g.transform.position - gameObject.transform.position).magnitude < shipData.range)
                {
                    shootTargets.Add(g);
                }
            }                
        }
    }

    public Vector3 FindMoveTargets()
    {
        float minMagnitude = float.MaxValue;
        Vector3 returnVal = Vector3.zero;
        foreach (AllyGroup a in gameController.GetComponent<GameController>().allyGroups)
        {
            foreach (GameObject g in a.allies)
            {
                float dist = (g.transform.position - gameObject.transform.position).magnitude;
                if (dist < minMagnitude)
                {
                    returnVal = g.transform.position;
                    minMagnitude = dist;
                }
            }
        }
        return returnVal;
    }
}
