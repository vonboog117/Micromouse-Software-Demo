using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManager : MonoBehaviour
{

    public const float WALL_HEIGHT = 0.05f;
    public const float WALL_WIDTH = 0.012f;
    public const float PASSAGE_WIDTH = 0.168f;
    public const float NODE_WIDTH = 0.18f;

    //private Node[,] units;
    //private Lattice[,] lattices;
    //private Edge[,] edges;

    private Node[] units = {new Node(0,0,true,false), new Node(0,1,false,false), new Node(1,0,false,false), new Node(1,1,false,true)};
    private Lattice[] lattices;
    private Edge[] edges;

    void Start(){
        //units = new Node[16,16];

    }

    void Update()
    {
        
    }

    void OnDrawGizmos(){
        Vector3 nodeSize = new Vector3(PASSAGE_WIDTH, WALL_HEIGHT, PASSAGE_WIDTH);
        Vector3 latticeSize = new Vector3(WALL_WIDTH, WALL_HEIGHT, WALL_WIDTH);
        Vector3 wallSizeA = new Vector3(WALL_WIDTH, WALL_HEIGHT, PASSAGE_WIDTH);
        Vector3 wallSizeB = new Vector3(PASSAGE_WIDTH, WALL_HEIGHT, WALL_WIDTH);

        /*
        if(units != null){
            Vector3 nodeAPos = new Vector3(PASSAGE_WIDTH/2 + units[0].x*PASSAGE_WIDTH, WALL_HEIGHT/2, PASSAGE_WIDTH/2 + units[0].y*PASSAGE_WIDTH);
            Vector3 nodeBPos = new Vector3(PASSAGE_WIDTH/2 + units[1].x*PASSAGE_WIDTH, WALL_HEIGHT/2, PASSAGE_WIDTH/2 + units[1].y*NODE_WIDTH);
            Vector3 nodeCPos = new Vector3(NODE_WIDTH/2 + units[2].x*PASSAGE_WIDTH, WALL_HEIGHT/2, PASSAGE_WIDTH/2 + units[2].y*PASSAGE_WIDTH);
            Vector3 nodeDPos = new Vector3(NODE_WIDTH/2 + units[3].x*PASSAGE_WIDTH, WALL_HEIGHT/2, PASSAGE_WIDTH/2 + units[3].y*NODE_WIDTH);

            Vector3 latticePos = new Vector3(NODE_WIDTH - WALL_WIDTH/2, WALL_HEIGHT/2, NODE_WIDTH - WALL_WIDTH/2);

            Vector3 wallPosA = new Vector3(NODE_WIDTH - WALL_WIDTH/2, WALL_HEIGHT/2, PASSAGE_WIDTH/2);
            Vector3 wallPosB = new Vector3(NODE_WIDTH - WALL_WIDTH/2, WALL_HEIGHT/2, NODE_WIDTH + PASSAGE_WIDTH/2);
            Vector3 wallPosC = new Vector3(PASSAGE_WIDTH/2, WALL_HEIGHT/2, NODE_WIDTH - WALL_WIDTH/2);
            Vector3 wallPosD = new Vector3(NODE_WIDTH + PASSAGE_WIDTH/2, WALL_HEIGHT/2, NODE_WIDTH - WALL_WIDTH/2);

            Vector3 nodeSizeB = new Vector3(NODE_WIDTH, WALL_HEIGHT, NODE_WIDTH);


            Gizmos.color = Color.black;
            Gizmos.DrawCube(nodeAPos, nodeSize);
            Gizmos.color = Color.white;
            Gizmos.DrawCube(nodeBPos, nodeSize);
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(nodeCPos, nodeSize);
            Gizmos.color = Color.green;
            Gizmos.DrawCube(nodeDPos, nodeSize);

            Gizmos.color = Color.red;
            Gizmos.DrawCube(latticePos, latticeSize);

            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(wallPosA, wallSizeA);
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(wallPosB, wallSizeA);
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(wallPosC, wallSizeB);
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(wallPosD, wallSizeB);
        }
        */

        /*
        Vector3 node1Pos = new Vector3(PASSAGE_WIDTH/2 + 0*NODE_WIDTH, WALL_HEIGHT/2, PASSAGE_WIDTH/2 + 0*PASSAGE_WIDTH);

        Vector3 node2Pos = new Vector3(PASSAGE_WIDTH/2 + 1*NODE_WIDTH, WALL_HEIGHT/2, PASSAGE_WIDTH/2 + 0*PASSAGE_WIDTH);
        Vector3 node3Pos = new Vector3(PASSAGE_WIDTH/2 + 2*NODE_WIDTH, WALL_HEIGHT/2, PASSAGE_WIDTH/2 + 0*PASSAGE_WIDTH);
        Vector3 node4Pos = new Vector3(PASSAGE_WIDTH/2 + 3*NODE_WIDTH, WALL_HEIGHT/2, PASSAGE_WIDTH/2 + 0*PASSAGE_WIDTH);

        Vector3 wallPosA = new Vector3(NODE_WIDTH*1 - WALL_WIDTH/2, WALL_HEIGHT/2, PASSAGE_WIDTH/2);
        Vector3 wallPosB = new Vector3(NODE_WIDTH*2 - WALL_WIDTH/2, WALL_HEIGHT/2, PASSAGE_WIDTH/2);
        Vector3 wallPosC = new Vector3(NODE_WIDTH*3 - WALL_WIDTH/2, WALL_HEIGHT/2, PASSAGE_WIDTH/2);

        Vector3 node5Pos = new Vector3(PASSAGE_WIDTH/2 + 0*NODE_WIDTH, WALL_HEIGHT/2, PASSAGE_WIDTH/2 + 1*NODE_WIDTH);
        Vector3 node6Pos = new Vector3(PASSAGE_WIDTH/2 + 0*NODE_WIDTH, WALL_HEIGHT/2, PASSAGE_WIDTH/2 + 2*NODE_WIDTH);
        Vector3 node7Pos = new Vector3(PASSAGE_WIDTH/2 + 0*NODE_WIDTH, WALL_HEIGHT/2, PASSAGE_WIDTH/2 + 3*NODE_WIDTH);

        Vector3 wallPosD = new Vector3(PASSAGE_WIDTH/2, WALL_HEIGHT/2, NODE_WIDTH*1 - WALL_WIDTH/2);
        Vector3 wallPosE = new Vector3(PASSAGE_WIDTH/2, WALL_HEIGHT/2, NODE_WIDTH*2 - WALL_WIDTH/2);
        Vector3 wallPosF = new Vector3(PASSAGE_WIDTH/2, WALL_HEIGHT/2, NODE_WIDTH*3 - WALL_WIDTH/2);

        Vector3 lattice1Pos = new Vector3(NODE_WIDTH*1 - WALL_WIDTH/2, WALL_HEIGHT/2, NODE_WIDTH*1 - WALL_WIDTH/2);
        Vector3 lattice2Pos = new Vector3(NODE_WIDTH*2 - WALL_WIDTH/2, WALL_HEIGHT/2, NODE_WIDTH*1 - WALL_WIDTH/2);
        Vector3 lattice3Pos = new Vector3(NODE_WIDTH*1 - WALL_WIDTH/2, WALL_HEIGHT/2, NODE_WIDTH*2 - WALL_WIDTH/2);

        Gizmos.color = Color.black;
        Gizmos.DrawCube(node1Pos, nodeSize);
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(node2Pos, nodeSize);
        Gizmos.color = Color.white;
        Gizmos.DrawCube(node3Pos, nodeSize);
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(node4Pos, nodeSize);

        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(wallPosA, wallSizeA);
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(wallPosB, wallSizeA);
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(wallPosC, wallSizeA);

        Gizmos.color = Color.blue;
        Gizmos.DrawCube(node5Pos, nodeSize);
        Gizmos.color = Color.white;
        Gizmos.DrawCube(node6Pos, nodeSize);
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(node7Pos, nodeSize);

        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(wallPosD, wallSizeB);
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(wallPosE, wallSizeB);
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(wallPosF, wallSizeB);

        Gizmos.color = Color.red;
        Gizmos.DrawCube(lattice1Pos, latticeSize);
        Gizmos.color = Color.red;
        Gizmos.DrawCube(lattice2Pos, latticeSize);
        Gizmos.color = Color.red;
        Gizmos.DrawCube(lattice3Pos, latticeSize);
        */
        
        for(int x = 0; x < 16; x++){
            for(int y = 0; y < 16; y++){
                Vector3 nodePos = GetUnitPosition(x, y);
                Vector3 wallAPos = GetWallPosition(x, y, true);
                Vector3 wallBPos = GetWallPosition(x, y, false);
                Vector3 latticePos = GetLatticePosition(x, y);

                if((x % 2 == 0 && y % 2 == 0) || (x % 2 == 1 && y % 2 == 1)){
                    Gizmos.color = Color.blue;
                }else{
                    Gizmos.color = Color.white;
                }
                //Gizmos.DrawCube(nodePos, nodeSize);

                if(x !=15){ 
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawCube(wallAPos, wallSizeA);
                }

                if(y != 15){
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawCube(wallBPos, wallSizeB);
                }

                if(x != 15 && y != 15){
                    Gizmos.color = Color.red;
                    Gizmos.DrawCube(latticePos, latticeSize);
                }

            }
        }
        
    }

    public Vector3 GetUnitPosition(int x, int y){
        return new Vector3(PASSAGE_WIDTH/2 + x*NODE_WIDTH, WALL_HEIGHT/2, PASSAGE_WIDTH/2 + y*NODE_WIDTH);
    }

    public Vector3 GetLatticePosition(int x, int y){
        return new Vector3(NODE_WIDTH*(x+1) - WALL_WIDTH/2, WALL_HEIGHT/2, NODE_WIDTH*(y+1) - WALL_WIDTH/2);
    }

    public Vector3 GetWallPosition(int x, int y, bool vertical){
        if(vertical){
            return new Vector3(NODE_WIDTH*(x+1) - WALL_WIDTH/2, WALL_HEIGHT/2, NODE_WIDTH*y + PASSAGE_WIDTH/2);
        }else{
            return new Vector3(NODE_WIDTH*x + PASSAGE_WIDTH/2, WALL_HEIGHT/2, NODE_WIDTH*(y+1) - WALL_WIDTH/2);
        }
    }

    public Lattice[] GetTwoLatticesFromNodes(Node nodeA, Node nodeB){
        return null; //Place holder
    }

    public class Node{
        public int x;
        public int y;
        public bool isStart;
        public bool isEnd;

        public Node(int _x, int _y, bool start, bool end){
            x = _x;
            y = _y;
            isStart = start;
            isEnd = end;
        }

        public Node(){
            x = -1;
            y = -1;
        }

        //Only returns true if the nodes x and y have been set
        public bool isValidNode(){
            if(x > -1 && y > -1){ return true; }
            return false;
        }
    }

    public class Lattice{
        public int x_idx;
        public int y_idx;
        public float x_pos;
        public float y_pos;
        public int numWalls;
        public List<Node> latticeNodes;

        public Lattice(){

        }
    }

    public class Edge{
        public bool isWall;
        public Node[] edgeNodes;
        public Lattice[] edgeLattices;

        public Edge(Node n1, Node n2, bool wall){
            edgeNodes = new Node[2];
            edgeNodes[0] = n1;
            edgeNodes[1] = n2;
            isWall = wall;
        }

        public Edge(){
            edgeNodes = new Node[2];
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

        //Returns false if the positions of the node does not match the position of either edge node, otherwise it returns true
        public bool NodeInEdge(Node node){
            if((edgeNodes[0].x != node.x || edgeNodes[0].y != node.y) &&
               (edgeNodes[1].x != node.x || edgeNodes[1].y != node.y)){
                return false;
            }
            return true;
        }

        //Returns true if the edge both contain the same nodes as edge e (in any order), otherwise returns false
        public bool CompareEdge(Edge e){
            if(!NodeInEdge(e.edgeNodes[0]) || !NodeInEdge(e.edgeNodes[1]))
                return false;
            return true;
        }
    }
}
