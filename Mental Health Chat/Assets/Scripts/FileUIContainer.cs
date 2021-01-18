using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class FileUIContainer : MonoBehaviour
{
    public TextMeshProUGUI fileNameText;
    public FileInfo file;
    public TextAsset test;
    EditTextFile loadManager;

    void Start()
    {
        loadManager = EditTextFile.Instance;
    }

    public void loadFile(){
        
        string txtDocumentName = Application.streamingAssetsPath + "/Char_Dialogues/" + file.Name;
        StreamReader sr = new StreamReader(txtDocumentName);
        string fileContent = sr.ReadToEnd();
        sr.Close();

        Debug.Log("Loading " + file.Name + " in " + txtDocumentName);
        Debug.Log(fileContent);

        loadManager.inputFieldFileName.text = Path.GetFileNameWithoutExtension(file.Name);
        loadManager.inputFieldChat.text = fileContent;
        loadManager.SetEditTextActive();
    }

    public void SetItem(FileInfo file){
        this.file = file;
        fileNameText.SetText(file.Name);
    }
}
