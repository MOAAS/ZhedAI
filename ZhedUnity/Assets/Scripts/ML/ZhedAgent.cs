using UnityEngine;
using ZhedSolver;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

using System.Collections.Generic;

public class ZhedAgent : Agent
{
    private GameManagerScript gameManager;

    private int numWins;
    private Dictionary<float,float[]> valuetilesMap = null;

    private List<ZhedBoard> zhedBoards = new List<ZhedBoard>(new ZhedBoard[] {
        //new ZhedBoard("Levels/level" + 1 + ".txt"),
        //new ZhedBoard("Levels/level" + 2 + ".txt"),
        //new ZhedBoard("Levels/level" + 3 + ".txt"),
        //new ZhedBoard("Levels/level" + 4 + ".txt"),
        //new ZhedBoard("Levels/levelx" + 1 + ".txt"),
        //new ZhedBoard("Levels/levelx" + 2 + ".txt"),
        //new ZhedBoard("Levels/levelx" + 3 + ".txt"),
        //new ZhedBoard("Levels/levelx" + 4 + ".txt"),
        //new ZhedBoard("Levels/levelx" + 5 + ".txt"),
        //new ZhedBoard("Levels/levelx" + 6 + ".txt"),
        //new ZhedBoard("Levels/levelx" + 7 + ".txt"),
        //new ZhedBoard("Levels/level" + 5 + ".txt"),
        //new ZhedBoard("Levels/level" + 6 + ".txt"),
        //new ZhedBoard("Levels/level" + 7 + ".txt"),
        //new ZhedBoard("Levels/level" + 8 + ".txt"),
        //new ZhedBoard("Levels/level" + 9 + ".txt"),
        //new ZhedBoard("Levels/level" + 10 + ".txt"),
        //new ZhedBoard("Levels/level" + 11 + ".txt"),
        //new ZhedBoard("Levels/level" + 12 + ".txt"),
    });

    public override void Initialize()
    {
        gameManager = GetComponentInParent<GameManagerScript>();
        SetResetParameters();

    }


    public override void CollectObservations(VectorSensor sensor){
       if (gameManager.zhedBoard == null)
           return;
       if(valuetilesMap == null){
           valuetilesMap = new Dictionary<float,float[]>();
           for(int i = 0; i<10 ;i++){
                List<float> tile = gameManager.zhedBoard.getValueTile(i);
                valuetilesMap[tile[0]] = new float[]{tile[1],tile[2],tile[3]};
            }
       }

       sensor.AddObservation(gameManager.zhedBoard.getFinishTile());   
       foreach(KeyValuePair<float,float[]> entry in valuetilesMap)
           sensor.AddObservation(new List<float>{entry.Key,entry.Value[0],entry.Value[1],entry.Value[2]});
    }

    public override void CollectDiscreteActionMasks(DiscreteActionMasker actionMasker){
        if(valuetilesMap != null){
            List<int> actionIndicesList = new List<int>();
            foreach(KeyValuePair<float,float[]> entry in valuetilesMap){
                if(entry.Value[2] == -10)
                    actionIndicesList.Add((int)entry.Key);
            }
            int[] actionIndices = actionIndicesList.ToArray();
            Debug.Log(actionIndices.Length);

            actionMasker.SetMask(1, actionIndices);
        }
    }

    public override void OnActionReceived(float[] vectorAction)
    {       
        if (gameManager.zhedBoard == null)
            return;
        int dir = Mathf.FloorToInt(vectorAction[0]);
        int tileToPlay = Mathf.FloorToInt(vectorAction[1]);
        if(valuetilesMap==null || valuetilesMap[tileToPlay][2] == -10)
            return;

        Coords coordsToPlay = new Coords((int)valuetilesMap[tileToPlay][0],(int)valuetilesMap[tileToPlay][1]);
        valuetilesMap[tileToPlay] = new float[]{-10,-10,-10};
        
        switch (dir) {
            case 0: gameManager.Play(coordsToPlay, Coords.MoveUp); break;
            case 1: gameManager.Play(coordsToPlay, Coords.MoveLeft); break;
            case 2: gameManager.Play(coordsToPlay, Coords.MoveDown); break;
            case 3: gameManager.Play(coordsToPlay, Coords.MoveRight); break;
            default: Debug.LogError("Error: " + dir + " is not a valid dir"); break;
        }

        if (gameManager.Loser()) {
            valuetilesMap = null;
            this.SetColor(Color.red);
            AddReward(-5f);
            EndEpisode();
        }
        else if (gameManager.Winner()) {
            this.SetColor(Color.green);
            valuetilesMap = null;
            AddReward(200f);
            numWins++;
            EndEpisode();
        }
        else {
            AddReward(gameManager.zhedBoard.getBoardTotalMaxValue() - ZhedSolver.Solver.Heuristic2(gameManager.zhedBoard)/100);
        }
    }

    public override void OnEpisodeBegin()
    {
        gameManager.ResetLevel();
        Academy.Instance.StatsRecorder.Add("Win Count",numWins);
        valuetilesMap = null;
        SetResetParameters();
    }

    public override void Heuristic(float[] actionsOut)
    {

    }

    public void SetResetParameters()
    {

    }

    public void SetColor(Color color) {
        this.GetComponent<Renderer>().material.color = color;
    }

}