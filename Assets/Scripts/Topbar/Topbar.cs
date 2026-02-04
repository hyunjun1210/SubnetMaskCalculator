using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class TopBar : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [DllImport("user32.dll")] private static extern IntPtr GetActiveWindow();
    [DllImport("user32.dll")] private static extern bool GetCursorPos(out POINT lpPoint);
    [DllImport("user32.dll")]
    private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter,
        int X, int Y, int cx, int cy, uint uFlags);
    [DllImport("user32.dll")] private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

    [DllImport("user32.dll")] private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    private const int SW_SHOWMINIMIZED = 2;

    private const uint SWP_NOSIZE = 0x0001;
    private const uint SWP_NOZORDER = 0x0004;

    private IntPtr hwnd;
    private bool isDragging = false;
    private Vector2 prevMousePos;

    [StructLayout(LayoutKind.Sequential)]
    private struct POINT
    {
        public int X;
        public int Y;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct RECT
    {
        public int Left, Top, Right, Bottom;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
#if !UNITY_EDITOR
        hwnd = GetActiveWindow();
        if (GetCursorPos(out POINT point))
        {
            prevMousePos = new Vector2(point.X, point.Y);
            isDragging = true;
        }
#endif
    }

    public void OnPointerUp(PointerEventData eventData)
    {
#if !UNITY_EDITOR
        isDragging = false;
#endif
    }

    public void OnDrag(PointerEventData eventData)
    {
#if !UNITY_EDITOR
        if (!isDragging) return;
        if (GetCursorPos(out POINT point))
        {
            Vector2 currentMousePos = new Vector2(point.X, point.Y);
            Vector2 delta = currentMousePos - prevMousePos;

            if (GetWindowRect(hwnd, out RECT rect))
            {
                int newX = rect.Left + (int)delta.x;
                int newY = rect.Top + (int)delta.y;

                SetWindowPos(hwnd, IntPtr.Zero, newX, newY, 0, 0, SWP_NOSIZE | SWP_NOZORDER);
                prevMousePos = currentMousePos;
            }
        }
#endif
    }

    public void OnMinimizeButton()
    {
#if !UNITY_EDITOR
    hwnd = GetActiveWindow();
    ShowWindow(hwnd, SW_SHOWMINIMIZED);
#endif
    }

    public void OnExitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
