using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }
    public RectTransform fileParentPanel;
    FileUIContainer fileContainer;
    private int reloadCount = 0;

    private void Start() {
        if(Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;   

        Directory.CreateDirectory(Application.streamingAssetsPath + "/Char_Dialogues/");
        fileContainer = Resources.Load<FileUIContainer>("Prefab/File_JustText_ToLoad");
    }

    /** 
    * If the files in game with the current game objects has a different count to files in the folder,
    *  it will delete the current game objects, else exit out of the method.
    * This is for when the user creates a new file in game or (Not tested) if the user creates a new file in the file explorer
    *  while the game is still running.
    */
    // See EditTextFile.LoadAvailableFiles for a note concerning similar methods.
    public void LoadAvailableFiles(){
        DirectoryInfo dir = new DirectoryInfo(Application.streamingAssetsPath + "/Char_Dialogues/");
        FileInfo[] info = dir.GetFiles("*.yarn");
        
        if(reloadCount == info.Length){
            return;
        }
        if(fileParentPanel.childCount > 0){
            Debug.Log("Reloading files");
            foreach(RectTransform child in fileParentPanel){
                GameObject.Destroy(child.gameObject);
            }
        }

        foreach(FileInfo f in info){
            print(f);
            FileUIContainer emptyFile = Instantiate(fileContainer);
            emptyFile.SetItem(f);
            emptyFile.transform.SetParent(fileParentPanel, false);
        }
        reloadCount = info.Length;
    }
}
