using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class villager_controller : MonoBehaviour
{
    private int VILLAGER_SPEED = 5;
    private int HUNGER_TICK_INCREASE = 50;
    private int CARRY_AMOUNT = 5;

    private static readonly System.Random getrandom = new System.Random();
    public string currentTask = "Idle";
    
    public int hunger = 0;
    private int hungerTickCount = 0;
    
    public string holding;
    public int holdingAmount = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Gravity();
        hungerTickCount++;
        if (hungerTickCount >= HUNGER_TICK_INCREASE){
            hunger++;
            hungerTickCount=0;
        }

        if (hunger >= 99){
            currentTask="Starving";
            return;
        }

        if(holding != null){
            currentTask="Depositing Resources";
            travelToStorehouse();
        }

        if (hunger >= 50 && storehouseHasFood()){
            currentTask="Going to Eat";
            travelToStorehouse();
            return;
        }

        GameObject farmland = GameObject.Find("Farmland");
        if(farmland.GetComponent<farmland_controller>().foodCount > 10){
            currentTask="Collecting Food";
            harvestFromFarmland();
        }



        int task  = getrandom.Next(1, 500);  // creates a number between 1 and 12
        if (task==20){
            currentTask="foobar";
        }
        else if (task==400){
            currentTask="idle";
        }
        Wander();
    }

    private void Gravity() {
        /**
        make the player follow the terrain every frame
        TODO: maybe round these to integers to prevent frequent updates?
        **/
        RaycastHit hit;
        int raycastDistance = 10;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance) && transform.position.y != hit.point.y + 1)
         {
            // print($"{transform.position} updating from {transform.position.y} to {hit.point.y + 1}");
            Vector3 pos = transform.position;
            pos.y = hit.point.y + 1;
            transform.position = pos;
         }
    }

    private void Wander(){
        // TODO: instead of moving either left or right, just pick a new random i <360
        int i = getrandom.Next(1, 500);
        switch(i){
            case 1: //left
                transform.Rotate(0.0f, -90.0f, 0.0f);
                break;
            case 2: //right
                transform.Rotate(0.0f, 90.0f, 0.0f);
                break;
        }
        transform.Translate(Vector3.forward * VILLAGER_SPEED/2 * Time.deltaTime);
    }

    private bool storehouseHasFood(){
        GameObject storehouse = GameObject.Find("Storehouse");
        if(storehouse.GetComponent<storehouse_controller>().foodCount < 1){
            return false;
        }
        return true;
    }

    private void travelToStorehouse(){
        GameObject storehouse = GameObject.Find("Storehouse");
        Vector3 targetPos = storehouse.transform.position;
        targetPos.y = transform.position.y;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * VILLAGER_SPEED);
        
        if(transform.position == targetPos){
            Debug.Log(transform.position);
            Debug.Log(storehouse.transform.position);
            int foodToTake = Math.Min(storehouse.GetComponent<storehouse_controller>().foodCount, hunger);
            storehouse.GetComponent<storehouse_controller>().foodCount -= foodToTake;
            hunger -= foodToTake;

            if(holding != null){
                switch(holding){
                    case "food":
                        storehouse.GetComponent<storehouse_controller>().foodCount += holdingAmount;
                        break;
                    case "wood":
                        storehouse.GetComponent<storehouse_controller>().woodCount += holdingAmount;
                        break;
                }
                holdingAmount = 0;
                holding = null;
            }
        }
    }

    private void harvestFromFarmland(){
        GameObject farmland = GameObject.Find("Farmland");
        transform.position = Vector3.MoveTowards(transform.position, farmland.transform.position, Time.deltaTime * VILLAGER_SPEED);
        // Debug.Log(farmland.transform.position);
        if(transform.position == farmland.transform.position){
            int foodToTake = Math.Min(farmland.GetComponent<farmland_controller>().foodCount, CARRY_AMOUNT);
            farmland.GetComponent<farmland_controller>().foodCount -= foodToTake;
            holding="food";
            holdingAmount=foodToTake;
        }
    }
}
