using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

using ZhedSolver;

public class ZhedAgent : Agent
{
    private float moveMissReward = -0.1f;
    private float moveHitReward = 2f;
    private float lossReward = -5f;
    private float winReward = 500f;
    private float moveValueRewardMultiplier = 0f;

    private int gridSize = 10;

    private int hits = 0;
    private int misses = 0;
    private int numWins = 0;

    private GameManagerScript gameManager;

    public override void Initialize()
    {
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
        if (gameManager.zhedBoard == null)
            return;

        //List<Coords> tiles = this.gameManager.zhedBoard.GetValueTilesCoords();
        //if (tiles.Count == 0)
        //    return;

        int direction = Mathf.FloorToInt(vectorAction[0]);
        Coords move = ToCoords(Mathf.FloorToInt(vectorAction[1]));
       // Coords move = new Coords(Mathf.FloorToInt(vectorAction[1]), Mathf.FloorToInt(vectorAction[2]));
        
        this.transform.position = this.gameManager.TilePos(move) + new Vector3(0, this.transform.position.y, 0);
        
        if (!gameManager.zhedBoard.ValidMove(move)) {
            misses++;
            AddReward(this.moveMissReward);
            return;
        }
        else {
            hits++;
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
            AddReward(this.lossReward);
            EndEpisode();
        }
        else if (gameManager.Winner()) {
            this.SetColor(Color.green);
            AddReward(this.winReward);
            numWins++;
            EndEpisode();
        }
        else {
            AddReward(gameManager.zhedBoard.getBoardMaxValue() * this.moveValueRewardMultiplier);
        }
    }

    public void SetColor(Color color) {
        this.GetComponent<Renderer>().material.color = color;
    }

    public override void OnEpisodeBegin()
    {
        float ratio = hits / (hits + misses + 0.00001f);
        hits = 0;
        misses = 0;
        Academy.Instance.StatsRecorder.Add("Hit Ratio", ratio);
        Academy.Instance.StatsRecorder.Add("Win Count", numWins);
        Debug.Log("Ratio: " + ratio);
        Debug.Log("Win count: " + numWins);
        gameManager.ResetLevel();
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