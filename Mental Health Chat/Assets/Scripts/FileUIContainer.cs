using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using Yarn;
using Yarn.Unity;
using Yarn.Compiler;

public class FileUIContainer : MonoBehaviour
{
    public TextMeshProUGUI fileNameText;
    public FileInfo file;
    public TextAsset test;
    EditTextFile loadManager;
    NPC npc;

    void Start()
    {
        loadManager = EditTextFile.Instance;
    }

    // Method is to be used with EditTextFile.cs in order to load files to edit
    // Can probably be simplify in regards with the file and whatever file.* it has 
    public void LoadFileToEdit(){
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

    // Method is to be used with GameManager.cs in order to load files for the DialogueRunner
    // Currently I don't know how to use this CompileFile method, ask the Yarn Spinner Discord on how to load Yarn
    //  file at runtime to allow custom dialogues by users.
    public void LoadFileToRun(){
        string txtDocumentName = Application.streamingAssetsPath + "/Char_Dialogues/" + file.Name;
        Debug.Log("Loading " + file.Name + " in " + txtDocumentName);
        // YarnProgram script1;
        Yarn.Program script;
        IDictionary<string, StringInfo> a;
        
        // This method returns the Program, which contains nodes and
        //  returns the strings
        Yarn.Compiler.Compiler.CompileFile(txtDocumentName, out script, out a);
        // Debug.Log(a.Count);
        // Debug.Log(a.Values.Count);
        // Debug.Log(a.Keys.Count);
        foreach(string k in a.Keys){
            print(k);
        }
        foreach(StringInfo k in a.Values){
            print(k.text);
        }

        // This is able to add the strings but is unable to add the Program object
        //  I do not see a method that adds in the Program object
        //  Get the fucking devs to answer the question of how to add in the fucknig nodes.
        if (a != null) {
            DialogueRunner dialogueRunner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
            // dialogueRunner.AddStringTable(script);
            dialogueRunner.AddStringTable(a);  
            print(dialogueRunner.CurrentNodeName); 
            print(dialogueRunner.startNode); 
            print(dialogueRunner.NodeExists("Shina"));
        }
        
        // Somehow a null reference when sending in the strings through the method.
        // npc.LoadScript2(a);
    }

    public void SetItem(FileInfo file){
        this.file = file;
        fileNameText.SetText(file.Name);
    }
}
