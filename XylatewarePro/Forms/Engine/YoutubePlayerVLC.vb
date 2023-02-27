Imports System.Threading
Imports LibVLCSharp.[Shared]

Public Class YoutubePlayerVLC


#Region " Declare "

    '  Public WithEvents VideoView1 As New LibVLCSharp.WinForms.VideoView With {.Dock = DockStyle.Fill, .Visible = False}
    Private LibOptions As String() = {"--input-repeat=10000"}
    Public WithEvents LibVLC1 As LibVLCSharp.Shared.LibVLC
    Public WithEvents MediaPlayer1 As LibVLCSharp.Shared.MediaPlayer
    Public WithEvents LibVLCMedia As LibVLCSharp.Shared.Media

    Private MediaUrl As String = String.Empty
    Dim ScreenSize As System.Drawing.Size = New System.Drawing.Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height)
#End Region


#Region " Constructor "

    Public Sub New(Optional ByVal MediaUrlEx As String = "")
        Try : AddHandler System.Windows.Forms.Application.ThreadException, AddressOf Application_Exception_Handler
            System.Windows.Forms.Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException, False)
        Catch : End Try
        MediaUrl = MediaUrlEx

        LibVLCSharp.Shared.Core.Initialize()
        LibVLC1 = New LibVLCSharp.Shared.LibVLC(LibOptions)

        MediaPlayer1 = New LibVLCSharp.Shared.MediaPlayer(LibVLC1)
        '    MediaPlayer1.AspectRatio = My.Computer.Screen.WorkingArea.Width & ":" & My.Computer.Screen.WorkingArea.Height '+ 50
        '    MediaPlayer1.Scale = 0

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Public Shared Sub Application_Exception_Handler(ByVal sender As Object, ByVal e As System.Threading.ThreadExceptionEventArgs)
        Dim ex As Exception = CType(e.Exception, Exception)
        Dim ExDialog As New CrashDialog
        ExDialog.Text += "  : LibVLCsharp"
        ExDialog.ErrorMessage = ex
        ExDialog.ShowDialog()
    End Sub

#End Region

    Private Sub VideoForm_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        '  Me.Size = New Size(My.Computer.Screen.WorkingArea.Width, My.Computer.Screen.WorkingArea.Height)
        Me.Location = New Point(0, 0)

        Me.WindowState = FormWindowState.Maximized
    End Sub

    Private Sub VideoForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Me.Hide()
            VideoView1.MediaPlayer = MediaPlayer1

            '  Dim IsPlayMute As Boolean = Not CBool(Core.Helpers.Utils.ReadIni("Settings", "MediaSound", False))

            '   VideoView1.MediaPlayer.Mute = IsPlayMute

            Play(MediaUrl)

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

        PlayerFullScreen()

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
    Dim _isFullScreen As Boolean = False
    ' //https://stackoverflow.com/questions/57373566/how-to-remove-video-black-bands-in-videoview-with-libvlcsharp
    Public Sub PlayerFullScreen()

        Me.Left = 0
        Me.Top = 0
        Me.Width = ScreenSize.Width
        Me.Height = ScreenSize.Height
        _isFullScreen = True
        '  _originalScale = MediaPlayer1.Scale
        '  _originalAspectRatio = MediaPlayer1.AspectRatio
        '  playerBar.Visibility = Visibility.Collapsed
        Dim mediaTrack As MediaTrack?

        Try
            mediaTrack = MediaPlayer1.Media?.Tracks?.FirstOrDefault(Function(x) x.TrackType = TrackType.Video)
        Catch __unusedException1__ As Exception
            mediaTrack = Nothing
        End Try

        If mediaTrack Is Nothing OrElse Not mediaTrack.HasValue Then
            Return
        End If

        ' Dim source As Windows.PresentationSource = Windows.PresentationSource.FromVisual(Me)
        Dim dpiX As Double = 1.0, dpiY As Double = 1.0

        ' If source IsNot Nothing Then
        'dpiX = source.CompositionTarget.TransformToDevice.M11
        'dpiY = source.CompositionTarget.TransformToDevice.M22
        '  End If

        Dim displayW = Me.Width * dpiX
        Dim displayH = Me.Height * dpiY
        Dim videoSwapped = mediaTrack.Value.Data.Video.Orientation = VideoOrientation.LeftBottom OrElse mediaTrack.Value.Data.Video.Orientation = VideoOrientation.RightTop
        Dim videoW = mediaTrack.Value.Data.Video.Width
        Dim videoH = mediaTrack.Value.Data.Video.Height

        If videoSwapped Then
            Dim swap = videoW
            videoW = videoH
            videoH = swap
        End If

        If mediaTrack.Value.Data.Video.SarNum <> mediaTrack.Value.Data.Video.SarDen Then videoW = videoW * mediaTrack.Value.Data.Video.SarNum / mediaTrack.Value.Data.Video.SarDen
        Dim ar = videoW / CSng(videoH)
        Dim dar = displayW / CSng(displayH)
        Dim scale As Single

        If dar >= ar Then
            scale = CSng(displayW) / videoW
        Else
            scale = CSng(displayH) / videoH
        End If

        Dim xscale, yscale As Single
        xscale = CSng((displayW / videoW))
        yscale = CSng((displayH / videoH))
        MediaPlayer1.Scale = If((xscale < yscale), xscale, yscale)
        Dim aspectRatio As String = String.Format("{0}:{1}", Me.Width, Me.Height)
        MediaPlayer1.AspectRatio = aspectRatio

    End Sub

End Class