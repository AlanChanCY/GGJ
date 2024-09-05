using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyNavigator : MonoBehaviour
{
    private GameController gameController;
    private AllyDataProvider data;

    // required data for the movement
    private Vector3 centerOfGroup;
    private Vector3 averageVelocity;
    private Vector3 avoidanceDirection;

    // marker deletion
    private const float markerDeleteRange = 0.4f;

    private const float epsilon = 0.1f;
    private const float targetMult = 2f;

    public Vector3 velocity;
    public Vector3 target;

    // Start is called before the first frame update
    void Start()
    {
        data = gameObject.GetComponent<AllyDataProvider>();
        gameController = data.gameController.GetComponent<GameController>();
        target = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if(data.activated)
        {
            // need to update every frame since enemies can die and the group will be updated
            AllyGroup allyGroup = data.gameController.GetComponent<GameController>().FindAllyGroup(data.allyGroupId);

            // calculate where the ship will go next
            centerOfGroup = (allyGroup.centerOfGroup - gameObject.transform.position) * data.allyData.cohesionMult;
            averageVelocity = allyGroup.averageVelocity * data.allyData.velocityMult;

            // need to calculate avoidance force individually :(
            avoidanceDirection = Vector3.zero;
            foreach (GameObject a in allyGroup.allies)
            {
                // dont consider itself
                if (a != gameObject)
                {
                    // the closer a ship is to another, the more it'll turn away, and if its a more costly ship, there will be even more turning away
                    Vector3 relPos = gameObject.transform.position - a.transform.position;
                    avoidanceDirection += relPos / relPos.sqrMagnitude * data.allyData.avoidanceMult * (1 + a.GetComponent<AllyDataProvider>().allyData.cost * 0.01f);
                }
            }
            // set velocity for ship
            velocity = (centerOfGroup + averageVelocity + avoidanceDirection + allyGroup.groupAvoidance + ((target - gameObject.transform.position).magnitude < epsilon ? Vector3.zero : target - gameObject.transform.position)*targetMult);
            velocity = velocity.magnitude > epsilon ? velocity.normalized : Vector3.zero;

        }
        else
        {
            velocity = Vector3.zero;
        }

        // set for shipMover
        gameObject.GetComponent<ShipMover>().setVel(velocity * data.allyData.speed);
    }
}
