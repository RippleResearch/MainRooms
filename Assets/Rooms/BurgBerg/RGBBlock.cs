using System.Diagnostics.Contracts;
using System.Text;
using UnityEngine;

public class RGBBlock
{ 
    public GameObject gameObject;
    public float sizePercent;
    public float increment;
    public Vector3 finalPos;
    public Vector3 Orientation;

    public RGBBlock() {
        SetVals(null, 1f, 0f, Vector3.zero, Vector3.zero);
    }
    public RGBBlock(GameObject gameObject) {
        SetVals(gameObject, 1f, 0f, Vector3.zero, gameObject.transform.position);
    }

    public RGBBlock(GameObject gameObject, float sizePercent, float increment, Vector3 Orientation, Vector3 finalPos) {
        Contract.Requires(sizePercent >= 0.0f && sizePercent <= 1.0f);

        SetVals(gameObject, sizePercent, increment, Orientation, finalPos);
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
