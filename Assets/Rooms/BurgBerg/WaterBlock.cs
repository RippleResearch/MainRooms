using UnityEngine;

public class WaterBlock : RGBBlock
{
    public const float waterInc = 1.0f / 4.0f;

    public WaterBlock() {
        SetVals(null, 1f, waterInc, Vector3.zero, Vector3.zero);
    }
    public WaterBlock(GameObject gameObject) {
        SetVals(gameObject, 1f, waterInc, Orientation, finalPos);
    }

    public WaterBlock(GameObject gameObject, Vector3 Orientation, Vector3 finalPos) {
        SetVals(gameObject, waterInc, waterInc, Orientation, finalPos);
    }

    public WaterBlock(GameObject gameObject, float inc, Vector3 Orientation, Vector3 finalPos) {
        SetVals(gameObject, inc, inc, Orientation, finalPos);
    }

    public override string ToString() {
        return "W " + base.ToString(); 
    }
}

