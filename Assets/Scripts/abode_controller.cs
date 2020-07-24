using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class abode_controller : MonoBehaviour
{
    public string ownedByCity = "Unknown";
    public bool built = true;
    public int health = 100; 
    public GameObject[] residents = new GameObject[3];

    // Start is called before the first frame update
    void Start()
    {
        // TODO, try to attribute ownedByCity to the nearest city center
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
