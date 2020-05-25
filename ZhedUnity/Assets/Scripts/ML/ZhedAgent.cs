using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

using System.Collections.Generic;

public class ZhedAgent : Agent
{
    private GameManagerScript gameManager;

    private int numWins;
    private Dictionary<float,float[]> valuetilesMap = null;

    public override void Initialize()
    {
        gameManager = GetComponentInParent<GameManagerScript>();
        SetResetParameters();

    }

/*  observations-> coordenadas/valores e indices de 1 a num de valtiles
    public override void CollectObservations(){
        if (gameManager.zhedBoard == null)
            return;
 
       AddVectorObs(gameManager.zhedBoard.getFinishTile());
       for(int i = 0; i<10 ;i++){
           AddVectorObs(gameManager.zhedBoard.getValueTile(i));
       }
       int num = gameManager.zhedBoard.getValueTilesSize();
       int[] actionIndices = new int[10 - num];
       int j = 0;
       for(int i = num ;i<10;i++){
           actionIndices[j] = i;
           j++; 
       }

       SetActionMask(1, actionIndices);
    }

    public override void AgentAction(float[] vectorAction)
    {       
        if (gameManager.zhedBoard == null)
            return;
        int dir = Mathf.FloorToInt(vectorAction[0]);
        int tileToPlay = Mathf.FloorToInt(vectorAction[1]);
        List<Coords> tiles = this.gameManager.zhedBoard.GetValueTilesCoords();
        Debug.Log(tileToPlay);
        if(tileToPlay>=gameManager.zhedBoard.getValueTilesSize())
            return;

        switch (dir) {
            case 0: gameManager.Play(tiles[tileToPlay], Coords.MoveUp); break;
            case 1: gameManager.Play(tiles[tileToPlay], Coords.MoveLeft); break;
            case 2: gameManager.Play(tiles[tileToPlay], Coords.MoveDown); break;
            case 3: gameManager.Play(tiles[tileToPlay], Coords.MoveRight); break;
            default: Debug.LogError("Error: " + dir + " is not a valid dir"); break;
        }

        if (gameManager.Loser()) {
            //this.SetColor(Color.red);
            AddReward(-5f);
            Done();
        }
        else if (gameManager.Winner()) {
            //this.SetColor(Color.green);
            AddReward(500f);
            Done();
        }
        else {
            AddReward(gameManager.zhedBoard.getBoardTotalMaxValue());
        }
    }
*/

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
       {
           // do something with entry.Value or entry.Key
           sensor.AddObservation(new List<float>{entry.Key,entry.Value[0],entry.Value[1],entry.Value[2]});
       }
       /*
       for(int i = 0; i<10 ;i++){
          AddVectorObs(gameManager.zhedBoard.getBoardRow(i));
       }
       */
/*
       List<int> actionIndicesList = new List<int>();
       foreach(KeyValuePair<float,float[]> entry in valuetilesMap){
           if(entry.Value[0] == -10)
            actionIndicesList.Add((int)entry.Key);
       }

       int[] actionIndices = actionIndicesList.ToArray();
       Debug.Log(actionIndices);

       SetActionMask(1, actionIndices);*/
    }

    public override void CollectDiscreteActionMasks(DiscreteActionMasker actionMasker){
        if(valuetilesMap != null){
            List<int> actionIndicesList = new List<int>();
            foreach(KeyValuePair<float,float[]> entry in valuetilesMap){
                if(entry.Value[2] == -10)
                    actionIndicesList.Add((int)entry.Key);
            }
            int[] actionIndices = actionIndicesList.ToArray();
            //Debug.Log(actionIndices.Length);

            actionMasker.SetMask(1, actionIndices);
        }
    }

    public override void OnActionReceived(float[] vectorAction)
    {       
        if (gameManager.zhedBoard == null)
            return;
        int dir = Mathf.FloorToInt(vectorAction[0]);
        int tileToPlay = Mathf.FloorToInt(vectorAction[1]);
        //List<Coords> tiles = this.gameManager.zhedBoard.GetValueTilesCoords();
        //Debug.Log(tileToPlay);
        if(valuetilesMap==null || valuetilesMap[tileToPlay][2] == -10)
            return;

        Coords coordsToPlay = new Coords((int)valuetilesMap[tileToPlay][0],(int)valuetilesMap[tileToPlay][1]);
        //Debug.Log("x: " + (int)valuetilesMap[tileToPlay][0] + " y: " + (int)valuetilesMap[tileToPlay][1]);
        valuetilesMap[tileToPlay] = new float[]{-10,-10,-10};
        
        switch (dir) {
            case 0: gameManager.Play(coordsToPlay, Coords.MoveUp); break;
            case 1: gameManager.Play(coordsToPlay, Coords.MoveLeft); break;
            case 2: gameManager.Play(coordsToPlay, Coords.MoveDown); break;
            case 3: gameManager.Play(coordsToPlay, Coords.MoveRight); break;
            default: Debug.LogError("Error: " + dir + " is not a valid dir"); break;
        }

        if (gameManager.Loser()) {
            //Debug.Log("gamer");
            valuetilesMap = null;
            AddReward(-5f);
            EndEpisode();
        }
        else if (gameManager.Winner()) {
            valuetilesMap = null;
            AddReward(200f);
            numWins++;
            EndEpisode();
        }
        else {
            //AddReward(gameManager.zhedBoard.getBoardTotalMaxValue());
            AddReward(gameManager.zhedBoard.getBoardTotalMaxValue() - ZhedSolver.Solver.Heuristic2(gameManager.zhedBoard)/100);
        }
    }

    public override void OnEpisodeBegin()
    {
        gameManager.ResetLevel();
        Academy.Instance.StatsRecorder.Add("Win Count",numWins);
        valuetilesMap = null;
       // gameObject.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
       // gameObject.transform.position = center.position;
       // m_BallRb.velocity = new Vector3(Random.Range(-1.5f, 1.5f), 0f, Random.Range(-1.5f, 1.5f));
       // ball.transform.position = new Vector3(0, center.position.y + 7.5f, 0) + center.position;
        //Reset the parameters when the Agent is reset.
        SetResetParameters();
    }

    public override void Heuristic(float[] actionsOut)
    {/*
        var action = new float[2];

        action[0] = -Input.GetAxis("Horizontal");
        action[1] = Input.GetAxis("Vertical");
        return action;*/
    }

    public void SetResetParameters()
    {

    }

}