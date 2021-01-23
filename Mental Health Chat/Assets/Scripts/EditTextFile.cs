using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class EditTextFile : MonoBehaviour
{
    // public InputField inputFieldChat;
    public static EditTextFile Instance { get; set; }
    public TMP_InputField inputFieldChat, inputFieldFileName;
    public TextMeshProUGUI createButton;
    public RectTransform fileParentPanel, editTextParentPanel, loadFilePP;
    FileUIContainer fileContainer;
    private int reloadCount = 0;

    private void Start() {
        if(Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;   

        Directory.CreateDirectory(Application.streamingAssetsPath + "/Char_Dialogues/");
        fileContainer = Resources.Load<FileUIContainer>("Prefab/File_JustText_ToEdit");
    }

    public void CreateTextFile(){
        if(inputFieldChat.text == "" && inputFieldFileName.text == "")
            return;

        // string txtDocumentName = Application.streamingAssetsPath + "/Char_Dialogues/" + "test" + ".yarn";
        string txtDocumentName = Application.streamingAssetsPath + "/Char_Dialogues/" + inputFieldFileName.text + ".yarn";

        // if(!File.Exists(txtDocumentName))
        //     File.WriteAllText(txtDocumentName, "");
        //     // File.WriteAllText(txtDocumentName, "title: \ntags: \n---\n");

        // File.AppendAllText(txtDocumentName, inputFieldChat.text + "\n===");
        File.WriteAllText(txtDocumentName, inputFieldChat.text);
    }

    public void Clear(){
        inputFieldFileName.text = "";
        inputFieldChat.text = "";
    }

    /** 
    * If the files in game with the current game objects has a different count to files in the folder,
    *  it will delete the current game objects, else exit out of the method.
    * This is for when the user creates a new file in game or (Not tested) if the user creates a new file in the file explorer
    *  while the game is still running.
    */
    // GameManager.cs has the same method, only difference is the fileContainer and fileParentPanel
    //  There may be a way to just make GameManager be the only file to have this method and assign whatever prefab
    //  and content panel to load the files.
    // Button does not allow more than 1 parameter to change, either look for a advance button or some smart way to do this.
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

    public void SetEditTextActive(){
        editTextParentPanel.gameObject.SetActive(true);
        loadFilePP.gameObject.SetActive(false);
        createButton.text = "Edit";
    }
}
