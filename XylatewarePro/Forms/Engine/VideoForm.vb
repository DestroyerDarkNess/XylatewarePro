Imports System.Threading
Imports LibVLCSharp.[Shared]

Public Class VideoForm

#Region " Properties "

    Private WallpaperEx As Core.Engine.WallpaperJsonLoader.WallpaperInfo = Nothing
    Public Property Wallpaper As Core.Engine.WallpaperJsonLoader.WallpaperInfo
        <DebuggerStepThrough>
        Get
            Return Me.WallpaperEx
        End Get
        Set(value As Core.Engine.WallpaperJsonLoader.WallpaperInfo)
            WallpaperEx = value
        End Set
    End Property

#End Region

#Region " Declare "

    '  Public WithEvents VideoView1 As New LibVLCSharp.WinForms.VideoView With {.Dock = DockStyle.Fill, .Visible = False}
    Private LibOptions As String() = {"--input-repeat=10000"}
    Public WithEvents LibVLC1 As LibVLCSharp.Shared.LibVLC
    Public WithEvents MediaPlayer1 As LibVLCSharp.Shared.MediaPlayer
    Public WithEvents LibVLCMedia As LibVLCSharp.Shared.Media

    Private MediaUrl As String = String.Empty
    ' Dim ScreenSize As System.Drawing.Size = New System.Drawing.Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height)

#End Region

#Region " Constructor "

    Public Sub New(Optional ByVal MediaUrlEx As String = "")
        Try : AddHandler Application.ThreadException, AddressOf Application_Exception_Handler
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException, False)
        Catch : End Try
        MediaUrl = MediaUrlEx

        LibVLCSharp.Shared.Core.Initialize()
        LibVLC1 = New LibVLCSharp.Shared.LibVLC(LibOptions)
        MediaPlayer1 = New LibVLCSharp.Shared.MediaPlayer(LibVLC1)
        MediaPlayer1.AspectRatio = My.Computer.Screen.WorkingArea.Width & ":" & My.Computer.Screen.WorkingArea.Height + 50
        MediaPlayer1.Scale = 0
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Public Shared Sub Application_Exception_Handler(ByVal sender As Object, ByVal e As System.Threading.ThreadExceptionEventArgs)
        Dim ex As Exception = CType(e.Exception, Exception)
        Dim ExDialog As New CrashDialog
        ExDialog.ErrorMessage = ex
        ExDialog.ShowDialog()
    End Sub

#End Region

    Private Sub VideoForm_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Me.Size = New Size(My.Computer.Screen.WorkingArea.Width, My.Computer.Screen.WorkingArea.Height)
        Me.Location = New Point(0, 0)

        Me.WindowState = FormWindowState.Maximized
    End Sub

    Private Sub VideoForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Me.Hide()
            VideoView1.MediaPlayer = MediaPlayer1

            Dim IsPlayMute As Boolean = Not CBool(Core.Helpers.Utils.ReadIni("Settings", "MediaSound", False))

            VideoView1.MediaPlayer.Mute = IsPlayMute

            Dim FilePath As String = String.Empty
            If Not MediaUrl = String.Empty Then
                FilePath = MediaUrl
            Else
                FilePath = IO.Path.Combine(Wallpaper.MainFolder, Wallpaper.ManifestJson.FileName)
            End If

            If IO.File.Exists(FilePath) = True Then
                Play(FilePath)
            Else
                Throw New Exception("File no Found")
            End If
        Catch ex As Exception
            Me.Hide()
            Core.Engine.DesktopEmbeder.RefreshDesktop()
            Me.Close()
        End Try
    End Sub

    Public Sub Destroy()
        MyBase.DestroyHandle()
    End Sub

#Region " No Windows Focus "

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
            createParamsA.ExStyle = createParamsA.ExStyle Or WS_EX_NOACTIVATE Or WS_EX_TOOLWINDOW 'Or WS_EX_TOPMOST
            Return createParamsA
        End Get
    End Property

#End Region

#Region " Player "

    Public Sub Play(ByVal Url As String)
        LibVLCMedia = New Media(LibVLC1, New Uri(Url))
        MediaPlayer1.Play(LibVLCMedia)
        Me.Show()
        Me.Opacity = 0.1 + 100 / 100
    End Sub

    Private Sub MediaPlayer1_EndReached(sender As Object, e As EventArgs) Handles MediaPlayer1.EndReached
        ThreadPool.QueueUserWorkItem(Function()
                                         Me.MediaPlayer1.Stop()
                                         Me.MediaPlayer1.Play()
                                         Return True
                                     End Function)

    End Sub

#End Region

End Class