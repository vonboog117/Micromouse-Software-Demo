using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManager : MonoBehaviour
{
    public class Node{
        public int x;
        public int y;

        public Node(int _x, int _y){
            x = _x;
            y = _y;
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

    public class Edge{
        public Node[] nodes;

        public Edge(Node n1, Node n2){
            nodes = new Node[2];
            nodes[0] = n1;
            nodes[1] = n2;
        }

        public Edge(){
            nodes = new Node[2];
        }

        //Only returns true if the nodes are immediate neighbors
        public bool isValidEdge(){
            if(!nodes[0].isValidNode() || !nodes[1].isValidNode()){ return false; }

            if((nodes[0].x == nodes[1].x && (nodes[0].y == nodes[1].y + 1 || nodes[0].y == nodes[1].y - 1)) || 
               (nodes[0].y == nodes[1].y && (nodes[0].x == nodes[1].x + 1 || nodes[0].x == nodes[1].x - 1))){
                return true;
            }

            return false;
        }

        //Returns 0 if the edge both contain the same nodes as edge e (in any order), otherwise returns 1
        public int compareEdge(Edge e){
            //If the first node in the edge is not in edge e they are not the same
            if((nodes[0].x != e.nodes[0].x || nodes[0].y != e.nodes[0].y) &&
               (nodes[0].x != e.nodes[1].x || nodes[0].y != e.nodes[1].y)){
                return 1;
            }

            if((nodes[1].x != e.nodes[0].x || nodes[1].y != e.nodes[0].y) &&
               (nodes[1].x != e.nodes[1].x || nodes[1].y != e.nodes[1].y)){
                return 1;
            }

            return 0;
        }
    }

    //private Node[,] units;

    void Start(){
        //units = new Node[16,16];

    }

    void Update()
    {
        
    }
}
