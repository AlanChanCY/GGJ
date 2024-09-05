using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyDataProvider : MonoBehaviour
{
    public GameObject gameController;
    public AllyData allyData;
    public int allyGroupId = 0;

    public bool player;
    public bool activated = false;
    public bool selected;

    // selection textures
    private Shader whiteShader;
    private Shader normalShader;

    public List<GameObject> shootTargets;
    // Start is called before the first frame update
    void Start()
    {
        // player ship will be the only "true" player ship
        player = gameObject.tag == "Player";
        // an array to hold enemies to fire at
        shootTargets = new List<GameObject>();
        gameController = GameObject.FindGameObjectWithTag("GameController");
        selected = false;

        whiteShader = Shader.Find("GUI/Text Shader");
        normalShader = Shader.Find("Sprites/Default");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FindShootTargets()
    {
        shootTargets = new List<GameObject>();
        // find out which targets are within range
        foreach (EnemyGroup eg in gameController.GetComponent<GameController>().enemyGroups)
        {
            foreach (GameObject g in eg.enemies)
            {
                if ((g.transform.position - gameObject.transform.position).magnitude < allyData.range)
                {
                    shootTargets.Add(g);
                }
            }
        }

    }

    public void Selected()
    {
        gameObject.GetComponent<SpriteRenderer>().material.shader = whiteShader;
        selected = true;
    }
    public void UnSelected()
    {
        gameObject.GetComponent<SpriteRenderer>().material.shader = normalShader;
        selected = false;
    }
}
