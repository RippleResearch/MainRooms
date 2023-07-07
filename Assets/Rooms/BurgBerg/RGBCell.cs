using System.Text;
using UnityEngine;

public class RGBCell
{
    public RGBBlock Block1;
    public RGBBlock Block2;
    public bool isActive = true;

    public RGBCell(RGBBlock block) {
        Block1 = block;
    }

    public void SwapBlocksAndVars() {
        RGBBlock temp = Block1;
        Block1 = Block2;
        Block2 = temp;

        Block2.increment = Block1.increment;
        Block2.Orientation = -Block1.Orientation;
    }

    public override string ToString() {
        StringBuilder sb = new();
        if (Block1 != null)sb.Append("Block1: " + Block1.gameObject.name + " ");
        else sb.Append("Block1 is null ");
        
        if(Block2 != null) sb.Append("Block2: " + Block2.gameObject.name + " ");
        else sb.Append("Block2 is null ");
        
        return sb.ToString();
    }

}
