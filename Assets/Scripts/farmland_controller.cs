using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class farmland_controller : MonoBehaviour
{
    private int FOOD_TICK_INCREASE = 1000;
    
    public int foodCount = 20;
    private int foodTickCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foodTickCount++;
        if(foodTickCount >= FOOD_TICK_INCREASE){
            foodTickCount=0;
            foodCount++;
        }
    }
}
