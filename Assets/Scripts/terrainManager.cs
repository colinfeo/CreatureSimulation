using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class terrainManager : MonoBehaviour {

    //object references
    //public terrainTile terrainTileProto;

    //constants
    float tileWidth = 1.0f;
    float tileHeight = 1.0f;
    static int tilesSceneWide = 250;
    static int tilesSceneHigh = 250;
    e_terrainTile[,] terrainArray;
    Mesh terrainMesh;
    MeshRenderer meshRenderer;
    MeshFilter meshFilter;

    void Awake() 
    {
        terrainArray = new e_terrainTile[tilesSceneWide,tilesSceneHigh];    
        for (int i = 0; i < terrainArray.GetLength(0); i++) 
        {
            for (int j = 0; j < terrainArray.GetLength(1); j++) 
            {
                Vector3 terrainTileLocation = new Vector3((i * tileWidth),(j * tileHeight),0.0f);
                //terrainTileLocation += new Vector3 (0.5f*tileWidth,0.5f*tileHeight,0.0f); //adjust for objects position being center-based
                e_terrainTile newTile = new e_terrainTile();
                newTile.fertility = 0;
                newTile.type = e_terrainType.enum_Ground;
                newTile.location = terrainTileLocation;
                terrainArray[i, j] = newTile;
                //terrainArray [i, j] = (terrainTile)Object.Instantiate(terrainTileProto,terrainTileLocation,Quaternion.identity);
                //terrainArray [i, j].tag = "Terrain";
            }
        }

        terrainMesh = new Mesh();
    }

    // Use this for initialization
    void Start () 
    {
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        meshFilter = gameObject.GetComponent<MeshFilter>();
        meshFilter.mesh = terrainMesh;

        GenerateTerrainDistribution(Time.time);   
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
        e_terrainTile tileInQuestion = terrainArray [Mathf.FloorToInt(location.x), Mathf.FloorToInt(location.y)];
        return tileInQuestion.type;
    }

    public void GenerateTerrainDistribution(float seed)
    {
        for (int i = 0; i < terrainArray.GetLength(0); i++) 
        {
            for (int j = 0; j < terrainArray.GetLength(1); j++) 
            {
                e_terrainTile currentTile = terrainArray[i, j];
                int perlinValue = Mathf.RoundToInt(Mathf.PerlinNoise(currentTile.location.x + seed, currentTile.location.y + seed));
                terrainArray[i, j].type = ((e_terrainType)perlinValue);

            }
        }
        UpdateMesh();
    }

    private void UpdateMesh()
    {
        //clear out terrain mesh
        terrainMesh.Clear();

        terrainMesh.subMeshCount = 2;
        /*Material[] materialsTemp = new Material[]
        {
            Resources.Load("mat_terrainGround") as Material,
            Resources.Load("mat_terrainWater") as Material
        };
        meshRenderer.materials = materialsTemp; */

        Vector3[] verticesTemp = new Vector3[(terrainArray.GetLength(0) + 1) * (terrainArray.GetLength(1) + 1)];
        Vector3[] normalsTemp = new Vector3[(terrainArray.GetLength(0) + 1) * (terrainArray.GetLength(1) + 1)];
        //int[] trianglesTemp = new int[terrainArray.GetLength(0) * terrainArray.GetLength(1) * 6];

        Dictionary<e_terrainType, List<int>> trianglesByMaterial = new Dictionary<e_terrainType, List<int>>();
        List<int> mat1TriList = new List<int>(terrainArray.GetLength(0) * terrainArray.GetLength(1) * 6);
        List<int> mat2TriList = new List<int>(terrainArray.GetLength(0) * terrainArray.GetLength(1) * 6);
        trianglesByMaterial[e_terrainType.enum_Ground] = mat1TriList;
        trianglesByMaterial[e_terrainType.enum_Water] = mat2TriList;

        //add vertices and normals
        for (int i = 0; i < terrainArray.GetLength(0) + 1; i++) 
        {
            for (int j = 0; j < terrainArray.GetLength(1) + 1; j++) 
            {
                int index = i * terrainArray.GetLength(0) + j;
                verticesTemp[index] = new Vector3(i*tileWidth, j*tileHeight);
                normalsTemp[index] = Vector3.forward;
            }
        }   

        //enter triangles
        //iterating through each tile
        for (int i = 0; i < terrainArray.GetLength(0); i++) 
        {
            for (int j = 0; j < terrainArray.GetLength(1); j++) 
            {
                //
                //get index of 4 vertices making up this tile
                // P1----P3
                // |     |
                // |     |
                // P2----P4
                //
                int P1 = (i * tilesSceneWide) + j;
                int P2 = ((i + 1) * tilesSceneWide) + j;
                int P3 = P1 + 1;
                int P4 = P2 + 1;
                //int indexStart = (i * tilesSceneWide + j) * 6;
                trianglesByMaterial[terrainArray[i, j].type].Add(P1);
                trianglesByMaterial[terrainArray[i, j].type].Add(P3);
                trianglesByMaterial[terrainArray[i, j].type].Add(P4);
                trianglesByMaterial[terrainArray[i, j].type].Add(P4);
                trianglesByMaterial[terrainArray[i, j].type].Add(P2);
                trianglesByMaterial[terrainArray[i, j].type].Add(P1);

                /*
                trianglesTemp[indexStart] = P1;
                trianglesTemp[indexStart + 1] = P3;
                trianglesTemp[indexStart + 2] = P4;
                trianglesTemp[indexStart + 3] = P4;
                trianglesTemp[indexStart + 4] = P2;
                trianglesTemp[indexStart + 5] = P1;
                */

            }
        }
        terrainMesh.vertices = verticesTemp;
        terrainMesh.normals = normalsTemp;
        //terrainMesh.triangles = trianglesTemp;
        terrainMesh.SetTriangles(trianglesByMaterial[e_terrainType.enum_Ground],0);
        terrainMesh.SetTriangles(trianglesByMaterial[e_terrainType.enum_Water],1);
        terrainMesh.RecalculateBounds();
        //terrainMesh.RecalculateNormals();
    }

    public enum e_terrainType
    {
        enum_Ground,
        enum_Water,
        enum_numOf
    };

    private struct e_terrainTile
    {
        public float fertility;
        public Vector3 location;
        public e_terrainType type;
    };
}
