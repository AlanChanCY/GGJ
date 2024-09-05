using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AllyWithCost
{
    public GameObject ally;
    public int cost;
}
public class PickupSpawner : MonoBehaviour
{
    private float visionRad;
    private const float mult = 1.05f;
    [Tooltip("Make sure this is sorted by cost")]
    public List<AllyWithCost> allies;
    public GameController gameController;
    public float nextSpawn;
    public float spawnDelay;
    // array to hold where the last 2 spawn attempts where, to avoid spawning in the same place over and over
    private Vector3[] spawnPosList;
    private const float minSpawnProximityMult = 1;
    private int numAlliesSpawned;

    // Start is called before the first frame update
    void Start()
    {
        gameController = gameObject.GetComponent<GameController>();
        spawnPosList = new Vector3[] { Vector3.zero, Vector3.zero };
        visionRad = gameController.visionRad;
        numAlliesSpawned = 0;
        nextSpawn += Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextSpawn && numAlliesSpawned < gameController.allyLimit)
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

            // spawning will be done such that allies will spawn as normal, but the spawn delay will be reduced by an amount proportional to the difference in cost between the allyCostCap and the spawned ally cost

            // pick allies that have less cost than cost cap
            List<AllyWithCost> spawnable = allies.FindAll(a => (a.ally.GetComponent<AllyDataProvider>().allyData.cost <= gameController.allyCostCap));

            GameObject spawned = Instantiate(spawnable[Random.Range(0, spawnable.Count)].ally, spawnVector, Quaternion.identity);            

            // delay the next spawn, with variance on how valuable the ally spawned was
            nextSpawn = Time.time + spawnDelay * (1 + spawned.GetComponent<AllyDataProvider>().allyData.cost / gameController.allyCostCap);

            // increment number of pickups spawned
            numAlliesSpawned++;
        }
    }

    public void DecrementNumSpawned()
    {
        numAlliesSpawned--;
    }
}
