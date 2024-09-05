using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyHealth : MonoBehaviour
{
    public int health;
    private AllyDataProvider data;
    public GameObject healthBarBack;
    public GameObject healthBar;

    private const float barLength = 0.2f;
    private const float barDown = -0.3f;
    private const float thickness = 0.1f;

    private Vector3 healthBarStart = new Vector3(-barLength, barDown, 0);
    private Vector3 healthBarEnd = new Vector3(barLength, barDown, 0);

    public GameObject splat;

    // Start is called before the first frame update
    void Start()
    {
        data = gameObject.GetComponent<AllyDataProvider>();
        health = data.allyData.health;

        healthBar = Instantiate(healthBar, gameObject.transform.position, Quaternion.identity, gameObject.transform);
        healthBar.transform.parent = gameObject.transform;
        healthBar.GetComponent<LineRenderer>().startWidth = healthBar.GetComponent<LineRenderer>().endWidth = data.player ? thickness : 0;
        healthBar.GetComponent<LineRenderer>().sortingOrder = 6;
        healthBarBack = Instantiate(healthBarBack, gameObject.transform.position, Quaternion.identity, gameObject.transform);
        healthBarBack.transform.parent = gameObject.transform;
        healthBarBack.GetComponent<LineRenderer>().startWidth = healthBarBack.GetComponent<LineRenderer>().endWidth = data.player ? thickness : 0;
        healthBarBack.GetComponent<LineRenderer>().sortingOrder = 5;

    }

    // Update is called once per frame
    void Update()
    {
        updateHealthBar(data.activated);
        updateHealthBarBack(data.activated);
    }

    public void Damage(int dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            GameObject.Instantiate(splat, this.transform.position,this.transform.rotation);
            data.gameController.GetComponent<GameController>().DeleteAlly(gameObject, data.allyGroupId, data.player);
            Destroy(gameObject);
        }
    }

    public void Heal()
    {
        if(health < data.allyData.health)
        {
            health++;
        }
        
    }

    private void updateHealthBarBack(bool activated)
    {
        healthBarBack.GetComponent<LineRenderer>().SetPositions(new Vector3[] { gameObject.transform.position + healthBarStart, gameObject.transform.position + healthBarEnd });
    }
    private void updateHealthBar(bool activated)
    {
        healthBar.GetComponent<LineRenderer>().SetPositions(new Vector3[] { gameObject.transform.position + healthBarStart, gameObject.transform.position + healthBarStart + new Vector3(2f * barLength * (float) health / (float) data.allyData.health,0,0)});
    }

    public void ShowBars()
    {
        healthBarBack.GetComponent<LineRenderer>().startWidth = healthBarBack.GetComponent<LineRenderer>().endWidth = thickness;
        healthBar.GetComponent<LineRenderer>().startWidth = healthBar.GetComponent<LineRenderer>().endWidth = thickness;
    }
}
