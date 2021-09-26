using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rover rover;
    private float damage = 1f;
    private float health = 10f;
    float chaseDistance = 0.3f;
    float attackDistance = 0.05f;
    private float speed = 0.05f;
    private float mapSize;

    void Start() {
        rover = transform.parent.Find("Rover").GetComponent<Rover>();
        mapSize = 0.5f * transform.parent.localScale.x;
        //rover = transform.parent.GetComponentInChildren<Rover>();
        Vector2 thrust = new Vector2(Random.Range(-speed, speed), (Random.Range(-speed, speed)));
        GetComponent<Rigidbody2D>().velocity = thrust.normalized * speed; 
    }

    void Update() {
        WrapAroundMap();
        ChaseRover();
        Attack();
    }
    void WrapAroundMap(){

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

    void ChaseRover()
    {
        Vector2 roverPosition = new Vector2(rover.transform.position.x, rover.transform.position.y);
        Vector2 enemyPosition = new Vector2(transform.position.x, transform.position.y);
        if(Vector2.Distance(enemyPosition, roverPosition) < chaseDistance)
        {
            Vector2 thrust = roverPosition - enemyPosition;
            GetComponent<Rigidbody2D>().velocity = thrust.normalized * speed;
        }
    }

    void Attack(){
        Vector2 roverPosition = new Vector2(rover.transform.position.x, rover.transform.position.y);
        Vector2 enemyPosition = new Vector2(transform.position.x, transform.position.y);
        if(Vector2.Distance(enemyPosition, roverPosition) < attackDistance)
        {
            rover.ReceiveHullDamage(damage);
        }
    }
}
