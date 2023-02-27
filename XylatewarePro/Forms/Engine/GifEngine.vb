Public Class GifEngine

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

    Private EnginePlayerEx As Core.Managed.SettingsLoader.Gif = Core.Managed.SettingsLoader.Gif.Xylateware
    Public Property EnginePlayer As Core.Managed.SettingsLoader.Gif
        <DebuggerStepThrough>
        Get
            Return Me.EnginePlayerEx
        End Get
        Set(value As Core.Managed.SettingsLoader.Gif)
            EnginePlayerEx = value
        End Set
    End Property

#End Region

#Region " Declare "

    Dim ScreenSize As System.Drawing.Size = New System.Drawing.Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height)

    Private MediaUrl As String = String.Empty

#End Region

#Region " Constructor "

    Public Sub New(Optional ByVal MediaUrlEx As String = "")
        Try : AddHandler Application.ThreadException, AddressOf Application_Exception_Handler
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException, False)
        Catch : End Try
        MediaUrl = MediaUrlEx

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Public Shared Sub Application_Exception_Handler(ByVal sender As Object, ByVal e As System.Threading.ThreadExceptionEventArgs)
        Dim ex As Exception = CType(e.Exception, Exception)
        Dim ExDialog As New CrashDialog
        ExDialog.Text += "  : GIF Player"
        ExDialog.ErrorMessage = ex
        ExDialog.ShowDialog()
        Application.Exit()
    End Sub

#End Region

    Private Sub GifEngine_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Me.Size = ScreenSize 'New Size(My.Computer.Screen.WorkingArea.Width, My.Computer.Screen.WorkingArea.Height)
        Me.Location = New Point(0, 0)
    End Sub

    Private Sub GifEngine_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Me.Hide()

            Dim FilePath As String = String.Empty
            If Not MediaUrl = String.Empty Then
                FilePath = MediaUrl
            Else
                FilePath = IO.Path.Combine(Wallpaper.MainFolder, Wallpaper.ManifestJson.FileName)
            End If

            If IO.File.Exists(FilePath) = True Then
                SetWallpaper(FilePath)
            Else
                Throw New Exception("File no Found")
            End If
        Catch ex As Exception
            Me.Hide()
        Core.Engine.DesktopEmbeder.RefreshDesktop()
        Me.Close()
        End Try
    End Sub

    Private Sub SetWallpaper(ByVal FileDir As String)
        Dim ImageLoad As Image = Image.FromFile(FileDir)

        If EnginePlayerEx = Core.Managed.SettingsLoader.Gif.Xylateware Then
            PictureBox1.Visible = False
            GifPlayer(ImageLoad)
        ElseIf EnginePlayerEx = Core.Managed.SettingsLoader.Gif.GDI Then
            PanelFX1.Visible = False
            PictureBox1.Image = ImageLoad
        ElseIf EnginePlayerEx = Core.Managed.SettingsLoader.Gif.ManualDecoder Then
            PictureBox1.Visible = False
            PlayerLow(ImageLoad)
        End If

        Me.Show()

    End Sub

    Public Sub PlayerLow(ByVal _BackgroundImage As Image)

        Dim Asynctask As New Task(New Action(Sub()

                                                 Dim gif As New GIF(_BackgroundImage)

                                                 Do Until gif.EndOfFrames ' Iterate frames until the end of frame count.
                                                     Try
                                                         Dim CurrentFrame As Image = gif.NextFrame()

                                                         Me.BeginInvoke(Sub()
                                                                            PanelFX1.BackgroundImage = CurrentFrame
                                                                        End Sub)

                                                         If (gif.EndOfFrames) Then
                                                             gif.ActiveFrameIndex = 0
                                                         End If
                                                         Application.DoEvents()
                                                     Catch ex As Exception

                                                     End Try
                                                 Loop

                                             End Sub), TaskCreationOptions.PreferFairness)
        Asynctask.Start()

    End Sub

    Private Sub GifPlayer(ByVal Preview As Image)
        GifPlayerMon = New GifPlayer(Preview, True, True)
        GifPlayerMon.Start()
    End Sub

    Public Sub Play()
        If GifPlayerMon IsNot Nothing Then
            GifPlayerMon.Start()
        End If
    End Sub

    Public Sub Pause()
        If GifPlayerMon IsNot Nothing Then
            GifPlayerMon.Stop()
        End If
    End Sub

#Region " Gif Player High "

    Private WithEvents GifPlayerMon As GifPlayer = Nothing

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Handles the <see cref="GifPlayer.GifPlayerStatusChanged"/> event of the <see cref="GifPlayerMon"/> instance.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <param name="sender">
    ''' The source of the event.
    ''' </param>
    ''' 
    ''' <param name="e">
    ''' The <see cref="GifPlayer.GifPlayerStatusChangedEventArgs"/> instance containing the event data.
    ''' </param>
    ''' ----------------------------------------------------------------------------------------------------
    Private Sub GifPlayerMon_GifPlayerStatusChanged(ByVal sender As Object, ByVal e As GifPlayer.GifPlayerStatusChangedEventArgs) Handles GifPlayerMon.GifplayerStatusChanged

        Dim CurrentFrame As GifDecoderManager.Frame = e.FrameData
        Dim Imagen As Bitmap = CurrentFrame.Bitmap

        PanelFX1.BackgroundImage = Imagen


    End Sub

#End Region

End Class