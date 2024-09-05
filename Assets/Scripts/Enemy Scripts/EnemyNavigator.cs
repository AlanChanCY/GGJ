using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNavigator : MonoBehaviour
{
    private GameController gameController;
    private EnemyDataProvider data;

    // required data for the movement
    private Vector3 centerOfGroup;
    private Vector3 averageVelocity;
    private Vector3 avoidanceDirection;

    // tuning multipliers for affecting how the enemies "flock"
    public float cohesionMult;
    public float velocityMult;
    public float avoidanceMult;
    public float groupAvoidanceMult;

    public Vector3 velocity;
    // only for commander
    public Vector3 target;
    public const float startMovingMult = 1.3f;


    // Start is called before the first frame update
    void Start()
    {
        data = gameObject.GetComponent<EnemyDataProvider>();
        gameController = data.gameController.GetComponent<GameController>();
        target = new Vector3(3, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // need to update every frame since enemies can die and the group will be updated
        EnemyGroup enemyGroup = gameController.FindEnemyGroup(data.enemyGroupId);

        // calculate where the ship will go next
        centerOfGroup = (enemyGroup.centerOfGroup - gameObject.transform.position) * cohesionMult;
        averageVelocity = enemyGroup.averageVelocity * velocityMult;

        // need to calculate avoidance force individually :(
        avoidanceDirection = Vector3.zero;
        foreach (GameObject e in enemyGroup.enemies)
        {
            // dont consider itself
            if (e != gameObject)
            {
                // the closer a ship is to another, the more it'll turn away, and if its a more costly ship, there will be even more turning away
                Vector3 relPos = gameObject.transform.position - e.transform.position;
                avoidanceDirection += relPos / relPos.sqrMagnitude * avoidanceMult * e.GetComponent<EnemyDataProvider>().shipData.cost;
            }
        }
        // set velocity for ship
        velocity = (centerOfGroup + averageVelocity + avoidanceDirection + enemyGroup.groupAvoidance).normalized;

        // the commander will lead, so it has a target to go to, if it is within vision radius
        if (data.commander)
        {
            target = data.FindMoveTargets();
            if ((gameObject.transform.position - target).magnitude < gameController.visionRad * startMovingMult)
            {                
                velocity = (velocity + target - gameObject.transform.position).normalized;                
            }
            else
            {
                velocity = Vector3.zero;
            }
        }

        // set for shipMover
        gameObject.GetComponent<ShipMover>().setVel(velocity * data.shipData.speed);
    }
}
