using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MazeController : MonoBehaviour {


    public GameObject Dirt, PathDirt, Lava, Water, Grass;

    public GameObject FlowBlock;
    public List<Tuple<int, int>> beats;
    public List<Tuple<Color, float>> color_and_inc;
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
    [Range(1 / 64.0f, 1f)]
    public float baseIncrement = 1 / 4.0f;


    [Range(1, 20)]
    public int maxTime = 15;
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

    ///////////////////////////////////////TEMP COLOR PALLETTES//////////////

    List<Color> colors1, beachColors, palettable;
    List<List<Color>> AllColors;

    public void GenerateColors() {
        colors1 = new List<Color> {
            HexToRGB("EDD4B2"),
            HexToRGB("d0a98f"),
            HexToRGB("4d243d"),
            HexToRGB("cac2b5"),
            HexToRGB("ecdcc9")
        };

        beachColors = new List<Color> {
            HexToRGB("ED6A5A"),
            HexToRGB("F4F1BB"),
            HexToRGB("9BC1BC"),
            HexToRGB("E6EBE0"),
            HexToRGB("36C9C6")
        };

        palettable = new List<Color> {
            HexToRGB("C4D6B0"),
            HexToRGB("477998"),
            HexToRGB("291F1E"),
            HexToRGB("F64740"),
            HexToRGB("A3333D")
        };

        AllColors = new List<List<Color>> {
                colors1,
                beachColors,
                palettable
          };
    }

    public Color HexToRGB(string hexValue) {
        // Remove any leading "#" if present
        if (hexValue.StartsWith("#"))
            hexValue = hexValue.Substring(1);

        // Convert the hexadecimal value to integer
        int hex = Convert.ToInt32(hexValue, 16);

        // Extract the RGB components
        int red = (hex >> 16) & 0xFF;
        int green = (hex >> 8) & 0xFF;
        int blue = hex & 0xFF;

        return new Color(red/255f, green/255f, blue / 255f); // Unity needs number to be between 0 and 1
    }

    // Start is called before the first frame update
    void Start() {   
        GenerateColors();
        
        /*
        color_and_inc = new List<Tuple<Color, float>> {
            new Tuple<Color, float>(new Color(77, 36, 61), waterIncrement),
            new Tuple<Color, float>(Color.red, lavaIncrement),
            new Tuple<Color, float>(Color.green, grassIncrement),
            new Tuple<Color, float>(Color.cyan, grassIncrement),
            new Tuple<Color, float>(Color.gray, grassIncrement),
        };*/


        rand = new System.Random();
        AllGameObjects = new List<GameObject>();
        sizeMultiplier = 1;
        width = 16;
        height = 9;
        InitializeMaze();
    }

    private void InitializeMaze() {
        beats = new List<Tuple<int, int>>();
        color_and_inc = new List<Tuple<Color, float>>();
        int numOfColors = UnityEngine.Random.Range(3, 15);
        HashSet<Color> used = new HashSet<Color>();

        for (int i = 0; i < numOfColors; i++) {
            if (i + 1 < colors1.Count) {
                beats.Add(new Tuple<int, int>(i, i + 1));
            }
            else {
                beats.Add(new Tuple<int, int>(i, 0));
            }
            Color color;
            do {
                List<Color> palate = AllColors[UnityEngine.Random.Range(0, AllColors.Count)];
                color = palate[UnityEngine.Random.Range(0, palate.Count)];
            } while (used.Contains(color));
            used.Add(color);

            color_and_inc.Add(new Tuple<Color, float>(color, baseIncrement));
        }



        AllGameObjects.Clear();
        sizeMultiplier = rand.Next(4, 8);

        if (sizeMultiplier != 1) {
            height = 9;
            width = 16;
        }

        if (sizeMultiplier > 2) {
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

        RemovePercentWalls(.05f);
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
        PlaceStartBlocks();
        timeSinceReset = -1;
    }

    private void RemovePercentWalls(float remove) {
        int removals = (int)(width * height * remove);
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
    }

    private void PlaceStartBlocks() {
        for (int i = 0; i < beats.Count; i++) {
            bool placed = false;
            var parentObj = new GameObject();
            parentObj.name = "Parent: " + i;
            parentObj.transform.parent = GameObject.FindGameObjectWithTag("Maze").transform;
            AllGameObjects.Add(parentObj);

            do {
                int x = UnityEngine.Random.Range(0, height);
                int z = UnityEngine.Random.Range(0, width);
                if (board[x, z] == PATH) {
                    GameObject startObj = Instantiate(FlowBlock, new Vector3(x, 0, z), Quaternion.identity);
                    startObj.GetComponent<Renderer>().material.color = color_and_inc[i].Item1;

                    AllGameObjects.Add(startObj);

                    startObj.name = i + " " + x + "," + z;
                    startObj.transform.parent = parentObj.transform;

                    if (cells[x, z].Block1 != null && cells[x, z].Block1.gameObject != null) {
                        Destroy(cells[x, z].Block1.gameObject);
                    }
                    cells[x, z].Block1 = new Block(startObj, i, Vector3.zero, startObj.transform.position, increment: color_and_inc[i].Item2); //Add ID to block
                    cells[x, z].isActive = true;
                    placed = true;
                }
            } while (!placed);
        }
    }
    private Block PlaceNewBlock(int x, int z) {
        GameObject go;
        if (x < 0 || z < 0 || z >= width || x >= height) {
            go = Instantiate(Dirt, new Vector3(x, 0, z), Quaternion.identity);
            go.name = "Dirt " + x + "," + z;
            go.transform.parent = GameObject.FindGameObjectWithTag("Dirt").transform;
            return new Block(go, -1);
        }
        else {
            //0 = WALL, 1 = PATH, 2 = TRACED PATH FROM START TO END
            if (board[x, z] == WALL) {
                go = Instantiate(Dirt, new Vector3(x, 0, z), Quaternion.identity);
                go.name = "Dirt " + x + "," + z;
                go.transform.parent = GameObject.FindGameObjectWithTag("Dirt").transform;

                return new Block(go, -1);
            }
            else {
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

    private bool Propigate(List<DestAndPos> spots, int x, int z) {
        Cell currCell = cells[x, z];
        int surrounded = 0;
        foreach (DestAndPos p in spots) {
            Cell neighborCell = cells[(int)p.dest.x, (int)p.dest.z];

            if (neighborCell.Block1 == null) { //1. empty
                AddBlockToCell(currCell.Block1, p);
                surrounded++;
            }
            else if (neighborCell.Block1.ID == currCell.Block1.ID) { //2. Its me
                surrounded++;
            }
            else if (neighborCell.Block1.ID == -1) { // 3. its wall
                surrounded++;
            }
            else if (Beats(currCell.Block1.ID, neighborCell.Block1.ID)) { //5. if i can break (We already know block 2 is null)
                AddBlockToCell(currCell.Block1, p);
                neighborCell.SwapBlocksAndVars();
                surrounded++;
            }
            //if it beats me but not full
            else if (neighborCell.Block2 == null && Beats(neighborCell.Block1.ID, currCell.Block1.ID)) { //4. Block that beats me
                if (neighborCell.Block1.sizePercent < 1f) { //4.5 block that beats me that is not full
                    AddBlockToCell(currCell.Block1, p);
                }
                else {//Block that beast me that is full
                    neighborCell.isActive = true;
                }
            } //6. Block I don't know (Do nothing)
        }
        return (surrounded == spots.Count);
    }

    private void Update() {

        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();

        }
        float diff = Time.time - timeSinceReset;
        if (timeSinceReset > 0 && diff > maxTime) {
            StartCoroutine(RequestReset(waitTime));
        }
        CheckForWinners();
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
                            if (cell.Block1.ID != -1) {
                                cell.isActive = Propigate(GetValidNeighbors(x, z), x, z);
                            }
                        }
                        else { //One block not full
                            Grow(cell);
                        }
                    }
                    else {
                        //sb.Append(x + "," + z + " " + cell);
                        filled = false;
                    }
                }
                else {
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
        }
        else {
            //Debug.Log(sb);
        }
    }

    private void CheckForWinners() {
        int alive = 0;
        for (int i = 0; i < beats.Count; i++) {
            if (GameObject.Find("Parent: " + i).transform.childCount > 0)
                alive++;
            if (alive >= 2)
                break;
        }
        if (alive == 1) {
            StartCoroutine(RequestReset(waitTime));
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
    private void ShrinkAndGrowBlock(Cell cell) {
        //We already know we are hitting a block && We also know that cell 2 is not null
        Debug.Assert(cell.Block1 != null && cell.Block2 != null);

        if (cell.Block1.sizePercent + cell.Block2.sizePercent >= 1f) {
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

    public void GrowBlock(Block block) {
        block.gameObject.transform.position += block.increment / 2 * block.Orientation;
        block.gameObject.transform.localScale += block.increment * VecAbs(block.Orientation);
        block.sizePercent = Vector3.Dot(block.gameObject.transform.localScale, VecAbs(block.Orientation));

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
        block.gameObject.transform.localScale -= block.increment * VecAbs(block.Orientation);
        block.sizePercent = Vector3.Dot(block.gameObject.transform.localScale, VecAbs(block.Orientation));

        if (block.sizePercent < block.increment) {
            AllGameObjects.Remove(block.gameObject);
            Destroy(block.gameObject);
            //DestroyImmediate(block.gameObject,true);
            cell.Block2 = null;
        }
    }

    private void AddBlockToCell(Block currBlock, DestAndPos p) {
        //Make sure at least one cell is empty before we place a new block inside
        if (cells[(int)p.dest.x, (int)p.dest.z].Block1 != null && cells[(int)p.dest.x, (int)p.dest.z].Block2 != null) {
            // Wait until whatever is happening in this cell to finish.
            return;
        }

        GameObject newObject = Instantiate(FlowBlock, Vector3.one, Quaternion.identity);
        newObject.GetComponent<Renderer>().material.color = color_and_inc[currBlock.ID].Item1;
        Block newBlock = new Block(newObject, currBlock.ID, p.dir, p.dest, increment: currBlock.increment, sizePercent: currBlock.increment);

        //Make new obj
        newObject.transform.position = p.dest + p.dir * (newBlock.increment / 2 - 0.5f);
        newObject.transform.localScale = Vector3.one - (1 - newBlock.increment) * VecAbs(p.dir);

        newObject.name = currBlock.ID + " " + p.dest.x + "," + p.dest.z;
        newObject.transform.parent = GameObject.Find("Parent: " + currBlock.ID).transform;

        //If cell is not active set it to active
        cells[(int)p.dest.x, (int)p.dest.z].isActive = true;

        //Check which block is empty
        if (cells[(int)p.dest.x, (int)p.dest.z].Block1 == null)
            cells[(int)p.dest.x, (int)p.dest.z].Block1 = newBlock;
        else
            cells[(int)p.dest.x, (int)p.dest.z].Block2 = newBlock;

        //Add to array for reset
        AllGameObjects.Add(newObject);
    }

    public bool IsInRange(int val, int bound) {
        return (val < bound && val >= 0);
    }

    private Vector3 VecAbs(Vector3 v) {
        return new Vector3(Math.Abs(v.x), Math.Abs(v.y), Math.Abs(v.z));
    }

    /// <summary>
    /// Checks if a beats b
    /// </summary>
    public bool Beats(int a, int b) {
        return beats.Contains(new Tuple<int, int>(a, b));
    }
}
