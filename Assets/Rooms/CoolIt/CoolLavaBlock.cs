using UnityEngine;

public class CoolLavaBlock : CoolBlock
{
    public const float lavaInc = 1.0f / 10.0f;

    public CoolLavaBlock() {
        SetVals(null, 1f, lavaInc, Vector3.zero, Vector3.zero);
    }
    public CoolLavaBlock(GameObject gameObject) {
        SetVals(gameObject, 1f, lavaInc, Vector3.zero, gameObject.transform.position);
    }

    public CoolLavaBlock(GameObject gameObject, Vector3 Orientation, Vector3 finalPos) {
        SetVals(gameObject, lavaInc, lavaInc, Orientation, finalPos);
    }

    public CoolLavaBlock(GameObject gameObject, float inc, Vector3 Orientation, Vector3 finalPos) {
        SetVals(gameObject, inc, inc, Orientation, finalPos);
    }

    public override string ToString() {
        return "L " + base.ToString();
    }

}
