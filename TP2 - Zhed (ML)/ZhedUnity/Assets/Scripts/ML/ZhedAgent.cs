using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

using System.Collections;
using System.Collections.Generic;

using ZhedSolver;

public class ZhedAgent : Agent
{
    private float moveMissReward = -0.1f;
    private float moveHitReward = 2f;
    private float lossReward = -5f;
    private float winReward = 50f;
    private float moveValueRewardMultiplier = 0f;

    private int gridSize = 8;

    private AgentStats stats;

    private GameManagerScript gameManager;

    private ZhedBoard level10 =  new ZhedBoard("Levels/level" + 10 + ".txt");

    private List<ZhedBoard> zhedBoards = new List<ZhedBoard>(new ZhedBoard[] {
        new ZhedBoard("Levels/level" + 1 + ".txt"),
        new ZhedBoard("Levels/level" + 2 + ".txt"),
        new ZhedBoard("Levels/level" + 3 + ".txt"),
        new ZhedBoard("Levels/level" + 4 + ".txt"),
        new ZhedBoard("Levels/level" + 5 + ".txt"),
        //new ZhedBoard("Levels/level" + 6 + ".txt"),
        //new ZhedBoard("Levels/level" + 7 + ".txt"),
        new ZhedBoard("Levels/levelx" + 1 + ".txt"),
        new ZhedBoard("Levels/levelx" + 2 + ".txt"),
        new ZhedBoard("Levels/levelx" + 3 + ".txt"),
        new ZhedBoard("Levels/levelx" + 4 + ".txt"),
        new ZhedBoard("Levels/levelx" + 5 + ".txt"),
        new ZhedBoard("Levels/levelx" + 6 + ".txt"),
       // new ZhedBoard("Levels/levelx" + 7 + ".txt"),
      // new ZhedBoard("Levels/level" + 8 + ".txt"),
      // new ZhedBoard("Levels/level" + 9 + ".txt"),
      // new ZhedBoard("Levels/level" + 10 + ".txt"),
    });

    public override void Initialize()
    {
        stats = new AgentStats();
        gameManager = GetComponentInParent<GameManagerScript>();
        SetResetParameters();

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        if (gameManager.zhedBoard == null)
            return;

        // Debug.Log("Collecting");
       // SetMask();

        for(int i = 0; i < gridSize ;i++){
           sensor.AddObservation(gameManager.zhedBoard.getBoardRow(i, gridSize));
        }
    }

    public override void CollectDiscreteActionMasks(DiscreteActionMasker actionMasker) {
        /*
        ZhedBoard zhedBoard = this.gameManager.zhedBoard;
        for (int i = 0; i < gridSize * gridSize; i++) {
            if (!zhedBoard.ValidMove(ToCoords(i))) {
                Debug.Log("Masking " + ToCoords(i));
                actionMasker.SetMask(1, i);
            }
        }
        */
    }

    public Coords ToCoords(int action) {
        return new Coords(action % gridSize, action / gridSize);
    }

    public override void OnActionReceived(float[] vectorAction)
    {       
        ZhedBoard board = gameManager.zhedBoard;
        if (board == null)
            return;

        //List<Coords> tiles = this.board.GetValueTilesCoords();
        //if (tiles.Count == 0)
        //    return;

        int direction = Mathf.FloorToInt(vectorAction[0]);
        Coords move = ToCoords(Mathf.FloorToInt(vectorAction[1]));
       // Coords move = new Coords(Mathf.FloorToInt(vectorAction[1]), Mathf.FloorToInt(vectorAction[2]));
        
        this.transform.position = this.gameManager.TilePos(move) + new Vector3(0, this.transform.position.y, 0);
        
        if (!board.ValidMove(move)) {
          // if (board.ValidMove(Coords.MoveUp(move)) ||
          //     board.ValidMove(Coords.MoveDown(move)) ||
          //     board.ValidMove(Coords.MoveLeft(move)) ||
          //     board.ValidMove(Coords.MoveRight(move))) 
          // {
          //     AddReward(this.moveMissReward / 2f);
          // }
            AddReward(this.moveMissReward);
            this.stats.OnMiss();
            return;
        }
        else {
            this.stats.OnHit();
            AddReward(this.moveHitReward);    
        }
 
        switch (direction) {
            case 0: gameManager.Play(move, Coords.MoveUp); break;  
            case 1: gameManager.Play(move, Coords.MoveLeft); break;
            case 2: gameManager.Play(move, Coords.MoveDown); break;
            case 3: gameManager.Play(move, Coords.MoveRight); break;
            default: Debug.LogError("Error: " + direction + " is not a valid dir"); break;
        }


        if (gameManager.Loser()) {
            this.SetColor(Color.red);
            AddReward(this.lossReward);// * Mathf.Max(0.5f, this.stats.MissRatio()));
            this.stats.OnLoss();
            EndEpisode();
        }
        else if (gameManager.Winner()) {
            this.SetColor(Color.green);
            AddReward(this.winReward);
            this.stats.OnWin();
            EndEpisode();
        }
        else if (board.ValidMove(move)) {
           // AddReward(gameManager.zhedBoard.getBoardTotalMaxValue() - ZhedSolver.Solver.Heuristic2(gameManager.zhedBoard) * 0.0075f);
          //  AddReward(gameManager.zhedBoard.getBoardMaxValue() * this.moveValueRewardMultiplier);
        }
    }

    public void SetColor(Color color) {
        this.GetComponent<Renderer>().material.color = color;
    }

    public override void OnEpisodeBegin()
    {
        if (gameManager.zhedBoard == null)
            return;


        this.stats.Print();
        Academy.Instance.StatsRecorder.Add("Hit Ratio", this.stats.HitRatio());
        Academy.Instance.StatsRecorder.Add("Win Count", this.stats.NumWins());
        Academy.Instance.StatsRecorder.Add("Win Ratio on Previous 5", this.stats.WinRatioLast5());
        this.stats.Reset();


        gameManager.LoadLevel(this.zhedBoards[Random.Range(0, this.zhedBoards.Count)]);
        //gameManager.ResetLevel();

       // gameObject.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
       // gameObject.transform.position = center.position;
       // m_BallRb.velocity = new Vector3(Random.Range(-1.5f, 1.5f), 0f, Random.Range(-1.5f, 1.5f));
       // ball.transform.position = new Vector3(0, center.position.y + 7.5f, 0) + center.position;
        //Reset the parameters when the Agent is reset.
       // SetResetParameters();
    }

    public override void Heuristic(float[] actionsOut)
    {
        //actionsOut[0] = -Input.GetAxis("Horizontal");
        //actionsOut[1] = Input.GetAxis("Vertical");
    }

    public void SetResetParameters()
    {

    }



}