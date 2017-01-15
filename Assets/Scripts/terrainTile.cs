using UnityEngine;
using System.Collections;

public enum e_terrainType
{
    enum_Ground,
    enum_Water,
    enum_numOf
};

public class terrainTile : MonoBehaviour {

    public Material mat_Ground;
    public Material mat_Water;
    e_terrainType terrainType;
    float fertility;

    // Use this for initialization
    void Start () 
    {
        
    }

    public e_terrainType GetTerrainType()
    {
        return terrainType;
    }

    public void setTerrainType(e_terrainType type)
    {
        if (type == e_terrainType.enum_Ground)
        {
            terrainType = e_terrainType.enum_Ground;
            GetComponent<MeshRenderer>().material = mat_Ground;
        }
        if (type == e_terrainType.enum_Water) 
        {
            terrainType = e_terrainType.enum_Water;
            GetComponent<MeshRenderer>().material = mat_Water;
        }


    }
}
