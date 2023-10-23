using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManager : MonoBehaviour
{

    [SerializeField]
    private GameObject latticePrefab;
    [SerializeField]
    private GameObject wallPrefab;
    private Maze maze;
    

    public const float WALL_HEIGHT = 0.05f;
    public const float WALL_WIDTH = 0.012f;
    public const float PASSAGE_WIDTH = 0.168f;
    public const float NODE_WIDTH = 0.18f;

    void Start(){
        maze = new Maze(latticePrefab, wallPrefab);
    }

    void Update()
    {
        
    }

    public void GenerateMaze(){
        maze.ClearMaze();
        maze.GenerateMaze();
    }

    public void GeneratePreset(int index){
        maze.ClearMaze();
        maze.GeneratePreset(index);
    }

    void OnDrawGizmos(){
        Vector3 nodeSize = new Vector3(PASSAGE_WIDTH, WALL_HEIGHT, PASSAGE_WIDTH);
        Vector3 latticeSize = new Vector3(WALL_WIDTH, WALL_HEIGHT, WALL_WIDTH);
        Vector3 wallSizeA = new Vector3(WALL_WIDTH, WALL_HEIGHT, PASSAGE_WIDTH);
        Vector3 wallSizeB = new Vector3(PASSAGE_WIDTH, WALL_HEIGHT, WALL_WIDTH);
        
        for(int x = 0; x < 16; x++){
            for(int y = 0; y < 16; y++){
                Vector3 nodePos = Node.GetUnitPosition(x, y);
                Vector3 wallAPos = Edge.GetWallPosition(x, y, true);
                Vector3 wallBPos = Edge.GetWallPosition(x, y, false);
                Vector3 latticePos = Lattice.GetLatticePosition(x, y);

                if((x % 2 == 0 && y % 2 == 0) || (x % 2 == 1 && y % 2 == 1)){
                    Gizmos.color = Color.blue;
                }else{
                    Gizmos.color = Color.white;
                }
                //Gizmos.DrawCube(nodePos, nodeSize);

                if(x !=15){ 
                    Gizmos.color = Color.yellow;
                    //Gizmos.DrawCube(wallAPos, wallSizeA);
                }

                if(y != 15){
                    Gizmos.color = Color.yellow;
                    //Gizmos.DrawCube(wallBPos, wallSizeB);
                }

                if(x != 15 && y != 15){
                    Gizmos.color = Color.red;
                    //Gizmos.DrawCube(latticePos, latticeSize);
                }

            }
        }
        
    }

    public class Maze{
        private static Node[,] nodes;
        private static Lattice[,] lattices;
        private static Edge[,] verticalEdges;
        private static Edge[,] horizontalEdges;

        //private static List<Edge> passages;
        private static Dictionary<int, List<int>> adjacencyMatrix;

        private static List<Node> visitedNodes;
        private static List<bool> visited;

        private static GameObject latticePrefab;
        private static GameObject wallPrefab;

        public Maze(GameObject lattice, GameObject wall){
            latticePrefab = lattice;
            wallPrefab = wall;

            nodes = new Node[16,16];
            lattices = new Lattice[15,15];
            verticalEdges = new Edge[15,16];
            horizontalEdges = new Edge[16,15];
            adjacencyMatrix = new Dictionary<int, List<int>>();

            for(int x = 0; x < 16; x++){
                for(int y = 0; y < 16; y++){
                    nodes[x,y] = new Node(x,y,false,false);
                    adjacencyMatrix.Add(nodes[x,y].index, new List<int>());

                    if(x != 0){
                        verticalEdges[x-1,y] = new Edge(nodes[x-1,y], nodes[x,y], x-1, y, false, true);
                        verticalEdges[x-1,y].prefab = wallPrefab;
                        adjacencyMatrix[nodes[x-1,y].index].Add(nodes[x,y].index);
                        adjacencyMatrix[nodes[x,y].index].Add(nodes[x-1,y].index);
                    }

                    if(y != 0){
                        horizontalEdges[x,y-1] = new Edge(nodes[x,y-1], nodes[x,y], x, y-1, false, false);
                        horizontalEdges[x,y-1].prefab = wallPrefab;
                        adjacencyMatrix[nodes[x,y-1].index].Add(nodes[x,y].index);
                        adjacencyMatrix[nodes[x,y].index].Add(nodes[x,y-1].index);
                    }

                    if(x != 15 && y != 15){
                        lattices[x,y] = new Lattice(x,y);
                        Instantiate(latticePrefab, Lattice.GetLatticePosition(x,y), Quaternion.identity);
                    }
                }
            }

            GenerateMaze();
        }

        public void ClearMaze(){
            adjacencyMatrix.Clear();
            for(int x = 0; x < 16; x++){
                for(int y = 0; y < 16; y++){
                    adjacencyMatrix.Add(nodes[x,y].index, new List<int>());

                    if(x != 0){
                        if(verticalEdges[x-1,y].isWall){
                            Destroy(verticalEdges[x-1,y].gameObject);
                        }
                        verticalEdges[x-1,y] = new Edge(nodes[x-1,y], nodes[x,y], x-1, y, false, true);
                        verticalEdges[x-1,y].prefab = wallPrefab;
                        adjacencyMatrix[nodes[x-1,y].index].Add(nodes[x,y].index);
                        adjacencyMatrix[nodes[x,y].index].Add(nodes[x-1,y].index);
                    }

                    if(y != 0){
                        if(horizontalEdges[x,y-1].isWall){
                            Destroy(horizontalEdges[x,y-1].gameObject);
                        }
                        horizontalEdges[x,y-1] = new Edge(nodes[x,y-1], nodes[x,y], x, y-1, false, false);
                        horizontalEdges[x,y-1].prefab = wallPrefab;
                        adjacencyMatrix[nodes[x,y-1].index].Add(nodes[x,y].index);
                        adjacencyMatrix[nodes[x,y].index].Add(nodes[x,y-1].index);
                    }

                    if(x != 15 && y != 15){
                        lattices[x,y].wallActive = new bool[]{false,false,false,false};
                        lattices[x,y].numWalls = 0;
                    }
                }
            }
        }

        public void GenerateMaze(){
            for(int x = 0; x < 15; x++){
                for(int y = 0; y < 15; y++){
                    //Get numWall 1-4
                    int numWalls = Random.Range(1,5);
                    float randomNum = Random.Range(0.0f, 1.0f);
                    if(randomNum < 0.5f){ numWalls = 1; }
                    else if(randomNum < 0.75f){ numWalls = 2; }
                    else if(randomNum < 0.875f){ numWalls = 3; }
                    else{ numWalls = 4; }
                    if(numWalls <= lattices[x,y].numWalls){ continue; }

                    //Get edges that are not marked as walls
                    List<int> indices = new List<int>();
                    int count = 0;
                    foreach(bool isWall in lattices[x,y].wallActive){
                        if(!isWall){ indices.Add(count); }
                        count++;
                    }

                    //Mark new walls
                    int numNewWalls = numWalls-lattices[x,y].numWalls;
                    for(int i = 0; i < numNewWalls; i++){
                        int newWallIndex = indices[Random.Range(0,indices.Count)];
                        lattices[x,y].numWalls++;
                        lattices[x,y].wallActive[newWallIndex] = true;

                        if(newWallIndex == 0){ //North
                            if(y != 14){
                                lattices[x,y+1].numWalls++;
                                lattices[x,y+1].wallActive[1] = true;
                            }
                            verticalEdges[x,y+1].gameObject = Instantiate(verticalEdges[x,y+1].prefab, Edge.GetWallPosition(x,y+1,true), Quaternion.identity);
                            verticalEdges[x,y+1].isWall = true;
                            adjacencyMatrix[verticalEdges[x,y+1].edgeNodes[0].index].Remove(verticalEdges[x,y+1].edgeNodes[1].index);
                            adjacencyMatrix[verticalEdges[x,y+1].edgeNodes[1].index].Remove(verticalEdges[x,y+1].edgeNodes[0].index);
                        } else if(newWallIndex == 1){ //South
                            if(y != 0){
                                lattices[x,y-1].numWalls++;
                                lattices[x,y-1].wallActive[0] = true;
                            }
                            verticalEdges[x,y].gameObject = Instantiate(verticalEdges[x,y].prefab, Edge.GetWallPosition(x,y,true), Quaternion.identity);
                            verticalEdges[x,y].isWall = true;
                            adjacencyMatrix[verticalEdges[x,y].edgeNodes[0].index].Remove(verticalEdges[x,y].edgeNodes[1].index);
                            adjacencyMatrix[verticalEdges[x,y].edgeNodes[1].index].Remove(verticalEdges[x,y].edgeNodes[0].index);
                        } else if(newWallIndex == 2){ //East
                            if(x != 14){
                                lattices[x+1,y].numWalls++;
                                lattices[x+1,y].wallActive[3] = true;
                            }
                            horizontalEdges[x+1,y].gameObject = Instantiate(horizontalEdges[x+1,y].prefab, Edge.GetWallPosition(x+1,y,false), Quaternion.Euler(0,90,0));
                            horizontalEdges[x+1,y].isWall = true;
                            adjacencyMatrix[horizontalEdges[x+1,y].edgeNodes[0].index].Remove(horizontalEdges[x+1,y].edgeNodes[1].index);
                            adjacencyMatrix[horizontalEdges[x+1,y].edgeNodes[1].index].Remove(horizontalEdges[x+1,y].edgeNodes[0].index);
                        } else if(newWallIndex == 3){ //3  West
                            if(x != 0){
                                lattices[x-1,y].numWalls++;
                                lattices[x-1,y].wallActive[2] = true;
                            }
                            horizontalEdges[x,y].gameObject = Instantiate(horizontalEdges[x,y].prefab, Edge.GetWallPosition(x,y,false), Quaternion.Euler(0,90,0));
                            horizontalEdges[x,y].isWall = true;
                            adjacencyMatrix[horizontalEdges[x,y].edgeNodes[0].index].Remove(horizontalEdges[x,y].edgeNodes[1].index);
                            adjacencyMatrix[horizontalEdges[x,y].edgeNodes[1].index].Remove(horizontalEdges[x,y].edgeNodes[0].index);
                        }

                        indices.Remove(newWallIndex);
                    }
                }
            }

            EnsureFeatures();
            EnsureConnected();
        }

        public void GeneratePreset(int index){
            for(int x = 0; x < 16; x++){
                for(int y = 0; y < 16; y++){
                    if(y != 15){
                        if(MazePreset.APEC_2018_vert[x,y] == 1){
                            verticalEdges[x,y].gameObject = Instantiate(verticalEdges[x,y].prefab, Edge.GetWallPosition(y,x,true), Quaternion.identity);
                            verticalEdges[x,y].isWall = true;
                        }
                    }

                    if(x != 15){
                        if(MazePreset.APEC_2018_hor[x,y] == 1){
                            horizontalEdges[x,y].gameObject = Instantiate(horizontalEdges[x,y].prefab, Edge.GetWallPosition(y,x,false), Quaternion.Euler(0,90,0));
                            horizontalEdges[x,y].isWall = true;
                        }
                    }
                }
            }
        }

        public static void EnsureConnected(){

            visitedNodes = new List<Node>();
            visited = new List<bool>();
            for(int x = 0; x < 16; x++){
               for(int y = 0; y < 16; y++){
                   visited.Add(false);
               }
            }

            int count = DFS(15,15);          

            if(count < 256){
                List<Edge> visitedEdges = GetVisitedWall();
                int wallIdx = Random.Range(0,visitedEdges.Count);
                Destroy(visitedEdges[wallIdx].gameObject);
                visitedEdges[wallIdx].isWall = false;
                adjacencyMatrix[visitedEdges[wallIdx].edgeNodes[0].index].Add(visitedEdges[wallIdx].edgeNodes[1].index);
                adjacencyMatrix[visitedEdges[wallIdx].edgeNodes[1].index].Add(visitedEdges[wallIdx].edgeNodes[0].index);
                EnsureConnected();
            }
        }

        public static List<Edge> GetVisitedWall(){
            List<Edge> edges = new List<Edge>();
            Edge[] endEdges = new Edge[]{horizontalEdges[7,6], horizontalEdges[8,6],
                                         verticalEdges[6,7], verticalEdges[8,7],
                                         verticalEdges[6,8], verticalEdges[8,8],
                                         horizontalEdges[7,8], horizontalEdges[8,8],
                                         verticalEdges[0,0]};
                                         
            foreach(Node node in visitedNodes){
                //North
                if(node.y != 15){
                    if(!visitedNodes.Contains(nodes[node.x,node.y+1])){
                        edges.Add(horizontalEdges[node.x,node.y]);
                    }
                }
                //South
                if(node.y != 0){
                    if(!visitedNodes.Contains(nodes[node.x,node.y-1])){
                        edges.Add(horizontalEdges[node.x,node.y-1]);
                    }
                }
                //East
                if(node.x != 15){
                    if(!visitedNodes.Contains(nodes[node.x+1,node.y])){
                        edges.Add(verticalEdges[node.x,node.y]);
                    }
                }
                //West
                if(node.x != 0){
                    if(!visitedNodes.Contains(nodes[node.x-1,node.y])){
                        edges.Add(verticalEdges[node.x-1,node.y]);
                    }
                }
            }

            foreach(Edge e in endEdges){
                if(edges.Contains(e)){
                    edges.Remove(e);
                }
            }

            return edges;
        }

        public static int DFS(int x, int y){
            int count = 0;
            visited[16*y + x] = true;
            visitedNodes.Add(nodes[x,y]);
            foreach(int idx in adjacencyMatrix[nodes[x,y].index]){
                int nextX = idx % 16;
                int nextY = idx / 16;
                if(!visited[idx]){
                    count += DFS(nextX, nextY);
                }
            }

            return count + 1;
        }

        public static void EnsureFeatures(){
            Edge[] endEdges = new Edge[]{horizontalEdges[7,6], horizontalEdges[8,6],
                                         verticalEdges[6,7], verticalEdges[8,7],
                                         verticalEdges[6,8], verticalEdges[8,8],
                                         horizontalEdges[7,8], horizontalEdges[8,8]};

            if(horizontalEdges[0,0].isWall){
                Destroy(horizontalEdges[0,0].gameObject);
                horizontalEdges[0,0].isWall = false;
                lattices[0,0].wallActive[2] = false;
                adjacencyMatrix[horizontalEdges[0,0].edgeNodes[0].index].Add(horizontalEdges[0,0].edgeNodes[1].index);
                adjacencyMatrix[horizontalEdges[0,0].edgeNodes[1].index].Add(horizontalEdges[0,0].edgeNodes[0].index);
                //nodes[0,0].passages.Add(horizontalEdges[0,0]);
                //passages.Add(horizontalEdges[0,0]);
            }

            if(!verticalEdges[0,0].isWall){
                verticalEdges[0,0].gameObject = Instantiate(verticalEdges[0,0].prefab, Edge.GetWallPosition(0,0,true), Quaternion.identity);
                verticalEdges[0,0].isWall = true;
                lattices[0,0].wallActive[1] = true;
                adjacencyMatrix[verticalEdges[0,0].edgeNodes[0].index].Remove(verticalEdges[0,0].edgeNodes[1].index);
                adjacencyMatrix[verticalEdges[0,0].edgeNodes[1].index].Remove(verticalEdges[0,0].edgeNodes[0].index);
                //nodes[0,0].passages.Remove(verticalEdges[0,0]);
                //passages.Remove(verticalEdges[0,0]);
            }
            
            if(horizontalEdges[7,7].isWall){
                Destroy(horizontalEdges[7,7].gameObject);
                horizontalEdges[7,7].isWall = false;
                lattices[7,7].wallActive[3] = false;
                lattices[6,7].wallActive[2] = false;
                adjacencyMatrix[horizontalEdges[7,7].edgeNodes[0].index].Add(horizontalEdges[7,7].edgeNodes[1].index);
                adjacencyMatrix[horizontalEdges[7,7].edgeNodes[1].index].Add(horizontalEdges[7,7].edgeNodes[0].index);
                //nodes[6,7].passages.Add(horizontalEdges[7,7]);
                //nodes[7,7].passages.Add(horizontalEdges[7,7]);
                //passages.Add(horizontalEdges[7,7]);
            }

            if(horizontalEdges[8,7].isWall){
                Destroy(horizontalEdges[8,7].gameObject);
                horizontalEdges[8,7].isWall = false;
                lattices[7,7].wallActive[2] = false;
                lattices[8,7].wallActive[3] = false;
                adjacencyMatrix[horizontalEdges[8,7].edgeNodes[0].index].Add(horizontalEdges[8,7].edgeNodes[1].index);
                adjacencyMatrix[horizontalEdges[8,7].edgeNodes[1].index].Add(horizontalEdges[8,7].edgeNodes[0].index);
                //nodes[7,7].passages.Add(horizontalEdges[8,7]);
                //nodes[8,7].passages.Add(horizontalEdges[8,7]);
                //passages.Add(horizontalEdges[8,7]);
            }

            if(verticalEdges[7,7].isWall){
                Destroy(verticalEdges[7,7].gameObject);
                verticalEdges[7,7].isWall = false;
                lattices[7,7].wallActive[1] = false;
                lattices[7,6].wallActive[0] = false;
                adjacencyMatrix[verticalEdges[7,7].edgeNodes[0].index].Add(verticalEdges[7,7].edgeNodes[1].index);
                adjacencyMatrix[verticalEdges[7,7].edgeNodes[1].index].Add(verticalEdges[7,7].edgeNodes[0].index);
                //nodes[7,7].passages.Add(horizontalEdges[7,7]);
                //nodes[7,6].passages.Add(horizontalEdges[7,7]);
                //passages.Add(verticalEdges[7,7]);
            }

            if(verticalEdges[7,8].isWall){
                Destroy(verticalEdges[7,8].gameObject);
                verticalEdges[7,8].isWall = false;
                lattices[7,7].wallActive[0] = false;
                lattices[7,8].wallActive[1] = false;
                adjacencyMatrix[verticalEdges[7,8].edgeNodes[0].index].Add(verticalEdges[7,8].edgeNodes[1].index);
                adjacencyMatrix[verticalEdges[7,8].edgeNodes[1].index].Add(verticalEdges[7,8].edgeNodes[0].index);
                //nodes[7,7].passages.Add(horizontalEdges[7,8]);
                //nodes[7,8].passages.Add(horizontalEdges[7,8]);
                //passages.Add(verticalEdges[7,8]);
            }

            int randomNum = Random.Range(0,8);
            for(int i = 0; i < 8; i++){
                if(i == randomNum){
                    if(endEdges[i].isWall){
                        Destroy(endEdges[i].gameObject);
                        endEdges[i].isWall = false;
                        adjacencyMatrix[endEdges[i].edgeNodes[0].index].Add(endEdges[i].edgeNodes[1].index);
                        adjacencyMatrix[endEdges[i].edgeNodes[1].index].Add(endEdges[i].edgeNodes[0].index);
                    }
                }else{
                    if(!endEdges[i].isWall && endEdges[i].vertical){
                        endEdges[i].gameObject = Instantiate(endEdges[i].prefab, Edge.GetWallPosition(endEdges[i].x,endEdges[i].y,true), Quaternion.identity);
                        endEdges[i].isWall = true;
                        adjacencyMatrix[endEdges[i].edgeNodes[0].index].Remove(endEdges[i].edgeNodes[1].index);
                        adjacencyMatrix[endEdges[i].edgeNodes[1].index].Remove(endEdges[i].edgeNodes[0].index);
                    }else if(!endEdges[i].isWall && !endEdges[i].vertical){
                        endEdges[i].gameObject = Instantiate(endEdges[i].prefab, Edge.GetWallPosition(endEdges[i].x,endEdges[i].y,false), Quaternion.Euler(0,90,0));
                        endEdges[i].isWall = true;
                        adjacencyMatrix[endEdges[i].edgeNodes[0].index].Remove(endEdges[i].edgeNodes[1].index);
                        adjacencyMatrix[endEdges[i].edgeNodes[1].index].Remove(endEdges[i].edgeNodes[0].index);
                    }
                }
            }
        }

        public void SetStart(int x, int y){ nodes[x,y].isStart = true; }

        public void SetEnd(int x, int y){ nodes[x,y].isEnd = true; }
    }

    public class Node{
        public int x;
        public int y;
        public int index;
        public bool isStart;
        public bool isEnd;
        public List<Edge> passages;

        public Node(int _x, int _y, bool start, bool end){
            x = _x;
            y = _y;
            isStart = start;
            isEnd = end;
            passages = new List<Edge>();
            index = 16*y + x;
        }

        public Node(){
            x = -1;
            y = -1;
            isStart = false;
            isEnd = false;
        }

        public void UpdateIndex(int x_idx, int y_idx){
            x = x_idx;
            y = y_idx;
        }

        //Only returns true if the nodes x and y have been set
        public bool isValidNode(){
            if(x > -1 && y > -1){ return true; }
            return false;
        }

        public static Vector3 GetUnitPosition(int x, int y){
            return new Vector3(PASSAGE_WIDTH/2 + x*NODE_WIDTH, WALL_HEIGHT/2, PASSAGE_WIDTH/2 + y*NODE_WIDTH);
        }

        public static int[] GetCoordFromIndex(int idx){
            int x = idx % 16;
            int y = idx / 16;
            int[] coords = new int[]{x,y};
            return coords;
        }
    }

    public class Lattice{
        public int x_idx;
        public int y_idx;
        public float x_pos;
        public float y_pos;
        public int numWalls;
        public bool[] wallActive; //{North, South, East, West}
        public List<Node> latticeNodes;

        public Lattice(int x, int y){
            x_idx = x;
            y_idx = y;

            Vector3 pos = GetLatticePosition(x,y);
            x_pos = pos.x;
            y_pos = pos.y;

            numWalls = 0;

            wallActive = new bool[]{false, false, false, false};
        }

        public static Vector3 GetLatticePosition(int x, int y){
            return new Vector3(NODE_WIDTH*(x+1) - WALL_WIDTH/2, WALL_HEIGHT/2, NODE_WIDTH*(y+1) - WALL_WIDTH/2);
        }
    }

    public class Edge{
        public bool isWall;
        public bool vertical;
        public int x;
        public int y;
        public Node[] edgeNodes;
        public Lattice[] edgeLattices;
        public GameObject prefab;
        public GameObject gameObject;

        public Edge(Node n1, Node n2, int _x, int _y, bool wall, bool vert){
            edgeNodes = new Node[2];
            edgeNodes[0] = n1;
            edgeNodes[1] = n2;
            x = _x;
            y = _y;
            isWall = wall;
            vertical = vert;
        }

        public Edge(){
            edgeNodes = new Node[2];
        }

        public static Vector3 GetWallPosition(int x, int y, bool vertical){
            if(vertical){
                return new Vector3(NODE_WIDTH*(x+1) - WALL_WIDTH/2, WALL_HEIGHT/2, NODE_WIDTH*y + PASSAGE_WIDTH/2);
            }else{
                return new Vector3(NODE_WIDTH*x + PASSAGE_WIDTH/2, WALL_HEIGHT/2, NODE_WIDTH*(y+1) - WALL_WIDTH/2);
            }
        }

        //Only returns true if the nodes are immediate neighbors
        public bool IsValidEdge(){
            if(!edgeNodes[0].isValidNode() || !edgeNodes[1].isValidNode()){ return false; }

            if((edgeNodes[0].x == edgeNodes[1].x && (edgeNodes[0].y == edgeNodes[1].y + 1 || edgeNodes[0].y == edgeNodes[1].y - 1)) || 
               (edgeNodes[0].y == edgeNodes[1].y && (edgeNodes[0].x == edgeNodes[1].x + 1 || edgeNodes[0].x == edgeNodes[1].x - 1))){
                return true;
            }

            return false;
        }

        //Returns -1 if the positions of the node does not match the position of either edge node, otherwise it returns the index of the given node
        public int NodeInEdge(Node node){
            if(edgeNodes[0].x == node.x || edgeNodes[0].y == node.y){
                return 0;
            }else if(edgeNodes[1].x == node.x || edgeNodes[1].y == node.y){
                return 1;
            }
            return -1;
        }

        //Returns true if the edge both contain the same nodes as edge e (in any order), otherwise returns false
        public bool CompareEdge(Edge e){
            if(NodeInEdge(e.edgeNodes[0]) == -1 || NodeInEdge(e.edgeNodes[1]) == -1)
                return false;
            return true;
        }
    }
}
