Imports System.Web.Script.Serialization
Imports ProcessHacker

Public Class WallConfig


#Region " Properties "

    Private ManifestData As Core.Engine.WallpaperJsonLoader.WallpaperInfo = Nothing
    Public Property Wallpaper As Core.Engine.WallpaperJsonLoader.WallpaperInfo
        <DebuggerStepThrough>
        Get
            Return Me.ManifestData
        End Get
        Set(value As Core.Engine.WallpaperJsonLoader.WallpaperInfo)
            ManifestData = value
        End Set
    End Property

    Public Property EmbedControl As Form = Nothing
    Public Property EmbedProcess As Process = Nothing
    Private phandle As Native.Objects.ProcessHandle = Nothing

    Public Property YoutubeID As String = String.Empty

#End Region

#Region " No Windows Focus "

    Private Const SW_SHOWNOACTIVATE As Integer = 4
    Private Const HWND_TOPMOST As Integer = -1
    Private Const SWP_NOACTIVATE As UInteger = &H10

    <System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint:="SetWindowPos")>
    Private Shared Function SetWindowPos(ByVal hWnd As Integer, ByVal hWndInsertAfter As Integer, ByVal X As Integer, ByVal Y As Integer, ByVal cx As Integer, ByVal cy As Integer, ByVal uFlags As UInteger) As Boolean
    End Function

    <System.Runtime.InteropServices.DllImport("user32.dll")>
    Private Shared Function ShowWindow(ByVal hWnd As System.IntPtr, ByVal nCmdShow As Integer) As Boolean
    End Function

    Public Shared Sub ShowInactiveTopmost(ByVal frm As System.Windows.Forms.Form)
        Try
            ShowWindow(frm.Handle, SW_SHOWNOACTIVATE)
            SetWindowPos(frm.Handle.ToInt32(), HWND_TOPMOST, frm.Left, frm.Top, frm.Width, frm.Height, SWP_NOACTIVATE)
        Catch ex As System.Exception
        End Try
    End Sub

    Protected Overrides ReadOnly Property ShowWithoutActivation As Boolean
        Get
            Return True
        End Get
    End Property

    Private Const WS_EX_TOPMOST As Integer = &H8
    Private Const WS_THICKFRAME As Integer = &H40000
    Private Const WS_CHILD As Integer = &H40000000
    Private Const WS_EX_NOACTIVATE As Integer = &H8000000
    Private Const WS_EX_TOOLWINDOW As Integer = &H80

    Protected Overrides ReadOnly Property CreateParams As CreateParams
        Get
            Dim createParamsA As CreateParams = MyBase.CreateParams
            createParamsA.ExStyle = createParamsA.ExStyle Or WS_EX_TOOLWINDOW Or WS_EX_NOACTIVATE Or WS_EX_TOOLWINDOW Or WS_EX_TOPMOST
            Return createParamsA
        End Get
    End Property


#End Region

    Public Sub New()
        AddHandler AppDomain.CurrentDomain.ProcessExit, AddressOf ProcessExitHandler
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub


    Dim IsGuiLoaded As Boolean = False

    Private Sub PakageInstaller_Load(sender As Object, e As EventArgs) Handles Me.Load

    End Sub

    Private Sub PakageInstaller_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        LoadData()
        IsGuiLoaded = True
    End Sub


    Public Sub LoadData()
        Try

            If ManifestData IsNot Nothing Then
                If ManifestData.ManifestJson.Title = String.Empty Then
                    TitleName.Text = IO.Path.GetFileNameWithoutExtension(ManifestData.MainFolder)
                Else
                    TitleName.Text = ManifestData.ManifestJson.Title
                End If

                If ManifestData.ManifestJson.Author = String.Empty Then
                    Label3.Text = "Power By S4Lsalsoft"
                Else
                    Label3.Text = ManifestData.ManifestJson.Author
                End If

                Label3.Text = ManifestData.ManifestJson.Contact

                Me.Label4.Text = "File Type : " + IO.Path.GetExtension(ManifestData.ManifestJson.FileName).Replace(".", "").ToUpper()
            Else
                Label3.Text = "Power By S4Lsalsoft"
                Me.Label4.Text = "Youtube Stream"
            End If

            If Me.EmbedControl IsNot Nothing Then

                If ManifestData IsNot Nothing Then
                    If Core.Helpers.Utils.IsVideoFormat(ManifestData.ManifestJson.FileName) Then
                        Label5.Visible = True
                        Guna2TrackBar1.Visible = True
                    End If
                Else
                    Label5.Visible = True
                    Guna2TrackBar1.Visible = True
                End If


                If TypeOf Me.EmbedControl Is VideoForm Then

                    Me.Label1.Text = "Engine : VLC"

                ElseIf TypeOf Me.EmbedControl Is WPFVideoEngine Then

                    Me.Label1.Text = "Engine : WPF.Video"

                ElseIf TypeOf Me.EmbedControl Is GifEngine Then

                    Dim GifEngine As GifEngine = DirectCast(Me.EmbedControl, GifEngine)
                    Me.Label1.Text = "Engine : " & GifEngine.EnginePlayer.ToString

                    If Not GifEngine.EnginePlayer = Core.Managed.SettingsLoader.Gif.Xylateware Then
                        Me.PauseToolStripMenuItem.Visible = False
                        Guna2CircleButton1.Visible = False
                    End If

                ElseIf TypeOf Me.EmbedControl Is YoutubePlayer Then

                    Me.Label1.Text = "Engine : YoutubePlayer"
                    Label5.Visible = True
                    Guna2TrackBar1.Visible = True

                End If

            ElseIf Me.EmbedProcess IsNot Nothing Then

                Me.Label1.Text = "Engine : " & IO.Path.GetFileNameWithoutExtension(EmbedProcess.ProcessName.ToString)
                phandle = New Native.Objects.ProcessHandle(EmbedProcess.Id, Native.Security.ProcessAccess.SuspendResume)

            End If

            If ManifestData IsNot Nothing Then

                Dim PreviewPath As String = IO.Path.Combine(ManifestData.MainFolder, ManifestData.ManifestJson.Thumbnail)

                Dim Preview As Bitmap = GetImageFromMemory(PreviewPath)

                Me.Guna2Panel2.BackgroundImage = Preview
                Me.NotifyIcon1.Icon = Preview.ToIcon(True, Color.Black)
            Else
                Try
                    Dim ImageUrl As String = "http://img.youtube.com/vi/" & YoutubeID & "/0.jpg"

                    Dim PaicTemp As New PictureBox
                    PaicTemp.Load(ImageUrl)
                    Dim ImageEx As Bitmap = PaicTemp.Image
                    Me.Guna2Panel2.BackgroundImage = ImageEx
                    Me.NotifyIcon1.Icon = ImageEx.ToIcon(True, Color.Black)
                Catch ex As Exception

                End Try
            End If

            If EmbedProcess IsNot Nothing Then
                Guna2Button2.Visible = True
            End If

            TitleName.Visible = True
            Label1.Visible = Not String.IsNullOrEmpty(Label1.Text)
            Label2.Visible = Not String.IsNullOrEmpty(Label2.Text)
            Label3.Visible = Not String.IsNullOrEmpty(Label3.Text)
            Label4.Visible = Not String.IsNullOrEmpty(Label4.Text)
            Guna2Panel2.Visible = Not (Me.Guna2Panel2.BackgroundImage Is Nothing)
            Me.Hide()
        Catch ex As Exception
            Dim ModernMessage As Guna.UI2.WinForms.Guna2MessageDialog = New Guna.UI2.WinForms.Guna2MessageDialog With {.Parent = Me, .Icon = Guna.UI2.WinForms.MessageDialogIcon.Error,
                                     .Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK, .Style = Guna.UI2.WinForms.MessageDialogStyle.Dark, .Caption = "Package Installer", .Text = ex.Message}

        If ModernMessage.Show = DialogResult.OK Then
            Me.Close()
        End If
        End Try
    End Sub

    Private Function GetImageFromMemory(FilePath As String) As Image
        Dim ba As Byte() = IO.File.ReadAllBytes(FilePath)
        Return XylatewarePro.Core.Helpers.Utils.ConvertbByteToImage(ba)
    End Function

    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        EndWallpaper()
    End Sub

    Private Sub EndToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles EndToolStripMenuItem1.Click
        EndWallpaper()
    End Sub

    Private Sub EndWallpaper()
        Me.NotifyIcon1.Visible = False
        If Me.EmbedControl IsNot Nothing Then
            Me.EmbedControl.Visible = False
            Me.EmbedControl.Hide()
            Me.EmbedControl.Close()
        ElseIf Me.EmbedProcess IsNot Nothing Then
            Try
                Me.EmbedProcess.Kill()
            Catch ex As Exception

            End Try
        End If
        Core.Engine.DesktopEmbeder.RefreshDesktop()
        Core.Helpers.Utils.Sleep(2)
        Core.Engine.DesktopEmbeder.RefreshDesktop()
        Environment.Exit(0)
    End Sub

    Private Sub EndToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EndToolStripMenuItem.Click
        Process.Start("explorer.exe", "/select, " & ManifestData.ManifestJson.FilePathJson)
    End Sub

    Private Sub Guna2CircleButton1_CheckedChanged(sender As Object, e As EventArgs) Handles Guna2CircleButton1.CheckedChanged
        MediaState()
    End Sub

    Public Sub MediaState()
        If IsGuiLoaded Then
            Dim State As Boolean = Guna2CircleButton1.Checked

            If Me.EmbedControl IsNot Nothing Then

                If State = False Then
                    PauseToolStripMenuItem.Text = "Pause"
                Else
                    PauseToolStripMenuItem.Text = "Play"
                End If

                If TypeOf Me.EmbedControl Is VideoForm Then

                    If State = False Then
                        DirectCast(Me.EmbedControl, VideoForm).MediaPlayer1.Play()
                    Else
                        DirectCast(Me.EmbedControl, VideoForm).MediaPlayer1.Pause()
                    End If


                ElseIf TypeOf Me.EmbedControl Is WPFVideoEngine Then

                    If State = False Then
                        DirectCast(Me.EmbedControl, WPFVideoEngine).MediaPlayer.Play()
                        Debug.WriteLine("played")
                    Else
                        DirectCast(Me.EmbedControl, WPFVideoEngine).MediaPlayer.Pause()
                    End If

                ElseIf TypeOf Me.EmbedControl Is GifEngine Then

                    Dim GifEngine As GifEngine = DirectCast(Me.EmbedControl, GifEngine)

                    If GifEngine.EnginePlayer = Core.Managed.SettingsLoader.Gif.Xylateware Then
                        If State = False Then
                            GifEngine.Play()
                        Else
                            GifEngine.Pause()
                        End If

                    End If

                ElseIf TypeOf Me.EmbedControl Is YoutubePlayer Then

                    If State = False Then
                        DirectCast(Me.EmbedControl, YoutubePlayer).AxWindowsMediaPlayer1.Ctlcontrols.play()
                    Else
                        DirectCast(Me.EmbedControl, YoutubePlayer).AxWindowsMediaPlayer1.Ctlcontrols.pause()
                    End If

                End If

            ElseIf EmbedProcess IsNot Nothing Then

                If phandle IsNot Nothing Then
                    If State = False Then
                        phandle.Resume()
                    Else
                        phandle.Suspend()
                    End If
                End If

            End If
        End If
    End Sub

    Private Sub PauseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PauseToolStripMenuItem.Click
        Guna2CircleButton1.Checked = Not Guna2CircleButton1.Checked
    End Sub

    Private Sub Guna2ControlBox1_Click(sender As Object, e As EventArgs) Handles Guna2ControlBox1.Click
        Me.Hide()
    End Sub

    Private Sub ShowToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ShowToolStripMenuItem.Click
        ShowHideMenu()
    End Sub

    Private Sub NotifyIcon1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles NotifyIcon1.MouseDoubleClick
        ShowHideMenu()
    End Sub

    Private Sub ShowHideMenu()
        If Me.Visible = True Then
            Me.Hide()
            ShowToolStripMenuItem.Text = "Show"
        Else
            Me.Show()
            ShowToolStripMenuItem.Text = "Hide"
        End If
    End Sub

    Private Sub Guna2TrackBar1_Scroll(sender As Object, e As ScrollEventArgs) Handles Guna2TrackBar1.Scroll
        Dim Vol As Integer = Guna2TrackBar1.Value

        If TypeOf Me.EmbedControl Is VideoForm Then

            DirectCast(Me.EmbedControl, VideoForm).MediaPlayer1.Volume = Vol

        ElseIf TypeOf Me.EmbedControl Is WPFVideoEngine Then

            Dim VolumeEX As Integer = (Vol * 1) / 100

            DirectCast(Me.EmbedControl, WPFVideoEngine).MediaPlayer.Volume = VolumeEX

        ElseIf TypeOf Me.EmbedControl Is YoutubePlayer Then

            DirectCast(Me.EmbedControl, YoutubePlayer).AxWindowsMediaPlayer1.settings.volume = Vol

        End If
    End Sub

    Dim DeskEmbeder As Core.Engine.DesktopEmbeder = Nothing

    Private Sub Guna2Button2_Click(sender As Object, e As EventArgs) Handles Guna2Button2.Click
        If DeskEmbeder Is Nothing Then
            DeskEmbeder = New Core.Engine.DesktopEmbeder
        End If

        Dim SysEmbed As Integer = Core.Helpers.Utils.ReadIni("Settings", "SysEmbed", 0)

        If EmbedProcess IsNot Nothing Then
            If SysEmbed = 0 Then
                DeskEmbeder.EmbedByHandle(EmbedProcess.MainWindowHandle, True)
            ElseIf SysEmbed = 1 Then
                BiliUPDesktopTool.DesktopEmbeddedWindowHelper.DesktopEmbedWindow(EmbedProcess.MainWindowHandle)
            End If
        ElseIf EmbedControl IsNot Nothing Then
            If SysEmbed = 0 Then
                DeskEmbeder.EmbedControl(EmbedControl, True)
            ElseIf SysEmbed = 1 Then
                BiliUPDesktopTool.DesktopEmbeddedWindowHelper.DesktopEmbedWindow(EmbedControl)
            End If
        End If
    End Sub


    Public Sub ProcessExitHandler(sender As Object, e As System.EventArgs)
        Try
            NotifyIcon1.Visible = False
            NotifyIcon1.Dispose()
            If EmbedProcess IsNot Nothing Then
                EmbedProcess.Kill()
            End If
        Catch ex As Exception
            Environment.Exit(0)
        End Try
    End Sub

End Class