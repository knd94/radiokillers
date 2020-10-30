using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Examine : MonoBehaviour
{
    public float rotationSpeed = 15f;
    
    Camera mainCam;
    GameObject clickedOnObject;

    Vector3 originalPosition;
    //Vector3 originalRotation;
    Quaternion originalRotation;

    bool examineMode;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        examineMode = false;
    }

    // Update is called once per frame
    private void Update()
    {
        ExitExamineMode();                      // ExitExamineMode comes first, because otherwise the script will exit as soon as it gets into examine mode.

        ClickObject();

        TurnObject();

    }

    void ClickObject()
    {
        if (Input.GetButtonDown("Interact") && examineMode == false)
        {
            RaycastHit hit;
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit) && hit.transform.gameObject.tag == "Interactable")
            {
                clickedOnObject = hit.transform.gameObject;

                originalPosition = clickedOnObject.transform.position;
                originalRotation = clickedOnObject.transform.rotation;

                clickedOnObject.transform.position = mainCam.transform.position + (transform.forward * 3f);

                Time.timeScale = 0;

                examineMode = true;
            }
        }
    }

    void TurnObject()
    {
        if (examineMode)
        {
            Cursor.lockState = CursorLockMode.None;

            float xAxis = Input.GetAxis("Mouse X") * rotationSpeed;
            float yAxis = Input.GetAxis("Mouse Y") * rotationSpeed;

            //if(Input.GetMouseButtonDown(0)) <--- only changes once!
            //{
            clickedOnObject.transform.Rotate(Vector3.up, -xAxis, Space.World);
            clickedOnObject.transform.Rotate(Vector3.forward, yAxis, Space.World);
            //}
        }
    }

    void ExitExamineMode()
    {
        if (Input.GetButtonDown("Interact") && examineMode)
        {
            clickedOnObject.transform.position = originalPosition;
            clickedOnObject.transform.eulerAngles = originalRotation.eulerAngles;

            Time.timeScale = 1;
            
            Cursor.lockState = CursorLockMode.Locked;

            examineMode = false;
        }
    }

}

/* Edited by: Nick on 08/17 & 08/18

Changes: 
* Fixed general typos at:
Line 53: Wrong indentation (just for better aesthetics),
Line 66: "Space" was written as "space",
Line 67: "Space" was written as "space".

* Line 36: Added a new Interact button, now we can always change it and it's more readable. I've set it to 'E'.

* Line 13: Changed originalRotation to a Quaternion instead of a Vector3, because it's now storing the whole rotation
a.k.a. it's more scalable and error-proof.

* Line 76: Further specified originalRotation.eulerAngles; it's clearer to read and it won't work otherwise because of change above.

* Line 59: Changed to while loop, otherwise it wouldn't continually rotate.

* Line 70: Changed new rotation to v3.up, xaxis, space.world, otherwise it would've been wrongly calculated, and possibly thrown out an error.

* Line 59 & 68: Cahnged the if-statement so that now there's continual motion when rotating the item at line 59, and further specified the problem at line 68.
This change was more or less a partial fix, since it's very late at night and I have other tasks to do, such as writing the coding standards for the team.

* Added a new system that allows us to choose which objects are interactable, and which aren't. Simply add the "Interactable" tag to the object.

* Line 78: Using the Interact button to also stop examining, because the right moust button is already used for zooming in and out.

* Line 71: Changed the xAxis to negative, now it rotates in the right direction.

* Line 72: Changed Vector3.right to Vector3.forward, it was rotating around the wrong axis.

Notes:

* This was actually a very big update, so I probably missed some changes I've made, such as small optimisations or fixes.

* Josh, please just hit me up when you need something like this, don't listen to every YouTuber. :D

TO-DO:

* Add an "Interactable" tag, so that the player can't interract with everything.    -   DONE!
    Right now he can interact with himself (hehe), and also the whole level.

* Update the script's structure and paradigm.                                       -   DONE!
*/