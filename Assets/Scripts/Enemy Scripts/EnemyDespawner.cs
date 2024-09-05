using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDespawner : MonoBehaviour
{
    private float enemyDespawnRange;
    private EnemySpawner enemySpawner;
    private GameController gameController;
    private const float mult = 3f;

    // dont wanna call the despawn checker often cuz its kinda expensive
    private float nextDespawnCheck;
    private const float despawnRate = 5;
    // Start is called before the first frame update
    void Start()
    {
        enemySpawner = gameObject.GetComponent<EnemySpawner>();
        gameController = gameObject.GetComponent<GameController>();
        enemyDespawnRange = gameController.visionRad * mult;
        nextDespawnCheck = despawnRate + Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > nextDespawnCheck)
        {
            enemyDespawnRange = gameController.visionRad * mult;
            CheckForDespawnableGroups();
            nextDespawnCheck += despawnRate;
        }
    }

    void CheckForDespawnableGroups()
    {
        for (int i = 0; i < gameController.enemyGroups.Count; i++)
        {
            // average position of the enemyGroup
            Vector3 averagePosition = gameController.enemyGroups[i].centerOfGroup;
            if ((gameController.visionCenter - averagePosition).magnitude > enemyDespawnRange)
            {
                DespawnGroup(i);
            }            
        }
    }

    void DespawnGroup(int index)
    {
        foreach(GameObject g in gameController.enemyGroups[index].enemies)
        {
            Destroy(g);
        }
        gameController.enemyGroups.RemoveAt(index);
    }
}
