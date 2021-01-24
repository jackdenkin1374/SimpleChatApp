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

    void Start(){
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
    public void LoadFileToRun(){
        string txtDocumentName = Application.streamingAssetsPath + "/Char_Dialogues/" + file.Name;
        Debug.Log("Loading " + file.Name + " in " + txtDocumentName);
        Yarn.Program script;
        IDictionary<string, StringInfo> dialogueStrings;
        
        // This method returns the Program, which contains nodes and returns the strings
        Yarn.Compiler.Compiler.CompileFile(txtDocumentName, out script, out dialogueStrings);

        #region prints for Program and StringInfo
        // Debug.Log(a.Count);
        // print("StringInfo");
        // foreach(string k in a.Keys){
        //     print(k);
        // }
        // foreach(StringInfo k in a.Values){
        //     print(k.text);
        // }

        // print("Program");
        // print(script);
        // print(script.Nodes.Count);
        // foreach(string k in script.Nodes.Keys){
        //     print(k);
        // }
        // foreach(Yarn.Node k in script.Nodes.Values){
        //     print(k);
        // }
        #endregion

        if (dialogueStrings != null && script != null) {
            DialogueRunner dialogueRunner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
            dialogueRunner.AddStringTable(dialogueStrings); 
            dialogueRunner.Dialogue.AddProgram(script); 
            // print(dialogueRunner.CurrentNodeName); 
            // print(dialogueRunner.startNode); 
            // print(dialogueRunner.NodeExists("Shina"));
        }
        
        // Somehow a null reference when sending in the strings through the method.
        // npc.LoadScript2(a);
    }

    public void SetItem(FileInfo file){
        this.file = file;
        fileNameText.SetText(file.Name);
    }
}
