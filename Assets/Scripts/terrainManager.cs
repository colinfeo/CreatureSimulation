using UnityEngine;
using System.Collections;

public class terrainManager : MonoBehaviour {

    //object references
    public terrainTile terrainTileProto;

    //constants
    float tileWidth = 1.0f;
    float tileHeight = 1.0f;
    static int tilesSceneWide = 200;
    static int tilesSceneHigh = 200;
    terrainTile[,] terrainArray;


    void Awake() 
    {
        tileWidth = terrainTileProto.transform.localScale.x;
        tileHeight = terrainTileProto.transform.localScale.y;
        terrainArray = new terrainTile[tilesSceneWide,tilesSceneHigh];    
        for (int i = 0; i < terrainArray.GetLength(0); i++) 
        {
            for (int j = 0; j < terrainArray.GetLength(1); j++) 
            {
                Vector3 terrainTileLocation = new Vector3((i * tileWidth),(j * tileHeight),0.0f);
                terrainTileLocation += new Vector3 (0.5f*tileWidth,0.5f*tileHeight,0.0f); //adjust for objects position being center-based
                terrainArray [i, j] = (terrainTile)Object.Instantiate(terrainTileProto,terrainTileLocation,Quaternion.identity);
                terrainArray [i, j].tag = "Terrain";
            }
        }
        GenerateTerrainDistribution(Time.time);
    }

    // Use this for initialization
    void Start () 
    {

    }
    
    // Update is called once per frame
    void Update () 
    {
    
    }

    public Vector3 getCenterOfTerrain()
    {
        return new Vector3 (terrainArray.GetLength(0) * tileWidth / 2.0f, terrainArray.GetLength(1) * tileHeight / 2.0f, 0.0f);
    }
        
    public e_terrainType getTypeAtLocation(Vector2 location)
    {
        if (location.x > tilesSceneWide * tileWidth || location.x < 0 || location.y < 0 || location.y > tilesSceneHigh * tileHeight) 
        {
            Debug.LogErrorFormat ("Asked for terrain type outside bounds of terrain");
            return e_terrainType.enum_numOf;
        }
        location.x = location.x / tileWidth;
        location.y = location.y / tileHeight;
        terrainTile tileInQuestion = terrainArray [Mathf.FloorToInt(location.x), Mathf.FloorToInt(location.y)];
        return tileInQuestion.GetTerrainType();
    }

    public void GenerateTerrainDistribution(float seed)
    {
        for (int i = 0; i < terrainArray.GetLength(0); i++) 
        {
            for (int j = 0; j < terrainArray.GetLength(1); j++) 
            {
                terrainTile currentTile = terrainArray[i, j];
                int perlinValue = Mathf.RoundToInt(Mathf.PerlinNoise(currentTile.transform.position.x + seed, currentTile.transform.position.y + seed));
                terrainArray[i, j].setTerrainType((e_terrainType)perlinValue);

            }
        }
    }
}
