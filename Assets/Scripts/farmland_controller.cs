using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class farmland_controller : MonoBehaviour
{   
    public int foodCount = 500;
    public float sown_percent = 0f;
    private static float SOWING_TICK = 0.005f;
    
    public float growth_percent = 0f;
    private static float GROWTH_TICK = 0.0005f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    // TODO: texture based on state
    {
        if (foodCount > 0) {
            return; // needs to be harvested by villagers
        }
        if (sown_percent <= 100f){
            return; // needs to be sown by villagers
        }
        if (growth_percent >= 100f) {
            foodCount = 500;
            sown_percent = 0f;
            growth_percent = 0f;
            return;
        }
        growth_percent += GROWTH_TICK;
    }

    public void sowCrops() {
        sown_percent += SOWING_TICK;
    }

    public int harvestCrops(int amountRequested) {
        int fetchedAmount = Math.Min(foodCount, amountRequested);
        foodCount -= fetchedAmount;
        return fetchedAmount;
    }
}
