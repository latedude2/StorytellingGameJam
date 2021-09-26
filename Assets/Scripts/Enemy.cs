using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rover rover;
    private float damage = 3f;
    private float health = 10f;
    float chaseDistance = 0.5f;
    float attackDistance = 0.05f;
    private float speed = 0.05f;
    private float mapSize;
    private float yModifier = 0.7f;

    void Start() {
        rover = transform.parent.Find("Rover").GetComponent<Rover>();
        mapSize = 0.5f * transform.parent.localScale.x;
        Vector2 thrust = new Vector2(Random.Range(-speed, speed), (Random.Range(-speed, speed)));
        GetComponent<Rigidbody2D>().velocity = thrust.normalized * speed; 
    }

    void Update() {
        WrapAroundMap();
        ChaseRover();
        Attack();
        Die();
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
        if(transform.position.y < -mapSize * yModifier)
        {
            transform.position = new Vector3(transform.position.x, mapSize * yModifier, transform.position.z);
        }
        else if(transform.position.y > mapSize * yModifier)
        {
            transform.position = new Vector3(transform.position.x, -mapSize * yModifier, transform.position.z);
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
            rover.ReceiveHullDamage(damage * Time.deltaTime);
        }
    }

    public void TakeDamage(float attack){
        health -= attack; 
    }

    void Die(){
        if(health < 0f)
        {
            GameObject.Find("Send").GetComponent<CommandLineButton>().PrintMessage("< Enemy destroyed!");
            Destroy(gameObject);
        }
    }
}
