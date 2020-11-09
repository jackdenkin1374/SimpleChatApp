﻿using System;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
     
[RequireComponent (typeof (Camera))]
public class TransparentWindowRaycast : MonoBehaviour
{
    [SerializeField]
    private Material m_Material;
    
    [SerializeField]
    private Camera mainCamera;
    
    private bool clickThrough = true;
    private bool prevClickThrough = true;
    
    private struct MARGINS
    {
        public int cxLeftWidth;
        public int cxRightWidth;
        public int cyTopHeight;
        public int cyBottomHeight;
    }
    
    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();
    
    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);
    
    [DllImport("user32.dll")]
    static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);
    
    [DllImport("user32.dll", EntryPoint = "SetLayeredWindowAttributes")]
    static extern int SetLayeredWindowAttributes(IntPtr hwnd, int crKey, byte bAlpha, int dwFlags);
    
    [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
    private static extern int SetWindowPos(IntPtr hwnd, int hwndInsertAfter, int x, int y, int cx, int cy, int uFlags);
    
    [DllImport("Dwmapi.dll")]
    private static extern uint DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS margins);
    
    const int GWL_STYLE = -16;
    const uint WS_POPUP = 0x80000000;
    const uint WS_VISIBLE = 0x10000000;
    const int HWND_TOPMOST = -1;
    
    int fWidth;
    int fHeight;
    IntPtr hwnd;
    MARGINS margins;
    
    void Start()
    {
        mainCamera = GetComponent<Camera> ();
    
        #if !UNITY_EDITOR // You really don't want to enable this in the editor..
    
        fWidth = Screen.width;
        fHeight = Screen.height;
        margins = new MARGINS() { cxLeftWidth = -1 };
        hwnd = GetActiveWindow();
    
        SetWindowLong(hwnd, GWL_STYLE, WS_POPUP | WS_VISIBLE);
        SetWindowPos(hwnd, HWND_TOPMOST, 0, 0, fWidth, fHeight, 32 | 64); //SWP_FRAMECHANGED = 0x0020 (32); //SWP_SHOWWINDOW = 0x0040 (64)
        DwmExtendFrameIntoClientArea(hwnd, ref margins);
    
        Application.runInBackground = true;
        #endif
    }
    
    void Update ()
    {
        // If our mouse is overlapping an object
        RaycastHit hit = new RaycastHit();
        clickThrough = !Physics.Raycast (mainCamera.ScreenPointToRay (Input.mousePosition).origin,
                mainCamera.ScreenPointToRay (Input.mousePosition).direction, out hit, 100,
                Physics.DefaultRaycastLayers)&&!OverUI();
    
        if (clickThrough != prevClickThrough) {
            if (clickThrough) {
                #if !UNITY_EDITOR
                SetWindowLong(hwnd, GWL_STYLE, WS_POPUP | WS_VISIBLE);
                SetWindowLong (hwnd, -20, (uint)524288 | (uint)32);//GWL_EXSTYLE=-20; WS_EX_LAYERED=524288=&h80000, WS_EX_TRANSPARENT=32=0x00000020L
                SetLayeredWindowAttributes (hwnd, 0, 255, 2);// Transparency=51=20%, LWA_ALPHA=2
                SetWindowPos(hwnd, HWND_TOPMOST, 0, 0, fWidth, fHeight, 32 | 64); //SWP_FRAMECHANGED = 0x0020 (32); //SWP_SHOWWINDOW = 0x0040 (64)
                #endif
            } else {
                #if !UNITY_EDITOR
                SetWindowLong (hwnd, -20, ~(((uint)524288) | ((uint)32)));//GWL_EXSTYLE=-20; WS_EX_LAYERED=524288=&h80000, WS_EX_TRANSPARENT=32=0x00000020L
                SetWindowPos(hwnd, HWND_TOPMOST, 0, 0, fWidth, fHeight, 32 | 64); //SWP_FRAMECHANGED = 0x0020 (32); //SWP_SHOWWINDOW = 0x0040 (64)
                #endif
            }
            prevClickThrough = clickThrough;
        }
    }
    
    void OnRenderImage(RenderTexture from, RenderTexture to)
    {
        Graphics.Blit(from, to, m_Material);
    }

    public bool OverUI() { //Use sparingly
        //Set up the new Pointer Event
        PointerEventData m_PointerEventData = new PointerEventData(EventSystem.current);
        m_PointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(m_PointerEventData, results);
        if (results.Count > 0) return true;
        return false;
    }

}
