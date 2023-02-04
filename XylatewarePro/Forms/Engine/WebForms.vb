Imports EO.WebBrowser

Public Class WebForms

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

        MediaUrl = MediaUrlEx
        AddHandler EO.Base.Runtime.CrashDataAvailable, AddressOf Me.Runtime_CrashDataAvailable
        AddHandler EO.Base.Runtime.Exception, AddressOf Me.EOBaseRuntime_Exception_Handler

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

    Private Sub WebForms_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Hide()

        Dim FilePath As String = String.Empty
            If Not MediaUrl = String.Empty Then
                FilePath = MediaUrl
            Else
                FilePath = IO.Path.Combine(Wallpaper.MainFolder, Wallpaper.ManifestJson.FileName)
            End If

            If IO.File.Exists(FilePath) = True Then
                Me.WebView1.Url = FilePath
            Else
                Throw New Exception("File no Found")
            End If
    End Sub

#Region " Browser Event "

    Private Sub EOBaseRuntime_Exception_Handler(ByVal sender As Object, ByVal e As EO.Base.ExceptionEventArgs)
        WebView1.Close(True)
        WebView1.Dispose()
        Me.Hide()
        Core.Engine.DesktopEmbeder.RefreshDesktop()
        Me.Close()
    End Sub

    Private Sub Runtime_CrashDataAvailable(sender As Object, e As EO.Base.CrashDataEventArgs)
        WebView1.Close(True)
        WebView1.Dispose()
        Me.Hide()
        Core.Engine.DesktopEmbeder.RefreshDesktop()
        Me.Close()
    End Sub

    Private Sub WebView1_LoadCompleted(sender As Object, e As LoadCompletedEventArgs) Handles WebView1.LoadCompleted
        Me.Show()
        Me.Opacity = 0.1 + 100 / 100
    End Sub

    Private Sub WebView1_LoadFailed(sender As Object, e As LoadFailedEventArgs) Handles WebView1.LoadFailed
        Me.Show()
        Me.Opacity = 0.1 + 100 / 100
    End Sub

    Private Sub WebForms_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Me.Size = ScreenSize 'New Size(My.Computer.Screen.WorkingArea.Width, My.Computer.Screen.WorkingArea.Height)
        Me.Location = New Point(0, 0)
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

    Public Sub Destroy()
        MyBase.DestroyHandle()
    End Sub

End Class