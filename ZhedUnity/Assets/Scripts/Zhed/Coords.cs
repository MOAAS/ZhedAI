using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Coords {
    public int x { get; set; }
    public int y { get; set; }

    public Coords(int x, int y) {
        this.x = x;
        this.y = y;
    }

    public static Coords MoveUp(Coords coords) {
        return new Coords(coords.x, coords.y - 1);
    }

    public static Coords MoveDown(Coords coords) {
        return new Coords(coords.x, coords.y + 1);
    }

    public static Coords MoveLeft(Coords coords) {
        return new Coords(coords.x - 1, coords.y);
    }
    
    public static Coords MoveRight(Coords coords) {
        return new Coords(coords.x + 1, coords.y);
    }

    public bool AlignedWith(Coords coords) {
        return this.x == coords.x || this.y == coords.y;
    }

    public override String ToString() {
        return "(" + this.x + "," + this.y + ")";
    }

    public override bool Equals(object obj) {
        
        if (obj == null || GetType() != obj.GetType())
            return false;
        Coords coords = obj as Coords;            
        return this.x == coords.x && this.y == coords.y;
    }
    
    public override int GetHashCode()
    {
        return this.x.GetHashCode() ^ this.y.GetHashCode();
    }
};
