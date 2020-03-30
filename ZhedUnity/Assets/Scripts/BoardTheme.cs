using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardTheme : MonoBehaviour
{
    public static Color primaryColor;
    public static Color idleColor;
    public static Color selectedColor;

    public GameObject colorPicker;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        primaryColor = colorPicker.GetComponent<FlexibleColorPicker>().color;
        idleColor = EndarkenColor(primaryColor, 1.25f);
        selectedColor = EndarkenColor(primaryColor, 2.25f);

        UpdateBoardColor();

        
    }

    private void UpdateBoardColor() {
        GetComponent<Renderer>().material.color = EndarkenColor(primaryColor, 2f); 
    }

    public static Color EndarkenColor(Color color, float divisor) {
        return new Color(color.r / divisor, color.g / divisor, color.b / divisor);
    }
}
