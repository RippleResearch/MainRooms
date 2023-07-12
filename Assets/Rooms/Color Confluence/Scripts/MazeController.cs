using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using UnityEngine;
using Color = UnityEngine.Color;

public class MazeController : MonoBehaviour {


    public GameObject Dirt, FlowBlock;
    public int sizeMultiplier;
    public int height, width;
    [Range(0f, 4f)]
    public float waitTime;
    [Range(1 / 64.0f, 1f)]
    public float baseIncrement;
    [Range(0f, 1f)]
    public float wallsRemoved;
    [Range(1, 120)]
    public int ResetAfter;
    public float timeSinceReset;
    public int active;
    public bool resetRequested;

    //Colors
    [HideInInspector] public KeyValuePair<string, List<Color>> nextPal;
    [HideInInspector] public bool palSet;
    [HideInInspector] public KeyValuePair<string, List<Color>> usedColors;

    //UI Button and Slider Fields
    [HideInInspector] public bool colorBlindMode;
    [HideInInspector] public int numOfColors;
    [HideInInspector] public bool randomNumOfColors;
    [HideInInspector] public bool updateColorDropDown;
    [HideInInspector] public bool randomSize;
    [HideInInspector] public bool rulesSet;
    [HideInInspector] public string methodName;
    [HideInInspector] public Dictionary<string, int> ruleMethodName;
    

    //private BFS bfs;
    private const int WALL = 0;
    private const int PATH = 1;
    private const int HEIGHT = 9;
    private const int WIDTH = 19;

    public List<GameObject> AllGameObjects;
    List<Tuple<int, int>> beats;
    List<Tuple<Color, float>> color_and_inc;
    List<int> rows;
    List<int> cols;
    [HideInInspector] public ColorController colorController;

    System.Random rand;
    int[,] board;
    Cell[,] cells;
    public class DestAndPos {
        public Vector3 dest;
        public Vector3 dir;

        public DestAndPos(Vector3 dest, Vector3 dir) {
            this.dest = dest;
            this.dir = dir;
        }
    }

    void Awake() {
        Screen.orientation = ScreenOrientation.LandscapeLeft; //Should auto rotate screen when first loading scene

        rand = new System.Random();
        AllGameObjects = new List<GameObject>();
        colorController = new ColorController(); // Automatically initalizes palletes

        //Default values on start
        waitTime = 2; //Do not wait after restart
        ResetAfter = 15; // Once filled, reset after 15 seconds
        sizeMultiplier = 4; // Need size so UI Can be seen (should not need this but do because of bad programming, you can fix it, I believe in you!)
        randomSize = true; // start with random size between 4 and 8
        palSet = false; // Start with random pals
        colorBlindMode = false; // Start with normal colors
        randomNumOfColors = true; // start with random num of colors
        updateColorDropDown = true; // Update the color drop down pallete values
        rulesSet = false; // Use random rules
        ruleMethodName = new Dictionary<string, int> { ["Spock Rules"] = 0, ["Random Rules"] = 1, ["No Rules"] = 2, ["Circle Rules"] = 3}; //Probably don't need but works for now

        InitializeMaze();
    }

    private void InitializeMaze() {
        Resources.UnloadUnusedAssets();
        AllGameObjects.Clear();
        updateColorDropDown = true;

        beats = new List<Tuple<int, int>>();
        color_and_inc = new List<Tuple<Color, float>>();

        //Set Colors and pairings
        if (randomNumOfColors) 
            numOfColors = 0;

        if(palSet) {
            usedColors = nextPal;
        }
        else
            usedColors = colorController.HexColorAndPair(ColorPalettes.RandomPalette(colors: numOfColors, colorBlind: colorBlindMode));

        for (int i = 0; i < usedColors.Value.Count; i++) {
            color_and_inc.Add(new Tuple<Color, float>(usedColors.Value[i], baseIncrement));
        }

        //if (usedColors.Value.Count % 2 == 0)
        //    beats = CirclePairings(usedColors.Value);
        //else
       
        //Set pairing rules
        int methodIndex = rulesSet ? ruleMethodName[methodName] : UnityEngine.Random.Range(0, 3);

        switch (methodIndex) {
            case 0:
                beats = SpockPairings(usedColors.Value);
                methodName = "Spock Rules";
               // Debug.Log("Spock");
                break;
            case 1: 
                beats = RandomPairings(usedColors.Value);
                methodName = "Random Rules";
                //Debug.Log("Rand");
                break;
            case 2:
                beats = EmptyPairings(usedColors.Value);
                methodName = "No Rules";
                //Debug.Log("None");
                break;
            case 3:
                beats = CirclePairings(usedColors.Value);
                methodName = "Circle Rules";
                break;
            default:
                Debug.Assert(false, "This case should not be used");
               // Debug.Break();
                break;
        }
        //beats = SpockPairings(usedColors.Value);
        //Debug.Log("Pairs: " + beats.Count);
        //Debug.Log("Items: " + string.Join("; ", beats));

        
        
        (height, width) = SetSizes();
        //Set camera
        Vector3 center = new Vector3(height / 2, height, width / 2);
        Camera.main.transform.position = center;
        //Set UI
        //GameObject.Find("UI").GetComponent<CanvasController>().Reposition(height, width);

        int spot = UnityEngine.Random.Range(0, height);
        if (spot % 2 == 1) {
            spot--;
        }
        System.Numerics.Vector2 start = new System.Numerics.Vector2(0, spot);
        ////////////PRIMS MAZE//////////////////
        PrimsMaze prims = new PrimsMaze(width, height, start, false);
        board = new int[height, width];
        board = prims.maze;

        RemovePercentWalls(wallsRemoved);
        ////////////PRIMS MAZE//////////////////    

        //Array of Cells
        cells = new Cell[height, width];

        //Generate Blocks
        for (int x = -1; x <= height; ++x) {
            for (int z = -1; z <= width; ++z) {
                //Place Block
                Cell cell = PlaceBlocksInCell(x, z);
                if (x >= 0 && z >= 0 && z < width && x < height) {
                    cells[x, z] = cell;
                    if (cell.Block1.gameObject.CompareTag("Dirt")) {
                        cells[x, z].isActive = false;   
                    }
                }
            }
        }
        timeSinceReset = Time.time;
        PlaceStartBlocks();
       
        //For random processing
        rows = Enumerable.Range(0, height).ToList();
        cols = Enumerable.Range(0, width).ToList();
        
        resetRequested = false;
    }

    private Cell PlaceBlocksInCell(int x, int z) {
        GameObject go;
        if (x < 0 || z < 0 || z >= width || x >= height) {
            go = Instantiate(Dirt, new Vector3(x, 0, z), Quaternion.identity);
            go.name = "Dirt " + x + "," + z;
            go.transform.parent = GameObject.FindGameObjectWithTag("Dirt").transform;
            AllGameObjects.Add(go);
            return new Cell(new Block(go, -2));
        }
        else {
            //0 = WALL, 1 = PATH, 2 = TRACED PATH FROM START TO END
            if (board[x, z] == WALL) {
                go = Instantiate(Dirt, new Vector3(x, 0, z), Quaternion.identity);
                go.name = "Dirt " + x + "," + z;
                go.transform.parent = GameObject.FindGameObjectWithTag("Dirt").transform;
                AllGameObjects.Add(go);
                return new Cell(new Block(go, -2));
            }
            else {
                GameObject ob1 = Instantiate(FlowBlock, new Vector3(x, 0, z), Quaternion.identity);
                ob1.name = "empty " + x + "," + z;
                ob1.transform.parent = GameObject.FindGameObjectWithTag("Maze").transform;
                ob1.GetComponent<Renderer>().enabled = false;

                GameObject ob2 = Instantiate(FlowBlock, new Vector3(x, 0, z), Quaternion.identity);
                ob2.name = "empty " + x + "," + z;
                ob2.transform.parent = GameObject.FindGameObjectWithTag("Maze").transform;
                ob2.GetComponent<Renderer>().enabled = false;
                AllGameObjects.Add(ob1);
                AllGameObjects.Add(ob2);
                // make blocks not shown.
                Cell cell = new Cell(new Block(ob1, -1));
                cell.Block2 = new Block(ob2, -1);
                cell.Block1.sizePercent = 0;
                return cell;
            }
        }
    }

    private void PlaceStartBlocks() {
        for (int i = 0; i < usedColors.Value.Count; i++) {
            bool placed = false;
            do {
                int x = UnityEngine.Random.Range(0, height);
                int z = UnityEngine.Random.Range(0, width);
                if (board[x, z] == PATH) {
                    Block block1 = cells[x, z].Block1;

                    Renderer rend = block1.gameObject.GetComponent<Renderer>();
                    rend.enabled = (true);
                    rend.material.color = color_and_inc[i].Item1;

                    block1.gameObject.name = i + " " + x + "," + z;
                    //block1.gameObject.transform.parent = parentObj.transform;

                    block1.SetVals(block1.gameObject, i, Vector3.zero, block1.gameObject.transform.position, increment: color_and_inc[i].Item2);
                    block1.ID = i;

                    cells[x, z].isActive = true;
                    placed = true;
                }
            } while (!placed);
        }
    }

    private void Update() {
        if(Time.time - timeSinceReset < waitTime) {
            return;
        }

        float diff = Time.time - timeSinceReset;
        if (resetRequested || (timeSinceReset > 0 && diff > ResetAfter)) {
            resetRequested = true;
            StartCoroutine(RequestReset(waitTime));
        }

        bool filled = true;
        rows = rows.OrderBy(_ => rand.Next()).ToList();
        cols = cols.OrderBy(_ => rand.Next()).ToList();
        active = 0;
        foreach (int x in rows) {
            foreach (int z in cols) {
                //If one full block
                Cell cell = cells[x, z];
                if (!cell.isActive) {
                    continue;
                }
                active++;
                if (cell.Block2.gameObject.name.StartsWith("empty")) {
                    if (!cell.Block1.gameObject.name.StartsWith("empty")) {
                        if (Mathf.Abs(cell.Block1.sizePercent - 1f) < .001f) {
                            if (cell.Block1.ID != -2) { // Not Dirt
                                Propigate(GetValidNeighbors(x, z), x, z);
                            }
                            cell.isActive = false;
                        }
                        else { //One block not full
                            Grow(cell);
                        }
                    }
                    else {
                        filled = false;
                    }
                }
                else {
                    if (!cell.Block1.gameObject.name.StartsWith("empty")) {
                        ShrinkAndGrowBlock(cell);
                    }
                    else {
                        cell.isActive = false;
                    }
                }
            }
        }
        if (filled) {
            if (timeSinceReset < 0) {
                timeSinceReset = Time.time;
            }
        }

        if(active == 0){
            resetRequested = true;
        }
    }


    //-------------------------------------------------------------------------
    //-------------------------------------------------------------------------
    /// <summary>
    /// Returns pairings based on spock Rock Paper Sci if odd
    /// or close to that if even.
    /// </summary>
    /// <param name="colors">Length must be >= 3</param>
    /// <returns></returns>
    private List<Tuple<int,int>> SpockPairings(List<Color> colors) {
       Debug.Assert(colors.Count >= 3); 

        List<Tuple<int, int>> pairs = new List<Tuple<int, int>>();
        if (colors.Count % 2 == 1) {
            for (int j = 0; j < colors.Count / 2; j++) {
                for (int i = 0; i < colors.Count; i++) {
                    pairs.Add(new Tuple<int, int>(i, (2 * j + (i + 1)) % colors.Count));
                }
            }
        }
        else {
            // Different for even. (This formula might also work for odd, but I didn't verify.)
            for (int i = 0; i < colors.Count; i++) {
                for (int j = i+1; j < colors.Count ; j++) {
                    if ((j-i) % 2 == 1) {
                        pairs.Add(new Tuple<int, int>(i, j));
                    } else {
                        pairs.Add(new Tuple<int, int>(j,i));
                    }
                }
            }

        }
        return pairs;
    }

    /// <summary>
    /// Returns two tuples one of ints for the pairings of who beats who.
    /// In this case it is just cirlce pairings (as in 1 -> 2 -> 3 and so on)
    /// </summary>
    /// <param name="colors">Length must be >= 3</param>
    private List<Tuple<int, int>> CirclePairings(List<Color> colors) {
        Contract.Requires(colors.Count >= 3);

        List<Tuple<int, int>> pairs = new List<Tuple<int, int>>();
        for (int i = 0; i < colors.Count-1; i++) {
            pairs.Add(new Tuple<int, int>(i, i + 1));
    }
    pairs.Add(new Tuple<int, int>(colors.Count-1, 0));

        return pairs;
    }
    private List<Tuple<int, int>> RandomPairings(List<Color> colors) {
        Debug.Assert(colors.Count >= 3);
        List<Tuple<int, int>> pairs = new List<Tuple<int, int>>();
       
            for (int i = 0; i < colors.Count; i++) {
                for (int j = i + 1; j < colors.Count; j++) {
                    if (rand.Next() % 2 == 0) {
                        pairs.Add(new Tuple<int, int>(i, j));
                    } else {
                        pairs.Add(new Tuple<int, int>(j, i));
                    }
                }
            }
        return pairs;
    }
    private List<Tuple<int, int>> EmptyPairings(List<Color> colors) {
        return new List<Tuple<int, int>>();
    }
    //-------------------------------------------------------------------------
    //-------------------------------------------------------------------------

    private void Propigate(List<DestAndPos> spots, int x, int z) {
        Cell currCell = cells[x, z];

        Debug.Assert(!currCell.Block1.gameObject.name.StartsWith("empty"));
        foreach (DestAndPos p in spots) {
            Cell neighborCell = cells[(int)p.dest.x, (int)p.dest.z];

            if (neighborCell.Block1.ID == -1) { //1. empty (-1 path)
                AddBlockToCell(currCell.Block1, p);
            }
            else if (neighborCell.Block1.ID == currCell.Block1.ID) { //2. its me
                continue; // Stop early for effic
            }
            else if (neighborCell.Block1.ID == -2) { // 3. its wall
                //This needs to be here so we don't check if an ID can beat -2 in the beats tuple list
                continue;
            }//6. else this block is not comparable
            else if (Beats(currCell.Block1.ID, neighborCell.Block1.ID)) { //5. if i can break (We already know block 2 is null)
                AddBlockToCell(currCell.Block1, p);
                neighborCell.SwapBlocksAndVars();
            }
            //if it beats me but not full
            else if (Beats(neighborCell.Block1.ID, currCell.Block1.ID)) {
                if (neighborCell.Block1.sizePercent < 1f && neighborCell.Block2.gameObject.name.StartsWith("empty")) {//4.5 block that beats me that is not full
                    AddBlockToCell(currCell.Block1, p);
                }
                else {//Block that beast me that is full
                    neighborCell.isActive = true;
                }
            }

            //else {
            //}
            currCell.isActive = false;
        }
    }


    private void AddBlockToCell(Block currBlock, DestAndPos p) {
        //Make sure at least one cell is empty before we place a new block inside
        if (!cells[(int)p.dest.x, (int)p.dest.z].Block1.gameObject.name.StartsWith("empty")
            && !cells[(int)p.dest.x, (int)p.dest.z].Block2.gameObject.name.StartsWith("empty")) {
            // Wait until whatever is happening in this cell to finish.
            return;
        }
        Block editBlock;
        //Block we are editing
        if (cells[(int)p.dest.x, (int)p.dest.z].Block1.gameObject.name.StartsWith("empty"))
            editBlock = cells[(int)p.dest.x, (int)p.dest.z].Block1;
        else
            editBlock = cells[(int)p.dest.x, (int)p.dest.z].Block2;

        //GameObject newObject = Instantiate(FlowBlock, Vector3.one, Quaternion.identity);
        Renderer rend = editBlock.gameObject.GetComponent<Renderer>();
        rend.enabled = (true);
        rend.material.color = usedColors.Value[currBlock.ID]; //so we can change color while runnning
        editBlock.SetVals(editBlock.gameObject, currBlock.ID, p.dir, p.dest,
            increment: baseIncrement, sizePercent: currBlock.increment);

        //Make new obj
        editBlock.gameObject.transform.position = p.dest + p.dir * (editBlock.increment / 2 - 0.5f);
        editBlock.gameObject.transform.localScale = Vector3.one - (1 - editBlock.increment) * VecAbs(p.dir);

        editBlock.gameObject.name = currBlock.ID + " " + p.dest.x + "," + p.dest.z;
        //editBlock.gameObject.transform.parent = GameObject.Find("Parent: " + currBlock.ID).transform;

        //If cell is not active set it to active
        cells[(int)p.dest.x, (int)p.dest.z].isActive = true;
        //Add to array for reset
        //AllGameObjects.Add(newObject);
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

    public void ShrinkSecondBlock(Cell cell) {
        Block block = cell.Block2;
        //Should have other blocks inc
        block.gameObject.transform.position -= block.increment / 2 * block.Orientation;
        block.gameObject.transform.localScale -= block.increment * VecAbs(block.Orientation);
        block.sizePercent = Vector3.Dot(block.gameObject.transform.localScale, VecAbs(block.Orientation));

        if (block.sizePercent < block.increment) {
            //AllGameObjects.Remove(block.gameObject);
            // Destroy(block.gameObject); change this
            //DestroyImmediate(block.gameObject,true);
            block.gameObject.name = "empty";
            block.gameObject.GetComponent<Renderer>().enabled = false;
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
    /// <summary>
    /// Grow every block in a given cell
    /// </summary>
    /// <param name="cell"></param>
    public void Grow(Cell cell) {
        if (!cell.Block1.gameObject.name.StartsWith("empty")) {
            GrowBlock(cell.Block1);
        }
        if (!cell.Block2.gameObject.name.StartsWith("empty")) {
            GrowBlock(cell.Block2);
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
        if (IsInRange(z - 1, width)) {
            spots.Add(new DestAndPos(new Vector3(x, 0, z - 1), new Vector3(0, 0, -1)));
        }
        //right
        if (IsInRange(x + 1, height)) {
            spots.Add(new DestAndPos(new Vector3(x + 1, 0, z), new Vector3(1, 0, 0)));
        }
        //down
        if (IsInRange(z + 1, width)) {
            spots.Add(new DestAndPos(new Vector3(x, 0, z + 1), new Vector3(0, 0, 1)));
        }
        //left
        if (IsInRange(x - 1, height)) {
            spots.Add(new DestAndPos(new Vector3(x - 1, 0, z), new Vector3(-1, 0, 0)));
        }

        return spots;
    }
    public bool IsInRange(int val, int bound) {
        return (val < bound && val >= 0);
    }

    private void RemovePercentWalls(float remove) {
        for (int x = 0; x < height; x++) {
            for (int z = 0; z < width; z++) {
                if (board[x, z] == WALL) {
                    List<DestAndPos> spots = GetValidNeighbors(x, z);
                    foreach (DestAndPos spot in spots) {
                        if (board[(int)spot.dest.x, (int)spot.dest.z] != WALL) {
                            if (UnityEngine.Random.Range(0, 1f) <= remove) {
                                board[x, z] = PATH;
                            }
                            break;
                        }
                    }
                }
            }
        }
    }

    private void CheckForWinners() {
        int alive = 0;
        for (int i = 0; i < usedColors.Value.Count; i++) {
            if (GameObject.Find("Parent: " + i).transform.childCount > 0)
                alive++;
            if (alive >= 2)
                break;
        }
        if (alive == 1) {
            StartCoroutine(RequestReset(waitTime));
        }
    }

    public IEnumerator RequestReset(float waitTime) {
        foreach (GameObject go in AllGameObjects) {
            if (go != null) {
                Destroy(go);
            }
        }
        InitializeMaze();
        StopAllCoroutines();
        yield return new WaitForSeconds(waitTime);

    }

    private (int height, int width) SetSizes() {
        if (randomSize) {
            sizeMultiplier = UnityEngine.Random.Range(4, 8);
        }


        height = HEIGHT * sizeMultiplier;
        width = WIDTH * sizeMultiplier;
        if (height % 2 == 0) {
            height--;
        }
        if (width % 2 == 0) {
            width--;
        }
        return (height, width);
    }
    private Vector3 VecAbs(Vector3 v) {
        return new Vector3(Math.Abs(v.x), Math.Abs(v.y), Math.Abs(v.z));
    }

    /// <summary>
    /// Checks if a beats b
    /// </summary>
    public bool Beats(int a, int b) {
        Debug.Assert(a >= 0 && b >= 0, a + " " + b);
        return beats.Contains(new Tuple<int, int>(a, b));
    }

}
