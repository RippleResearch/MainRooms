using System;
using System.Diagnostics.Contracts;
using System.Text;
using UnityEngine;

public class Block
{
    public GameObject gameObject;
    public float sizePercent;
    public float increment;
    public Vector3 finalPos;
    public Vector3 Orientation;
    public int ID;

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="ID"></param>
    public Block(GameObject gameObject, int ID) {
        SetVals(gameObject, ID, Vector3.zero, gameObject.transform.position);
    }

    /// <summary>
    /// Full constructor
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="ID"></param>
    /// <param name="Orientation"></param>
    /// <param name="finalPos"></param>
    /// <param name="sizePercent"></param>
    /// <param name="increment"></param>
    public Block(GameObject gameObject, int ID, Vector3 Orientation, Vector3 finalPos, float sizePercent = 1f, float increment = 0f) {
        SetVals(gameObject, ID, Orientation, finalPos, sizePercent, increment);
    }

    /// <summary>
    /// Full set vals with optional parameters in size and increment with defaults 1 and 0 respectivley
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="ID"></param>
    /// <param name="Orientation"></param>
    /// <param name="finalPos"></param>
    /// <param name="sizePercent"></param>
    /// <param name="increment"></param>
    public void SetVals(GameObject gameObject, int ID, Vector3 Orientation, Vector3 finalPos, float sizePercent = 1f, float increment = 0f) {
        if(sizePercent < 0.0f && sizePercent > 1.0f) {
            throw new FormatException();
        }

        this.gameObject = gameObject;
        this.ID = ID;
        
        this.sizePercent = sizePercent;
        this.increment = increment;
        this.Orientation = Orientation;
        this.finalPos = finalPos;
    }

    public override string ToString() {
        StringBuilder sb = new();
        if(gameObject != null) {
            sb.AppendJoin(", ", gameObject.name, sizePercent, increment, Orientation, finalPos);
        }
        else {
            sb.AppendJoin(", ", "Unknown", sizePercent, increment, Orientation, finalPos);
        }
        
        return sb.ToString();
    }
}
