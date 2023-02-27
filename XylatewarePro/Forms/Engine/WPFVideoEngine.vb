Imports System.Windows
Imports System.Windows.Controls

Public Class WPFVideoEngine


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

    Dim ScreenSize As System.Drawing.Size = New System.Drawing.Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height)

    Private MediaUrl As String = String.Empty
    Public WithEvents MediaPlayer As MediaElement = New MediaElement With {.Name = "MediaElement1", .Width = ScreenSize.Width, .Height = ScreenSize.Height}

#End Region

#Region " Constructor "

    Public Sub New(Optional ByVal MediaUrlEx As String = "")
        Try : AddHandler System.Windows.Forms.Application.ThreadException, AddressOf Application_Exception_Handler
            System.Windows.Forms.Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException, False)
        Catch : End Try
        MediaUrl = MediaUrlEx
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Public Shared Sub Application_Exception_Handler(ByVal sender As Object, ByVal e As System.Threading.ThreadExceptionEventArgs)
        Dim ex As Exception = CType(e.Exception, Exception)
        Dim ExDialog As New CrashDialog
        ExDialog.Text += "  : WPF Video Player"
        ExDialog.ErrorMessage = ex
        ExDialog.ShowDialog()
    End Sub

#End Region

    Private Sub WPFVideoEngine_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Me.Size = ScreenSize
        Me.Location = New System.Drawing.Point(0, 0)
        '  Me.WindowState = FormWindowState.Maximized
    End Sub

    Private Sub WPFVideoEngine_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Me.Hide()

            Dim FilePath As String = String.Empty
            If Not MediaUrl = String.Empty Then
                FilePath = MediaUrl
            Else
                FilePath = IO.Path.Combine(Wallpaper.MainFolder, Wallpaper.ManifestJson.FileName)
            End If

            If IO.File.Exists(FilePath) = True Then

                Dim host As System.Windows.Forms.Integration.ElementHost = New System.Windows.Forms.Integration.ElementHost()
                host.Dock = DockStyle.Fill
                host.Child = MediaPlayer
                MediaPlayer.HorizontalAlignment = System.Windows.HorizontalAlignment.Center
                Me.Controls.Add(host)

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
        MediaPlayer.Source = New Uri(Url)
        MediaPlayer.Stretch = System.Windows.Media.Stretch.UniformToFill
        MediaPlayer.LoadedBehavior = MediaState.Manual
        MediaPlayer.Play()
        Me.Show()
        Me.Opacity = 0.1 + 100 / 100
    End Sub

    Private Sub MediaPlayer_MediaEnded(sender As Object, e As RoutedEventArgs) Handles MediaPlayer.MediaEnded
        Me.MediaPlayer.Position = TimeSpan.FromMilliseconds(1.0)
        Me.MediaPlayer.Play()
    End Sub

    Private Sub MediaPlayer_MediaFailed(sender As Object, e As ExceptionRoutedEventArgs) Handles MediaPlayer.MediaFailed
        System.Windows.Forms.Application.Exit()
    End Sub

#End Region


End Class