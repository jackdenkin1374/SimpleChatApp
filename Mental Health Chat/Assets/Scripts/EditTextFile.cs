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
        fileContainer = Resources.Load<FileUIContainer>("Prefab/File_JustText");
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
