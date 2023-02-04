Imports System
Imports System.Runtime.InteropServices
Imports System.Windows
Imports System.Windows.Forms
Imports System.Windows.Interop

Namespace BiliUPDesktopTool
    Friend Class DesktopEmbeddedWindowHelper

        Public Shared Sub DesktopEmbedWindow(ByVal hWndC As IntPtr)
            Dim pWndA = GetDesktopIconHandle()
            If pWndA <> IntPtr.Zero Then
                WinAPIHelper.SetParent(hWndC, pWndA)
            Else
                System.Windows.Forms.MessageBox.Show("嵌入桌面失败！")
            End If
        End Sub

        Public Shared Sub DesktopEmbedWindow(ByVal window As Window)
            Dim pWndA = GetDesktopIconHandle()
            If pWndA <> IntPtr.Zero Then
                WinAPIHelper.SetParent(New WindowInteropHelper(window).Handle, pWndA)
            Else
                System.Windows.Forms.MessageBox.Show("嵌入桌面失败！")
            End If
        End Sub

        Public Shared Sub DesktopEmbedWindow(ByVal window As Form)

            Dim pWndA = GetDesktopIconHandle()

            If pWndA <> IntPtr.Zero Then
                WinAPIHelper.SetParent(window.Handle, pWndA)
                window.Show()
            Else
                System.Windows.Forms.MessageBox.Show("嵌入桌面失败！")
            End If
        End Sub



        Private Declare Function FindWindowEx Lib "user32" Alias "FindWindowExA" (ByVal hWnd1 As Integer, ByVal hWnd2 As Integer, ByVal lpsz1 As String, ByVal lpsz2 As String) As Integer
        Private Declare Function GetDesktopWindow Lib "user32" Alias "GetDesktopWindow" () As IntPtr

        Public Shared Function GetDesktopIconHandle() As IntPtr
            '获取物理系统桌面图标的句柄，用于嵌入实现置后显示
            Dim HandleDesktop As Integer = GetDesktopWindow
            Dim HandleTop As Integer = 0
            Dim LastHandleTop As Integer = 0
            Dim HandleSHELLDLL_DefView As Integer = 0
            Dim HandleSysListView32 As Integer = 0

            '在WorkerW结构里搜索
            Do Until HandleSysListView32 > 0
                HandleTop = FindWindowEx(HandleDesktop, LastHandleTop, "WorkerW", vbNullString)
                HandleSHELLDLL_DefView = FindWindowEx(HandleTop, 0, "SHELLDLL_DefView", vbNullString)
                If HandleSHELLDLL_DefView > 0 Then HandleSysListView32 = FindWindowEx(HandleSHELLDLL_DefView, 0, "SysListView32", "FolderView")
                LastHandleTop = HandleTop
                If LastHandleTop = 0 Then Exit Do
            Loop

            '如果找到了，立即返回
            If HandleSysListView32 > 0 Then Return HandleSysListView32

            '未找到，则在Progman里搜索(用于兼容WinXP系统)
            Do Until HandleSysListView32 > 0
                HandleTop = FindWindowEx(HandleDesktop, LastHandleTop, "Progman", "Program Manager")
                HandleSHELLDLL_DefView = FindWindowEx(HandleTop, 0, "SHELLDLL_DefView", vbNullString)
                If HandleSHELLDLL_DefView > 0 Then HandleSysListView32 = FindWindowEx(HandleSHELLDLL_DefView, 0, "SysListView32", "FolderView")
                LastHandleTop = HandleTop
                If LastHandleTop = 0 Then Exit Do : Return 0
            Loop
            Return HandleSysListView32
        End Function


    End Class
End Namespace
