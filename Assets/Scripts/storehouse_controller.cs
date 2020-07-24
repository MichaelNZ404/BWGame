using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class storehouse_controller : MonoBehaviour
{
    public int woodCount = 2000;
    public int foodCount = 500;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(foodCount <= 50){
            print("food low!");
        }
    }
}
