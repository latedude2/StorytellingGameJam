using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rover : MonoBehaviour
{
    public enum RoverStatus
    {
        neutral = 1
        defending = 2,
        moving = 3,
        scanning = 4
    }

    float fuel = 100f;
    float hullHealth = 100f;
    float wheelHealth = 100f;
    float ammo = 100f;

    [SerializeField] private float movementSpeed = 1f;
    RoverStatus roverStatus = RoverStatus.defending;
    private bool moving = false;
    private float posXStart;
    private float posYStart;
    private float moveDistance;

    [SerializeField] private float rotationSpeed = 1f;
    private bool rotating = false;
    private float rotationStart;
    private float rotationAngle;

    private float angle = 0;
    private float prevAngle = 0;
    private float angleCounter = 0;

    void Start()
    {
        gameObject.transform.position = new Vector3(Random.Range(-.4f, .4f), Random.Range(-.4f, .4f), -1);
        gameObject.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
    }

    void Update()
    {
        if (roverStatus == RoverStatus.moving)
        {
            gameObject.transform.position += gameObject.transform.up * movementSpeed * Time.deltaTime;
            
            float dist = Vector3.Distance(new Vector3(posXStart, posYStart, -1), transform.position);
            if (dist >= moveDistance)
            {
                roverStatus = RoverStatus.neutral;
            }
        }

        if(rotating)
        {
            if (rotationAngle < 0)
            {
                gameObject.transform.Rotate(Vector3.forward * (rotationSpeed * Time.deltaTime));
            } else
            {
                gameObject.transform.Rotate(Vector3.forward * (-rotationSpeed * Time.deltaTime));
            }
            
            prevAngle = angle;
            angle = Quaternion.Angle(transform.rotation, Quaternion.Euler(0, 0, rotationStart));

            angleCounter = angleCounter + Mathf.Abs(prevAngle - angle);

            if (angleCounter >= Mathf.Abs(rotationAngle))
            {
                rotating = false;
            }
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
        moveDistance = x * 0.1f;
    }

    public void Rotate(int degrees)
    {
        angle = Quaternion.Angle(transform.rotation, Quaternion.Euler(0, 0, rotationStart));
        angleCounter = 0;    

        rotationStart = gameObject.transform.rotation.z;
        rotationAngle = degrees;

        rotating = true;
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
