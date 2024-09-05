using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorTouchScript : MonoBehaviour
{
    Vector3 _box_start_pos = Vector3.zero;
    Vector3 _box_end_pos = Vector3.zero;
    public Texture texture;
    private Camera mainCam;
    private GameController gameController;
    public GameObject marker;

    //for touch controls
    int tapCount = 0;
    float maxDubbleTapTime = 0.3f;
    float newTime;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        gameController = gameObject.GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        BoxSelect();
    }

    void BoxSelect()
    {
        // Called while the user is holding the mouse down.
        if (Input.touchCount > 0)
        {
            // Called on the first update where the user has pressed the mouse button.
            if (Input.GetTouch(0).phase == TouchPhase.Began)
                _box_start_pos = Input.GetTouch(0).position;
            else  // Else we must be in "drag" mode.
                _box_end_pos = Input.GetTouch(0).position;
        }

        else
        {
            // Handle the case where the player had been drawing a box but has now released.
            if (_box_end_pos != Vector3.zero && _box_start_pos != Vector3.zero)
            {
                // change the screen coords to world coords
                Vector3 boxEndWorld = mainCam.ScreenToWorldPoint(_box_end_pos);
                Vector3 boxStartWorld = mainCam.ScreenToWorldPoint(_box_start_pos);
                List<GameObject> selected = new List<GameObject>();
                // check whether any player ships are within the box

                for (int i = 0; i < gameObject.GetComponent<GameController>().allyGroups.Count; i++)
                {
                    for (int j = 0; j < gameObject.GetComponent<GameController>().allyGroups[i].allies.Count; j++)
                    {
                        if (gameObject.GetComponent<GameController>().allyGroups[i].allies[j] && isBetween(gameObject.GetComponent<GameController>().allyGroups[i].allies[j].gameObject.transform.position.x, boxEndWorld.x, boxStartWorld.x) && isBetween(gameObject.GetComponent<GameController>().allyGroups[i].allies[j].gameObject.transform.position.y, boxEndWorld.y, boxStartWorld.y))
                        {
                            gameObject.GetComponent<GameController>().allyGroups[i].allies[j].GetComponent<AllyDataProvider>().Selected();
                            selected.Add(gameObject.GetComponent<GameController>().allyGroups[i].allies[j]);
                        }
                        else
                        {
                            gameObject.GetComponent<GameController>().allyGroups[i].allies[j].GetComponent<AllyDataProvider>().UnSelected();
                        }
                    }
                }
                StartCoroutine(Target(selected));

                // Reset box positions.
                _box_end_pos = _box_start_pos = Vector3.zero;
            }
        }
    }

    /// <summary>
    ///taken from http://answers.unity.com/answers/1176000/view.html
    /// </summary>
    void OnGUI()
    {
        // If we are in the middle of a selection draw the texture.
        if (_box_start_pos != Vector3.zero && _box_end_pos != Vector3.zero)
        {
            // Create a rectangle object out of the start and end position while transforming it
            // to the screen's cordinates.
            Rect rect = new Rect(_box_start_pos.x, Screen.height - _box_start_pos.y,
                _box_end_pos.x - _box_start_pos.x,
                -1 * (_box_end_pos.y - _box_start_pos.y));
            // Draw the texture.
            GUI.DrawTexture(rect, texture);
        }
    }

    private bool isBetween(float target, float bound1, float bound2)
    {
        if (bound1 > bound2)
        {
            return target < bound1 && target > bound2;
        }
        else
        {
            return target < bound2 && target > bound1;
        }
    }

    // target if left click, cancel if right click
    private IEnumerator Target(List<GameObject> selected)
    {
        if (selected.Count > 0)
        {
            yield return new WaitUntil(() => Input.touchCount == 0);
            yield return new WaitUntil(() => Input.touchCount > 0);
            Vector3 target = mainCam.ScreenToWorldPoint(Input.mousePosition);

            // make a new allyGroup
            gameController.allyGroups.Add(new AllyGroup() { allies = new List<GameObject>(), averageVelocity = Vector3.zero, centerOfGroup = Vector3.zero, groupAvoidance = Vector3.zero, id = gameController.allyId });
            for (int i = 0; i < selected.Count; i++)
            {
                if (selected[i])
                {
                    // delete allies from the old group
                    gameController.TransferAlly(selected[i], selected[i].GetComponent<AllyDataProvider>().allyGroupId, gameController.allyId);
                    selected[i].GetComponent<AllyNavigator>().target = target;
                    selected[i].GetComponent<AllyDataProvider>().UnSelected();
                }
            }

            gameController.allyId++;
            tapCount = 0;
            AllyGroup allyGroup = gameController.FindAllyGroup(gameController.allyId - 1);
        }
    }

    public bool DetectTaps()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended)
            {
                tapCount += 1;
            }
            if (tapCount == 1)
            {
                newTime = Time.time + maxDubbleTapTime;
            }
            else if (tapCount == 2 && Time.time <= newTime)
            {
                //Whatever you want after a dubble tap    
                //return true;
            }
        }
        if (Time.time > newTime)
        {
            tapCount = 0;
        }
        return true;
    }
}
