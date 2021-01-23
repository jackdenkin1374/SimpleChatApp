using System;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Not in use, does not work
[RequireComponent (typeof (Camera))]
public class TransparentWindowRaycast2 : MonoBehaviour {

    [SerializeField]
    private Material m_Material;

    [SerializeField]
    private Camera mainCamera;
    
    private bool clickThrough = true;
    private bool prevClickThrough = true;

    [DllImport("user32.dll")]
    public static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);

    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

    [DllImport("user32.dll", SetLastError = true)]
    static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    [DllImport("user32.dll")]
    static extern int SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);

    private struct MARGINS {
        public int cxLeftWidth;
        public int cxRightWidth;
        public int cyTopHeight;
        public int cyBottomHeight;
    }

    [DllImport("Dwmapi.dll")]
    private static extern uint DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS margins);

    const int GWL_STYLE = -20;
    const uint WS_POPUP = 0x00080000;
    const uint WS_VISIBLE = 0x00000020;
    const int GWL_EXSTYLE = -20;
    const uint WS_EX_LAYERED = 0x00080000;
    const uint WS_EX_TRANSPARENT = 0x00000020;
    static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
    const uint LWA_COLORKEY = 0x00000001;

    private IntPtr hWnd;

    #if !UNITY_EDITOR
    private void Start() {
        //MessageBox(new IntPtr(0), "Hello World!", "Hello Dialog", 0);

        hWnd = GetActiveWindow();

        MARGINS margins = new MARGINS { cxLeftWidth = -1 };
        DwmExtendFrameIntoClientArea(hWnd, ref margins);

        SetWindowLong(hWnd, GWL_EXSTYLE, WS_EX_LAYERED | WS_EX_TRANSPARENT);
        //SetLayeredWindowAttributes(hWnd, 0, 0, LWA_COLORKEY);

        SetWindowPos(hWnd, HWND_TOPMOST, 0, 0, 0, 0, 0);

        Application.runInBackground = true;
    }
    #endif
    
    // private void Update() {
    //     SetClickthrough(Physics2D.OverlapPoint(UtilsClass.GetMouseWorldPosition()) == null);
    // }
    

    // private void SetClickthrough(bool clickthrough) {
    //     if (clickthrough) {
    //         SetWindowLong(hWnd, GWL_EXSTYLE, WS_EX_LAYERED | WS_EX_TRANSPARENT);
    //     } else {
    //         SetWindowLong(hWnd, GWL_EXSTYLE, WS_EX_LAYERED);
    //     }
    // }

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
                SetWindowLong(hWnd, GWL_STYLE, WS_POPUP | WS_VISIBLE);
                // SetWindowLong (hWnd, -20, (uint)524288 | (uint)32);//GWL_EXSTYLE=-20; WS_EX_LAYERED=524288=&h80000, WS_EX_TRANSPARENT=32=0x00000020L
                SetLayeredWindowAttributes (hWnd, 0, 255, 2);// Transparency=51=20%, LWA_ALPHA=2
                // SetWindowPos(hWnd, HWND_TOPMOST, 0, 0, fWidth, fHeight, 32 | 64); //SWP_FRAMECHANGED = 0x0020 (32); //SWP_SHOWWINDOW = 0x0040 (64)
                SetWindowPos(hWnd, HWND_TOPMOST, 0, 0, 0, 0, 0);
                #endif
            } else {
                #if !UNITY_EDITOR
                // SetWindowLong (hWnd, -20, ~(((uint)524288) | ((uint)32)));//GWL_EXSTYLE=-20; WS_EX_LAYERED=524288=&h80000, WS_EX_TRANSPARENT=32=0x00000020L
                SetWindowLong (hWnd, -20, WS_POPUP | WS_VISIBLE);
                SetWindowLong (hWnd, -20, WS_POPUP);
                // SetWindowPos(hWnd, HWND_TOPMOST, 0, 0, fWidth, fHeight, 32 | 64); //SWP_FRAMECHANGED = 0x0020 (32); //SWP_SHOWWINDOW = 0x0040 (64)
                SetWindowPos(hWnd, HWND_TOPMOST, 0, 0, 0, 0, 0);
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
