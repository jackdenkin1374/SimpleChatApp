using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotkeysManager : MonoBehaviour
{
    [SerializeField]
    private GameObject menu;

    // Update is called once per frame
    void Update()
    {
        if(menu.activeSelf == true && Input.GetKeyDown(KeyCode.P))
            menu.SetActive(false);
        else if(Input.GetKeyDown(KeyCode.P))
            menu.SetActive(true);
    }
}
