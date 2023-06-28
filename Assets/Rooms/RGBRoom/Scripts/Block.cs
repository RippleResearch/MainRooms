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

    /*public Block() {
        SetVals(null, 1f, 0f, Vector3.zero, Vector3.zero);
    }
    public Block(GameObject gameObject) {
        SetVals(gameObject, 1f, 0f, Vector3.zero, gameObject.transform.position);
    }
*/
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
        this.gameObject = gameObject;
        this.ID = ID;
        
        this.sizePercent = sizePercent;
        this.increment = increment;
        this.Orientation = Orientation;
        this.finalPos = finalPos;
    }

   /* public Block(GameObject gameObject, float sizePercent, float increment, Vector3 Orientation, Vector3 finalPos) {
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
    }*/

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
