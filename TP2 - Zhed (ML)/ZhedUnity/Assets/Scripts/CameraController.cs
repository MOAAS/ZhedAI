using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    
        // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horInput = Input.GetAxis("Horizontal");
        float verInput = Input.GetAxis("Vertical");

        transform.position += new Vector3(horInput, 0, verInput);

        transform.position += transform.forward * Input.GetAxis("Mouse ScrollWheel") * 10;
    
    }
}
