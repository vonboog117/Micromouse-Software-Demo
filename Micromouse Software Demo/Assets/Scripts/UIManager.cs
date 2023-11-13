using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public GameObject textPrefab;

    public GameObject MazeCanvas;
    public Button FindButton;
    public Button ReturnButton;
    public Button RaceButton;

    private static GameObject[,] nodeTexts = new GameObject[16,16];
    private bool textActive = false;
    private bool isManual = false;
    //private static int count = 0;

    void Start(){
        InstantiateText();
    }

    private void InstantiateText(){
        for(int x = 0; x < 16; x++){
            for(int y = 0; y < 16; y++){
                Vector3 unitPos = MazeManager.Node.GetUnitPosition(x,y);
                nodeTexts[x,y] = Instantiate(textPrefab, unitPos, Quaternion.Euler(90,0,0), MazeCanvas.transform);
            }
        }
    }

    public static void UpdateText(int[,] values){
        if(values.GetLength(0) != 16 || values.GetLength(1) != 16){return;}

        //count++;
        //Debug.Log(count);
        for(int x = 0; x < 16; x++){
            for(int y = 0; y < 16; y++){
                //nodeTexts[x,y].GetComponent<TextMeshProUGUI>().SetText(count.ToString());
                //Debug.Log(x + " " + y + " " +values[x,y]);
                nodeTexts[x,y].GetComponent<TextMeshProUGUI>().SetText(values[x,y].ToString());
            }
        }
    }

    public static void UpdateText(int x, int y, int values){
        nodeTexts[x,y].GetComponent<TextMeshProUGUI>().SetText(values.ToString());
    }

    public void ToggleTextActive(){
        textActive = !textActive;

        for(int x = 0; x < 16; x++){
            for(int y = 0; y < 16; y++){
                nodeTexts[x,y].SetActive(textActive);
            }
        }
    }

    public void ChangeStateButtons(int state){
        if(state == 0){ //Manual
            FindButton.interactable = isManual;
            ReturnButton.interactable = isManual;
            RaceButton.interactable = isManual;
            isManual = !isManual;
        }else if(state == 1){ //Find
            FindButton.interactable = true;
            ReturnButton.interactable = false;
            RaceButton.interactable = false;
        }else if(state == 2){ //Return
            FindButton.interactable = false;
            ReturnButton.interactable = true;
            RaceButton.interactable = false;
        }else if(state == 3){ //Race
            FindButton.interactable = false;
            ReturnButton.interactable = false;
            RaceButton.interactable = true;
        }
    }

}
