using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rover : MonoBehaviour
{
    public enum RoverStatus
    {
        defending = 1,
        moving = 2,
        scanning = 3
    }

    float fuel = 100f;
    float hullHealth = 100f;
    float wheelHealth = 100f;
    float ammo = 100f;

    [SerializeField] private float movementSpeed = 1f;
    RoverStatus roverStatus = RoverStatus.defending;

    void Start()
    {
        gameObject.transform.position = new Vector3(Random.Range(-.4f, .4f), Random.Range(-.4f, .4f), -1);
        gameObject.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
    }

    void Update()
    {
        if (roverStatus == RoverStatus.moving)
        {
            gameObject.transform.position += gameObject.transform.forward * movementSpeed * Time.deltaTime;
        }
        if(hullHealth < 0)
        {
            EndGameHull();
        }
    }

    public void Move(int x)
    {
        float posXStart = gameObject.transform.position.x;
        float posYStart = gameObject.transform.position.y;
        roverStatus = RoverStatus.moving;
    }

    public void ReceiveHullDamage(float damage){
        hullHealth -= damage;
    }

    void EndGameHull()
    {
        Debug.Log("Game Over because hull rip");
    }

    void Defend(){

    }
    
}
