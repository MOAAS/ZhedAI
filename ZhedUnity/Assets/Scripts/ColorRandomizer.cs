using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorRandomizer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {        
        GetComponent<Renderer>().material.color = RandomizeColor(BoardTheme.primaryColor); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static Color RandomizeColor(Color defaultColor) {
        float divisor = Random.Range(1f, 1.5f);
        return BoardTheme.EndarkenColor(defaultColor, divisor);
    }
}
