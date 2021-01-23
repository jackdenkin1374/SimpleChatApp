using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn;
using Yarn.Unity;

public class HotkeysManager : MonoBehaviour
{
    [SerializeField]
    private GameObject menu;
    [SerializeField]
    private DialogueRunner dr;
    [SerializeField]
    private NPC npc;

    // Update is called once per frame
    void Update()
    {
        if(menu.activeSelf == true && Input.GetKeyDown(KeyCode.P))
            menu.SetActive(false);
        else if(Input.GetKeyDown(KeyCode.P))
            menu.SetActive(true);
        // The physics2D detection need testing 
        if(Input.GetMouseButtonDown(0) && !dr.IsDialogueRunning && 
            Physics2D.OverlapPoint(UtilsClass.GetMouseWorldPosition()) == null){
            Debug.Log("Start dialogue from click");
            dr.StartDialogue(npc.talkToNode);
        }
        if(Input.GetKeyDown(KeyCode.Space)){
            Debug.Log("Start dialogue");
            dr.StartDialogue(npc.talkToNode);
        }
        //if(isDialogRunning && Input Mouse Left Click && options not up?)
            //yarncommand continue dialog / skip autocomplete / go to next line
    }
}
