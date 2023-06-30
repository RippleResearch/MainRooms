using UnityEngine;

public class GrassBlock : Block
{
    public const float grassInc = 1.0f / 4.0f;

    public GrassBlock(GameObject gameObject, int ID) : base(gameObject, ID) {
    }
    /*public GrassBlock() {
        SetVals(null, 1f, grassInc, Vector3.zero, Vector3.zero);
    }
    public GrassBlock(GameObject gameObject) {
        SetVals(gameObject, 1f, grassInc, Orientation, finalPos);
    }

    public GrassBlock(GameObject gameObject, Vector3 Orientation, Vector3 finalPos) {
        SetVals(gameObject, grassInc, grassInc, Orientation, finalPos);
    }

    public GrassBlock(GameObject gameObject, float inc, Vector3 Orientation, Vector3 finalPos) {
        SetVals(gameObject, inc, inc, Orientation, finalPos);
    }

    public override string ToString() {
        return "G " + base.ToString(); 
    }*/
}

