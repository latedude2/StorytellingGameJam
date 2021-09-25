using UnityEngine;

public class MapNoise : MonoBehaviour
{
    private int width = 200;
    private int height = 200;
    private int[,] heightMap = new int[200, 200];

    public float bigFreqAmplitude = 1f;
    public float mediumFreqAmplitude = 1f;
    public float lowFreqAmplitude = 1f;

    private float scale = 1f;
    private float offsetX = 0f;
    private float offsetY = 0f;

    private int terrainHueMin = 30;
    private int terrainSaturationMin = 40;

    private int terrainHueMax = 80;
    private int terrainSaturationMax = 100;

    Color convertedCol = new Color(255,255,255);

    void Start()
    {
        Randomize();

        Renderer renderer = GetComponent<Renderer>();

        renderer.material.mainTexture = GenerateTexture();
    }

    void Randomize()
    {
        scale = Random.Range(1f, 1.5f);
        offsetX = Random.Range(0f, 99999f);
        offsetY = Random.Range(0f, 99999f);

        float hue = Random.Range((float)terrainHueMin/255, (float)terrainHueMax/255);
        float sat = Random.Range((float)terrainSaturationMin/255, (float)terrainSaturationMax/255);

        convertedCol = Color.HSVToRGB(hue, sat, 1)*255;
    }

    Texture2D GenerateTexture()
    {
        Texture2D texture = new Texture2D(width, height);

        for(int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color color = CalculateHeight(x, y);
                heightMap[x,y] = color.grayscale;
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

        Color posCol = new Color(perlinCol, perlinCol, perlinCol);
        
        posCol = new Color(perlinCol * (convertedCol.r/255), perlinCol * (convertedCol.g/255), perlinCol * (convertedCol.b/255));

        return posCol;
    }
}
