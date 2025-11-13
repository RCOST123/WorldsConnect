using System;
using UnityEngine;

public class UnstablePlatform : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //if player lands on platform
        //then roll dice
        //maybe delete object

    }

    // Update is called once per frame

    void Update()
    {
        /*
        //add a collision counter so only 1st collision triggers a round of Random() and ruin().
        Random();
        if (randomIntRange <= 50)
        {
            ruin();
        }
        */
    }
/*
    void Random()
    //random int betweeon 0 and 100 noninclusivwe
    {
        Random rand = new Random();
        int randomIntRange = rand.Next(0, 100);

    }
    void ruin()
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            Destroy(collision.gameObject);
        }
    }
    */
}
