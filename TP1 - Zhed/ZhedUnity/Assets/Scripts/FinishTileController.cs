using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishTileController : MonoBehaviour
{
    private bool winner;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.winner)
            this.GetComponent<Renderer>().material.color = BoardTheme.selectedColor;
        else this.GetComponent<Renderer>().material.color = BoardTheme.idleColor;
    }

    public void WinGame() {
        this.winner = true;
    }
}
