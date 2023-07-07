using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolMazeController : MonoBehaviour
{

    public GameObject Dirt, PathDirt, Lava, Water;
    public int height, width;
    [Range(1f, 11f)]
    public int sizeMultiplier;
    private int currentHeight, currentWidth;
    [Range(0f, 4f)]
    public float waitTime;
    public bool hidePath = true; //Default value to fill path with blocks
    public bool showPathWhenDone = true;

    int[,] board;
    System.Random rand;

    [SerializeField] GameObject finalLava;
    [SerializeField] CoolCell[,] cells;
    
    [Range(1/64.0f, 1f)]
    public float waterIncrement = 1 / 8.0f; // .125f;  //.0625f;
    [Range(1 / 64.0f, 1f)]
    public float lavaIncrement = 1 / 16.0f;  // .03125f;

    [SerializeField] List<GameObject> AllGameObjects;

    private BFS bfs;

    public class DestAndPos {
        public Vector3 dest;
        public Vector3 dir;

        public DestAndPos(Vector3 dest, Vector3 dir) {
            this.dest = dest;
            this.dir = dir;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rand = new System.Random();
        AllGameObjects = new List<GameObject>();
        sizeMultiplier = 1;
        width = 16;
        height = 9;
        InitializeMaze();
    }

    private void InitializeMaze() {
        sizeMultiplier = rand.Next(1, 4);

        if (sizeMultiplier != 1) {
            height = 9;
            width = 16;
        }

        height *= sizeMultiplier;
        width *= sizeMultiplier;
        if (height % 2 == 0) {
            height--;
        }
        if (width % 2 == 0) {
            width--;
        }

        //Used so if height gets changed while running it wont break
        currentHeight = height;
        currentWidth = width;

        Vector3 center = new Vector3(height / 2, height*1.25f, width*.45f);
        Camera.main.transform.position = center;

        int spot = UnityEngine.Random.Range(0, height);
        if (spot % 2 == 1) {
            spot--;
        }
        System.Numerics.Vector2 start = new System.Numerics.Vector2(0, spot);
        ////////////PRIMS MAZE//////////////////
        PrimsMaze prims = new PrimsMaze(width, height, start, false);
        bfs = new BFS();
        board = new int[height, width];
        board = prims.maze;
        System.Numerics.Vector2 end = bfs.ComputeAndGetEnd(start, board);
        ////////////PRIMS MAZE//////////////////    
        

        //Array of Cells
        cells = new CoolCell[height, width];
        
        //Generate Blocks
        for (int x = -1; x <= height; ++x) {
            for (int z = -1; z <= width; ++z) {
                //Place Block
                CoolBlock block = PlaceNewBlock(start, end, x, z);
                if (x >= 0 && z >= 0 && z < width && x < height) {
                    cells[x, z] = new CoolCell(block);
                    if(block != null)
                        if (block.gameObject.CompareTag("Dirt") || block.gameObject.CompareTag("Path"))
                            cells[x, z].isActive = false;
                }
                if(block != null)
                    AllGameObjects.Add(block.gameObject);
                
            }
        }
    }


    private CoolBlock PlaceNewBlock(System.Numerics.Vector2 start, System.Numerics.Vector2 end, int x, int z) {
        GameObject go;
        if (x < 0 || z < 0 || z >= width || x >= height) {
            go = Instantiate(Dirt, new Vector3(x, 0, z), Quaternion.identity);
            go.name = "Dirt " + x + "," + z;
            go.transform.parent = GameObject.FindGameObjectWithTag("Dirt").transform;
            return new CoolBlock(go);
        }
        else if (z == start.X && x == start.Y) {
            go = Instantiate(Water, new Vector3(x, 0, z), Quaternion.identity);
            go.name = "Water " + x + "," + z;
            go.transform.parent = GameObject.FindGameObjectWithTag("Water").transform;
            //startWater = go;
            return new CoolWaterBlock(go, Vector3.zero, go.transform.position);
        }
        else if (z == end.X && x == end.Y) {
            go = Instantiate(Lava, new Vector3(x, 0, z), Quaternion.identity);
            go.name = "Lava " + x + "," + z;
            go.transform.parent = GameObject.FindGameObjectWithTag("Lava").transform;
            finalLava = go;
            return new CoolLavaBlock(go);
        }
        else {
            //0 = WALL, 1 = PATH, 2 = TRACED PATH FROM START TO END
            if (board[x, z] != 0) {
                if (hidePath) {
                    go = Instantiate(PathDirt, new Vector3(x, 0, z), Quaternion.identity);
                    go.name = "Path " + x + "," + z;
                    go.transform.parent = GameObject.FindGameObjectWithTag("Path").transform;
                    return new CoolBlock(go);
                }
                else {
                    return null;
                }           
            }
            else {
                go = Instantiate(Dirt, new Vector3(x, 0, z), Quaternion.identity);
                go.name = "Dirt " + x + "," + z;
                go.transform.parent = GameObject.FindGameObjectWithTag("Dirt").transform;

                return new CoolBlock(go);
            }

        }
    }

    public IEnumerator RequestReset(float waititme) {
        yield return new WaitForSeconds(waititme);
        foreach(GameObject go in AllGameObjects) {
            if (go != null) {
                Destroy(go);
            }
        }
        AllGameObjects.Clear();
        InitializeMaze();
        StopAllCoroutines();
    }

    /// <summary>
    /// Processes mouse hit upon left click of the mouse. 
    /// If the mouses cursors is hitting a block with a "Path" tag
    /// then the block is destroyed. 
    /// </summary>
    private void ProcessHit() {
        try {
            //Check if left mouse but clicked
            if (Input.GetMouseButton(0)) {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit)) {
                    Vector3 pos = hit.transform.position;
                    if ((int)pos.x < 0 || (int)pos.z < 0 || (int)pos.x >= height || (int)pos.z >= width) {
                        return;
                    }
                    GameObject go = hit.transform.gameObject;
                    //Debug.log("hit something: " + go.name + " Tag: " + go.tag);
                    if (go.CompareTag("Path")) {
                        int x = (int) Mathf.Round(hit.transform.position.x);
                        int z = (int) Mathf.Round(hit.transform.position.z);

                        if (cells[x, z].Block1.gameObject.Equals(go)) {
                            cells[x, z].Block1 = null;
                            Destroy(go);
                            //Debug.log("Destroyed");
                            if(cells[x,z].Block2 != null) {
                                //Debug.log("moved");
                                cells[x, z].Block1 = cells[x, z].Block2;
                                cells[x, z].Block2 = null;
                            }
                        }
                        //RemoveBlock(new Vector3(x, 0, z));
                    }
                }
            }
        }
        catch (Exception e) {
            ////Debug.logException(e);
        }
    }

    /// <summary>
    /// Gets passed a set of cordinates corresponding to a cell
    /// on the graph. Then checks its direct neighbors if they are 
    /// within the bounds of the array. Each neighbor is then reacted to. 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    public List<DestAndPos> GetValidNeighbors(int x, int z) {

        List<DestAndPos> spots = new List<DestAndPos>();
        //up
        if(IsInRange(z-1, currentWidth)) {
           
            spots.Add(new DestAndPos(new Vector3(x, 0, z-1), new Vector3(0,0,-1)));
        }
        //right
        if (IsInRange(x+1, currentHeight)) {
            spots.Add(new DestAndPos(new Vector3(x + 1, 0, z), new Vector3(1, 0, 0)));
        }
        //down
        if(IsInRange(z+1, currentWidth)) {
            spots.Add(new DestAndPos(new Vector3(x, 0, z + 1), new Vector3(0, 0, 1)));
        }
        //left
        if(IsInRange(x-1, currentHeight)) {
            spots.Add(new DestAndPos(new Vector3(x - 1, 0, z), new Vector3(-1, 0, 0)));
        }

        return spots;
    }

    /// <summary>
    /// Helper function to determine if something is within the bounds of the array.
    /// It checks if val is < bound and if val >= 0 and returns true or false accordingly.
    /// </summary>
    /// <param name="val">position to test </param>
    /// <param name="bound"></param>
    /// <returns></returns>
    public bool IsInRange(int val, int bound) {
        return (val < bound && val >= 0);
    }

    // Update is called once per frame
    void Update()
    {
       ProcessHit();
       for(int x = 0; x < currentHeight; x++) {
            for(int z = 0; z < currentWidth; z++) {
                //If one full block
                CoolCell cell = cells[x, z];
                if (!cell.isActive) 
                    continue;

                //Debug.log(cell);
                if (cell.Block2 == null) {
                    //For now
                    if (cell.Block1 != null) {
                        if (Mathf.Abs(cell.Block1.sizePercent - 1f) < .001f ) {
                            if(Propigate(cell.Block1, x, z) == true) {
                                cell.isActive = false;
                            }
                        }
                        else { //One block not full
                            Grow(cell);
                        }
                    }
                }
                else if(cell.Block1 != null){
                    ShrinkAndGrowBlock(cell);
                }
            }
        }
        if (finalLava == null) {
            //if (showPathWhenDone) {
            //    for (int i = 0; i < bfs.startToFinish.Count; i++) {
            //        System.Numerics.Vector2 path = bfs.startToFinish.Pop();

            //        //Switch Variables around for maze again
            //        Debug.Assert(cells[(int)path.Y, (int)path.X].Block1.gameObject != null);
            //        GameObject go = cells[(int)path.Y, (int)path.X].Block1.gameObject;
            //        go.GetComponent<Renderer>().material.color = Color.white;
            //    }
            //}
            StartCoroutine(RequestReset(waitTime));
        }
    }

    public void Grow(CoolCell cell) {
       if(cell.Block1 != null) {
            GrowBlock(cell.Block1);
        } 
       if(cell.Block2 != null) {
            GrowBlock(cell.Block2);
        }
    }

    public void GrowBlock(CoolBlock block) {
       block.gameObject.transform.position += block.increment / 2 * block.Orientation;
       block.gameObject.transform.localScale += block.increment * vecAbs(block.Orientation);
       block.sizePercent = Vector3.Dot(block.gameObject.transform.localScale, vecAbs(block.Orientation));

        if (Vector3.Distance(block.gameObject.transform.position, block.finalPos) < block.increment) {
            ////Debug.log("Stopped Growing: " + cell.Block1);
            block.sizePercent = 1f;
            //Set size and scale to full
            block.gameObject.transform.position = block.finalPos;
            block.gameObject.transform.localScale = Vector3.one;
        }
    }

    public void ShrinkSecondBlock(CoolCell cell) {
        CoolBlock block = cell.Block2;
        //Should have other blocks inc
        block.gameObject.transform.position -= block.increment / 2 * block.Orientation;
        block.gameObject.transform.localScale -= block.increment * vecAbs(block.Orientation);
        block.sizePercent = Vector3.Dot(block.gameObject.transform.localScale, vecAbs(block.Orientation));

        if (block.sizePercent < block.increment) {
            Destroy(cell.Block2.gameObject);
            cell.Block2 = null;
        }
    }
    private void ShrinkAndGrowBlock(CoolCell cell) {
        //We already know we are hitting a block && We also know that cell 2 is not null
        Debug.Assert(cell.Block1 != null && cell.Block2 != null);

        if(cell.Block1.sizePercent + cell.Block2.sizePercent >= 1f){   
            GrowBlock(cell.Block1);
            ShrinkSecondBlock(cell);
        }
        else {
            GrowBlock(cell.Block1);
            GrowBlock(cell.Block2);

            if (cell.Block1.sizePercent + cell.Block2.sizePercent >= 1f) {
                cell.Block2.increment = cell.Block1.increment;
            }
        }
    }

    private bool Propigate(CoolBlock block, int x, int z) {
        List<DestAndPos> spots; 
        if (block.gameObject.CompareTag("Water")) {
            spots = GetValidNeighbors(x, z);
            return PropigateWater(spots);
        }
        else if (block.gameObject.CompareTag("Lava")) {
            spots = GetValidNeighbors(x, z);
            return PropigateLava(spots);
        }
        return false;
    }
    //Return true iff surrounded
    private bool PropigateLava(List<DestAndPos> spots) {
        int surrounded = 0;
        foreach (DestAndPos p in spots) {
            CoolCell cell = cells[(int) p.dest.x,(int) p.dest.z];
            if (cell.Block1 == null) {
                AddBlockToCell(Lava, typeof(CoolLavaBlock), lavaIncrement, "Lava", p);
                surrounded++;
            }
            else if (cell.Block2 == null && cell.Block1.gameObject.CompareTag("Water") && cell.Block1.sizePercent < 1f) {
                AddBlockToCell(Lava, typeof(CoolLavaBlock), lavaIncrement, "Lava", p);
                surrounded++;
            }
            else if (cell.Block1.gameObject.CompareTag("Path")) {
                if (rand.Next(5) == 1) {
                    AddBlockToCell(Lava, typeof(CoolLavaBlock), lavaIncrement, "Lava", p);
                    cell.SwapBlocksAndVars( );
                    ShrinkSecondBlock(cell);
                    surrounded++;
                }
                
            }
            else if (cell.Block1.gameObject.CompareTag("Lava")) {
                surrounded++;
            }
            else if (cell.Block1.gameObject.CompareTag("Dirt")) {
                surrounded++;
            }
        }
        return (surrounded == spots.Count);
    }

    //Return true iff surrounded
    private bool PropigateWater(List<DestAndPos> spots) {
        int surrounded = 0;
        foreach (DestAndPos p in spots) {
            CoolCell cell = cells[(int)p.dest.x, (int)p.dest.z];
            if (cell.Block2 == null) {
                if (cell.Block1 == null) {
                    AddBlockToCell(Water, typeof(CoolWaterBlock), waterIncrement, "Water", p);
                    surrounded++;
                }
                else if(cell.Block1.gameObject.CompareTag("Lava")) {
                    AddBlockToCell(Water, typeof(CoolWaterBlock), waterIncrement, "Water", p);
                    cell.SwapBlocksAndVars();
                    ShrinkAndGrowBlock(cell);
                    surrounded++;

                }
                else if (cell.Block1.gameObject.CompareTag("Water")) {
                    surrounded++;
                }
                else if (cell.Block1.gameObject.CompareTag("Dirt")) {
                    surrounded++;
                }
            }
            else {
                //Debug.log("Why are we here");
            }
        }
        //Debug.log(surrounded + " " + spots.Count);
        return (surrounded == spots.Count);
    }

    private void AddBlockToCell(GameObject gameObject, Type block, float inc, string name, DestAndPos p) {
        GameObject newObject = Instantiate(gameObject, Vector3.one, Quaternion.identity);
        object[] paramList = { newObject, inc, p.dir, p.dest };
        CoolBlock b = (CoolBlock)Activator.CreateInstance(block, paramList);


        //Make new obj
        newObject.transform.position = p.dest + p.dir * (b.increment / 2 - 0.5f);
        newObject.transform.localScale = Vector3.one - (1 - b.increment) * vecAbs(p.dir);
        newObject.name = name + " " + p.dest.x + "," + p.dest.z;
        newObject.transform.parent = GameObject.FindGameObjectWithTag(name).transform;

        //Make sure at least one cell is empty before we place a new block inside
        Debug.Assert(cells[(int)p.dest.x, (int)p.dest.z].Block1 == null || cells[(int)p.dest.x, (int)p.dest.z].Block2 == null);

        //If cell is not active set it to active
        cells[(int)p.dest.x, (int)p.dest.z].isActive = true;

        //Check which block is empty
        if (cells[(int)p.dest.x, (int)p.dest.z].Block1 == null)
            cells[(int)p.dest.x, (int)p.dest.z].Block1 = b;
        else
            cells[(int)p.dest.x, (int)p.dest.z].Block2 = b;

        //Add to array for reset
        AllGameObjects.Add( newObject );
    }

    private Vector3 vecAbs(Vector3 v) {
        return new Vector3(Math.Abs(v.x), Math.Abs(v.y), Math.Abs(v.z));
    }

    //private void RemoveBlock(Vector3 p) {
    //    if(cells[(int)p.x, (int)p.z].Block1 != null){
    //        Destroy(cells[(int)p.x, (int)p.z].Block1.gameObject);
    //        cells[(int)p.x, (int)p.z].Block1 = null;
    //    }
    //    if(cells[(int)p.x, (int)p.z].Block2 != null) {
    //        Destroy(cells[(int)p.x, (int)p.z].Block2.gameObject);
    //        cells[(int)p.x, (int)p.z].Block2 = null;
    //    } 
    //}
}
