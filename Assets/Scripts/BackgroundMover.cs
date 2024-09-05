using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// To give the impression that the background is moving without having an infinately sized background image
public class BackgroundMover : MonoBehaviour
{
    // constant background template
    public GameObject backGround;
    // camera
    public GameObject camera;
    // defines which corner of the current background the camera is above
    private CameraBGPos camBGPos;
    // x and y dimensions of the sprite
    private Vector3 spriteSize;
    // tracks which background the camera is over currently
    private Vector3 bgCoord;
    // camera position
    Vector3 camPos;

    private const float zVal = 10;
    // Start is called before the first frame update

    // holds the instantiated background gameobjects -> order is CCW ie(center, right, topright, top   or center, left, leftbottom, bottom)
    private GameObject[] bgArray = { null, null, null, null };
    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        bgCoord = Vector3.zero;
        spriteSize = backGround.GetComponent<SpriteRenderer>().size;
        // get camera position initially
        camPos = camera.transform.position;
        // establish the camera's position relative to the background
        camBGPos = (CameraBGPos)(camPos.y > bgCoord.y * spriteSize.y ? 2 : 0) + (camPos.x > bgCoord.x * spriteSize.x ? 1 : 0);
        createBackGrounds(camBGPos);
    }

    // Update is called once per frame
    void Update()
    {
        // get camera position
        camPos = camera.transform.position;
        // shift current background indexing so that the camera is always on top of the current background
        bgCoord.x = Mathf.RoundToInt(camPos.x / spriteSize.x);
        bgCoord.y = Mathf.RoundToInt(camPos.y / spriteSize.y);

        // store previous camBGPos
        CameraBGPos prevCamBGPos = camBGPos;

        // determine where the camera is relative to the current background center
        // this is done by using the enum: Top half:   first bit = 0, Bottom half: first bit = 1          
        //                                 Left half: second bit = 0, Right half: second bit = 1
        camBGPos = (CameraBGPos)(camPos.y < bgCoord.y * spriteSize.y ? 2 : 0) + (camPos.x > bgCoord.x * spriteSize.x ? 1 : 0);

        // if there is a change in the position of the camera relative to the background update the backgrounds
        if(camBGPos != prevCamBGPos)
        {
            deleteBackGrounds();
            createBackGrounds(camBGPos);
        }      
    }

    void deleteBackGrounds()
    {
        foreach (GameObject bg in bgArray)
        {
            Destroy(bg);
        }
    }

    void createBackGrounds(CameraBGPos camBGPos)
    {
        // spawn in the center background
        bgArray[0] = Instantiate(backGround, new Vector3(bgCoord.x * spriteSize.x, bgCoord.y * spriteSize.y, zVal), Quaternion.identity, gameObject.transform);

        // spawn in other backgrounds according to camBgPos
        if(camBGPos == CameraBGPos.TopLeft)
        {
            bgArray[1] = Instantiate(backGround, new Vector3(bgCoord.x * spriteSize.x, (bgCoord.y + 1) * spriteSize.y, zVal), Quaternion.identity, gameObject.transform);
            bgArray[2] = Instantiate(backGround, new Vector3((bgCoord.x - 1) * spriteSize.x, (bgCoord.y + 1) * spriteSize.y, zVal), Quaternion.identity, gameObject.transform);
            bgArray[3] = Instantiate(backGround, new Vector3((bgCoord.x - 1) * spriteSize.x, bgCoord.y * spriteSize.y, zVal), Quaternion.identity, gameObject.transform);
        }
        else if (camBGPos == CameraBGPos.TopRight)
        {
            bgArray[1] = Instantiate(backGround, new Vector3((bgCoord.x + 1) * spriteSize.x, bgCoord.y * spriteSize.y, zVal), Quaternion.identity, gameObject.transform);
            bgArray[2] = Instantiate(backGround, new Vector3((bgCoord.x + 1) * spriteSize.x, (bgCoord.y + 1) * spriteSize.y, zVal), Quaternion.identity, gameObject.transform);
            bgArray[3] = Instantiate(backGround, new Vector3(bgCoord.x * spriteSize.x, (bgCoord.y + 1) * spriteSize.y, zVal), Quaternion.identity, gameObject.transform);
        }
        else if(camBGPos == CameraBGPos.BottomLeft)
        {
            bgArray[1] = Instantiate(backGround, new Vector3((bgCoord.x - 1) * spriteSize.x, bgCoord.y * spriteSize.y, zVal), Quaternion.identity, gameObject.transform);
            bgArray[2] = Instantiate(backGround, new Vector3((bgCoord.x - 1) * spriteSize.x, (bgCoord.y - 1) * spriteSize.y, zVal), Quaternion.identity, gameObject.transform);
            bgArray[3] = Instantiate(backGround, new Vector3(bgCoord.x * spriteSize.x, (bgCoord.y - 1) * spriteSize.y, zVal), Quaternion.identity, gameObject.transform);
        }
        else if(camBGPos == CameraBGPos.BottomRight)
        {
            bgArray[1] = Instantiate(backGround, new Vector3(bgCoord.x * spriteSize.x, (bgCoord.y - 1) * spriteSize.y, zVal), Quaternion.identity, gameObject.transform);
            bgArray[2] = Instantiate(backGround, new Vector3((bgCoord.x + 1) * spriteSize.x, (bgCoord.y - 1) * spriteSize.y, zVal), Quaternion.identity, gameObject.transform);
            bgArray[3] = Instantiate(backGround, new Vector3((bgCoord.x + 1) * spriteSize.x, bgCoord.y * spriteSize.y, zVal), Quaternion.identity, gameObject.transform);
        }
    }
}
