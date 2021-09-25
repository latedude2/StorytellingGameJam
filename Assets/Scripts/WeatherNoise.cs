using UnityEngine;

public class WeatherNoise : MonoBehaviour
{
    public int width = 256;
    public int height = 256;

    public float bigFreqAmplitude = 1f;
    public float mediumFreqAmplitude = 1f;
    public float lowFreqAmplitude = 1f;

    public float weatherSpeed = 1f;

    private float scale = 1f;
    private float offsetX = 0f;
    private float offsetY = 0f;

    private int terrainHueMin = 140;
    private int terrainSaturationMin = 00;

    private int terrainHueMax = 180;
    private int terrainSaturationMax = 50;

    private int weatherDirectionCountdown = 1000;
    private bool weatherMovingEast = false;
    private bool weatherMovingNorth = false;

    Color convertedCol = new Color(255, 255, 255);

    void Start()
    {
        Randomize();

        Renderer renderer = GetComponent<Renderer>();

        GetComponent<Renderer>().material.mainTexture = GenerateTexture();
    }

    void Update()
    {
        if (weatherMovingEast)
        {
            gameObject.transform.position += new Vector3(Random.Range(0f, 1f) * weatherSpeed, Random.Range(-.1f, .1f) * weatherSpeed, 0);
        } else
        {
            gameObject.transform.position += new Vector3(Random.Range(-1f, 0f) * weatherSpeed, Random.Range(-.1f, .1f) * weatherSpeed, 0);
        }

        if (weatherMovingNorth)
        {
            gameObject.transform.position += new Vector3(Random.Range(.1f, .1f) * weatherSpeed, Random.Range(0f, 1f) * weatherSpeed, 0);
        } else
        {
            gameObject.transform.position += new Vector3(Random.Range(.1f, .1f) * weatherSpeed, Random.Range(-1f, 0f) * weatherSpeed, 0);
        }

        weatherDirectionCountdown--;
        if (weatherDirectionCountdown <= 0)
        {
            int weatherSelect = Random.Range(0, 2);
            if (weatherSelect == 0)
            {
                weatherMovingEast = !weatherMovingEast;
            } else if (weatherSelect == 1)
            {
                weatherMovingNorth = !weatherMovingNorth;
            }
            weatherDirectionCountdown = Random.Range(1000,2000);
        }
    }

    void Randomize()
    {
        scale = Random.Range(100f, 150f);
        offsetX = Random.Range(0f, 99999f);
        offsetY = Random.Range(0f, 99999f);

        float hue = Random.Range((float)terrainHueMin / 255, (float)terrainHueMax / 255);
        float sat = Random.Range((float)terrainSaturationMin / 255, (float)terrainSaturationMax / 255);

        convertedCol = Color.HSVToRGB(hue, sat, 1) * 255;
    }

    Texture2D GenerateTexture()
    {
        Texture2D texture = new Texture2D(width, height);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color color = CalculateHeight(x, y);
                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();
        return texture;
    }

    Color CalculateHeight(int x, int y)
    {
        float xCoordBig = (1 / bigFreqAmplitude) * (float)x / width * scale + offsetX;
        float xCoordMed = (1 / mediumFreqAmplitude) * (float)x / width * scale + offsetX;
        float xCoordLow = (1 / lowFreqAmplitude) * (float)x / width * scale + offsetX;

        float yCoordBig = (1 / bigFreqAmplitude) * (float)y / height * scale + offsetY;
        float yCoordMed = (1 / mediumFreqAmplitude) * (float)y / height * scale + offsetY;
        float yCoordLow = (1 / lowFreqAmplitude) * (float)y / height * scale + offsetY;

        float xCoord = ((bigFreqAmplitude * xCoordBig) + (mediumFreqAmplitude * xCoordMed) + (lowFreqAmplitude * xCoordLow)) / (bigFreqAmplitude + mediumFreqAmplitude + lowFreqAmplitude);
        float yCoord = ((bigFreqAmplitude * yCoordBig) + (mediumFreqAmplitude * yCoordMed) + (lowFreqAmplitude * yCoordLow)) / (bigFreqAmplitude + mediumFreqAmplitude + lowFreqAmplitude);

        float perlinCol = Mathf.PerlinNoise(xCoord, yCoord);

        Color posCol = new Color(perlinCol, perlinCol, perlinCol, 0f);
        
        if(perlinCol > .7f)
        {
            posCol = new Color(perlinCol * (convertedCol.r / 255), perlinCol * (convertedCol.g / 255), perlinCol * (convertedCol.b / 255), (perlinCol/2));
        }

        return posCol;
    }
}
