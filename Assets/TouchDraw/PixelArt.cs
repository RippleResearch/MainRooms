// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class PixelArt : MonoBehaviour
// {

//     private Grid<GridObject> grid;

//     private void Awake(){
//         grid = new Grid<GridObject>(10,10,1f,Vector3.zero, (Grid<GridObject> g, int x, int y)=>new GridObject(g,x,y));
//     }

//     private class GridObject{
//         private Grid<GridObject> grid;
//         private int x;
//         private int y;
//         private Color color;

//         public GridObject(Grid<GridObject> grid,int x,int y){
//             this.grid = grid;
//             this.x = x;
//             this.y = y;
            
//         }

//         public override string ToString(){
//             return ((int)color.r).ToString();
//         }
//     }
// }
