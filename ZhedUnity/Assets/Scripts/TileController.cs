using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using ZhedSolver;

public class TileController : MonoBehaviour
{
    private Animator animator;   
    private TextMeshPro tileText;   
    private GameManagerScript gameManager;

    public int tileValue;
    public Coords coords;


    private bool selected = false;

    // Start is called before the first frame update
    void Start()
    {
        this.animator = GetComponent<Animator>();
        this.tileText = transform.Find("Tile Text").GetComponent<TextMeshPro>();
        this.gameManager = GameObject.Find("GameManager").GetComponent<GameManagerScript>();

        this.updateColor();
    }


    // Update is called once per frame
    void Update()
    {
        this.tileText.text = this.tileValue.ToString();
    }

    public void SetTileInfo(Coords coords, int value) {
        this.coords = coords;
        this.tileValue = value;
    }

    public void Select(bool selected) {
        this.selected = selected;
        this.animator.SetBool("selected", selected);
        this.updateColor();
    }

    void updateColor() {
        if (this.selected)
            this.GetComponent<Renderer>().material.color = BoardTheme.selectedColor;
        else this.GetComponent<Renderer>().material.color = BoardTheme.idleColor;
    }

    void OnMouseDown() {
        this.gameManager.OnPieceSelected(this);
        //GameObject.Find("GameManager").GetComponent<GameManagerScript>().Play(this, Coords.MoveLeft);
    }
}
