using UnityEngine;
using System.Collections;

public class gameManager : MonoBehaviour {
    public Camera mainCamera; 
    terrainManager terrainScript;

    void Awake ()
    {
        terrainScript = GetComponent<terrainManager>();
    }

    void Start ()
    {
        mainCamera.transform.position = terrainScript.getCenterOfTerrain() + new Vector3(0.0f,0.0f,-30.0f);
    }
    // Update is called once per frame
    void Update () 
    {
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 screenSpaceMouse = Input.mousePosition;
            screenSpaceMouse.z = -1.0f * (mainCamera.transform.position.z);
            Vector3 vec = mainCamera.ScreenToWorldPoint(screenSpaceMouse);
            Debug.Log(terrainScript.getTypeAtLocation(vec));
        }    

        if (Input.GetMouseButtonUp(1))
        {
            terrainScript.GenerateTerrainDistribution(Time.time);
            terrainScript.
        }
    }
}
