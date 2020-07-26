using UnityEngine;
using UnityEngine.UI;
using System.Collections;


/*
NOTES: 

DRAG FORWARD
god hand is locked to screen edges which prevents drag from going past hand

ROTATE AND PITCH:
middle click rotate and pitch is centered on god hand
edge of screen rotate and pitch are camera only

GAMEPLAY:
god hand has maximum distance from camera, just raycast through for target.
no maximum range for grab, just becomes difficult to see
detail decreases with zoom, influence ring should be visible from space.
hard bounding box around island.
ability to show info next to god hand (eg zoom with mouse icon, or the amount of wood being picked up)
press S for villager debug info, has range (same as godhand?) NAME|ACTION|AGE LIFE FOOD
double click terrain to fly over and zoom (with wind sounds)
throwing projectiles with respect to perspective
temple has enterance, creature pen, and worship sites

MIRACLES:

SOUNDS:
soft thud when grabbing terrain
wind when high up.
villagers laughing etc
water noise when near ocean
birds when near ground
animal sounds
footsteps of villagers
rock rolling sound

COOL FEATRURES:
challenge room in temple / challenges.
creature cave
gods playground
small map in center of temple

CREATURE:
discipline/pretting
leashes
AI
*/

public class godhand_controller : MonoBehaviour {
 
    /* 
    Godhand controller, replicating movement seen in Lionhead's Black & White series. 
    https://strategywiki.org/wiki/Black_%26_White/Controls
    */
    float ZOOM_SPEED = 5000f;
    float ZOOM_MIN_CLAMP = 5f;
    float ZOOM_MAX_CLAMP = 300f;

    float MOVE_SPEED = 2f;
    float ROTATE_SPEED = .6f;
    float PIVOT_MIN_CLAMP = 5f;
    float PIVOT_MAX_CLAMP = 85f;

    float MOUSE_AXIS_SENSITIVITY = 0.1f;

    public GameObject GodHandObject;  
    
     void Start(){
        Cursor.visible = false;
        // Cursor.lockState = CursorLockMode.Confined;
        // Cursor.lockState = CursorLockMode.None;
    }

    void Update () {
        handleMouseClick();
    }

    private void handleMouseClick(){
        if (Input.GetMouseButton(2)) { //middle mouse down / rotate and pitch 
            if (Input.GetAxis("Mouse X") > MOUSE_AXIS_SENSITIVITY) { //rotate left
                transform.RotateAround(GodHandObject.transform.position, new Vector3(0, 1, 0), Time.deltaTime * ROTATE_SPEED *500);
            }
            if (Input.GetAxis("Mouse X") < -MOUSE_AXIS_SENSITIVITY) { //rotate right
                transform.RotateAround(GodHandObject.transform.position, new Vector3(0, -1, 0), Time.deltaTime * ROTATE_SPEED * 500);
            }
            if (Input.GetAxis("Mouse Y") < -MOUSE_AXIS_SENSITIVITY) { //pitch down

                if (transform.localEulerAngles.x > PIVOT_MIN_CLAMP) {
                    transform.RotateAround(GodHandObject.transform.position, transform.right, Time.deltaTime * -ROTATE_SPEED * 500);
                }
            }
            if (Input.GetAxis("Mouse Y") > MOUSE_AXIS_SENSITIVITY) { //pitch up
                if (transform.localEulerAngles.x < PIVOT_MAX_CLAMP) {
                    transform.RotateAround(GodHandObject.transform.position, transform.right, Time.deltaTime * ROTATE_SPEED * 500);
                }
            }
        }
        else if (Input.GetMouseButton(0) && Input.GetMouseButton(1)) { // both mouse down / zoom
            // TODO: support double mouse click zoom
        }
        else if (Input.GetMouseButton(0)) { //left mouse down / grab land
            // TODO: use godhand to lock movement inside of screen
            Vector3 p = GetMouseAxisMovement();
            Vector3 newPosition = transform.position;
            transform.Translate(p); //TODO: fix the speed of forward movement at different heights
            newPosition.x = transform.position.x;
            newPosition.z = transform.position.z;
            transform.position = newPosition;
        }
        else if (Input.GetMouseButton(1)) { // right mouse down / action"
            // TODO: implement actions
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0f ) { // scroll up / zoom in
            Vector3 newVector = Vector3.MoveTowards(transform.position, GodHandObject.transform.position, Time.deltaTime * ZOOM_SPEED);
            if (newVector.y > ZOOM_MIN_CLAMP) {
                transform.position = newVector;
            } 
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f ) { // scroll down / zoom out
            Vector3 newVector = Vector3.MoveTowards(transform.position, GodHandObject.transform.position, Time.deltaTime * -ZOOM_SPEED);
            if (newVector.y < ZOOM_MAX_CLAMP) {
                transform.position = newVector;
            } 
        }
        else {
            positionGodHand();
        }
    }

    private Vector3 GetMouseAxisMovement() {
        //TODO use proximity to GodHand control speed
        Vector3 p_Velocity = new Vector3();
        if (Input.GetAxis("Mouse Y") < -MOUSE_AXIS_SENSITIVITY) { //forward
            p_Velocity += new Vector3(0, 0, MOVE_SPEED);
        }
        if (Input.GetAxis("Mouse Y") > MOUSE_AXIS_SENSITIVITY) { //back
            p_Velocity += new Vector3(0, 0, -MOVE_SPEED);
        }
        if (Input.GetAxis("Mouse X") < -MOUSE_AXIS_SENSITIVITY) { //left
            p_Velocity += new Vector3(MOVE_SPEED, 0, 0);
        }
        if (Input.GetAxis("Mouse X") > MOUSE_AXIS_SENSITIVITY) { //right
            p_Velocity += new Vector3(-MOVE_SPEED, 0, 0);
        }
        return p_Velocity;
    }

    private void positionGodHand() {
        /* raycast from camera to position GodHand on ground. */
        // TODO: sex maximum godhand distance
        RaycastHit hitInfo = new RaycastHit();
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo)) {
            GodHandObject.transform.position = hitInfo.point; 
        } 
    }
}