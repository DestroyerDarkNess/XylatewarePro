Imports System.ComponentModel
Imports System.IO
Imports System.Net
Imports YoutubeExplode
Imports YoutubeExplode.Videos.Streams

Public Class WallPaperPreview

    Private WallStyleTypeList As List(Of Core.Engine.WallpaperStyle) = [Enum].GetValues(GetType(Core.Engine.WallpaperStyle)).Cast(Of Core.Engine.WallpaperStyle)().ToList()

#Region " Properties "

    Public WallpaperAPI As Core.Wallpaper.WallHaven = Nothing

    Private WallInfo As Core.Engine.WallpaperJsonLoader.WallpaperInfo = Nothing
    Public Property WallpaperInfo As Core.Engine.WallpaperJsonLoader.WallpaperInfo
        <DebuggerStepThrough>
        Get
            Return Me.WallInfo
        End Get
        Set(value As Core.Engine.WallpaperJsonLoader.WallpaperInfo)
            WallInfo = value
        End Set
    End Property

    Private InfoEx As Core.Wallpaper.WallHaven.Info = Nothing
    Public Property Wallpaper As Core.Wallpaper.WallHaven.Info
        <DebuggerStepThrough>
        Get
            Return Me.InfoEx
        End Get
        Set(value As Core.Wallpaper.WallHaven.Info)
            InfoEx = value
        End Set
    End Property

    Public Property Youtube As String = String.Empty

#End Region

    Private Sub PreviewCon_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        WallpaperAPI = Core.Manage.Instances.MainUI.WallStore.WallpaperAPI

        If WallInfo IsNot Nothing Then

            Dim flag3 As Boolean = XylatewarePro.Core.Helpers.Utils.IsImageFormat(WallInfo.ManifestJson.FileName)

            If flag3 Then
                Dim WallStyleList As List(Of Object) = [Enum].GetValues(GetType(Core.Engine.WallpaperStyle)).Cast(Of Object)().ToList()
                Guna2ComboBox2.Items.AddRange(WallStyleList.ToArray)
                Guna2ComboBox2.SelectedIndex = 0
                Guna2ComboBox2.Visible = True
            End If

            Label1.Text = WallInfo.ManifestJson.Title
            Label2.Text = WallInfo.ManifestJson.Desc
            Label3.Text = WallInfo.ManifestJson.Contact



            If Label3.Text.ToLower.StartsWith("http") Or Label3.Text.ToLower.StartsWith("www") Then
                Label3.Cursor = Cursors.Hand
            End If

            GifPlayer(WallInfo)

            Guna2Button1.Text = "Set Wallpaper"
            Guna2Button1.Visible = True

            Guna2Button3.Text = "Default Wallpaper"
            Guna2Button3.Visible = True

        ElseIf InfoEx IsNot Nothing Then
            LoadImage(InfoEx.Preview)
        ElseIf Not Youtube = String.Empty Then
            LoadPreviewFromYoutube()
        End If

        Label1.Visible = Not String.IsNullOrEmpty(Label1.Text)
        Label2.Visible = Not String.IsNullOrEmpty(Label2.Text)
        Label3.Visible = Not String.IsNullOrEmpty(Label3.Text)
    End Sub

    Private StreamUrl As String = String.Empty

    Private Sub LoadPreviewFromYoutube()
        Dim ImageUrl As String = "http://img.youtube.com/vi/" & Core.Helpers.Utils.getID(Youtube).ToString & "/0.jpg"

        ' Label1.Text = " Resolution : " & GetImage.Resolution.Width & "x" & GetImage.Resolution.Height
        '  Label1.Visible = True
        Guna2ProgressBar1.Visible = True
        Client.DownloadDataAsync(New Uri(ImageUrl))
        LoadStream()
    End Sub

    Private Async Sub LoadStream()
        Try
            Dim YoutubeApi As YoutubeClient = New YoutubeClient
            Dim StreamManifest As StreamManifest = Await YoutubeApi.Videos.Streams.GetManifestAsync(Youtube)
            Dim MuxedStreamInfos As MuxedStreamInfo = StreamManifest.GetMuxedStreams.GetWithHighestVideoQuality()
            StreamUrl = MuxedStreamInfos.Url
            Guna2Button1.Text = "Set Wallpaper"
            Guna2Button3.Text = "Default Wallpaper"
            Guna2Button1.Visible = True
            Guna2Button3.Visible = True
        Catch ex As Exception
            Guna2Button5.Visible = True
            AppNotify.MakeDialog(Core.Manage.Instances.MainUI, ex.Message, Color.Red)
        End Try
    End Sub


    Public WithEvents Client As WebClient = New WebClient


    Private Sub Client_DownloadDataCompleted(sender As Object, e As DownloadDataCompletedEventArgs) Handles Client.DownloadDataCompleted
        Try
            Guna2CircleProgressBar1.Animated = False
            Guna2CircleProgressBar1.Visible = False
            Guna2ProgressBar1.Visible = False

            Dim result As Byte() = e.Result
            Dim ImageMemoryPointer As MemoryStream = New MemoryStream(result)
            Dim image As Image = Image.FromStream(ImageMemoryPointer)
            Me.PanelFX1.BackgroundImage = image

            If Youtube = String.Empty Then
                Guna2Button1.Visible = True
                Guna2Button3.Visible = True
            End If

        Catch ex As Exception
            SetFailed("Error: " & ex.Message)
        End Try
    End Sub


    Private Sub Client_DownloadProgressChanged(sender As Object, e As DownloadProgressChangedEventArgs) Handles Client.DownloadProgressChanged
        Guna2ProgressBar1.Value = e.ProgressPercentage
    End Sub

    Public Sub SetFailed(ByVal Message As String)
        Me.BeginInvoke(Sub()
                           Label2.ForeColor = Color.Tomato
                           Label2.Text = Message
                           Label2.Visible = True
                           Guna2Button5.Visible = True
                       End Sub)
    End Sub

    Private Sub Guna2Button5_Click(sender As Object, e As EventArgs) Handles Guna2Button5.Click
        If InfoEx IsNot Nothing Then
            Label2.Visible = False
            Label2.Text = ""
            Label2.ForeColor = Color.White
            LoadImage(InfoEx.Preview)
        ElseIf Not Youtube = String.Empty Then
            LoadStream()
        End If
    End Sub

    Public Sub LoadImage(ByVal LinkTarget As String)
        Guna2Button5.Visible = False
        Guna2ProgressBar1.Visible = True
        Guna2CircleProgressBar1.Animated = True
        Guna2CircleProgressBar1.Visible = True

        Dim Asynctask As New Task(New Action(Async Sub()

                                                 Dim HTML_Source As String = Me.WallpaperAPI.GetSourcePage(LinkTarget)

                                                 If HTML_Source = String.Empty Then
                                                     SetFailed("Error getting data from the server. Please try again.")
                                                     Exit Sub
                                                 End If

                                                 Dim GetImage As Core.Wallpaper.WallHaven.ImageInfo = Me.WallpaperAPI.GetImageInfo(HTML_Source)

                                                 If GetImage Is Nothing Then
                                                     SetFailed("Error processing data, please try again")
                                                     Exit Sub
                                                 End If

                                                 Me.BeginInvoke(Sub()
                                                                    Label1.Text = " Resolution : " & GetImage.Resolution.Width & "x" & GetImage.Resolution.Height
                                                                    Label1.Visible = True
                                                                    Guna2ProgressBar1.Visible = True
                                                                    Client.DownloadDataAsync(New Uri(GetImage.DownloadUrl))
                                                                End Sub)

                                             End Sub), TaskCreationOptions.PreferFairness)
        Asynctask.Start()

    End Sub

    Public Sub GifPlayer(ByVal WallInfoEx As Core.Engine.WallpaperJsonLoader.WallpaperInfo)
        If GifPlayerMon IsNot Nothing Then GifPlayerMon.Dispose()

        If LCase(IO.Path.GetExtension(WallInfoEx.ManifestJson.Preview)) = ".gif" Then
            Dim ImagePreview As String = IO.Path.Combine(WallInfoEx.FullPath, WallInfoEx.ManifestJson.Preview)

            GifPlayerMon = New GifPlayer(Image.FromFile(ImagePreview), True, True)
            GifPlayerMon.SetInterval(100)
            Guna2CircleButton1.Visible = True
            Guna2CircleButton1.Checked = False

        Else
            Dim ImagePreview As String = IO.Path.Combine(WallInfoEx.FullPath, WallInfoEx.ManifestJson.Thumbnail)
            PanelFX1.BackgroundImage = Image.FromFile(ImagePreview)
            Dim flag3 As Boolean = XylatewarePro.Core.Helpers.Utils.IsImageFormat(WallInfo.ManifestJson.FileName)
            If flag3 Then
                Dim Resolution As String = "Resolution: " & PanelFX1.BackgroundImage.Width & "x" & PanelFX1.BackgroundImage.Height
                If Label1.Text = String.Empty Then
                    Label1.Text = Resolution
                Else
                    If Not Label2.Text = String.Empty Then Label2.Text += Environment.NewLine
                    Label2.Text += Resolution
                End If
            End If
        End If
    End Sub

    Public Sub MediaPlayer(ByVal ImageEx As Image, ByVal IsGif As Boolean)
        If GifPlayerMon IsNot Nothing Then GifPlayerMon.Dispose()

        If IsGif = True Then

            GifPlayerMon = New GifPlayer(ImageEx, True, True)
            GifPlayerMon.SetInterval(100)
            Guna2CircleButton1.Visible = True
            Guna2CircleButton1.Checked = False

        Else
            PanelFX1.BackgroundImage = ImageEx
            Dim Resolution As String = "Resolution: " & PanelFX1.BackgroundImage.Width & "x" & PanelFX1.BackgroundImage.Height
            If Label1.Text = String.Empty Then
                Label1.Text = Resolution
            Else
                If Not Label2.Text = String.Empty Then Label2.Text += Environment.NewLine
                Label2.Text += Resolution
            End If
        End If
    End Sub

    Private Function GetImageFromMemory(ByVal FilePath As String) As Image
        Dim ba As Byte() = IO.File.ReadAllBytes(FilePath)
        Return XylatewarePro.Core.Helpers.Utils.ConvertbByteToImage(ba)
    End Function

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
        Guna2CircleButton1.Refresh()
    End Sub

    Private Sub Guna2CircleButton1_CheckedChanged(sender As Object, e As EventArgs) Handles Guna2CircleButton1.CheckedChanged
        If GifPlayerMon IsNot Nothing Then
            If Guna2CircleButton1.Checked = False Then
                GifPlayerMon.Start()
            Else
                GifPlayerMon.Stop()
            End If
        End If
    End Sub

    Private Sub Label3_Click(sender As Object, e As EventArgs) Handles Label3.Click
        Process.Start(Label3.Text)
    End Sub

    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        Try


            If WallInfo IsNot Nothing Then

                SetWallpaper()

            ElseIf InfoEx IsNot Nothing Then

                Guna2Button1.Enabled = False

                Dim Path As String = "WallPaper" & Core.Helpers.Utils.RandomString(5)

                Dim SavePath As String = IO.Path.Combine(Core.Manage.Paths.CacheWallpaperPath, Path)

                If IO.Directory.Exists(SavePath) Then
                    IO.Directory.Delete(SavePath)
                End If

                IO.Directory.CreateDirectory(SavePath)

                Dim text3 As String = "WallImage.Jpeg"

                Dim ImagePath As String = IO.Path.Combine(SavePath, text3)

                PanelFX1.BackgroundImage.Save(ImagePath, Imaging.ImageFormat.Jpeg)

                Dim fileDir As String = IO.Path.Combine(SavePath, "manifest.json")

                Dim manifest As Core.Engine.WallpaperJsonLoader.Manifest = New Core.Engine.WallpaperJsonLoader.Manifest

                manifest.AppVersion = "1.0.0.0"
                manifest.Author = "S4Lsalsoft"
                manifest.Contact = " Discord : Destroyer#8328"
                manifest.FileName = text3
                manifest.Preview = text3
                manifest.Thumbnail = text3
                manifest.Title = InfoEx.Name
                manifest.Desc = " Image Downloaded with Xylateware Pro "
                Dim flag3 As Boolean = XylatewarePro.Core.Helpers.Utils.FileWriteText(fileDir, manifest.ToString())

                If flag3 = True Then

                    Guna2Button1.Visible = False
                    Guna2ProgressBar1.Visible = True
                    Guna2ProgressBar1.ProgressColor = Color.Cyan
                    Guna2ProgressBar1.ProgressColor2 = Color.LightCyan

                    For i As Integer = 0 To Guna2ProgressBar1.Maximum
                        Guna2ProgressBar1.Value = i
                        Core.Helpers.Utils.Sleep(5, Core.Helpers.Utils.Measure.Milliseconds)
                    Next

                    Status.Text = "It has been installed successfully."
                    Status.BackColor = Color.DodgerBlue

                End If

            ElseIf Not Youtube = String.Empty Then

                Core.Engine.DesktopEmbeder.RefreshDesktop()
                If Core.Manage.Instances.LastEngine IsNot Nothing Then
                    Try
                        Core.Manage.Instances.LastEngine.Kill()
                        For Each Proc As Process In Process.GetProcessesByName("mpv")
                            Proc.Kill()
                        Next
                    Catch ex As Exception

                    End Try
                End If
                Core.Engine.DesktopEmbeder.RefreshDesktop()

                If Not StreamUrl = String.Empty Then
                    Try
                        Core.Manage.Instances.LastEngine = Process.Start(Application.ExecutablePath, "-youtube " & Core.Helpers.Utils.getID(Youtube).ToString & " " & StreamUrl)
                        Core.Manage.Instances.MainUI.Youtube.PauseVideo()
                    Catch ex As Exception
                        AppNotify.MakeDialog(Core.Manage.Instances.MainUI, ex.Message, Color.Red)
                    End Try
                End If

            End If



        Catch ex As Exception
            Guna2Button1.Enabled = True
            Status.Text = ex.Message
            Status.BackColor = Color.DarkRed
        End Try

    End Sub

    Private Sub Guna2Button3_Click(sender As Object, e As EventArgs) Handles Guna2Button3.Click

        If WallInfo IsNot Nothing Then

            Core.Engine.DesktopEmbeder.RefreshDesktop()

            If Core.Manage.Instances.LastEngine IsNot Nothing Then
                Try
                    Core.Manage.Instances.LastEngine.Kill()

                    For Each Proc As Process In Process.GetProcessesByName("mpv")
                        Proc.Kill()
                    Next
                Catch ex As Exception

                End Try
            End If

            Core.Engine.DesktopEmbeder.RefreshDesktop()

        ElseIf InfoEx IsNot Nothing Then

            Dim SaveAs As String = Core.Helpers.Utils.SaveFile("Image Files|*.png")
            If SaveAs IsNot Nothing Then
                PanelFX1.BackgroundImage.Save(SaveAs, Imaging.ImageFormat.Png)
            End If

        ElseIf Not Youtube = String.Empty Then
            Core.Engine.DesktopEmbeder.RefreshDesktop()
            If Core.Manage.Instances.LastEngine IsNot Nothing Then
                Try
                    Core.Manage.Instances.LastEngine.Kill()
                    For Each Proc As Process In Process.GetProcessesByName("mpv")
                        Proc.Kill()
                    Next
                Catch ex As Exception

                End Try
            End If
            Core.Engine.DesktopEmbeder.RefreshDesktop()
        End If

    End Sub

    Public IsGUILoaded As Boolean = False

    Private Sub Guna2ComboBox2_MouseHover(sender As Object, e As EventArgs) Handles Guna2ComboBox2.MouseHover
        IsGUILoaded = True
    End Sub

    Private Sub Guna2ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Guna2ComboBox2.SelectedIndexChanged
        If IsGUILoaded = True Then SetWallpaper()
    End Sub

    Public Sub SetWallpaper()

        Core.Engine.DesktopEmbeder.RefreshDesktop()
        If Core.Manage.Instances.LastEngine IsNot Nothing Then
            Try
                Core.Manage.Instances.LastEngine.Kill()
                For Each Proc As Process In Process.GetProcessesByName("mpv")
                    Proc.Kill()
                Next
            Catch ex As Exception

            End Try
        End If

        Dim flag3 As Boolean = XylatewarePro.Core.Helpers.Utils.IsImageFormat(WallInfo.ManifestJson.FileName)

        If flag3 Then

            Dim FileMedia As String = IO.Path.Combine(WallInfo.MainFolder, WallInfo.ManifestJson.FileName)

            Try

                Dim StyleWall As Core.Engine.WallpaperStyle = WallStyleTypeList(Guna2ComboBox2.SelectedIndex)
                Core.Engine.Wallpaper.SetDesktopWallpaper(FileMedia, StyleWall)
            Catch ex As Exception
                Core.Engine.DesktopEmbeder.SetImage(FileMedia)
            End Try
        Else

            Core.Manage.Instances.LastEngine = Process.Start(Application.ExecutablePath, "-start " & """" & WallInfo.ManifestJson.FilePathJson & """")
        End If
    End Sub


    'Private Sub Guna2CircleButton1_Click(sender As Object, e As EventArgs) Handles Guna2CircleButton1.Click
    '    MsgBox(Label1.Visible)
    '    Label1.Invalidate()
    '    Label1.Update()
    '    Label1.Refresh()
    'End Sub


#End Region



End Class
