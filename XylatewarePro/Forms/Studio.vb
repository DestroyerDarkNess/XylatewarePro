Imports System.IO

Public Class Studio

#Region " Declare "

    Private PageTarget As Core.Manage.PagedList(Of Core.Engine.WallpaperJsonLoader.WallpaperInfo) = Nothing
    Private ListenerEngine As New Core.Manage.ControlLister With {.OrientationControls = Orientation.Horizontal, .Margen = New Point(20, 10)}

    Public Shared FfmpegArgumentVideoTo2Gif As String = <a><![CDATA[-t 5 -i ""%ImputFile%"" -vf ""fps=10,scale=320:-1:flags=lanczos,split[s0][s1];[s0]palettegen[p];[s1][p]paletteuse"" -loop 0 ""%OutputFile%"" ]]></a>.Value

    Public Shared FfmpegArgumentGifToVideo As String = <a><![CDATA[-i ""%ImputFile%"" -movflags faststart -pix_fmt yuv420p -vf ""scale=trunc(iw/2)*2:trunc(ih/2)*2"" ""%OutputFile%"" ]]></a>.Value

#End Region


    Public Sub New()
        SetStyle(ControlStyles.SupportsTransparentBackColor, True)
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        '   Me.BackColor = Color.Transparent
    End Sub

    Private Sub Dashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Dashboard_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        StartUI()
    End Sub

    Public Sub StartUI()
        Me.Guna2Panel1.AllowDrop = True
        Me.Guna2Panel1.Cursor = Cursors.Hand
        Me.Label1.AllowDrop = True
        Me.Label1.Cursor = Cursors.Hand

        ' Panel1.BackColor = Color.FromArgb(80, 28, 29, 33)
        'Panel3.BackColor = Color.FromArgb(80, 21, 21, 21)
    End Sub

    Dim ProcessListener As Boolean = False

    Private Sub Label1_DragDrop(sender As Object, e As DragEventArgs) Handles Label1.DragDrop
        Dim source As String() = CType(e.Data.GetData(DataFormats.FileDrop), String())
        ProcessWallpaper(source.ToList().FirstOrDefault())
    End Sub

    ' Token: 0x06000909 RID: 2313 RVA: 0x00035514 File Offset: 0x00033714
    Private Sub Guna2Panel1_DragDrop(sender As Object, e As DragEventArgs) Handles Guna2Panel1.DragDrop
        Dim source As String() = CType(e.Data.GetData(DataFormats.FileDrop), String())
        ProcessWallpaper(source.ToList().FirstOrDefault())
    End Sub

    ' Token: 0x0600090A RID: 2314 RVA: 0x0003554C File Offset: 0x0003374C
    Private Sub Label1_DragEnter(sender As Object, e As DragEventArgs) Handles Label1.DragEnter
        Dim dataPresent As Boolean = e.Data.GetDataPresent(DataFormats.FileDrop)
        If dataPresent Then
            e.Effect = DragDropEffects.Copy
        Else
            e.Effect = DragDropEffects.None
        End If
    End Sub

    ' Token: 0x0600090B RID: 2315 RVA: 0x00035584 File Offset: 0x00033784
    Private Sub Guna2Panel1_DragEnter(sender As Object, e As DragEventArgs) Handles Guna2Panel1.DragEnter
        Dim dataPresent As Boolean = e.Data.GetDataPresent(DataFormats.FileDrop)
        If dataPresent Then
            e.Effect = DragDropEffects.Copy
        Else
            e.Effect = DragDropEffects.None
        End If
    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click
        Open()
    End Sub

    Private Sub Guna2Panel1_Click(sender As Object, e As EventArgs) Handles Guna2Panel1.Click
        Open()
    End Sub

    Public Sub Open()
        Dim text As String = Core.Helpers.Utils.OpenFile("Media (Image/Video/WebPage)|*.GIF;*.JPEG;*.JPG;*.PNG;*.BMP;*.mp4;*.mkv;*.AMV;*.avi;*.FLV;*.html")

        If IO.File.Exists(text) = True Then
            ProcessWallpaper(text)
        End If
    End Sub


    Dim ProcessConverting As Process = Nothing
    Dim MediaPreview As WallPaperPreview = Nothing
    Dim PreviewPath As String = IO.Path.Combine(IO.Path.GetTempPath, "XyPreview.gif")
    Dim TempVideo As String = IO.Path.Combine(IO.Path.GetTempPath, "TempVideo.mp4")
    Dim MainFileEx As String = String.Empty
    Dim GifImg As Image = Nothing

    Public Sub ProcessWallpaper(ByVal FilePath As String)
        MainFileEx = FilePath
        Dim Asynctask As New Task(New Action(Async Sub()
                                                 '  Try
                                                 ClearCache()

                                                 If XylatewarePro.Core.Helpers.Utils.IsVideoFormat(FilePath) Then

                                                     SetLog("Preview is being generated, please wait...", Color.Orange)
                                                     Me.ProcessConverting = Core.Helpers.Utils.OpenAsHidden("ffmpeg.exe", FfmpegArgumentVideoTo2Gif.Replace("%ImputFile%", FilePath).Replace("%OutputFile%", Me.PreviewPath))

                                                     If Me.ProcessConverting IsNot Nothing Then
                                                         Me.BeginInvoke(Sub()
                                                                            Guna2Panel1.Visible = False
                                                                            Guna2ProgressBar1.Visible = True
                                                                            CancelButom.Visible = True
                                                                        End Sub)
                                                         Me.ProcessConverting.WaitForExit()
                                                         System.Threading.Thread.Sleep(5000)
                                                         Dim ImageEx As Image = GetImageFromMemory(PreviewPath)
                                                         If ImageEx Is Nothing Then
                                                             AppNotify.MakeDialog(Core.Manage.Instances.MainUI, "Error to Unknown in memory.", Color.Red)
                                                             ClearCache()
                                                         Else
                                                             Me.BeginInvoke(Sub()
                                                                                Guna2Panel1.Visible = True
                                                                                Guna2ProgressBar1.Style = ProgressBarStyle.Blocks
                                                                                CancelButom.Visible = False
                                                                                Dim MPreview As WallPaperPreview = New WallPaperPreview With {.Visible = False, .Dock = DockStyle.Fill}
                                                                                MPreview.MediaPlayer(ImageEx, True)
                                                                                GifImg = ImageEx
                                                                                Core.Manage.Instances.MainUI.MediaPreview(True, MPreview)
                                                                                MediaPreview = MPreview
                                                                                Guna2Panel2.Visible = True
                                                                            End Sub)
                                                         End If

                                                     Else
                                                         AppNotify.MakeDialog(Core.Manage.Instances.MainUI, "ffmpeg.exe Not Found!", Color.Red)
                                                         ClearCache()
                                                     End If

                                                 ElseIf XylatewarePro.Core.Helpers.Utils.IsGifFormat(FilePath) Then

                                                     Dim ModernMessage As Guna.UI2.WinForms.Guna2MessageDialog = New Guna.UI2.WinForms.Guna2MessageDialog With {.Parent = Me, .Icon = Guna.UI2.WinForms.MessageDialogIcon.None,
                                                    .Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo, .Style = Guna.UI2.WinForms.MessageDialogStyle.Light, .Caption = "Converter", .Text = "Do you want to convert the GIF to MP4? (This could improve reproduction as wallpaper, but at the expense of more memory.)"}

                                                     If ModernMessage.Show = DialogResult.Yes Then

                                                         SetLog("Converting media file, please wait...", Color.Orange)
                                                         Me.ProcessConverting = XylatewarePro.Core.Helpers.Utils.OpenAsHidden("ffmpeg.exe", FfmpegArgumentGifToVideo.Replace("%ImputFile%", FilePath).Replace("%OutputFile%", Me.TempVideo))

                                                         If Me.ProcessConverting IsNot Nothing Then
                                                             Me.BeginInvoke(Sub()
                                                                                Guna2Panel1.Visible = False
                                                                                Guna2ProgressBar1.Visible = True
                                                                                CancelButom.Visible = True
                                                                            End Sub)
                                                             Me.ProcessConverting.WaitForExit()
                                                             System.Threading.Thread.Sleep(5000)

                                                             Dim ImageEx As Image = GetImageFromMemory(FilePath)
                                                             If ImageEx Is Nothing Then
                                                                 AppNotify.MakeDialog(Core.Manage.Instances.MainUI, "Error to Unknown in memory.", Color.Red)
                                                                 ClearCache()
                                                             Else
                                                                 MainFileEx = TempVideo
                                                                 Me.BeginInvoke(Sub()
                                                                                    Guna2Panel1.Visible = True
                                                                                    Guna2ProgressBar1.Style = ProgressBarStyle.Blocks
                                                                                    CancelButom.Visible = False
                                                                                    Dim MPreview As WallPaperPreview = New WallPaperPreview With {.Visible = False, .Dock = DockStyle.Fill}
                                                                                    MPreview.MediaPlayer(ImageEx, True)
                                                                                    Core.Manage.Instances.MainUI.MediaPreview(True, MPreview)
                                                                                    MediaPreview = MPreview
                                                                                    GifImg = ImageEx
                                                                                    Guna2Panel2.Visible = True
                                                                                End Sub)
                                                             End If


                                                         Else
                                                             AppNotify.MakeDialog(Core.Manage.Instances.MainUI, "ffmpeg.exe Not Found!", Color.Red)
                                                             ClearCache()
                                                         End If


                                                     Else

                                                         Dim ImageEx As Image = GetImageFromMemory(FilePath)
                                                         If ImageEx Is Nothing Then
                                                             AppNotify.MakeDialog(Core.Manage.Instances.MainUI, "Error to Unknown in memory.", Color.Red)
                                                             ClearCache()
                                                         Else

                                                             Me.BeginInvoke(Sub()
                                                                                Guna2Panel1.Visible = True
                                                                                Guna2ProgressBar1.Style = ProgressBarStyle.Blocks
                                                                                CancelButom.Visible = False
                                                                                Dim MPreview As WallPaperPreview = New WallPaperPreview With {.Visible = False, .Dock = DockStyle.Fill}
                                                                                MPreview.MediaPlayer(ImageEx, True)
                                                                                Core.Manage.Instances.MainUI.MediaPreview(True, MPreview)
                                                                                MediaPreview = MPreview
                                                                                Guna2Panel2.Visible = True
                                                                            End Sub)
                                                         End If

                                                     End If

                                                 ElseIf XylatewarePro.Core.Helpers.Utils.IsImageFormat(FilePath) Then

                                                     Dim ImageEx As Image = GetImageFromMemory(FilePath)
                                                     If ImageEx Is Nothing Then
                                                         AppNotify.MakeDialog(Core.Manage.Instances.MainUI, "Error to Unknown in memory.", Color.Red)
                                                         ClearCache()
                                                     Else
                                                         Dim MPreview As WallPaperPreview = New WallPaperPreview With {.Visible = False, .Dock = DockStyle.Fill}
                                                         MPreview.MediaPlayer(ImageEx, False)
                                                         Core.Manage.Instances.MainUI.MediaPreview(True, MPreview)
                                                         MediaPreview = MPreview
                                                         Me.BeginInvoke(Sub()
                                                                            Guna2Panel2.Visible = True
                                                                            Guna2ProgressBar1.Style = ProgressBarStyle.Blocks
                                                                            CancelButom.Visible = False
                                                                        End Sub)
                                                     End If


                                                 Else
                                                     AppNotify.MakeDialog(Core.Manage.Instances.MainUI, "Unknown format: " & FilePath.Split(".").LastOrDefault.ToUpper.ToString, Color.Red)
                                                 End If
                                                 'Catch ex As Exception
                                                 '    AppNotify.MakeDialog(Core.Manage.Instances.MainUI, ex.Message, Color.Red)
                                                 '    ClearCache()
                                                 'End Try
                                             End Sub), TaskCreationOptions.PreferFairness)
        Asynctask.Start()
    End Sub

    Private Sub SetLog(ByVal str As String, Optional ByVal bColor As Color = Nothing)
        Me.BeginInvoke(Sub()
                           If Not bColor = Nothing Then
                               LogLabel.BackColor = BackColor
                           End If

                           LogLabel.Text = str
                       End Sub)
    End Sub

    Dim CustomThumbnail As String = IO.Path.Combine(IO.Path.GetTempPath, "XyThumbnail.png")
    Dim Manifest As String = IO.Path.Combine(IO.Path.GetTempPath, "manifest.json")

    Public Sub PackWallpalper(Optional ByVal Install As Boolean = False)

        If ProcessListener = False Then
            ProcessListener = True

            Dim Asynctask As New Task(New Action(Async Sub()


                                                     Dim FilesToPakg As List(Of FileInfo) = New List(Of FileInfo)

                                                     Dim WallInfo As Core.Engine.WallpaperJsonLoader.Manifest = New Core.Engine.WallpaperJsonLoader.Manifest

                                                     WallInfo.AppVersion = Core.Manage.Instances.FileVer
                                                     WallInfo.Title = Guna2TextBox1.Text
                                                     WallInfo.Desc = Guna2TextBox2.Text
                                                     WallInfo.Author = Guna2TextBox3.Text

                                                     WallInfo.FileName = IO.Path.GetFileName(MainFileEx)

                                                     If XylatewarePro.Core.Helpers.Utils.IsVideoFormat(MainFileEx) Or XylatewarePro.Core.Helpers.Utils.IsGifFormat(MainFileEx) Then

                                                         WallInfo.Preview = IO.Path.GetFileName(PreviewPath)
                                                         Dim FrameEx As GifDecoderManager.Frame = GifDecoderManager.GetFrames(GifImg, 0)
                                                         Dim Thumbnail As Image = FrameEx.Bitmap.Clone()
                                                         Thumbnail.Save(CustomThumbnail, Imaging.ImageFormat.Png)
                                                         Thumbnail.Dispose()
                                                         WallInfo.Thumbnail = IO.Path.GetFileName(CustomThumbnail)

                                                         FilesToPakg.Add(New IO.FileInfo(MainFileEx))
                                                         FilesToPakg.Add(New IO.FileInfo(CustomThumbnail))
                                                         FilesToPakg.Add(New IO.FileInfo(PreviewPath))

                                                         'ElseIf XylatewarePro.Core.Helpers.Utils.IsGifFormat(MainFileEx) Then

                                                         '    WallInfo.Preview = PreviewPath
                                                         '    Dim FrameEx As GifDecoderManager.Frame = GifDecoderManager.GetFrames(GifImg, 0)
                                                         '    Dim Thumbnail As Image = FrameEx.Bitmap.Clone()
                                                         '    Thumbnail.Save(CustomThumbnail, Imaging.ImageFormat.Png)
                                                         '    Thumbnail.Dispose()
                                                         '    WallInfo.Thumbnail = IO.Path.GetFileName(CustomThumbnail)

                                                         '    FilesToPakg.Add(New IO.FileInfo(MainFileEx))
                                                         '    FilesToPakg.Add(New IO.FileInfo(CustomThumbnail))
                                                         '    FilesToPakg.Add(New IO.FileInfo(PreviewPath))

                                                     ElseIf XylatewarePro.Core.Helpers.Utils.IsImageFormat(MainFileEx) Then

                                                         WallInfo.Preview = IO.Path.GetFileName(MainFileEx)
                                                         WallInfo.Thumbnail = IO.Path.GetFileName(MainFileEx)

                                                         FilesToPakg.Add(New IO.FileInfo(MainFileEx))

                                                     End If

                                                     Dim Writejson As Boolean = XylatewarePro.Core.Helpers.Utils.FileWriteText(Manifest, WallInfo.ToString())

                                                     If Writejson = True Then

                                                         FilesToPakg.Add(New IO.FileInfo(Manifest))

                                                         Dim ZipPackage As String = Me.CompressPlugin("TempPlugin.zip", FilesToPakg)

                                                         If Install Then
                                                             Process.Start(Application.ExecutablePath, "-install " & """" & ZipPackage & """")
                                                         Else
                                                             Me.BeginInvoke(Sub()
                                                                                Dim text As String = XylatewarePro.Core.Helpers.Utils.SaveFile("Xylateware Package|*.xypkg")
                                                                                If text IsNot Nothing Then
                                                                                    IO.File.Copy(ZipPackage, text)
                                                                                End If
                                                                            End Sub)

                                                         End If

                                                     Else
                                                         AppNotify.MakeDialog(Core.Manage.Instances.MainUI, "Error to Write File.", Color.Red)
                                                     End If

                                                     ClearCache()
                                                     Core.Helpers.Utils.Sleep(2)
                                                     Me.BeginInvoke(Sub()
                                                                        Guna2ProgressBar1.Visible = False
                                                                    End Sub)


                                                     ProcessListener = False
                                                 End Sub), TaskCreationOptions.PreferFairness)
            Asynctask.Start()
        Else

            AppNotify.MakeDialog(Core.Manage.Instances.MainUI, "There is a thread running, wait a few seconds while it finishes.", Color.Orange)

        End If

    End Sub

    Public Function CompressPlugin(ByVal FileN As String, ByVal FilesToZip As List(Of IO.FileInfo)) As String

        Dim ZipName As String = IO.Path.GetFileNameWithoutExtension(FileN) & ".zip"
        Dim ZipTempDir As String = IO.Path.Combine(IO.Path.GetTempPath, ZipName)

        If IO.File.Exists(ZipTempDir) Then
            IO.File.Delete(ZipTempDir)
        End If

        Core.ZipCore.ZipFileWithProgress.CreateFromFiles(FilesToZip.ToArray, Nothing, ZipTempDir,
           New Progress(Of Double)(
    Sub(p)
        Dim Progress As Integer = Val(p * 100)
        If Progress <= 100 Then
            Guna2ProgressBar1.Value = Progress
        End If
    End Sub))

        Return ZipTempDir
    End Function

    Private Function GetImageFromMemory(ByVal FilePath As String) As Image
        Dim ba As Byte() = IO.File.ReadAllBytes(FilePath)
        Return XylatewarePro.Core.Helpers.Utils.ConvertbByteToImage(ba)
    End Function

    Private Sub ClearCache()
        Me.BeginInvoke(Sub()
                           Guna2Panel2.Visible = False
                           Guna2Panel1.Visible = True
                           If MediaPreview IsNot Nothing Then
                               MediaPreview.Dispose()
                               MediaPreview = Nothing
                           End If
                           If GifImg IsNot Nothing Then
                               GifImg.Dispose()
                               GifImg = Nothing
                           End If
                           Core.Manage.Instances.MainUI.MediaPreview(False)
                           Guna2ProgressBar1.Value = 0
                           Guna2ProgressBar1.Style = ProgressBarStyle.Marquee
                           CancelButom.Visible = False
                           Guna2Panel2.Visible = False
                           Guna2TextBox1.Text = ""
                           Guna2TextBox2.Text = ""
                           Guna2TextBox3.Text = ""
                           SetLog("", Color.Transparent)
                           Try
                               If IO.File.Exists(PreviewPath) Then
                                   IO.File.Delete(PreviewPath)
                               End If
                               If IO.File.Exists(TempVideo) Then
                                   IO.File.Delete(TempVideo)
                               End If
                               If IO.File.Exists(CustomThumbnail) Then
                                   IO.File.Delete(CustomThumbnail)
                               End If
                           Catch ex As Exception

                           End Try

                       End Sub)
    End Sub

    Private Sub Guna2TextBox1_TextChanged(sender As Object, e As EventArgs) Handles Guna2TextBox1.TextChanged
        If MediaPreview IsNot Nothing Then
            MediaPreview.Label1.Text = Guna2TextBox1.Text
            MediaPreview.Label1.Visible = Not String.IsNullOrEmpty(Guna2TextBox1.Text)
        End If
    End Sub

    Private Sub Guna2TextBox2_TextChanged(sender As Object, e As EventArgs) Handles Guna2TextBox2.TextChanged
        If MediaPreview IsNot Nothing Then
            MediaPreview.Label2.Text = Guna2TextBox2.Text
            MediaPreview.Label2.Visible = Not String.IsNullOrEmpty(Guna2TextBox2.Text)
        End If
    End Sub

    Private Sub Guna2TextBox3_TextChanged(sender As Object, e As EventArgs) Handles Guna2TextBox3.TextChanged
        If MediaPreview IsNot Nothing Then
            MediaPreview.Label3.Text = Guna2TextBox3.Text
            MediaPreview.Label3.Visible = Not String.IsNullOrEmpty(Guna2TextBox3.Text)
        End If
    End Sub

    Private Sub CancelButom_Click(sender As Object, e As EventArgs) Handles CancelButom.Click
        Try
            If ProcessConverting IsNot Nothing Then
                ProcessConverting.Kill()
            End If
            ClearCache()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        PackWallpalper(True)
    End Sub

    Private Sub Guna2Button3_Click(sender As Object, e As EventArgs) Handles Guna2Button3.Click
        PackWallpalper()
    End Sub

    Dim Rand As New Random

    Private Sub Guna2ProgressBar1_ValueChanged(sender As Object, e As EventArgs) Handles Guna2ProgressBar1.ValueChanged
        Dim num As Integer = Me.Rand.[Next](0, 10)
        Dim flag As Boolean = num >= 1 And num <= 2
        If flag Then
            Me.Guna2ProgressBar1.ProgressColor = Color.FromArgb(Me.Rand.[Next](0, 255), Me.Rand.[Next](0, 255), Me.Rand.[Next](0, 255))
            Me.Guna2ProgressBar1.ProgressColor2 = Color.FromArgb(Me.Rand.[Next](0, 255), Me.Rand.[Next](0, 255), Me.Rand.[Next](0, 255))
        Else
            Dim flag2 As Boolean = num >= 5 And num <= 7
            If flag2 Then
                Me.Guna2ProgressBar1.ProgressColor = Color.FromArgb(Me.Rand.[Next](0, 255), Me.Rand.[Next](0, 255), Me.Rand.[Next](0, 255))
                Me.Guna2ProgressBar1.ProgressColor2 = Color.FromArgb(Me.Rand.[Next](0, 255), Me.Rand.[Next](0, 255), Me.Rand.[Next](0, 255))
            Else
                Dim flag3 As Boolean = num = 10
                If flag3 Then
                    Me.Guna2ProgressBar1.ProgressColor = Color.SpringGreen
                    Me.Guna2ProgressBar1.ProgressColor = Color.SpringGreen
                Else
                    Dim flag4 As Boolean = num = 0
                    If flag4 Then
                        Me.Guna2ProgressBar1.ProgressColor = Color.DodgerBlue
                        Me.Guna2ProgressBar1.ProgressColor = Color.DodgerBlue
                    End If
                End If
            End If
        End If
    End Sub
End Class