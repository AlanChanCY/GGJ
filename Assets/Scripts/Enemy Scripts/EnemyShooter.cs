using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    // list of targets in range
    private List<GameObject> targets;    
    private GameObject gameController;

    // last time it shot
    private float nextShot;
    // just so it looks a bit more interesting than a firing squad
    private const float shotVariance = 0.2f;

    // shipData
    private EnemyDataProvider data;


    // Start is called before the first frame update
    void Start()
    {
        targets = new List<GameObject>();
        gameController = GameObject.FindGameObjectWithTag("GameController");
        data = gameObject.GetComponent<EnemyDataProvider>();
        nextShot = 1 / (data.shipData.fireRate + (Random.value - 0.5f) * shotVariance);
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > nextShot)
        {
            Shoot();
            nextShot = Time.time + 1 / (data.shipData.fireRate + (Random.value - 0.5f) * shotVariance);
        }
    }

    void Shoot()
    {
        data.FindShootTargets();
        targets = data.shootTargets;

        // randomly fire at one of them if they can
        if (targets.Count > 0)
        {
            GameObject target = targets[Random.Range(0, targets.Count)];
            // spawn the shot, and set the verticies so it looks nice
            Instantiate(gameController.GetComponent<GameController>().enemyShot).GetComponent<LineRenderer>().SetPositions(new Vector3[] { gameObject.transform.position, target.transform.position });

            target.GetComponent<AllyHealth>().Damage(data.shipData.damagePerShot);
        }        
    }
}
