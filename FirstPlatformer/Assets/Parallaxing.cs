using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour
{
    public Transform[] backgrounds;     // Array of all the back- and foregrounds to be parallaxed
    private float[] parallaxScales;     // The Proportion of the cameras movement to move the backgrounds by
    public float smoothing = 1f;        // How smooth the parallax is going to be. Make sure to set above 0
    private Transform cam;              // Reference to the main Cameras tranform
    private Vector3 previousCamPos;     // The Position of the camera in the previous frame

    // Called before Start(). Great for References.
    void Awake()
    {
        //set up the cam reference
        cam = Camera.main.transform;
    }

    // Use this for initialization
    void Start()
    {
        // The previous frame had the current frames cam position
        previousCamPos = cam.position;

        // Assigning corresponding parallax Scales
        parallaxScales = new float[backgrounds.Length];
        for (int i = 0; i < backgrounds.Length; i++)
        {
            parallaxScales[i] = backgrounds[i].position.z * -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // For each Background
        for (int i = 0; i < backgrounds.Length; i++)
        {
            // Parallax is the opposite of the camera movement because the previous frame multiplied by the scale
            float parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i];

            // Set a target x Position which is the current position + the parallax
            float backgroundTargetPosX = backgrounds[i].position.x + parallax;

            // create a target position which is the backgrounds current position with its target x position
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

            // fade between current position and target position using lerp
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
        }

		// set prevCamPos to the cameras position at the end of the frame
		previousCamPos = cam.position;
    }
}
