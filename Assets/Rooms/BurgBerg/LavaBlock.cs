using UnityEngine;

public class LavaBlock : RGBBlock
{
    public const float lavaInc = 1.0f / 10.0f;

    public LavaBlock() {
        SetVals(null, 1f, lavaInc, Vector3.zero, Vector3.zero);
    }
    public LavaBlock(GameObject gameObject) {
        SetVals(gameObject, 1f, lavaInc, Vector3.zero, gameObject.transform.position);
    }

    public LavaBlock(GameObject gameObject, Vector3 Orientation, Vector3 finalPos) {
        SetVals(gameObject, lavaInc, lavaInc, Orientation, finalPos);
    }

    public LavaBlock(GameObject gameObject, float inc, Vector3 Orientation, Vector3 finalPos) {
        SetVals(gameObject, inc, inc, Orientation, finalPos);
    }

    public override string ToString() {
        return "L " + base.ToString();
    }

}
