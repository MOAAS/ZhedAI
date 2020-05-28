using UnityEngine;

using System.Collections;
using System.Collections.Generic;

using ZhedSolver;

public class AgentStats {
    private int hits = 0;
    private int misses = 0;
    private int numWins = 0;

    private List<bool> last5  = new List<bool>();

    public void OnWin() {
        numWins++;
        last5.Add(true);
        if (last5.Count > 5)
            last5.RemoveAt(0);
    }

    public void OnLoss() {
        last5.Add(false);
        if (last5.Count > 5)
            last5.RemoveAt(0);
    }

    public void OnHit() {
        this.hits++;
    }

    public void OnMiss() {
        this.misses++;
    }

    public float WinRatioLast5() {
        if (last5.Count == 0)
            return 0;
        int numWinsLast5 = 0;
        foreach (bool win in last5) {
            if (win)
                numWinsLast5++;
        }
        return numWinsLast5 / last5.Count;
    }

    public int NumWins() {
        return this.numWins;
    }

    public float HitRatio() {
        if (hits + misses == 0)
            return 0;
        return hits / (float)(hits + misses);
    }

    public float MissRatio() {
        if (hits + misses == 0)
            return 0;
        return misses / (float)(hits + misses);
    }

    public void Reset() {
        this.hits = 0;
        this.misses = 0;
        this.last5 = new List<bool>();
    }

    public void Print() {
        Debug.Log("Ratio: " + HitRatio() + ", hits = " + hits + ", misses = " + misses);
        Debug.Log("Win count: " + numWins);
        Debug.Log("Win ratio: " + WinRatioLast5());
    }


}
