Imports System
Imports System.Runtime.InteropServices

Namespace BiliUPDesktopTool
    Module WinAPIHelper

        <System.Runtime.InteropServices.DllImport("gdi32.dll")>
        Function DeleteObject(ByVal hObject As IntPtr) As Boolean
        End Function

        <DllImport("user32.dll", CharSet:=CharSet.Auto)>
        Function FindWindow(
        <MarshalAs(UnmanagedType.LPTStr)> ByVal lpClassName As String,
        <MarshalAs(UnmanagedType.LPTStr)> ByVal lpWindowName As String) As IntPtr
        End Function

        <DllImport("user32")>
        Function FindWindowEx(ByVal hWnd1 As IntPtr, ByVal hWnd2 As IntPtr, ByVal lpsz1 As String, ByVal lpsz2 As String) As IntPtr
        End Function

        <DllImport("user32.dll", EntryPoint:="SendMessageA")>
        Function SendMessage(ByVal hwnd As IntPtr, ByVal wMsg As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
        End Function

        <DllImport("User32.dll")>
        Function SetForegroundWindow(ByVal hWnd As IntPtr) As Boolean
        End Function

        <DllImport("user32.dll")>
        Function SetParent(ByVal hWndChild As IntPtr, ByVal hWndNewParent As IntPtr) As IntPtr
        End Function

        <DllImport("user32.dll")>
        Function SetWindowPos(ByVal hWnd As IntPtr, ByVal hWndlnsertAfter As IntPtr, ByVal X As Integer, ByVal Y As Integer, ByVal cx As Integer, ByVal cy As Integer, ByVal Flags As UInteger) As Boolean
        End Function

        <DllImport("User32.dll")>
        Function ShowWindow(ByVal hWnd As IntPtr, ByVal cmdShow As Integer) As Boolean
        End Function

        <DllImport("kernel32.dll")>
        Function SetProcessWorkingSetSize(ByVal process As IntPtr, ByVal minSize As Integer, ByVal maxSize As Integer) As Boolean
        End Function

        <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
        Function SendMessageTimeout(ByVal hWnd As IntPtr, ByVal Msg As UInteger, ByVal wParam As UIntPtr, ByVal lParam As IntPtr, ByVal fuFlags As SendMessageTimeoutFlags, ByVal uTimeout As UInteger, <Out> ByRef lpdwResult As UIntPtr) As IntPtr
        End Function

        <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
        Function SendMessageTimeout(ByVal windowHandle As IntPtr, ByVal Msg As UInteger, ByVal wParam As IntPtr, ByVal lParam As IntPtr, ByVal flags As SendMessageTimeoutFlags, ByVal timeout As UInteger, <Out> ByRef result As IntPtr) As IntPtr
        End Function

        <DllImport("user32.dll", EntryPoint:="SendMessageTimeout", SetLastError:=True, CharSet:=CharSet.Auto)>
        Function SendMessageTimeoutText(ByVal hWnd As IntPtr, ByVal Msg As Integer, ByVal countOfChars As Integer, ByVal text As Text.StringBuilder, ByVal flags As SendMessageTimeoutFlags, ByVal uTImeoutj As UInteger, <Out> ByRef result As IntPtr) As UInteger
        End Function

        <Flags()>
        Public Enum SendMessageTimeoutFlags
            SMTO_NORMAL = 0
            SMTO_BLOCK = 1
            SMTO_ABORTIFHUNG = 2
            SMTO_NOTIMEOUTIFNOTHUNG = 8
            SMTO_ERRORONEXIT = 32
        End Enum

    End Module
End Namespace
