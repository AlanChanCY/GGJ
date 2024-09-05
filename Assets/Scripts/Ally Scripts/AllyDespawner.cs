using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyDespawner : MonoBehaviour
{
    private float allyDespawnRange;
    private PickupSpawner pickupSpawner;
    private GameController gameController;
    private const float mult = 4f;

    // dont wanna call the despawn checker often cuz its kinda expensive
    private float nextDespawnCheck;
    private const float despawnRate = 5;
    // Start is called before the first frame update
    void Start()
    {
        pickupSpawner = gameObject.GetComponent<PickupSpawner>();
        gameController = gameObject.GetComponent<GameController>();
        allyDespawnRange = gameController.visionRad * mult;
        nextDespawnCheck = despawnRate + Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextDespawnCheck)
        {
            allyDespawnRange = gameController.visionRad * mult;
            CheckForDespawnableAllies();
            nextDespawnCheck += despawnRate;
        }
    }

    void CheckForDespawnableAllies()
    {
        // check for pickups that are too far away
        GameObject[] pickups = GameObject.FindGameObjectsWithTag("PickUps");
        for (int i = 0; i < pickups.Length; i++)
        {
            if((pickups[i].transform.position - gameController.visionCenter).magnitude > allyDespawnRange)
            {
                // just destroy the pickups since they're not linked to anything
                Destroy(pickups[i]);
                pickupSpawner.DecrementNumSpawned();
            }
        }

        List<GameObject> toDelete = new List<GameObject>();
        // check for allies that are too far away
        foreach(AllyGroup a in gameController.allyGroups)
        {
            foreach(GameObject g in a.allies)
            {
                if((g.transform.position - gameController.visionCenter).magnitude > allyDespawnRange)
                {
                    // collect the allies that must die
                    toDelete.Add(g);
                }
            }
        }
        foreach(GameObject g in toDelete)
        {
            // null check just in case the ally dies between the collection and the deletion
            if(g)
            {
                // delete the ally properly
                gameController.DeleteAlly(g, g.GetComponent<AllyDataProvider>().allyGroupId, g.GetComponent<AllyDataProvider>().player);
                Destroy(g);
            }
        }



    }
}
