
Imports System.Windows
Imports System.Windows.Controls
Imports EO.WebBrowser
Imports YoutubeExplode
Imports YoutubeExplode.Converter
Imports YoutubeExplode.Videos.Streams

Public Class YoutubePlayer


#Region " Declare "


    Public Property Url As String = String.Empty
    Dim ScreenSize As System.Drawing.Size = New System.Drawing.Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height)

#End Region


#Region " Constructor "

    Public Sub New(Optional ByVal MediaUrlEx As String = "")
        Try : AddHandler System.Windows.Forms.Application.ThreadException, AddressOf Application_Exception_Handler
            System.Windows.Forms.Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException, False)
        Catch : End Try


        '  MediaPlayer1.AspectRatio = Me.Width & ":" & Me.Height
        '  MediaPlayer1.Scale = 0

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

    Private Sub YoutubePlayer_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Me.Size = ScreenSize
        Me.Location = New System.Drawing.Point(0, 0)
        Play(Url)
    End Sub

    Private Sub YoutubePlayer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AxWindowsMediaPlayer1.uiMode = "none"
        If Core.Helpers.Utils.ReadIni("Settings", "YoutubeBars", 0) = 1 Then
            Me.TransparencyKey = Color.Black
        End If
    End Sub


#Region " Player "

    Public Sub Play(ByVal Url As String)
        AxWindowsMediaPlayer1.stretchToFit = True
        AxWindowsMediaPlayer1.URL = Url
        Debug.WriteLine(Url)
        AxWindowsMediaPlayer1.settings.autoStart = True
        AxWindowsMediaPlayer1.settings.setMode("loop", True)

        Me.Show()
        Me.Opacity = 0.1 + 100 / 100
    End Sub

#End Region

End Class