using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceMarker : MonoBehaviour
{

    Vector3 mousePos;
    public GameObject Marker;
    int markerCount = 0;







    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


    }

    void PlaceMark()
    {
        if (Input.GetMouseButton(1) && markerCount == 0)
        {
            mousePos = Input.mousePosition;
            mousePos.z = -1; //so that marker isnt convered by background
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            Instantiate(Marker, mousePos, new Quaternion());
            markerCount++;
        }
    }

    public void ResetMarkCount()
    {
        markerCount = 0;
    }

    /// <summary>
    ///taken from http://answers.unity.com/answers/1176000/view.html
    /// </summary>

}
