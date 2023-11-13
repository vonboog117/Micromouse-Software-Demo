using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{

    enum State{
        Manual,
        Idle,
        Find,
        Return,
        Race
    }

    private const float COORD_DIF = 0.045f;

    public GameObject appearance;

    private Rigidbody rb;
    private Vector3 velocity;
    private Maze maze = new Maze();
    private State mouseState;

    private int orientation;
    private Vector2 mouseCoords;
    private Vector2 nextCoords;
    private Vector3 nextPos;


    // Start is called before the first frame update
    void Start(){
        rb = GetComponent<Rigidbody>();
        mouseState = State.Idle;
        orientation = 0;
        nextCoords = new Vector2(-1,-1);
    }

    // Update is called once per frame
    void Update(){
        mouseCoords = MazeManager.Node.GetUnitCoords(transform.position);

        float horIn = Input.GetAxisRaw("Horizontal");
        float verIn = Input.GetAxisRaw("Vertical");
        if(horIn > 0){
            //velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0).normalized;
            orientation = 1;
            appearance.transform.rotation = Quaternion.Euler(0f,90f,0f);
        }else if(horIn < 0){
            //velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0).normalized;
            orientation = 3;
            appearance.transform.rotation = Quaternion.Euler(0f,-90f,0f);
        }else if(verIn > 0){
            //velocity = new Vector3(0, 0, Input.GetAxisRaw("Vertical")).normalized;
            orientation = 0;
            appearance.transform.rotation = Quaternion.Euler(0f,0f,0f);
        }else if(verIn < 0){
            //velocity = new Vector3(0, 0, Input.GetAxisRaw("Vertical")).normalized;
            orientation = 2;
            appearance.transform.rotation = Quaternion.Euler(0f,180f,0f);
        }

        // if(mouseState == State.Manual){
        //     velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        // }

        Vector3 rayOrigin = transform.position;
        RaycastHit hit;
        int layerMask = 1 << 3;
        if(Physics.Raycast(rayOrigin, transform.forward, out hit, 0.15f, layerMask)){
            HandleWallHit(hit);
        }
        if(Physics.Raycast(rayOrigin, transform.right, out hit, 0.15f, layerMask)){
            HandleWallHit(hit);
        }
        if(Physics.Raycast(rayOrigin, -transform.right, out hit, 0.15f, layerMask)){
            HandleWallHit(hit);
        }
        if(Physics.Raycast(rayOrigin, -transform.forward, out hit, 0.15f, layerMask)){
            HandleWallHit(hit);
        }

        if((int)nextCoords.x > (int)mouseCoords.x && (int)nextCoords.y == (int)mouseCoords.y){
           // Debug.Log("Right");
        }else if((int)nextCoords.x < (int)mouseCoords.x && (int)nextCoords.y == (int)mouseCoords.y){
           // Debug.Log("Left");
        }else if((int)nextCoords.x == (int)mouseCoords.x && (int)nextCoords.y > (int)mouseCoords.y){
            //Debug.Log("Up");
        }else if((int)nextCoords.x == (int)mouseCoords.x && (int)nextCoords.y < (int)mouseCoords.y){
            //Debug.Log("Down");
        }
        //Debug.Log((int)nextCoords.x + " " + (int)nextCoords.y + " cur " + (int)mouseCoords.x + " " + (int)mouseCoords.y);
    }

    void FixedUpdate(){
        switch(mouseState){
            case State.Manual:
                //nextCoords = GetNextCoords();
                //Vector3 nextPos = MazeManager.Node.GetUnitPosition((int)nextCoords.x, (int)nextCoords.y);
                velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
                rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
                break;
            case State.Idle:
                break;
            case State.Find:
                MoveToNextUnit();
                break;
            case State.Return:
                break;
            case State.Race:
                break;
        }
    }

    private void MoveToNextUnit(){
        if(((int)mouseCoords.x == 0 && (int)mouseCoords.y == 0 && nextCoords.x == -1) ||
        //   (Mathf.Abs(mouseCoords.x - nextCoords.x) <= COORD_DIF && Mathf.Abs(mouseCoords.y - nextCoords.y) <= COORD_DIF)){
            (Mathf.Abs(transform.position.x - nextPos.x) <= COORD_DIF && Mathf.Abs(transform.position.z - nextPos.z) <= COORD_DIF)){
            nextCoords = GetNextCoords();
            nextPos = MazeManager.Node.GetUnitPosition((int)nextCoords.x, (int)nextCoords.y);
            velocity = new Vector3(nextPos.x - transform.position.x, 0, nextPos.z - transform.position.z).normalized;
            Debug.Log(nextCoords + " " + nextPos + " "+ mouseCoords + " " + velocity);
        }
        //velocity = new Vector3(nextCoords.x - mouseCoords.x, 0, nextCoords.y - mouseCoords.y).normalized;
        //Debug.Log(nextCoords + " " + mouseCoords + " " + velocity);
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        //Debug.Log(velocity);
    }

    public void ResetMazeMemory(){
        gameObject.transform.position = new Vector3(0.07f, 0.02f, 0.0721f);
        maze.ResetMaze();
    }

    public void ChangeMouseState(int newState){
        if(newState == 0){
            if(mouseState == State.Manual){ mouseState = State.Idle; }
            else{ mouseState = State.Manual; }
        }else if(newState == 1){
            mouseState = State.Find;
        }else if(newState == 2){
            mouseState = State.Return;
        }else if(newState == 3){
            mouseState = State.Race;
        }
    }

    public Vector2 GetNextCoords(){
        Vector2 coords = new Vector2(100,100);
        int curX = (int)mouseCoords.x;
        int curY = (int)mouseCoords.y;
        int minValue = 300;

        List<int> neighborIndices = maze.GetAdjacentNodeIndices(curX, curY);

        foreach(int index in neighborIndices){
            int x = index % 16;
            int y = index / 16;
            if(maze.nodes[x, y].value < minValue){
                coords.x = x;
                coords.y = y;
                minValue = maze.nodes[x, y].value;
            }
        }

        return coords;
    }

    public void HandleWallHit(RaycastHit hit){
        Vector3 wallIndices = MazeManager.Edge.GetWallIndex(hit.collider.gameObject.transform.parent.position, hit.collider.gameObject.transform.parent.rotation);
        if(!MazeManager.maze.GetFound((int)wallIndices.x, (int)wallIndices.y, wallIndices.z == 1) && hit.collider.gameObject.tag == "Wall"){
            MazeManager.maze.SetFound((int)wallIndices.x, (int)wallIndices.y, wallIndices.z == 1, true);
            int idxA = 16 * ((int)wallIndices.y) + (int)wallIndices.x;
            if(wallIndices.z == 1){
                maze.removeEdge(idxA, idxA + 1);
            }else{
                maze.removeEdge(idxA, idxA + 16);
            }
            maze.FloodFill(7,7);
        }
        if(!hit.collider.gameObject.GetComponent<MeshRenderer>().enabled){
            GameObject wall = hit.collider.gameObject.transform.parent.gameObject;
            foreach(MeshRenderer renderer in wall.GetComponentsInChildren<MeshRenderer>()){
                renderer.enabled = true;
            }
        }
    }

    public class Maze{
        public Node[,] nodes;
        public Dictionary<int, List<Edge>> edges;
        public Dictionary<int, List<int>> adjMatrix;

        public Maze(){
            nodes = new Node[16,16];
            edges = new Dictionary<int, List<Edge>>();
            adjMatrix = new Dictionary<int, List<int>>();

            for(int x = 0; x < 16; x++){
                for(int y = 0; y < 16; y++){
                    nodes[x,y] = new Node(x,y,0);
                    edges.Add(nodes[x,y].index, new List<Edge>());
                    adjMatrix.Add(nodes[x,y].index, new List<int>());
                    if(x != 0){ addEdge(nodes[x,y], nodes[x-1,y]); }
                    if(y != 0){ addEdge(nodes[x,y], nodes[x,y-1]); }
                }
            }
            removeEdge(0,1);
        }

        public void ResetMaze(){
            edges = new Dictionary<int, List<Edge>>();
            adjMatrix = new Dictionary<int, List<int>>();

            for(int x = 0; x < 16; x++){
                for(int y = 0; y < 16; y++){
                    edges.Add(nodes[x,y].index, new List<Edge>());
                    adjMatrix.Add(nodes[x,y].index, new List<int>());
                    if(x != 0){ addEdge(nodes[x,y], nodes[x-1,y]); }
                    if(y != 0){ addEdge(nodes[x,y], nodes[x,y-1]); }
                }
            }
            removeEdge(0,1);
        }

        public void FloodFill(int x_idx, int y_idx){
            ClearNodeValues();
            Queue<Node> queue = new Queue<Node>();
            if((x_idx == 7 && y_idx == 7) ||
               (x_idx == 7 && y_idx == 8) ||
               (x_idx == 8 && y_idx == 7) ||
               (x_idx == 8 && y_idx == 8)){
                nodes[7,7].value = 0;
                nodes[7,8].value = 0;
                nodes[8,7].value = 0;
                nodes[8,8].value = 0;
                queue.Enqueue(nodes[7,7]);
                queue.Enqueue(nodes[7,8]);
                queue.Enqueue(nodes[8,7]);
                queue.Enqueue(nodes[8,8]);
            }else{
                nodes[x_idx, y_idx].value = 0;
                queue.Enqueue(nodes[x_idx, y_idx]);
            }

            while(queue.Count != 0){
                Node cur = queue.Dequeue();
                foreach(int neighbor in adjMatrix[cur.index]){
                    int x = neighbor % 16;
                    int y = neighbor / 16;
                    if(nodes[x,y].value == -1){
                        nodes[x,y].value = cur.value + 1;
                        UIManager.UpdateText(x,y,nodes[x,y].value);
                        queue.Enqueue(nodes[x,y]);
                    }
                }
            }
        }

        public List<int> GetAdjacentNodeIndices(int x, int y){
            return adjMatrix[nodes[x,y].index];
        }

        public void addEdge(Node nodeA, Node nodeB){
            Edge edge = new Edge(nodeA, nodeB);
            if(!edges[nodeA.index].Contains(edge)){
                edges[nodeA.index].Add(edge);
                edges[nodeB.index].Add(edge);
                adjMatrix[nodeA.index].Add(nodeB.index);
                adjMatrix[nodeB.index].Add(nodeA.index);
            }
        }

        public void removeEdge(int nodeIdxA, int nodeIdxB){
            Node nodeA = nodes[nodeIdxA % 16, nodeIdxA / 16];
            Node nodeB = nodes[nodeIdxB % 16, nodeIdxB / 16];

            if(adjMatrix[nodeA.index].Contains(nodeB.index)){
                adjMatrix[nodeA.index].Remove(nodeB.index);
                adjMatrix[nodeB.index].Remove(nodeA.index);
            }
        }

        public int[,] GetNodeValues(){
            int[,] values = new int[16,16];
            for(int x = 0; x < 16; x++){
                for(int y = 0; y < 16; y++){
                    values[x,y] = nodes[x,y].value;
                }
            }
            return values;
        }

        public void ClearNodeValues(){
            for(int x = 0; x < 16; x++){
                for(int y = 0; y < 16; y++){
                    nodes[x,y].value = -1;
                }
            }
        }
    
        public struct Node{
            public int x;
            public int y;
            public int value;
            public int index;

            public Node(int _x, int _y, int _v){
                x = _x;
                y = _y;
                value = _v;
                index = 16*y + x;
            }
        }

        public struct Edge{
            public Node[] nodes;

            public Edge(Node nodeA, Node nodeB){
                nodes = new Node[2];
                nodes[0] = nodeA;
                nodes[1] = nodeB;
            }
        }
    }
}
