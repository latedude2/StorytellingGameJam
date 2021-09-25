using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rover : MonoBehaviour
{
    float fuel = 100f;
    float hullHealth = 100f;
    float wheelHealth = 100f;
    float ammo = 100f;

    [SerializeField] private float movementSpeed = 1f;
    private bool moving = false;

    public enum roverStatus
    {
        defending = 1,
        moving = 2,
        scanning = 3
    }

    void Start()
    {
        gameObject.transform.position = new Vector3(Random.Range(-.4f, .4f), Random.Range(-.4f, .4f), -1);
        gameObject.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
    }

    void Update()
    {
        if (moving)
        {
            gameObject.transform.position += gameObject.transform.forward * movementSpeed * Time.deltaTime;
        }
    }

    public void Move(int x)
    {
        float posXStart = gameObject.transform.position.x;
        float posYStart = gameObject.transform.position.y;

        moving = true;
    }
    
}
