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

    public Block() {
        SetVals(null, 1f, 0f, Vector3.zero, Vector3.zero);
    }
    public Block(GameObject gameObject) {
        SetVals(gameObject, 1f, 0f, Vector3.zero, gameObject.transform.position);
    }

    public Block(GameObject gameObject, int ID) {
        SetVals(gameObject, 1f, ID, 0f,Vector3.zero, gameObject.transform.position);
    }

    public Block(GameObject gameObject, float sizePercent, float increment, Vector3 Orientation, Vector3 finalPos) {
        Contract.Requires(sizePercent >= 0.0f && sizePercent <= 1.0f);

        SetVals(gameObject, sizePercent, increment, Orientation, finalPos);
    }

    //Using ID
    public Block(GameObject gameObject, float sizePercent, int ID, float increment, Vector3 Orientation, Vector3 finalPos) {
        Contract.Requires(sizePercent >= 0.0f && sizePercent <= 1.0f);

        SetVals(gameObject, sizePercent, ID, increment, Orientation, finalPos);
    }

    //Using ID
    public void SetVals(GameObject gameObject, float sizePercent, int ID, float increment, Vector3 Orientation, Vector3 finalPos) {
        this.gameObject = gameObject;
        this.sizePercent = sizePercent;
        this.increment = increment;

        this.ID = ID;
        this.Orientation = Orientation;
        this.finalPos = finalPos;
    }
    public void SetVals(GameObject gameObject, float sizePercent, float increment, Vector3 Orientation, Vector3 finalPos) {
        this.gameObject = gameObject;
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
