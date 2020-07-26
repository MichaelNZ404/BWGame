using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class archive_snippets : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //TODO, use these for side of screen pitching/rotating
        // transform.Rotate(0f, ROTATE_SPEED, 0f); 
        // transform.Rotate(0f, -ROTATE_SPEED, 0f);
        // transform.Rotate(ROTATE_SPEED, 0, 0f);
        // transform.Rotate(-ROTATE_SPEED, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        // move towards godhand in air
        //Vector3 target = new Vector3(GodHandObject.transform.position.x, transform.position.y, GodHandObject.transform.position.z);
        // transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * -MOVE_SPEED);

        // old set cursor
        //  Cursor.SetCursor(handtext, Vector2.zero, CursorMode.Auto);

        // old camera rotation code
        // private Vector3 lastMouse = new Vector3(255, 255, 255); //kind of in the middle of the screen, rather than at the top (play)
        // float mainSpeed = 100.0f; //regular speed
        // float camSens = 0.25f; //How sensitive it with mouse

        // lastMouse = Input.mousePosition - lastMouse ;
        // lastMouse = new Vector3(-lastMouse.y * camSens, lastMouse.x * camSens, 0 );
        // lastMouse = new Vector3(transform.eulerAngles.x + lastMouse.x , transform.eulerAngles.y + lastMouse.y, 0);
        // transform.eulerAngles = lastMouse;
        // lastMouse =  Input.mousePosition;
        //Mouse  camera angle done.  
    }
}
