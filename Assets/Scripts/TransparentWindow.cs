﻿using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class TransparentWindow : MonoBehaviour
{
    [SerializeField]
    private Material m_Material;

    private struct MARGINS
    {
        public int cxLeftWidth;
        public int cxRightWidth;
        public int cyTopHeight;
        public int cyBottomHeight;
    }

    // Define function signatures to import from Windows APIs

    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

    [DllImport("Dwmapi.dll")]
    private static extern uint DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS margins);

    [DllImport("user32.dll")]
    private static extern int SetWindowPos(IntPtr hwnd, int hwndInsertAfter, int x, int y, int cx, int cy, int uFlags);


    // Definitions of window styles
    const int GWL_STYLE = -16;
    const uint WS_POPUP = 0x80000000;
    const uint WS_VISIBLE = 0x10000000;

    void Start()
    {
#if !UNITY_EDITOR
        var margins = new MARGINS() { cxLeftWidth = -1 };

        // Get a handle to the window
        var hwnd = GetActiveWindow();

        // Set properties of the window
        // See: https://msdn.microsoft.com/en-us/library/windows/desktop/ms633591%28v=vs.85%29.aspx
        SetWindowLong(hwnd, GWL_STYLE, WS_POPUP | WS_VISIBLE);

        // Collin Code
        SetWindowPos(hwnd, 0, 200, 200, 96, 96, 32 | 64);

    // Extend the window into the client area
    // See: https://msdn.microsoft.com/en-us/library/windows/desktop/aa969512%28v=vs.85%29.aspx 
        DwmExtendFrameIntoClientArea(hwnd, ref margins);
#endif
    }

    // Pass the output of the camera to the custom material
    // for chroma replacement
    void OnRenderImage(RenderTexture from, RenderTexture to)
    {
        Graphics.Blit(from, to, m_Material);
    }
}