using UnityEngine;
using System.Collections.Generic;
using Yarn.Unity;
using Yarn;
/// attached to the non-player characters, and stores the name of the Yarn
/// node that should be run when you talk to them.
public class NPC : MonoBehaviour {

    public static NPC Instance { get; set; }
    public string characterName = "";
    public string talkToNode = "";

    [Header("Optional")]
    public YarnProgram scriptToLoad;

    void Start () {
        if(Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
            
        // Loading in the YarnProgram loads in the nodes and strings into the dialogue runner
        if (scriptToLoad != null) {
            DialogueRunner dialogueRunner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
            dialogueRunner.Add(scriptToLoad);  
            // print(dialogueRunner.CurrentNodeName); 
            // print(dialogueRunner.startNode);      
            // print(dialogueRunner.NodeExists("Shina"));  
        }
    }

    public void LoadScript(YarnProgram scriptToLoad){
        if (scriptToLoad != null) {
            DialogueRunner dialogueRunner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
            dialogueRunner.Add(scriptToLoad);                
        }
    }

    public void LoadScript(Program program, IDictionary<string, Yarn.Compiler.StringInfo> stringInfo){
        if (scriptToLoad != null) {
            DialogueRunner dialogueRunner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
            dialogueRunner.AddStringTable(stringInfo);          
            dialogueRunner.Dialogue.AddProgram(program);       
        }
    }
}