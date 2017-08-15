using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class Tiling : MonoBehaviour
{
    public int offsetX = 2;
    public bool hasARightBuddy = false;
    public bool hasALeftBuddy = false;
    public bool reverseScale = false;       // used if object is not tileable
    private float spriteWidth = 0f;
    private Camera cam;
    private Transform myTransform;

    void Awake()
    {
        cam = Camera.main;
        myTransform = transform;
    }

    // Use this for initialization
    void Start()
    {
        SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
        spriteWidth = sRenderer.sprite.bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        // does it still need buddies?
        if (!hasALeftBuddy || !hasARightBuddy)
        {
            // calculate the cams extend (half the width) of what the camera sees in world coordinates
            float camHorizontalExtend = cam.orthographicSize * Screen.width / Screen.height;

            // calculate the x position where the cam can see the edge of the sprite (element)
            float edgeVisiblePosRight = (myTransform.position.x + spriteWidth / 2) - camHorizontalExtend;
            float edgeVisiblePosLeft = (myTransform.position.x - spriteWidth / 2) + camHorizontalExtend;

            // checking if we can see the edge of the element and then calling MakeNewBuddy if we can
            if (cam.transform.position.x >= edgeVisiblePosRight - offsetX && !hasARightBuddy)
            {
				MakeNewBuddy(1);
				hasARightBuddy = true;
            }
            else if (cam.transform.position.x <= edgeVisiblePosLeft + offsetX && !hasALeftBuddy)
            {
				MakeNewBuddy(-1);
				hasALeftBuddy = true;
            }
        }
    }

    // creates a buddy on the side required; 1 is right, -1 left
    void MakeNewBuddy(int rightOrLeft)
    {
        // calculating the new position for our new buddy
        Vector3 newPosition = new Vector3(myTransform.position.x + spriteWidth * rightOrLeft, myTransform.position.y, myTransform.position.z);
        // create new buddy and store in variable
		Transform newBuddy = Instantiate(myTransform, newPosition, myTransform.rotation) as Transform;

		// if not tileable lets reverse the x size to get rid of ugly seams
        if (reverseScale)
        {
            newBuddy.localScale = new Vector3(newBuddy.localScale.x * -1, newBuddy.localScale.y, newBuddy.localScale.z);
        }

		newBuddy.parent = myTransform.parent;
		if(rightOrLeft > 0)
		{
			newBuddy.GetComponent<Tiling>().hasALeftBuddy = true;
		}
		else
		{
			newBuddy.GetComponent<Tiling>().hasARightBuddy = true;
		}
    }
}
