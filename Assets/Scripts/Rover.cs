using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

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
    Slider fsb, dsb, asb;

    float waterX;
    float waterY;


    void Start()
    {
        map = GameObject.FindWithTag("Map").GetComponent<MapNoise>();
        GameObject.FindWithTag("RoverCamGlow").GetComponent<PostProcessVolume>().profile.TryGetSettings(out robotCamGlow);
        droneDisplay = GameObject.FindWithTag("DroneDisplay");
        fsb = GameObject.FindWithTag("FSBSlider").GetComponent<Slider>();
        dsb = GameObject.FindWithTag("DSBSlider").GetComponent<Slider>();
        asb = GameObject.FindWithTag("ASBSlider").GetComponent<Slider>();

        enemyDisplay = transform.parent.GetComponent<EnemyDisplay>();
        gameObject.transform.position = new Vector3(Random.Range(-.4f, .4f), Random.Range(-.4f, .4f), -3);
        gameObject.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));

        GenerateWaterLocation();
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
                RotateDroneDisplay(Random.Range(178f, 182f));
            } else
            {
                RotateDroneDisplay(180f);
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

        FuelConsumption();

        if(hullHealth < 5)
        {
            if (wheelHealth <= 0)
            {
                EndGameWheels();
            } else
            {
                EndGameHull();
            }
        }

        if(fuel <= 0)
        {
            EndGameFuel();
        }
    }

    public void GenerateWaterLocation()
    {
        waterX = Random.Range(-0.4f, 0.4f);
        waterY = Random.Range(-0.3f, 0.2f);
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
        dsb.value = 9 - (int)(hullHealth / 10);
    }

    void EndGameHull()
    {
        Debug.Log("Game Over because hull rip");
    }

    void EndGameWheels()
    {
        Debug.Log("Game Over because wheels rip");
    }

    void EndGameFuel()
    {
        Debug.Log("Game Over because fuel rip");
    }

    void Defend(){
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(new Vector2(enemy.transform.position.x, enemy.transform.position.y), new Vector2(transform.position.x, transform.position.y));
            if(distance < combatDistance)
            {
                if(roverStatus == RoverStatus.defending || ammo > 0)
                {
                    enemy.GetComponent<Enemy>().TakeDamage(attack * defenceMultiplier * Time.deltaTime);
                }
                else 
                {
                    enemy.GetComponent<Enemy>().TakeDamage(attack * Time.deltaTime);
                }
                ammo -= attack * Time.deltaTime;
                asb.value = 9 - (int)(ammo / 10);
                return;
            }
        }
    }

    void FuelConsumption()
    {
        fuel -= 0.001f;
        if(roverStatus == RoverStatus.moving || rotating)
        {
            fuel -= 0.002f;
        }
        if (roverStatus == RoverStatus.defending) 
        {
            fuel -= 0.004f;
        }
        fsb.value = 9 - (int)(fuel / 10);
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

        float distanceToWater = Vector2.Distance(new Vector2(transform.position.x, transform.position.y), new Vector2(waterX, waterY));
        Debug.Log("Distance to water: " + distanceToWater);
        int percentage = 100 - (int)(100 *(distanceToWater / 2f));
        if (percentage < 0)
        {
            percentage = 0;
        }
        if(percentage > 90) 
        {
            EndGameWin();
        }
        else{
            GameObject.Find("Send").GetComponent<CommandLineButton>().PrintMessage("< Ground humidity: " + percentage + "%");
        }
    }
    
    void EndGameWin()
    {
        GameObject.Find("Send").GetComponent<CommandLineButton>().PrintMessage("< Water spring detected!");
    }
}
