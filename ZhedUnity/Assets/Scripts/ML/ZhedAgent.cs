using UnityEngine;
using MLAgents;

using System.Collections.Generic;

public class ZhedAgent : Agent
{
    private GameManagerScript gameManager;

    public override void InitializeAgent()
    {
        gameManager = GetComponentInParent<GameManagerScript>();
        SetResetParameters();

    }

    public override void CollectObservations()
    {
        if (gameManager.zhedBoard == null)
            return;

       // AddVectorObs(center.position.z - gameObject.transform.position.z);
       // AddVectorObs(gameManager.zhedBoard.GetValueTiles().Count);

        //List<List<int>> board = gameManager.zhedBoard.GetBoard();
    }

    public override void AgentAction(float[] vectorAction)
    {       
        if (gameManager.zhedBoard == null)
            return;

        /*
        if (gameManager.Loser()) {
            Done();
            return;
        }
        */
   
       // Debug.Log(vectorAction[0] + ":" + vectorAction[1]);

        List<Coords> tiles = this.gameManager.zhedBoard.GetValueTilesCoords();
        if (tiles.Count == 0)
            return;

        int tileToPlay = Mathf.RoundToInt(vectorAction[1] + 1) / 2 * (tiles.Count - 1);
        if (vectorAction[0] > 0.5)
            gameManager.Play(tiles[tileToPlay], Coords.MoveLeft);
        else if (vectorAction[0] > 0)
            gameManager.Play(tiles[tileToPlay], Coords.MoveDown);
        else if (vectorAction[0] > -0.5)
            gameManager.Play(tiles[tileToPlay], Coords.MoveRight);            
        else gameManager.Play(tiles[tileToPlay], Coords.MoveUp);

        if (gameManager.Loser()) {
            SetReward(-20f);
            Done();
        }
        else if (gameManager.Winner()) {
            SetReward(999999f);
        }
        else {
            SetReward(gameManager.zhedBoard.getBoardTotalMaxValue());
        }
    }

    public override void AgentReset()
    {
        gameManager.ResetLevel();
       // gameObject.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
       // gameObject.transform.position = center.position;
       // m_BallRb.velocity = new Vector3(Random.Range(-1.5f, 1.5f), 0f, Random.Range(-1.5f, 1.5f));
       // ball.transform.position = new Vector3(0, center.position.y + 7.5f, 0) + center.position;
        //Reset the parameters when the Agent is reset.
        SetResetParameters();
    }

    public override float[] Heuristic()
    {
        var action = new float[2];

        action[0] = -Input.GetAxis("Horizontal");
        action[1] = Input.GetAxis("Vertical");
        return action;
    }

    public void SetResetParameters()
    {

    }

}