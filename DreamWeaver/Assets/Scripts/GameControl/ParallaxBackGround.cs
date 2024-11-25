using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackGround : MonoBehaviour
{
    private GameObject cam;
    [SerializeField] private float parallaxEffect; // move slow than camera
    private float xPosition;
    private float length;
    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera");
        xPosition = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceMove = cam.transform.position.x * (1 - parallaxEffect);// why cam position but not position change of camera?? not good 
        float distanceToMove = cam.transform.position.x * parallaxEffect;
        transform.position = new Vector3(xPosition + distanceToMove, transform.position.y);
        // move the background
        if (distanceMove > xPosition + length) 
            xPosition = xPosition + length;
        else if (distanceMove < xPosition - length)
            xPosition = xPosition - length;

    }
}
