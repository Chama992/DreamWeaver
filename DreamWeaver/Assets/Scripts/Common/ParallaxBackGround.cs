using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParallaxBackGround : MonoBehaviour
{
    private GameObject cam;
    [SerializeField] private float parallaxEffect; // move slow than camera
    private float xPosition;
    private float yPosition;
    private float xlength;
    private float ylength;
    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera");
        xPosition = transform.position.x;
        yPosition = transform.position.y;
        xlength = GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        ylength = GetComponent<SpriteRenderer>().sprite.bounds.size.y;
    }

    // Update is called once per frame
    void Update()
    {
        //X
        float distanceXMove = cam.transform.position.x * (1 - parallaxEffect);// why cam position but not position change of camera?? not good 
        float distanceXToMove = cam.transform.position.x * parallaxEffect;
        transform.position = new Vector3(xPosition + distanceXToMove, transform.position.y);
        // move the background
        if (distanceXMove > xPosition + xlength) 
            xPosition = xPosition + xlength;
        else if (distanceXMove < xPosition - xlength)
            xPosition = xPosition - xlength;
        //Y
        float distanceyMove = cam.transform.position.y * (1 - parallaxEffect);// why cam position but not position change of camera?? not good 
        float distanceYToMove = cam.transform.position.y * parallaxEffect;
        transform.position = new Vector3(yPosition + distanceYToMove, transform.position.y);
        // move the background
        if (distanceyMove > yPosition + ylength) 
            yPosition = yPosition + ylength;
        else if (distanceyMove < yPosition - ylength)
            yPosition = yPosition - ylength;

    }
}
