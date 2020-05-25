using System.Collections;
using System.Collections.Generic;
using TMPro;

using UnityEngine;

using ZhedSolver;
using System;
using System.Threading.Tasks;

public class GameManagerScript : MonoBehaviour
{

    // Prefabs
    public GameObject floatingPreviewPrefab;
    public GameObject emptyTilePrefab;
    public GameObject valueTilePrefab;
    public GameObject usedTilePrefab;
    public GameObject finishTilePrefab;
 
    // UI
    public GameObject youWin;
    public GameObject youLose;
    
    // Boards
    private String boardPath;
    public ZhedBoard zhedBoard;
    public ZhedBoard initialZhedBoard;

    // Child objects
    private GameObject board;
    private Dictionary<Coords, GameObject> finishTiles;
    private Dictionary<Coords, GameObject> valueTiles;
    private TextMeshPro boardText;


    // Interaction
    private TileController selectedTile;
    private float selectTime;
    private bool coolGraphics;

    // State
    private bool gameOver;


    private const float SPAWN_DELAY_SECS = 0.1f;
    private const float SOLVER_DELAY_SECS = 0.8f;



    // Start is called before the first frame update
    void Start() {
        this.boardText = this.gameObject.transform.Find("Board Text").GetComponent<TextMeshPro>();
        this.coolGraphics = false;
    }



    public void Hint() {
        if (this.zhedBoard.GetValueTiles().Count == 0)
            return;
        Solver solver = new Solver(this.zhedBoard);
        ZhedStep step = solver.GetHint();
        if (step == null)
            return;
        StartCoroutine(this.PlayZhedStep(step, 0));
    }

    public void Solve() {        
        Solver solver = new Solver(this.zhedBoard);

        var task = Task.Run(() => solver.Solve(SearchMethod.Greedy));
        if (task.Wait(TimeSpan.FromSeconds(10))) {
            List<ZhedStep> zhedSteps = solver.Solve(SearchMethod.Greedy);
            for (int i = 0; i < zhedSteps.Count; i++)
                StartCoroutine(this.PlayZhedStep(zhedSteps[i], i * SOLVER_DELAY_SECS));
        }
    }
    
    private IEnumerator PlayZhedStep(ZhedStep step, float delay) {
        yield return new WaitForSeconds(delay);
        TileController tile = this.valueTiles[step.coords].GetComponent<TileController>();
        

        switch(step.operations) {
            case Operations.MoveUp: Play(tile.coords, Coords.MoveUp); break;
            case Operations.MoveDown: Play(tile.coords, Coords.MoveDown); break;
            case Operations.MoveLeft: Play(tile.coords, Coords.MoveLeft); break;
            case Operations.MoveRight: Play(tile.coords, Coords.MoveRight); break;
        }
    }

    public void ToggleCoolGraphics() {
        this.coolGraphics = !coolGraphics;
        ResetLevel();
    }

    public void ResetLevel() {
        if (this.initialZhedBoard == null)
            return;
        this.zhedBoard = this.initialZhedBoard;

        if (transform.Find("Board") != null) {
            Destroy(transform.Find("Board").gameObject);
        }

        this.board = new GameObject("Board");
        this.board.transform.parent = this.gameObject.transform;


        this.gameOver = false;
        this.valueTiles = new Dictionary<Coords, GameObject>();
        this.finishTiles = new Dictionary<Coords, GameObject>();

        for (int y = 0; y < zhedBoard.height; y++) {
            for (int x = 0; x < zhedBoard.width; x++) {
                MakeTile(new Coords(x, y), emptyTilePrefab, Color.white);
            }
        }        
      

        if (this.coolGraphics) {
            foreach (int[] tile in zhedBoard.GetValueTiles()) {
                Coords coords = new Coords(tile[0], tile[1]);
                GameObject valueTileObject = MakeTile(coords, valueTilePrefab, BoardTheme.idleColor);
                valueTileObject.GetComponent<TileController>().SetTileInfo(coords, tile[2]);
                this.valueTiles.Add(coords, valueTileObject);
            }

            foreach (int[] tile in zhedBoard.GetFinishTiles()) {
                Coords coords = new Coords(tile[0], tile[1]);
                this.finishTiles.Add(coords, MakeTile(coords, finishTilePrefab, Color.white));
            }
        }

    }

    public void LoadLevel(String path) {
        this.initialZhedBoard = new ZhedBoard(path);
        ResetLevel();
        // Update Camera
        GameObject.Find("Main Camera").transform.position = new Vector3(0, zhedBoard.height, -zhedBoard.height / 5.0f);
    }

    public Vector3 TilePos(Coords coords) {
        return new Vector3(coords.x + 0.5f - zhedBoard.width / 2.0f, 0, zhedBoard.height / 2.0f - coords.y - 0.5f) + transform.position;
    }

    GameObject MakeTile(Coords coords, GameObject prefab, Color color) {
        GameObject gameObject = Instantiate(prefab, TilePos(coords), prefab.transform.rotation, board.transform);
        gameObject.GetComponentInChildren<Renderer>().material.color = color;
        return gameObject;
    }


    // Update is called once per frame
    void Update()
    {
        if (zhedBoard == null) {
            boardText.text = "";
            return;
        }

        if (!this.coolGraphics) {
            boardText.text = this.zhedBoard.ToString();
        }    
        else boardText.text = "";

        if (this.selectedTile == null || Time.time < this.selectTime + 0.1f)
            return;

        if (Input.GetMouseButtonDown(0)) {
            Vector3 tilePos = Camera.main.WorldToScreenPoint(selectedTile.gameObject.transform.position);
            Vector3 mousePos = Input.mousePosition;

            Vector2 diffVec = new Vector2(mousePos.x - tilePos.x, mousePos.y - tilePos.y);

            float angle = Vector2.Angle(diffVec, Vector2.right);
            angle = Mathf.Sign(Vector3.Cross(diffVec, Vector2.right).z) > 0 ? (360 - angle) % 360 : angle;

            this.selectedTile.Select(false);

            if (angle < 45 || angle >= 315)
                this.Play(this.selectedTile.coords, Coords.MoveRight);
            else if (angle < 135)
                this.Play(this.selectedTile.coords, Coords.MoveUp);
            else if (angle < 225)
                this.Play(this.selectedTile.coords, Coords.MoveLeft);
            else this.Play(this.selectedTile.coords, Coords.MoveDown);

            DestroyFloatingPreviews();
        }
    }


    void DestroyFloatingPreviews() {
        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("FloatingPreview"))
            Destroy(gameObject);
    }

    public void OnPieceSelected(TileController tile) {
        DestroyFloatingPreviews();

        if (this.selectedTile != null)
            this.selectedTile.Select(false);

        if (this.selectedTile == tile) {      
            this.selectedTile = null;
        }
        else {
            this.selectedTile = tile;
            this.selectTime = Time.time;
            this.selectedTile.Select(true);

            List<Coords> coordsUp = this.zhedBoard.GetSpreadInDir(tile.coords, Coords.MoveUp);
            List<Coords> coordsDown = this.zhedBoard.GetSpreadInDir(tile.coords, Coords.MoveDown);
            List<Coords> coordsLeft = this.zhedBoard.GetSpreadInDir(tile.coords, Coords.MoveLeft);
            List<Coords> coordsRight = this.zhedBoard.GetSpreadInDir(tile.coords, Coords.MoveRight);
            for (int i = 0; i < coordsUp.Count; i++) StartCoroutine(MakePreviewTile(coordsUp[i], i * SPAWN_DELAY_SECS));
            for (int i = 0; i < coordsDown.Count; i++) StartCoroutine(MakePreviewTile(coordsDown[i], i * SPAWN_DELAY_SECS));
            for (int i = 0; i < coordsLeft.Count; i++) StartCoroutine(MakePreviewTile(coordsLeft[i], i * SPAWN_DELAY_SECS));
            for (int i = 0; i < coordsRight.Count; i++) StartCoroutine(MakePreviewTile(coordsRight[i], i * SPAWN_DELAY_SECS));
        }
    }


    public void Play(Coords coords, Func<Coords, Coords> moveFunction) {  
        if (gameOver)
            return;
        if (!this.zhedBoard.ValidMove(coords)) {
            Debug.Log("Invalid move: " + coords);
            return;
        }


        if (this.coolGraphics) {
            TileController tile = this.valueTiles[coords].GetComponent<TileController>();
            Coords currentCoords = tile.coords;
            Destroy(tile.gameObject);        
            StartCoroutine(MakeUsedTile(currentCoords, 0));

            for (int tileValue = tile.tileValue, numUsedTiles = 1; tileValue > 0; tileValue--, numUsedTiles++) {
                currentCoords = moveFunction(currentCoords);
                if(!zhedBoard.inbounds(currentCoords))
                    break;
                switch (zhedBoard.TileValue(currentCoords)) {
                    case ZhedBoard.EMPTY_TILE: StartCoroutine(MakeUsedTile(currentCoords, numUsedTiles * SPAWN_DELAY_SECS)); break;
                    case ZhedBoard.FINISH_TILE: StartCoroutine(MakeWinnerTile(currentCoords, numUsedTiles * SPAWN_DELAY_SECS)); break;
                    default: tileValue++; numUsedTiles--; break;
                }
            }
        }

        this.zhedBoard = ZhedBoard.SpreadTile(this.zhedBoard, coords, moveFunction);


        if (this.Winner()) {
            youWin.SetActive(true);
            gameOver = true;
        }
        else if (this.Loser()) {
            youLose.SetActive(true);
            gameOver = true;
        }
    }

    public bool Winner() {
        return this.zhedBoard.Winner();
    }

    public bool Loser() {
        return this.zhedBoard.Loser();
    }
    

    private IEnumerator MakeUsedTile(Coords coords, float delay) {
        yield return new WaitForSeconds(delay);
        MakeTile(coords, usedTilePrefab, BoardTheme.idleColor);
    }

    private IEnumerator MakePreviewTile(Coords coords, float delay) {
        yield return new WaitForSeconds(delay);
        MakeTile(coords, floatingPreviewPrefab, BoardTheme.selectedColor);          
    }

    private IEnumerator MakeWinnerTile(Coords coords, float delay) {
        yield return new WaitForSeconds(delay);
        Destroy(finishTiles[coords]);
        MakeTile(coords, finishTilePrefab, BoardTheme.selectedColor);          
    }
}
