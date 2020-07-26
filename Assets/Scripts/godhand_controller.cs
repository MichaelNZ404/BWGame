using UnityEngine;
using UnityEngine.UI;
using System.Collections;


/*
NOTES: 

DRAG FORWARD
drag forward is directly in line with god hand
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
    float MOVE_SPEED = 2f;
    float ROTATE_SPEED = .6f;
    float MOUSE_AXIS_SENSITIVITY = 0.1f;

     
    float mainSpeed = 100.0f; //regular speed
    float camSens = 0.25f; //How sensitive it with mouse
    private Vector3 lastMouse = new Vector3(255, 255, 255); //kind of in the middle of the screen, rather than at the top (play)

    public GameObject GodHandObject;  
    
     void Start(){
        Cursor.visible = false;
        // Cursor.lockState = CursorLockMode.Confined;
        //  Cursor.lockState = CursorLockMode.None;
        //  Cursor.SetCursor(handtext, Vector2.zero, CursorMode.Auto);
    }

    void Update () {
        // handleMouseClick();
        
        // lastMouse = Input.mousePosition - lastMouse ;
        // lastMouse = new Vector3(-lastMouse.y * camSens, lastMouse.x * camSens, 0 );
        // lastMouse = new Vector3(transform.eulerAngles.x + lastMouse.x , transform.eulerAngles.y + lastMouse.y, 0);
        // transform.eulerAngles = lastMouse;
        // lastMouse =  Input.mousePosition;
        //Mouse  camera angle done.  
       
        handleMouseClick();
        //Keyboard commands
        // Vector3 p = GetBaseInput();
        // p = p * mainSpeed;
       
        // p = p * Time.deltaTime;
        // Vector3 newPosition = transform.position;
        // if (Input.GetKey(KeyCode.Space)){ //If player wants to move on X and Z axis only
        //     transform.Translate(p);
        //     newPosition.x = transform.position.x;
        //     newPosition.z = transform.position.z;
        //     transform.position = newPosition;
        // }
        // else{
        //     transform.Translate(p);
        // }
       
    }
     
    private Vector3 GetBaseInput() { //returns the basic values, if it's 0 than it's not active.
        Vector3 p_Velocity = new Vector3();
        if (Input.GetKey (KeyCode.W)){
            p_Velocity += new Vector3(0, 0 , 1);
        }
        if (Input.GetKey (KeyCode.S)){
            p_Velocity += new Vector3(0, 0, -1);
        }
        if (Input.GetKey (KeyCode.A)){
            p_Velocity += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey (KeyCode.D)){
            p_Velocity += new Vector3(1, 0, 0);
        }
        return p_Velocity;
    }

    private void handleMouseClick(){
        if (Input.GetMouseButton(2)) {
            print("middle mouse down / rotate and pitch");
            // transform.RotateAround(GodHandObject.transform.position, transform.up, Time.deltaTime * -ROTATE_SPEED);
            if (Input.GetAxis("Mouse X") > MOUSE_AXIS_SENSITIVITY) { //rotate left
                float x = Input.GetAxis("Mouse X");
                transform.RotateAround(GodHandObject.transform.position, new Vector3(0, 1, 0), Time.deltaTime * ROTATE_SPEED *500);
                // transform.Rotate(0f, ROTATE_SPEED, 0f);
            }
            if (Input.GetAxis("Mouse X") < -MOUSE_AXIS_SENSITIVITY) { //rotate right
                transform.RotateAround(GodHandObject.transform.position, new Vector3(0, -1, 0), Time.deltaTime * ROTATE_SPEED * 500);
                // transform.Rotate(0f, -ROTATE_SPEED, 0f);
            }
            if (Input.GetAxis("Mouse Y") < -MOUSE_AXIS_SENSITIVITY) { //pitch down
                transform.Rotate(ROTATE_SPEED, 0, 0f);
                // transform.RotateAround(GodHandObject.transform.position, transform.right, Time.deltaTime * -ROTATE_SPEED);
            }
            if (Input.GetAxis("Mouse Y") > MOUSE_AXIS_SENSITIVITY) { //pitch up
                transform.Rotate(-ROTATE_SPEED, 0f, 0f);
                // transform.RotateAround(GodHandObject.transform.position, transform.right, Time.deltaTime * ROTATE_SPEED);
            }
        }
        // else if (Input.GetMouseButton(0) && Input.GetMouseButton(1)) {
        //     print("both mouse down / zoom");
        // }
        else if (Input.GetMouseButton(0)) {
            print("left mouse down / grab land");
            // move in the direction the camera is facing. TODO: use godhand to lock movement
            Vector3 p = GetMouseAxisMovement();
            Vector3 newPosition = transform.position;
            transform.Translate(p);
            newPosition.x = transform.position.x;
            // newPosition.y = transform.position.y;
            newPosition.z = transform.position.z;
            transform.position = newPosition;
        }
        else if (Input.GetMouseButton(1)) {
            print("right mouse down / action");
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0f ) {
            print("scroll up / zoom in");
            transform.position = Vector3.MoveTowards(transform.position, GodHandObject.transform.position, Time.deltaTime * ZOOM_SPEED);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f ) {
            print("scroll down / zoom out");
            transform.position = Vector3.MoveTowards(transform.position, GodHandObject.transform.position, Time.deltaTime * -ZOOM_SPEED);
        }
        else {
            // print("follow");
            followMouse();
        }
    }

    private Vector3 GetMouseAxisMovement() {
        //TODO use proximity to GodHand control speed
        Vector3 p_Velocity = new Vector3();
        if (Input.GetAxis("Mouse Y") < -MOUSE_AXIS_SENSITIVITY) { //forward
            p_Velocity += new Vector3(0, 0, MOVE_SPEED);
            // Vector3 target = new Vector3(GodHandObject.transform.position.x, transform.position.y, GodHandObject.transform.position.z);
            // transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * MOVE_SPEED);
        }
        if (Input.GetAxis("Mouse Y") > MOUSE_AXIS_SENSITIVITY) { //back
            p_Velocity += new Vector3(0, 0, -MOVE_SPEED);
            // Vector3 target = new Vector3(GodHandObject.transform.position.x, transform.position.y, GodHandObject.transform.position.z);
            // transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * -MOVE_SPEED);
        }
        if (Input.GetAxis("Mouse X") < -MOUSE_AXIS_SENSITIVITY) { //left
            p_Velocity += new Vector3(MOVE_SPEED, 0, 0);
            // Vector3 target = new Vector3(GodHandObject.transform.position.x, transform.position.y, GodHandObject.transform.position.z);
            // // target = Quaternion.Euler(0, -90, 0) * target;
            // target = Quaternion.AngleAxis(90, Vector3.up) * target;
            // transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * -MOVE_SPEED);
        }
        if (Input.GetAxis("Mouse X") > MOUSE_AXIS_SENSITIVITY) { //right
            p_Velocity += new Vector3(-MOVE_SPEED, 0, 0);
            // Vector3 target = new Vector3(GodHandObject.transform.position.x, transform.position.y, GodHandObject.transform.position.z);
            // // target = Quaternion.Euler(0, 90, 0) * target;
            // Vector3 newtarget = Quaternion.AngleAxis(-90, Vector3.up) * target;
            // print($"Old target: {target}, New target: {newtarget}");
            // transform.position = Vector3.MoveTowards(transform.position, newtarget, Time.deltaTime * -MOVE_SPEED);
        }
        return p_Velocity;
    }

    private void followMouse() {
        RaycastHit hitInfo = new RaycastHit();
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo)) {
            // print($"hit {hitInfo.transform.position}");
            GodHandObject.transform.position = hitInfo.point; 
        } 
    }
}