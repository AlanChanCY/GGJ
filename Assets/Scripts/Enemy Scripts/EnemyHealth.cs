using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health;
    private EnemyDataProvider data;

    public GameObject splat;
    // Start is called before the first frame update
    void Start()
    {
        data = gameObject.GetComponent<EnemyDataProvider>();
        health = data.shipData.health;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Damage(int dmg)
    {
        health -= dmg;
        if(health <= 0)
        {
            GameObject.Instantiate(splat, this.transform.position, this.transform.rotation);
            data.gameController.GetComponent<GameController>().DeleteEnemy(gameObject, data.enemyGroupId, data.commander);
            data.gameController.GetComponent<GameController>().addScore(data.shipData.cost);
            Destroy(gameObject);
        }      
    }
}
