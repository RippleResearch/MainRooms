using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeController : MonoBehaviour {

    public GameObject Dirt, PathDirt, Lava, Water, Grass;
    public int height, width;
    [Range(1f, 11f)]
    public int sizeMultiplier;
    private int currentHeight, currentWidth;
    [Range(0f, 4f)]
    public float waitTime = 1;
    //public bool hidePath = true; //Default value to fill path with blocks
    //public bool showPathWhenDone = true;

    int[,] board;
    System.Random rand;

    //[SerializeField] GameObject startWater, finalLava;
    [SerializeField] Cell[,] cells;

    [Range(1 / 64.0f, 1f)]
    public float waterIncrement = 1 / 4.0f;
    [Range(1 / 64.0f, 1f)]
    public float lavaIncrement = 1 / 4.0f;
    [Range(1 / 64.0f, 1f)]
    public float grassIncrement = 1 / 4.0f;

    private int maxTime = 30;
    public float timeSinceReset;

    [SerializeField] List<GameObject> AllGameObjects;

    //private BFS bfs;
    private const int WALL = 0;
    private const int PATH = 1;

    public class DestAndPos {
        public Vector3 dest;
        public Vector3 dir;

        public DestAndPos(Vector3 dest, Vector3 dir) {
            this.dest = dest;
            this.dir = dir;
        }
    }

    // Start is called before the first frame update
    void Start() {
        rand = new System.Random();
        AllGameObjects = new List<GameObject>();
        sizeMultiplier = 1;
        width = 16;
        height = 9;
        InitializeMaze();
    }

    private void InitializeMaze() {
        AllGameObjects.Clear();
        sizeMultiplier = rand.Next(1, 8);

        if (sizeMultiplier != 1) {
            height = 9;
            width = 16;
        }

        if(sizeMultiplier>2) {
            waterIncrement = 1;
            lavaIncrement = 1;
            grassIncrement = 1;
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

        Vector3 center = new Vector3(height / 2, height, width / 2);
        Camera.main.transform.position = center;

        int spot = UnityEngine.Random.Range(0, height);
        if (spot % 2 == 1) {
            spot--;
        }
        System.Numerics.Vector2 start = new System.Numerics.Vector2(0, spot);
        ////////////PRIMS MAZE//////////////////
        PrimsMaze prims = new PrimsMaze(width, height, start, false);

        //bfs = new BFS();
        board = new int[height, width];
        board = prims.maze;

        int removals = (int)(width * height * .05);
        while (removals > 0) {
            int x = UnityEngine.Random.Range(0, height);
            int z = UnityEngine.Random.Range(0, width);
            if (board[x, z] == WALL) {
                List<DestAndPos> spots = GetValidNeighbors(x, z);
                foreach (DestAndPos dAndp in spots) {
                    if (board[(int)dAndp.dest.x, (int)dAndp.dest.z] != WALL) {
                        board[x, z] = PATH;
                        removals--;
                        break;
                    }
                }
            }
        }

        //System.Numerics.Vector2 end = bfs.ComputeAndGetEnd(start, board);
        ////////////PRIMS MAZE//////////////////    

        //Array of Cells
        cells = new Cell[height, width];

        //Generate Blocks
        for (int x = -1; x <= height; ++x) {
            for (int z = -1; z <= width; ++z) {
                //Place Block
                Block block = PlaceNewBlock(x, z);
                if (x >= 0 && z >= 0 && z < width && x < height) {
                    cells[x, z] = new Cell(block);
                    if (block != null)
                        if (block.gameObject.CompareTag("Dirt") || block.gameObject.CompareTag("Path"))
                            cells[x, z].isActive = false;
                }
                if (block != null)
                    AllGameObjects.Add(block.gameObject);
            }
        }
        GameObject[] starts = { Water, Lava, Grass };
        foreach (GameObject genericObject in starts) {
            bool placed = false;
            do {
                int x = UnityEngine.Random.Range(0, height);
                int z = UnityEngine.Random.Range(0, width);
                if (board[x, z] == PATH) {
                    GameObject startObj = Instantiate(genericObject, new Vector3(x, 0, z), Quaternion.identity);
                    AllGameObjects.Add(startObj);
                    startObj.name = genericObject.tag + " " + x + "," + z;
                    startObj.transform.parent = GameObject.FindGameObjectWithTag(genericObject.tag).transform;
                    if (cells[x, z].Block1 != null && cells[x, z].Block1.gameObject != null) {
                        Destroy(cells[x, z].Block1.gameObject);
                    }
                    cells[x, z].Block1 = new Block(startObj);
                    cells[x, z].isActive = true;
                    placed = true;
                }
            } while (!placed);
        }
        timeSinceReset = -1;
    }


    private Block PlaceNewBlock(int x, int z) {
        GameObject go;
        if (x < 0 || z < 0 || z >= width || x >= height) {
            go = Instantiate(Dirt, new Vector3(x, 0, z), Quaternion.identity);
            go.name = "Dirt " + x + "," + z;
            go.transform.parent = GameObject.FindGameObjectWithTag("Dirt").transform;
            return new Block(go);
        } else {
            //0 = WALL, 1 = PATH, 2 = TRACED PATH FROM START TO END
            if (board[x, z] == WALL) {
                go = Instantiate(Dirt, new Vector3(x, 0, z), Quaternion.identity);
                go.name = "Dirt " + x + "," + z;
                go.transform.parent = GameObject.FindGameObjectWithTag("Dirt").transform;

                return new Block(go);
            } else {
                return null;          
            }
        }
    }

    public IEnumerator RequestReset(float waititme) {
        yield return new WaitForSeconds(waititme);
        foreach (GameObject go in AllGameObjects) {
            if (go != null) {
                Destroy(go);
            }
        }
        InitializeMaze();
        StopAllCoroutines();
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
        if (IsInRange(z - 1, currentWidth)) {
            spots.Add(new DestAndPos(new Vector3(x, 0, z - 1), new Vector3(0, 0, -1)));
        }
        //right
        if (IsInRange(x + 1, currentHeight)) {
            spots.Add(new DestAndPos(new Vector3(x + 1, 0, z), new Vector3(1, 0, 0)));
        }
        //down
        if (IsInRange(z + 1, currentWidth)) {
            spots.Add(new DestAndPos(new Vector3(x, 0, z + 1), new Vector3(0, 0, 1)));
        }
        //left
        if (IsInRange(x - 1, currentHeight)) {
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

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();

        }
        float diff = Time.time - timeSinceReset;
        if (timeSinceReset > 0 && diff > maxTime) {
            StartCoroutine(RequestReset(waitTime));
        }
        int w = GameObject.FindGameObjectWithTag(Water.tag).transform.childCount;
        int l = GameObject.FindGameObjectWithTag(Lava.tag).transform.childCount;
        int g = GameObject.FindGameObjectWithTag(Grass.tag).transform.childCount;
        if (w == 0) {
            if (l == 0 || g == 0) {
                StartCoroutine(RequestReset(waitTime));
            }
        } else {
            if (l == 0 && g == 0) {
                StartCoroutine(RequestReset(waitTime));
            }
        }
        //ProcessHit();
        bool filled = true;
        //StringBuilder sb = new StringBuilder();
        for (int x = 0; x < currentHeight; x++) {
            for (int z = 0; z < currentWidth; z++) {
                //If one full block
                Cell cell = cells[x, z];
                if (!cell.isActive)
                    continue;

                //Debug.log(cell);
                if (cell.Block2 == null) {
                    //For now
                    if (cell.Block1 != null) {
                        if (Mathf.Abs(cell.Block1.sizePercent - 1f) < .001f) {
                            if (Propigate(cell.Block1, x, z) == true) {
                                cell.isActive = false;
                            }
                        } else { //One block not full
                            Grow(cell);
                        }
                    } else {
                        //sb.Append(x + "," + z + " " + cell);
                        filled = false;
                    }
                } else {
                    if (cell.Block1 != null) {
                        ShrinkAndGrowBlock(cell);
                    }
                }
            }
        }
        if (filled) {
            //Debug.Log("Filled!");
            if (timeSinceReset < 0) {
                timeSinceReset = Time.time;
            }
        } else {
            //Debug.Log(sb);
        }
    }

    public void Grow(Cell cell) {
        if (cell.Block1 != null) {
            GrowBlock(cell.Block1);
        }
        if (cell.Block2 != null) {
            GrowBlock(cell.Block2);
        }
    }

    public void GrowBlock(Block block) {
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

    public void ShrinkSecondBlock(Cell cell) {
        Block block = cell.Block2;
        //Should have other blocks inc
        block.gameObject.transform.position -= block.increment / 2 * block.Orientation;
        block.gameObject.transform.localScale -= block.increment * vecAbs(block.Orientation);
        block.sizePercent = Vector3.Dot(block.gameObject.transform.localScale, vecAbs(block.Orientation));

        if (block.sizePercent < block.increment) {
            AllGameObjects.Remove(block.gameObject);
            Destroy(block.gameObject);
            //DestroyImmediate(block.gameObject,true);
            cell.Block2 = null;
        }
    }
    private void ShrinkAndGrowBlock(Cell cell) {
        //We already know we are hitting a block && We also know that cell 2 is not null
        Debug.Assert(cell.Block1 != null && cell.Block2 != null);

        if (cell.Block1.sizePercent + cell.Block2.sizePercent >= 1f) {
            GrowBlock(cell.Block1);
            ShrinkSecondBlock(cell);
        } else {
            GrowBlock(cell.Block1);
            GrowBlock(cell.Block2);

            if (cell.Block1.sizePercent + cell.Block2.sizePercent >= 1f) {
                cell.Block2.increment = cell.Block1.increment;
            }
        }
    }

    private bool Propigate(Block block, int x, int z) {
        if (block.gameObject.CompareTag("Dirt") || block.gameObject.CompareTag("Path")) {
            return false;
        }
        List<DestAndPos> spots = GetValidNeighbors(x, z);
        if (block.gameObject.CompareTag("Water")) {
            return PropigateWater(spots);
        } else if (block.gameObject.CompareTag("Lava")) {
            return PropigateLava(spots);
        } else if (block.gameObject.CompareTag("Grass")) {
            return PropigateGrass(spots);
        }
        return false;
    }

    //Return true iff surrounded
    private bool PropigateGrass(List<DestAndPos> spots) {
        int surrounded = 0;
        foreach (DestAndPos p in spots) {
            Cell cell = cells[(int)p.dest.x, (int)p.dest.z];
            if (cell.Block1 == null) {
                addGrass(p);
                surrounded++;
            } else if (cell.Block1.gameObject.CompareTag("Grass")) {
                surrounded++;
            } else if (cell.Block1.gameObject.CompareTag("Dirt")) {
                surrounded++;
            } else if (cell.Block1.gameObject.CompareTag("Path")) {
                if (rand.Next(5) == 1) {
                    addGrass(p);
                    cell.SwapBlocksAndVars();
                    ShrinkSecondBlock(cell);
                    surrounded++;
                }
            } else if (cell.Block1.gameObject.CompareTag("Water")) {
                addGrass(p);
                cell.SwapBlocksAndVars();
                surrounded++;
            } else if (cell.Block1.gameObject.CompareTag("Lava") && cell.Block2 == null && cell.Block1.sizePercent < 1f) {
                addGrass(p);
                surrounded++;
            }
        }
        return (surrounded == spots.Count);
    }

    //Return true iff surrounded
    private bool PropigateLava(List<DestAndPos> spots) {
        int surrounded = 0;
        foreach (DestAndPos p in spots) {
            Cell cell = cells[(int)p.dest.x, (int)p.dest.z];
            if (cell.Block1 == null) {
                addLava(p);
                surrounded++;
            } else if (cell.Block1.gameObject.CompareTag("Lava")) {
                surrounded++;
            } else if (cell.Block1.gameObject.CompareTag("Dirt")) {
                surrounded++;
            } else if (cell.Block1.gameObject.CompareTag("Path")) {
                if (rand.Next(5) == 1) {
                    addLava(p);
                    cell.SwapBlocksAndVars();
                    ShrinkSecondBlock(cell);
                    surrounded++;
                }
            } else if (cell.Block1.gameObject.CompareTag("Grass")) {
                addLava(p);
                cell.SwapBlocksAndVars();
                surrounded++;
            } else if (cell.Block1.gameObject.CompareTag("Water") && cell.Block2 == null && cell.Block1.sizePercent < 1f) {
                addLava(p);
                surrounded++;
            }
        }
        return (surrounded == spots.Count);
    }

    //Return true iff surrounded
    private bool PropigateWater(List<DestAndPos> spots) {
        int surrounded = 0;
        foreach (DestAndPos p in spots) {
            Cell cell = cells[(int)p.dest.x, (int)p.dest.z];
            if (cell.Block1 == null) {
                addWater(p);
                surrounded++;
            } else if (cell.Block1.gameObject.CompareTag("Water")) {
                surrounded++;
            } else if (cell.Block1.gameObject.CompareTag("Dirt")) {
                surrounded++;
            } else if (cell.Block1.gameObject.CompareTag("Path")) {
                if (rand.Next(5) == 1) {
                    addWater(p);
                    cell.SwapBlocksAndVars();
                    ShrinkSecondBlock(cell);
                    surrounded++;
                }
            } else if (cell.Block1.gameObject.CompareTag("Lava")) {
                addWater(p);
                cell.SwapBlocksAndVars();
                surrounded++;
            } else if (cell.Block1.gameObject.CompareTag("Grass") && cell.Block2 == null && cell.Block1.sizePercent < 1f) {
                addWater(p);
                surrounded++;
            }
        }
        return (surrounded == spots.Count);
    }

    private void addLava(DestAndPos p) {
        AddBlockToCell(Lava, typeof(LavaBlock), lavaIncrement, "Lava", p);
    }
    private void addWater(DestAndPos p) {
        AddBlockToCell(Water, typeof(WaterBlock), waterIncrement, "Water", p);
    }
    private void addGrass(DestAndPos p) {
        AddBlockToCell(Grass, typeof(GrassBlock), grassIncrement, "Grass", p);
    }
    private void AddBlockToCell(GameObject gameObject, Type block, float inc, string name, DestAndPos p) {
        //Make sure at least one cell is empty before we place a new block inside
        if (cells[(int)p.dest.x, (int)p.dest.z].Block1 != null && cells[(int)p.dest.x, (int)p.dest.z].Block2 != null) {
            // Wait until whatever is happening in this cell to finish.
            return;
        }
        GameObject newObject = Instantiate(gameObject, Vector3.one, Quaternion.identity);
        object[] paramList = { newObject, inc, p.dir, p.dest };
        Block b = (Block)Activator.CreateInstance(block, paramList);


        //Make new obj
        newObject.transform.position = p.dest + p.dir * (b.increment / 2 - 0.5f);
        newObject.transform.localScale = Vector3.one - (1 - b.increment) * vecAbs(p.dir);
        newObject.name = name + " " + p.dest.x + "," + p.dest.z;
        newObject.transform.parent = GameObject.FindGameObjectWithTag(name).transform;


        //If cell is not active set it to active
        cells[(int)p.dest.x, (int)p.dest.z].isActive = true;

        //Check which block is empty
        if (cells[(int)p.dest.x, (int)p.dest.z].Block1 == null)
            cells[(int)p.dest.x, (int)p.dest.z].Block1 = b;
        else
            cells[(int)p.dest.x, (int)p.dest.z].Block2 = b;

        //Add to array for reset
        AllGameObjects.Add(newObject);
    }

    private Vector3 vecAbs(Vector3 v) {
        return new Vector3(Math.Abs(v.x), Math.Abs(v.y), Math.Abs(v.z));
    }

    private void RemoveBlock(Vector3 p) {
        if (cells[(int)p.x, (int)p.z].Block1 != null) {
            Destroy(cells[(int)p.x, (int)p.z].Block1.gameObject);
            cells[(int)p.x, (int)p.z].Block1 = null;
        }
        if (cells[(int)p.x, (int)p.z].Block2 != null) {
            Destroy(cells[(int)p.x, (int)p.z].Block2.gameObject);
            cells[(int)p.x, (int)p.z].Block2 = null;
        }
    }
}
