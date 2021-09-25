using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rover rover;
    private float speed = 0.05f;
    private float mapSize;

    void Start() {
        mapSize = 0.5f * transform.parent.localScale.x;
        //rover = transform.parent.GetComponentInChildren<Rover>();
        Vector2 thrust = new Vector2(Random.Range(-speed, speed), (Random.Range(-speed, speed)));
        GetComponent<Rigidbody2D>().velocity = thrust; 
    }

    void Update() {
        wrapAroundMap();
    }
    void wrapAroundMap(){

        if(transform.position.x < -mapSize)
        {
            transform.position = new Vector3(mapSize, transform.position.y, transform.position.z);
        }
        else if(transform.position.x > mapSize)
        {
            transform.position = new Vector3(-mapSize, transform.position.y, transform.position.z);
        }
        if(transform.position.y < -mapSize)
        {
            transform.position = new Vector3(transform.position.x, mapSize, transform.position.z);
        }
        else if(transform.position.y > mapSize)
        {
            transform.position = new Vector3(transform.position.x, -mapSize, transform.position.z);
        }
    }

}
