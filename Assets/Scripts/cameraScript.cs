using UnityEngine;
using System.Collections;

public class cameraScript : MonoBehaviour 
{
    public float translationSpeed;
    public float scrollSpeed; 
	
    // Use this for initialization
	void Start () 
    {
        
	}
	
	// Update is called once per frame
	void Update () 
    {
        float horizontalValue = Input.GetAxis("Horizontal") * translationSpeed;
        float verticalValue = Input.GetAxis("Vertical") * translationSpeed;
        float scrollWheelValue = Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        gameObject.transform.Translate(new Vector3(horizontalValue,verticalValue,scrollWheelValue));

	}
}
