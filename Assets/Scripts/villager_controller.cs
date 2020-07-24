using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class villager_controller : MonoBehaviour
{
    private int VILLAGER_SPEED = 5;
    private int CARRY_AMOUNT = 50;
    private static readonly System.Random getrandom = new System.Random();

    public GameObject home;
    public string currentStatus = "Idle";

    public float health = 100f;
    private static float STARVING_HEALTH_TICK = 1E-3f;
    private static float SLEEP_DEPREVATION_HEALTH_TICK = 1E-2f;
    private static float HOMELESS_SLEEP_TICK = 1E-4f;


    public float sleep = 100f;
    private static float SLEEP_TICK = 1E-3f;
    private static float SLEEP_RECOVERY_HOME_TICK = 1E-2f;
    private static float SLEEP_RECOVERY_HOMELESS_TICK = 1E-3f;
    
    public float hunger = 0f;
    private static float HUNGER_TICK = 1E-8f;
    
    public string holding = "Nothing"; // Nothing | Food | Wood
    public int holdingAmount = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Gravity();
        if (health <= 0) {
            currentStatus = "Dead";
            return;
        }

        sleep -= SLEEP_TICK;
        hunger += HUNGER_TICK;
        if (hunger >= 99){
            health -= STARVING_HEALTH_TICK;
        }
        if (sleep <= 0) {
            health -= SLEEP_DEPREVATION_HEALTH_TICK;
        }

        // hauling action end
        if(holding != "Nothing"){
            currentStatus="Depositing Resources";
            travelToStorehouse();
            return;
        }

        // hungry action end
        if (hunger >= 50 && storehouseHasFood()){
            currentStatus="Going to Eat";
            travelToStorehouse();
            return;
        }

        // tired action end
        if (sleep <= 50){
            if (home) {
                currentStatus = "Going home";
                // TODO: travelHome();
                sleep += SLEEP_RECOVERY_HOME_TICK;
                return;
            } else {
                currentStatus = "Sleeping on floor";
                health -= HOMELESS_SLEEP_TICK;
                sleep += SLEEP_RECOVERY_HOMELESS_TICK;
                return;
            }
        }

        // TODO: this is where we would do diciple actions, breed/farm/wc/etc based on nature
        // for now we just farm by default
        GameObject farmland = GameObject.Find("Farmland");
        if(farmland.GetComponent<farmland_controller>().foodCount > 0){
            currentStatus="Harvesting Crop";
            interactWithFarmland();
            return;
        }
        if(farmland.GetComponent<farmland_controller>().sown_percent < 100f){
            currentStatus="Sowing Field";
            interactWithFarmland();
            return;
        }


        // Non-diciples, or tired people can now rest
        // int task  = getrandom.Next(1, 500);  // creates a number between 1 and 12
        // if (task==20){
        //     currentStatus="foobar";
        // }
        // else if (task==400){
            currentStatus="idle";
            // return;
        // }
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
            int foodToTake = Math.Min(storehouse.GetComponent<storehouse_controller>().foodCount, (int) Math.Floor(hunger));
            storehouse.GetComponent<storehouse_controller>().foodCount -= foodToTake;
            hunger -= foodToTake;

            if(holding != "Nothing"){
                switch(holding){
                    case "food":
                        storehouse.GetComponent<storehouse_controller>().foodCount += holdingAmount;
                        break;
                    case "wood":
                        storehouse.GetComponent<storehouse_controller>().woodCount += holdingAmount;
                        break;
                }
                holdingAmount = 0;
                holding = "Nothing";
            }
        }
    }

    private void interactWithFarmland(){
        GameObject farmland = GameObject.Find("Farmland");
        Vector3 targetPos = farmland.transform.position;
        targetPos.y = transform.position.y;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * VILLAGER_SPEED);
        if(transform.position == targetPos){
            if(farmland.GetComponent<farmland_controller>().foodCount > 0){
                holding="food";
                holdingAmount=farmland.GetComponent<farmland_controller>().harvestCrops(CARRY_AMOUNT);
                return;
            }
            if(farmland.GetComponent<farmland_controller>().sown_percent < 100f){
                farmland.GetComponent<farmland_controller>().sowCrops();
                return;
            }
        }
    }
}
