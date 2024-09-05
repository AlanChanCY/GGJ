using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUps : MonoBehaviour
{
    private const float recruitmentRange = 0.5f;

    private AllyDataProvider data;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        data = gameObject.GetComponent<AllyDataProvider>();

        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (!data.activated && player)
        {
            // when any player gets close enough, become recruited
            foreach (AllyGroup a in data.gameController.GetComponent<GameController>().allyGroups)
            {
                foreach (GameObject g in a.allies)
                {
                    if ((g.transform.position - gameObject.transform.position).magnitude < recruitmentRange)
                    {
                        data.activated = true;
                        // change tag to player so enemies will fire at it
                        gameObject.tag = "Player";
                        // add to the playerFleet so that enemies can target it, add it to the allyGroup of the ally that activated it
                        data.gameController.GetComponent<GameController>().AddAlly(gameObject, g.GetComponent<AllyDataProvider>().allyGroupId);
                        // make the target the same as the one that activated it
                        gameObject.GetComponent<AllyNavigator>().target = g.GetComponent<AllyNavigator>().target;
                        // spawn one more ally
                        data.gameController.GetComponent<PickupSpawner>().DecrementNumSpawned();
                        gameObject.GetComponent<AllyHealth>().ShowBars();
                        break;
                    }
                }
            }
        }        
    }
}

