Imports System.Runtime.InteropServices
Imports DIH_Pro.Core

Public Class SplashForm

    <DllImport("kernel32.dll", SetLastError:=True, CharSet:=CharSet.Ansi, ExactSpelling:=True)>
    Public Shared Function GetProcAddress(ByVal hModule As IntPtr, ByVal procName As String) As UIntPtr
    End Function

    <DllImport("kernel32.dll", SetLastError:=True, CharSet:=CharSet.Ansi)>
    Public Shared Function LoadLibrary(ByVal lpFileName As String) As IntPtr
    End Function

    Dim ScreenSize As Size

    Private Sub SplashForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.ScreenSize = New Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height)
        Me.Location = New Point(Me.ScreenSize.Width + MyBase.Width, 30)
        StartGUI()
    End Sub

    Public Sub SetTopMost(ByVal Value As Boolean)
        Me.BeginInvoke(Sub()
                           Me.TopMost = Value
                       End Sub)
    End Sub

    Public Sub SetStatus(ByVal Value As String)
        Me.BeginInvoke(Sub()
                           Me.Label3.Text = Value
                       End Sub)
    End Sub

    Public Sub StartGUI()

        Guna2ShadowForm1.SetShadowForm(Me)
        Label2.Text = "v" & Core.Manage.Instances.FileVer

    End Sub

    Private Sub ShowBar()
        Dim num As Integer = 0
        While True
            Dim x As Integer = MyBase.Location.X
            Dim flag As Boolean = x > Me.ScreenSize.Width - MyBase.Width
            If Not flag Then
                Exit While
            End If
            MyBase.Location = New Point(MyBase.Location.X - 1, MyBase.Location.Y)
            Application.DoEvents()
            num -= 1
            num += 1
            If num > 2 Then
                GoTo IL_9C
            End If
        End While
        MyBase.Location = New Point(Me.ScreenSize.Width - MyBase.Width, MyBase.Location.Y)
IL_9C:
        Me.Refresh()
    End Sub

    Private Delegate Sub LoadCompletedEventHandler()
    Private Event LoadCompleted As LoadCompletedEventHandler

    Public Sub New()
        InitializeComponent()
        AddHandler LoadCompleted, AddressOf StartupForm_LoadCompleted
    End Sub

    Private Delegate Sub InvokeMethod()

    Private Sub StartupForm_LoadCompleted()
        Dim startup As Task = Task.Run(Sub() Core.Model.Startup.Start(Guna2ProgressBar1, Label3))
        Application.DoEvents()

        Me.Refresh()

        Task.Run(Sub()
                     startup.Wait()

                     Me.BeginInvoke(Sub()
                                        Label3.Text = "Loading Module Symbols..."
                                        Me.Refresh()
                                        Core.Helpers.Utils.Sleep(500, Core.Helpers.Utils.Measure.Milliseconds)
                                    End Sub)


                     Me.BeginInvoke(Sub()
                                        Core.Manage.Remix_Injector.Start()
                                    End Sub)

                     Dim close As InvokeMethod = Sub()
                                                     Me.Close()
                                                 End Sub

                     Invoke(close)
                 End Sub)
    End Sub

    Private Sub StartupForm_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Application.DoEvents()
        Me.Refresh()
        ShowBar()
        RaiseEvent LoadCompleted()
    End Sub

End Class