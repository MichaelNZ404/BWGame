using UnityEngine;
using UnityEngine.UI;
using System;

public class godhand_controller : MonoBehaviour {
 
    /* 
    Godhand controller, replicating movement seen in Lionhead's Black & White series. 
    https://strategywiki.org/wiki/Black_%26_White/Controls

    // TODO: set godhand emission
    */
    float ZOOM_SPEED = 5000f;
    float ZOOM_MIN_CLAMP = 5f;
    float ZOOM_MAX_CLAMP = 300f;

    float MOVE_SPEED = 2f;
    float ROTATE_SPEED = .6f;
    float PIVOT_MIN_CLAMP = 5f;
    float PIVOT_MAX_CLAMP = 85f;

    float MOUSE_AXIS_SENSITIVITY = 0.1f;

    public float MAX_GODHAND_DISTANCE = 50f;
    public float GODLIGHT_HEIGHT_OFFSET = 10f;
    public float GODHAND_HEIGHT_OFFSET = 2f;

    bool isCastedDown = false;

    public GameObject holding;
    public GameObject GodHandObject;  
    public GameObject GodLight;
    
     void Start(){
        Cursor.visible = false;
        // Cursor.lockState = CursorLockMode.Confined;
        // Cursor.lockState = CursorLockMode.None;

        GodLight = new GameObject("Point Light");
        Light lightComp = GodLight.AddComponent<Light>();
        lightComp.transform.position = new Vector3(GodHandObject.transform.position.x, GodHandObject.transform.position.y + GODLIGHT_HEIGHT_OFFSET, GodHandObject.transform.position.z);
        GodLight.transform.parent = GodHandObject.transform;
        
        lightComp.range = 100;
        lightComp.intensity = 150;
    }

    void Update () {
        handleMouseClick();
    }

    private void handleMouseClick(){
        if (Input.GetMouseButton(2) || Input.GetMouseButton(1) || Input.GetMouseButton(0)) {
            castGodHandDown();
        }
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
        else if (Input.GetMouseButton(0)) { //left mouse down / grab land
            // TODO: use godhand to lock movement inside of screen
            Vector3 p = GetMouseAxisMovement();
            Vector3 newPosition = transform.position;
            transform.Translate(p); //TODO: fix the speed of forward movement at different heights
            newPosition.x = transform.position.x;
            newPosition.z = transform.position.z;
            transform.position = newPosition;
        }
        else if (Input.GetMouseButtonDown(1)) { // right mouse down / action
            if (holding) {
                Vector3 forceVector = new Vector3(100,-100,100);
                holding.GetComponent<Collider>().enabled = true;
                holding.GetComponent<Rigidbody>().isKinematic = false;
                holding.GetComponent<Rigidbody>().AddForce(forceVector*5);
                holding = null;
            }
            else {
                RaycastHit hitInfo = new RaycastHit();
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo) && hitInfo.transform.tag == "Grabable") {
                    holding = hitInfo.collider.gameObject; 
                    // holding.GetComponent<Rigidbody>().useGravity = false;
                    holding.GetComponent<Rigidbody>().isKinematic = true;
                    holding.GetComponent<Collider>().enabled = false;
                    holding.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                }
            }
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

    private void castGodHandDown() {
        if (isCastedDown == false) {
            isCastedDown = true;
            RaycastHit hitInfo = new RaycastHit();
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo)) {
                GodHandObject.transform.position = hitInfo.point;
                GodLight.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y + GODLIGHT_HEIGHT_OFFSET, hitInfo.point.z);
                if (holding) {
                    holding.transform.position = GodHandObject.transform.position;
                }
            }
        }
    }
    private void positionGodHand() {
        isCastedDown = false;
        /* raycast from camera to position GodHand on ground. */
        RaycastHit hitInfo = new RaycastHit();
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo)) {
            float step = Math.Min(MAX_GODHAND_DISTANCE, hitInfo.distance);
            GodHandObject.transform.position = Vector3.MoveTowards(transform.position, hitInfo.point, step);
            GodLight.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y + GODLIGHT_HEIGHT_OFFSET, hitInfo.point.z);
            if (holding) {
                holding.transform.position = GodHandObject.transform.position;
            }
        }
    }
}