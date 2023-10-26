using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{

    private bool[,] vertWallFound = new bool[15,16];
    private bool[,] horWallFound = new bool[16,15];

    private Rigidbody rb;
    private Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        for(int i = 0; i < 15*16; i++){ 
            vertWallFound[i % 15, i / 15] = false; 
            horWallFound[i % 16, i / 16] = false;
        }
    }

    // Update is called once per frame
    void Update(){
        velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        Vector3 rayOrigin = transform.position;
        RaycastHit hit;
        int layerMask = 1 << 3;
        if(Physics.Raycast(rayOrigin, transform.forward, out hit, 0.15f, layerMask)){
            //Debug.Log(MazeManager.Edge.GetWallIndex(hit.collider.gameObject.transform.parent.position, hit.collider.gameObject.transform.parent.rotation));
            if(!hit.collider.gameObject.GetComponent<MeshRenderer>().enabled){
                Vector3 wallIndices = MazeManager.Edge.GetWallIndex(hit.collider.gameObject.transform.parent.position, hit.collider.gameObject.transform.parent.rotation);
                MazeManager.Maze.SetFound((int)wallIndices.x, (int)wallIndices.y, wallIndices.z == 1, true);
                GameObject wall = hit.collider.gameObject.transform.parent.gameObject;
                foreach(MeshRenderer renderer in wall.GetComponentsInChildren<MeshRenderer>()){
                    renderer.enabled = true;
                }
            }
        }
        if(Physics.Raycast(rayOrigin, transform.right, out hit, 0.15f, layerMask)){
            if(!hit.collider.gameObject.GetComponent<MeshRenderer>().enabled){
                Vector3 wallIndices = MazeManager.Edge.GetWallIndex(hit.collider.gameObject.transform.parent.position, hit.collider.gameObject.transform.parent.rotation);
                MazeManager.Maze.SetFound((int)wallIndices.x, (int)wallIndices.y, wallIndices.z == 1, true);
                GameObject wall = hit.collider.gameObject.transform.parent.gameObject;
                foreach(MeshRenderer renderer in wall.GetComponentsInChildren<MeshRenderer>()){
                    renderer.enabled = true;
                }
            }
        }
        if(Physics.Raycast(rayOrigin, -transform.right, out hit, 0.15f, layerMask)){
            if(!hit.collider.gameObject.GetComponent<MeshRenderer>().enabled){
                Vector3 wallIndices = MazeManager.Edge.GetWallIndex(hit.collider.gameObject.transform.parent.position, hit.collider.gameObject.transform.parent.rotation);
                MazeManager.Maze.SetFound((int)wallIndices.x, (int)wallIndices.y, wallIndices.z == 1, true);
                GameObject wall = hit.collider.gameObject.transform.parent.gameObject;
                foreach(MeshRenderer renderer in wall.GetComponentsInChildren<MeshRenderer>()){
                    renderer.enabled = true;
                }
            }
        }
        if(Physics.Raycast(rayOrigin, -transform.forward, out hit, 0.15f, layerMask)){
            if(!hit.collider.gameObject.GetComponent<MeshRenderer>().enabled){
                Vector3 wallIndices = MazeManager.Edge.GetWallIndex(hit.collider.gameObject.transform.parent.position, hit.collider.gameObject.transform.parent.rotation);
                MazeManager.Maze.SetFound((int)wallIndices.x, (int)wallIndices.y, wallIndices.z == 1, true);
                GameObject wall = hit.collider.gameObject.transform.parent.gameObject;
                foreach(MeshRenderer renderer in wall.GetComponentsInChildren<MeshRenderer>()){
                    renderer.enabled = true;
                }
            }
        }
    }

    void FixedUpdate(){
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }

    // private void OnTriggerEnter(Collider other){
    //     if(other.tag == "Wall"){
    //         Debug.Log("Wall");
    //     }
    // }
}
