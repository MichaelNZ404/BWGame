using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tree_controller : MonoBehaviour
{
    private int GROWTH_TICKS_TO_INCREASE_SIZE = 100;
    public int size = 0;
    private int growthTickCount = 0;
    private static readonly System.Random getrandom = new System.Random();
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if(size<1){
            growthTickCount++;
            if(growthTickCount >= GROWTH_TICKS_TO_INCREASE_SIZE){
                size = 1;
                transform.localScale = new Vector3(2, 2, 2);
            }
        }
        if(size==1){
            if (getrandom.Next(1, 500000000) == 1){
                GameObject tree = GameObject.Find("Tree");
                Vector3 spawnloc = new Vector3(
                    transform.position.x + getrandom.Next(1, 5),
                    transform.position.y,
                    transform.position.z + getrandom.Next(1, 5)
                );
                GameObject newTree = Instantiate(tree, spawnloc, transform.rotation);
                newTree.GetComponent<tree_controller>().size = 0;
            }
        }
    }
}
