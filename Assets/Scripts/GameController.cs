using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct EnemyGroup
{
    public List<GameObject> enemies;
    public int id;
    // required data for the movement
    public Vector3 centerOfGroup;
    public Vector3 averageVelocity;
    // make sure groups dont overlap
    public Vector3 groupAvoidance;
}

[System.Serializable]
public struct AllyGroup
{
    public List<GameObject> allies;
    public int id;
    // required data for the movement
    public Vector3 centerOfGroup;
    public Vector3 averageVelocity;
    // make sure groups dont overlap
    public Vector3 groupAvoidance;
}


public class GameController : MonoBehaviour
{
    public List<EnemyGroup> enemyGroups;
    public List<AllyGroup> allyGroups;
    public int allyId;

    public GameObject mainCam;
    public GameObject enemyShot;
    public GameObject allyShot;
    public GameObject player;
    private GameObject spawnedPlayer;

    public GameState gameState;
    public int score;
    public int enemyCost;
    public int enemyGroupLimit;
    private float nextEnemyCostRampTime;
    public float enemyCostRampDelay;


    // allies spawn differently to enemies
    public int allyCostCap;
    public int allyLimit;

    public Vector3 visionCenter;
    public float visionRad;

    // Start is called before the first frame update
    void Start()
    {
        gameState = GameState.Start;
        score = 0;
        
        mainCam = GameObject.FindGameObjectWithTag("MainCamera");
        visionCenter = Vector3.zero;
        // initialize enemy groups
        enemyGroups = new List<EnemyGroup>();

        //ensure player is spawned before doing anything else
        spawnedPlayer = null;
        StartCoroutine("SpawnPlayer");

        allyId = 1;
        nextEnemyCostRampTime = 30;
    }

    // Update is called once per frame
    void Update()
    {
        if(spawnedPlayer)
        {
            visionCenter = mainCam.transform.position;

            // calculate enemy movement data for each enemygroup
            for (int i = 0; i < enemyGroups.Count; i++)
            {
                Vector3 groupAvoidance = Vector3.zero;
                Vector3 avgVel = Vector3.zero;
                // the more costly the ship, the more it affects the average position
                Vector3 avgPos = Vector3.zero;
                foreach (GameObject enemy in enemyGroups[i].enemies)
                {
                    avgVel += enemy.GetComponent<EnemyNavigator>().velocity;
                    avgPos += enemy.transform.position;
                }

                avgPos /= enemyGroups[i].enemies.Count;
                avgVel /= enemyGroups[i].enemies.Count;

                // calculate amount of avoidance for each group
                foreach (EnemyGroup e in enemyGroups)
                {
                    if (e.id != enemyGroups[i].id)
                    {
                        Vector3 relPos = enemyGroups[i].centerOfGroup - e.centerOfGroup;
                        groupAvoidance += relPos / relPos.sqrMagnitude * e.enemies.Count;
                    }
                }
                // put into enemygroup struct
                enemyGroups[i] = new EnemyGroup() { enemies = enemyGroups[i].enemies, id = enemyGroups[i].id, averageVelocity = avgVel, centerOfGroup = avgPos, groupAvoidance = groupAvoidance };
            }

            // calculate ally movement data for each allygroup
            for (int i = 0; i < allyGroups.Count; i++)
            {
                Vector3 groupAvoidance = Vector3.zero;
                Vector3 avgVel = Vector3.zero;
                // the more costly the ship, the more it affects the average position
                Vector3 avgPos = Vector3.zero;
                foreach (GameObject ally in allyGroups[i].allies)
                {
                    avgVel += ally.GetComponent<AllyNavigator>().velocity;
                    avgPos += ally.transform.position;
                }

                avgPos /= allyGroups[i].allies.Count;
                avgVel /= allyGroups[i].allies.Count;

                // calculate amount of avoidance for each group
                foreach (AllyGroup a in allyGroups)
                {
                    if (a.id != allyGroups[i].id)
                    {
                        Vector3 relPos = allyGroups[i].centerOfGroup - a.centerOfGroup;
                        groupAvoidance += relPos / relPos.sqrMagnitude * a.allies.Count;
                    }
                }
                // put into allygroup struct
                allyGroups[i] = new AllyGroup() { allies = allyGroups[i].allies, id = allyGroups[i].id, averageVelocity = avgVel, centerOfGroup = avgPos, groupAvoidance = groupAvoidance };
            }
        }        

        // ramp difficulty
        if(Time.time > nextEnemyCostRampTime)
        {
            enemyCost++;
            nextEnemyCostRampTime = Time.time + enemyCostRampDelay;
        }
    }

    public void FindNewEnemyLeader(int id)
    {
        List<GameObject> enemies = FindEnemyGroup(id).enemies;
        // designate a group leader, highest cost ship
        GameObject leader = null;
        int highestCost = 0;
        foreach (GameObject g in enemies)
        {
            // reset ships' commander settings
            g.GetComponent<EnemyDataProvider>().commander = false;
            // find leader
            if (g.GetComponent<EnemyDataProvider>().shipData.cost > highestCost)
            {
                leader = g;
                highestCost = g.GetComponent<EnemyDataProvider>().shipData.cost;
            }
        }
        leader.GetComponent<EnemyDataProvider>().commander = true;
    }

    // find enemy group by id
    public EnemyGroup FindEnemyGroup(int id)
    {
        return enemyGroups.Find(e => e.id == id);
    }

    // called when an enemy is killed
    public void DeleteEnemy(GameObject enemy, int id, bool leaderKilled)
    {
        // need to identify which group its from
        EnemyGroup enemyGroup = FindEnemyGroup(id);
        enemyGroup.enemies.Remove(enemy);
        // if there are still enemies in the group, dont delete the group
        if (enemyGroup.enemies.Count > 0)
        {
            // if the leader is kiled, a new one needs to be found
            if (leaderKilled)
            {
                FindNewEnemyLeader(id);
            }           
        }
        // if the enemy group is empty, remove it and decrement group count
        else
        {
            enemyGroups.Remove(enemyGroup);
        }
    }

    // find ally group by id
    public AllyGroup FindAllyGroup(int id)
    {
        return allyGroups.Find(e => e.id == id);
    }

    // removing allies from an allyGroup
    public void DeleteAlly(GameObject Ally, int id, bool PlayerKilled)
    {
        AllyGroup allyGroup = FindAllyGroup(id);
        allyGroup.allies.Remove(Ally);
        // this is if the player is removed from any allyGroup at any time
        if(PlayerKilled)
        {
            // confirm whether player is dead
            if(Ally.GetComponent<AllyHealth>().health <= 0)
            {
                SceneManager.LoadScene("EndScreen", LoadSceneMode.Single);
            }
        }
        // removing allyGroups
        if(allyGroup.allies.Count <= 0)
        {
            allyGroups.Remove(allyGroup);
        }
    }

    // adding allies to an allyGroup
    public void AddAlly(GameObject Ally, int id)
    {
        AllyGroup allyGroup = FindAllyGroup(id);
        //Debug.Log(allyGroup.allies.Count);
        allyGroup.allies.Add(Ally);
        Ally.GetComponent<AllyDataProvider>().allyGroupId = id;
    }

    // transferring  allies from an allyGroup to another
    public void TransferAlly(GameObject Ally, int fromId, int toId)
    {
        AddAlly(Ally, toId);
        DeleteAlly(Ally, fromId, Ally.GetComponent<AllyDataProvider>().player);
    }

    private IEnumerator SpawnPlayer()
    {
        yield return new WaitForSeconds(0.1f);
        // spawn player
        spawnedPlayer = Instantiate(player, Vector3.zero, Quaternion.identity);
        // initialize player fleet and put in the player
        allyGroups = new List<AllyGroup>();
        allyGroups.Add(new AllyGroup() { allies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player")), averageVelocity = Vector3.zero, centerOfGroup = Vector3.zero, groupAvoidance = Vector3.zero, id = 0 });

        // initialize camera targetting
        mainCam.GetComponent<Camera2DFollow>().Initialize(spawnedPlayer.transform);
    }

    public void addScore(int s)
    {
        score += s;
    }
}
