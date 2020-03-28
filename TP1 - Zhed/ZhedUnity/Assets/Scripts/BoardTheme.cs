using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardTheme : MonoBehaviour
{
    public static Color primaryColor = new Color(0.478f, 0.425f, 0.876f);


    public static Color idleColor = new Color(0.5f, 0.4f, 0.5f);
    public static Color selectedColor = new Color(0.3f, 0.2f, 0.3f);

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().material.color = EndarkenColor(primaryColor, 1.2f);        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static Color EndarkenColor(Color color, float divisor) {
        return new Color(color.r / divisor, color.g / divisor, color.b / divisor);
    }
}
