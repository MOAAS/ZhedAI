using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelButton : MonoBehaviour
{
    public int levelNo;

    private Button button;
    private TextMeshProUGUI buttonText;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>();

        if (levelNo != 0) {
            button.onClick.AddListener(delegate { 
                GameObject.Find("GamerManager").GetComponent<GamerManagerScript>().LoadLevel("Levels/level" + levelNo + ".txt");
            });
            name = "Load Level " + levelNo;        
            buttonText.text = "Level " + levelNo;
        }
        else {
            name = "No Level Load";
            buttonText.text = "Coming soon";
        }
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
