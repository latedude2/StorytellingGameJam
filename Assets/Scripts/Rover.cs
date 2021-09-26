using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Rover : MonoBehaviour
{
    public enum RoverStatus
    {
        neutral = 1,
        defending = 2,
        moving = 3,
        scanningEnemy = 4,
        scanningWater = 5
    }

    [SerializeField] float fuel = 100f;
    [SerializeField] float hullHealth = 100f;
    [SerializeField] float wheelHealth = 100f;
    [SerializeField] float ammo = 1000f;

    float combatDistance = 0.3f;
    float attack = 1f;
    float defenceMultiplier = 5f;

    [SerializeField] private float movementSpeed = 1f;
    private float terrainSpeedMod = 1f;
    RoverStatus roverStatus = RoverStatus.neutral;
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

    private EnemyDisplay enemyDisplay;

    MapNoise map;
    Bloom robotCamGlow;
    GameObject droneDisplay;

    void Start()
    {
        map = GameObject.FindWithTag("Map").GetComponent<MapNoise>();
        GameObject.FindWithTag("RoverCamGlow").GetComponent<PostProcessVolume>().profile.TryGetSettings(out robotCamGlow);
        droneDisplay = GameObject.FindWithTag("DroneDisplay");

        enemyDisplay = transform.parent.GetComponent<EnemyDisplay>();
        gameObject.transform.position = new Vector3(Random.Range(-.4f, .4f), Random.Range(-.4f, .4f), -3);
        gameObject.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
    }

    void Update()
    {
        Defend();
        if (roverStatus == RoverStatus.moving)
        {
            Vector3 speed = gameObject.transform.up * movementSpeed * (.5f + wheelHealth/200) * Time.deltaTime;
            if (TerrainValue() < .25f)
            {
                WheelDamage();
                speed *= terrainSpeedMod;
                RotateDroneDisplay(Random.Range(-2f, 2f));
            } else
            {
                RotateDroneDisplay(0f);
            }
            gameObject.transform.position += speed;
            
            float dist = Vector3.Distance(new Vector3(posXStart, posYStart, transform.position.z), transform.position);
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

        if(hullHealth <= 0)
        {
            EndGameHull();
        }
    }

    public void Move(int x)
    {
        posXStart = gameObject.transform.position.x;
        posYStart = gameObject.transform.position.y;
        roverStatus = RoverStatus.moving;
        moveDistance = x * 0.1f;
    }

    float TerrainValue()
    {
        int roverMapPosX = (int)((gameObject.transform.localPosition.x + .5f) * 1000);
        int roverMapPosY = (int)((gameObject.transform.localPosition.y + .5f) * 1000);

        return map.heightMap[roverMapPosX, roverMapPosY];
    }

    void WheelDamage()
    {
        terrainSpeedMod = TerrainValue()*2;

        wheelHealth -= terrainSpeedMod * 0.05f;
        if (wheelHealth < 0)
        {
            wheelHealth = 0;
            ReceiveHullDamage(terrainSpeedMod*0.05f);
        }

        robotCamGlow.color.value = new Color(1-(wheelHealth/100),0+(wheelHealth/100),0);
    }

    void RotateDroneDisplay(float value)
    {
        droneDisplay.transform.eulerAngles = new Vector3(0,0,value);
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
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(new Vector2(enemy.transform.position.x, enemy.transform.position.y), new Vector2(transform.position.x, transform.position.y));
            if(distance < combatDistance)
            {
                if(roverStatus == RoverStatus.defending)
                {
                    enemy.GetComponent<Enemy>().TakeDamage(attack * defenceMultiplier * Time.deltaTime);
                }
                else 
                {
                    enemy.GetComponent<Enemy>().TakeDamage(attack * Time.deltaTime);
                }
                ammo -= attack * Time.deltaTime;
                return;
            }
        }
    }

    public void DefensiveStance()
    {
        roverStatus = RoverStatus.defending;
    }

    public void ScanForEnemies(){
        roverStatus = RoverStatus.scanningEnemy;
        enemyDisplay.ShowEnemies();
    }

    public void ScanForWater(){
        roverStatus = RoverStatus.scanningWater;
    }
    
    
}
