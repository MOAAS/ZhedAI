using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorRandomizer : MonoBehaviour
{
    public float minimumDivisor = 1f;
    public float maximumDivisor = 1.5f;

    // Start is called before the first frame update
    void Start()
    {        
        GetComponent<Renderer>().material.color = RandomizeColor(BoardTheme.primaryColor); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Color RandomizeColor(Color defaultColor) {
        float divisor = Random.Range(minimumDivisor, maximumDivisor);        
        return BoardTheme.EndarkenColor(defaultColor, divisor);
    }
}
