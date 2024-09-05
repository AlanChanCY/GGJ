using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EnemyWithCost
{
    public GameObject enemy;
    public int cost;
}

public class EnemySpawner : MonoBehaviour
{
    private float visionRad;
    private const float mult = 1.5f;
    [Tooltip("Make sure this is sorted by cost")]
    public EnemyWithCost[] enemies;
    public GameController gameController;
    public float nextSpawn;
    public float spawnDelay;
    // spiral spawning
    private float goldenAngle = Mathf.PI * (3f - Mathf.Sqrt(5)) * Mathf.Rad2Deg;
    private const float spiralExpansion = 0.05f;
    // array to hold where the last 2 spawn attempts where, to avoid spawning in the same place over and over
    private Vector3[] spawnPosList;
    private const float minSpawnProximityMult = 1;

    private int enemyGroupId;

    // Start is called before the first frame update
    void Start()
    {
        gameController = gameObject.GetComponent<GameController>();
        spawnPosList = new Vector3[] { Vector3.zero, Vector3.zero};
        enemyGroupId = 0;
        visionRad = gameController.visionRad;
        nextSpawn += Time.time;

    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > nextSpawn && gameController.enemyGroups.Count < gameController.enemyGroupLimit)
        {
            // generate spawnVector
            Vector3 spawnVector;
            do
            {
                spawnVector = (Vector3)(Random.insideUnitCircle.normalized * visionRad * mult) + gameController.visionCenter;
            }
            // keep generating new spawnVectors if the current one is too close to the last two
            while ((spawnVector - spawnPosList[0]).magnitude < visionRad * minSpawnProximityMult || (spawnVector - spawnPosList[1]).magnitude < visionRad * minSpawnProximityMult);

            // once a good spawnVector is found, add it to the list of the last 2 spawnpositions and remove the last one
            spawnPosList[1] = spawnPosList[0];
            spawnPosList[0] = spawnVector;

            // Create Array to hold all of the enemies in the enemyGroup
            List<GameObject> enemies = new List<GameObject>();

            // cumulative cost of the spawning
            int remainingCost = gameController.enemyCost;
            // vector to make sure the enemies dont spawn on each other
            Vector3 spawnVariance = Vector3.up * 0.5f;
            // index of the array to take the enemy to spawn from
            int enemyIndex = 0;

            // spawn enemies randomly around the spawnVector
            while(remainingCost > 0)
            {
                // spawn enemy if there is enough cost to do so
                if (this.enemies[enemyIndex].cost <= remainingCost)
                {               
                    // append to enemyGroup
                    enemies.Add(Instantiate(this.enemies[enemyIndex].enemy, spawnVector + spawnVariance, Quaternion.identity));
                    // decrease remaining cost
                    remainingCost -= this.enemies[enemyIndex].cost;
                }                

                // chance to shift down from spawning a few strong enemies to spawning many weaker enemies (if not at the end of the array)
                if(enemyIndex < this.enemies.Length - 1)
                {
                    // shift down with increasing chances as remainingCost decreases
                    if (remainingCost > 0 && Random.value < this.enemies[enemyIndex].cost / remainingCost)
                    {
                        enemyIndex++;
                    }
                   
                }
                
                // move the spawnVariance across a golden angle to avoid overlapping
                spawnVariance = (Quaternion.Euler(0, 0, goldenAngle) * spawnVariance);
                // increase the length of the spawnvariance vector to avoid overlapping
                spawnVariance = spawnVariance * (spawnVariance.magnitude + spiralExpansion) / spawnVariance.magnitude;
            }

            // delay the next spawn
            nextSpawn = Time.time + spawnDelay;

            // create a new enemyGroup
            EnemyGroup enemyGroup = new EnemyGroup() { enemies = enemies, id = enemyGroupId };
            enemyGroupId++;
            // give the enemyGroup to the gameController for safe keeping
            gameController.enemyGroups.Add(enemyGroup);

            // tell enemies which enemygroup they belong to
            foreach (GameObject g in enemies)
            {
                // set enemygroup ids
                g.GetComponent<EnemyDataProvider>().enemyGroupId = enemyGroup.id;
            }

            gameController.FindNewEnemyLeader(enemyGroup.id);
        }        
    }    
}
