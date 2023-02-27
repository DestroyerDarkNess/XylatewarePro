Imports System.Runtime.InteropServices
Imports Microsoft.Web.WebView2.Core

Public Class EdgeForm

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

    Private MediaUrl As String = String.Empty
    Dim ScreenSize As System.Drawing.Size = New System.Drawing.Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height)

#End Region

#Region " Constructor "

    Public Sub New(Optional ByVal MediaUrlEx As String = "")

        Try : AddHandler Application.ThreadException, AddressOf Application_Exception_Handler
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException, False)
        Catch : End Try

        '
        MediaUrl = MediaUrlEx

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Public Shared Sub Application_Exception_Handler(ByVal sender As Object, ByVal e As System.Threading.ThreadExceptionEventArgs)
        Dim ex As Exception = CType(e.Exception, Exception)
        Dim ExDialog As New CrashDialog
        ExDialog.Text += "  : Edge Browser"
        ExDialog.ErrorMessage = ex
        ExDialog.ShowDialog()
    End Sub

#End Region

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

    Public Shared EmbedFIleCode As String = <a><![CDATA[
<img src="%FILE%" alt="this slowpoke moves" width="%SizeX%" height="%SizeY%" />
]]></a>.Value

    Private Sub EdgeForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Hide()
        'Me.Opacity = 0.1 + 100 / 100
        Dim FilePath As String = String.Empty
        If Not MediaUrl = String.Empty Then
            FilePath = MediaUrl
        Else
            FilePath = IO.Path.Combine(Wallpaper.MainFolder, Wallpaper.ManifestJson.FileName)
        End If

        If IO.File.Exists(FilePath) = True Then

            If Core.Helpers.Utils.IsGifFormat(FilePath) Then

                Dim CodeHtml As String = EmbedFIleCode.Replace("%FILE%", "file://" & FilePath).Replace("%SizeX%", ScreenSize.Width.ToString).Replace("%SizeY%", ScreenSize.Height.ToString)

                Me.WebView21.NavigateToString(CodeHtml)

            Else
                Me.WebView21.Source = New Uri(FilePath)
            End If

        Else
            Throw New Exception("File no Found")
        End If
        '  WebView21.Source = New Uri("https://www.youtube.com/watch?v=LC-RlwqHZ2A")
    End Sub

    Private FstRun As Boolean = True
    Private WebReady As Boolean = False

    Private Sub Form1_Activated(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Activated
        If FstRun = True Then
            FstRun = False
            InitAsync()
            Wait()
        End If
    End Sub

    Private Sub WebView21_CoreWebView2Ready(ByVal sender As Object, ByVal e As EventArgs) Handles WebView21.CoreWebView2InitializationCompleted
        WebReady = True
    End Sub

    Private Async Sub InitAsync()
        If IO.Directory.Exists(Core.Manage.Paths.CachePath) = False Then
            IO.Directory.CreateDirectory(Core.Manage.Paths.CachePath)
        End If
        Await WebView21.EnsureCoreWebView2Async(Await CoreWebView2Environment.CreateAsync(Nothing, Core.Manage.Paths.CachePath))
    End Sub

    Private Sub Wait()
        While Not WebReady = True
            System.Windows.Forms.Application.DoEvents()
        End While
    End Sub

    Private Sub WebView21_NavigationCompleted(sender As Object, e As CoreWebView2NavigationCompletedEventArgs) Handles WebView21.NavigationCompleted
        Me.Opacity = 0.1 + 100 / 100
        Me.Show()
    End Sub

    Private Sub EdgeForm_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Me.Size = ScreenSize 'New Size(My.Computer.Screen.WorkingArea.Width, My.Computer.Screen.WorkingArea.Height)
        Me.Location = New Point(0, 0)
    End Sub

End Class