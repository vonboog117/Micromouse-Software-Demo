using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManager : MonoBehaviour
{
    public class Node{
        public bool[] edges;
        public int x_idx;
        public int y_idx;

        public Node(bool n_edge, bool w_edge, bool s_edge, bool e_edge, int x, int y){
            edges = new bool[4];
            edges[0] = n_edge;
            edges[1] = w_edge;
            edges[2] = s_edge;
            edges[3] = e_edge;

            x_idx = x;
            y_idx = y;
        }

        public Node(){
            edges = new bool[4];
            edges[0] = false;
            edges[1] = false;
            edges[2] = false;
            edges[3] = false;

            x_idx = 0;
            y_idx = 0;
        }
    }

    private Node[,] units;

    void Start(){
        units = new Node[16,16];
    }

    void Update()
    {
        
    }
}
