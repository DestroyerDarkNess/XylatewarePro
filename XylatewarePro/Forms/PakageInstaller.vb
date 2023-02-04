Imports System.Web.Script.Serialization

Public Class PakageInstaller

    Private TempPathEx As String = String.Empty
    Public Property TargetFile As String = String.Empty


    Private Sub PakageInstaller_Load(sender As Object, e As EventArgs) Handles Me.Load
        TempPathEx = IO.Path.Combine(IO.Path.GetTempPath, IO.Path.GetFileNameWithoutExtension(TargetFile))
    End Sub

    Private Sub PakageInstaller_Shown(sender As Object, e As EventArgs) Handles Me.Shown

        Core.Helpers.Utils.Sleep(3)
        Panel1.Visible = False
        Guna2CircleProgressBar1.Animated = True
        Guna2CircleProgressBar1.Visible = True
        Guna2ControlBox1.Visible = True
        Label1.Text = "Loading..."
        Core.Helpers.Utils.Sleep(3)
        Descompress()
    End Sub

    Public Sub Descompress()
        Try
            If IO.Directory.Exists(TempPathEx) Then
                IO.Directory.Delete(TempPathEx, True)
            End If
            IO.Directory.CreateDirectory(TempPathEx)
        Catch ex As Exception

        End Try

        Core.ZipCore.ZipFileWithProgress.ExtractToDirectory(TargetFile, TempPathEx,
             New Progress(Of Double)(
      Sub(p)
          Dim Progress As Integer = Val(p * 100)
          If Progress <= 100 Then
              Guna2CircleProgressBar1.Value = Progress
          End If
      End Sub))

        Core.Helpers.Utils.Sleep(3)
        LoadData()
    End Sub

    Public Sub LoadData()
        Try

            Dim FileManifest As String = String.Empty

            If IO.File.Exists(IO.Path.Combine(TempPathEx, "manifest.json")) Then
                FileManifest = IO.Path.Combine(TempPathEx, "manifest.json")
            ElseIf IO.File.Exists(IO.Path.Combine(TempPathEx, "livelyInfo.json")) Then
                FileManifest = IO.Path.Combine(TempPathEx, "livelyInfo.json")
            Else
                Dim ModernMessage As Guna.UI2.WinForms.Guna2MessageDialog = New Guna.UI2.WinForms.Guna2MessageDialog With {.Parent = Me, .Icon = Guna.UI2.WinForms.MessageDialogIcon.Error,
                                                      .Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK, .Style = Guna.UI2.WinForms.MessageDialogStyle.Dark, .Caption = "Package Analizer", .Text = "The file 'manifest.json' was not found in the package."}

                If ModernMessage.Show = DialogResult.OK Then
                    Me.Close()
                End If
            End If


            Dim JsonCode As String = Core.Helpers.Utils.FileReadText(FileManifest)

            If JsonCode = String.Empty Then
                Dim ModernMessage As Guna.UI2.WinForms.Guna2MessageDialog = New Guna.UI2.WinForms.Guna2MessageDialog With {.Parent = Me, .Icon = Guna.UI2.WinForms.MessageDialogIcon.Error,
                                                    .Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK, .Style = Guna.UI2.WinForms.MessageDialogStyle.Dark, .Caption = "Package Analizer", .Text = "The 'manifest.json' file is incorrectly formatted or empty."}

                If ModernMessage.Show = DialogResult.OK Then
                    Me.Close()
                End If
            End If

            Dim ManifestData = New JavaScriptSerializer().Deserialize(Of Core.Engine.WallpaperJsonLoader.Manifest)(JsonCode)

            If ManifestData.Title = String.Empty Then
                TitleName.Text = IO.Path.GetFileNameWithoutExtension(TargetFile)
            Else
                TitleName.Text = ManifestData.Title
            End If

            If ManifestData.Author = String.Empty Then
                Label3.Text = "Power By S4Lsalsoft"
            Else
                Label3.Text = ManifestData.Author
            End If

            Label3.Text = ManifestData.Contact

            Dim PreviewPath As String = IO.Path.Combine(TempPathEx, ManifestData.Preview)

            Dim Preview As Image = GetImageFromMemory(PreviewPath)


            TitleName.Visible = True
            Label2.Visible = True
            Label3.Visible = True
            Label4.Visible = True
            Guna2Panel2.Visible = True

            Label1.Visible = False
            Guna2CircleProgressBar1.Animated = False
            Guna2CircleProgressBar1.Visible = False

            GifPlayer(Preview, PreviewPath.ToLower.EndsWith(".gif"))
            Guna2Button1.Enabled = False
            Guna2Button1.Visible = True
            Core.Helpers.Utils.Sleep(3)
            Guna2Button1.Enabled = True

        Catch ex As Exception
            Dim ModernMessage As Guna.UI2.WinForms.Guna2MessageDialog = New Guna.UI2.WinForms.Guna2MessageDialog With {.Parent = Me, .Icon = Guna.UI2.WinForms.MessageDialogIcon.Error,
                                         .Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK, .Style = Guna.UI2.WinForms.MessageDialogStyle.Dark, .Caption = "Package Installer", .Text = ex.Message}

            If ModernMessage.Show = DialogResult.OK Then
                Me.Close()
            End If
        End Try
    End Sub


    Public Sub GifPlayer(ByVal ImagePreview As Image, Optional ByVal IsGif As Boolean = False)
        If GifPlayerMon IsNot Nothing Then GifPlayerMon.Dispose()

        If IsGif = True Then
            GifPlayerMon = New GifPlayer(ImagePreview, True, True)
            GifPlayerMon.SetInterval(100)
            GifPlayerMon.Start()
        Else
            Guna2Panel2.BackgroundImage = ImagePreview
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

        Guna2Panel2.BackgroundImage = Imagen

    End Sub

#End Region

    Dim Rand As New Random

    Private Function GetImageFromMemory(FilePath As String) As Image
        Dim ba As Byte() = IO.File.ReadAllBytes(FilePath)
        Return XylatewarePro.Core.Helpers.Utils.ConvertbByteToImage(ba)
    End Function

    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        Guna2Button1.Visible = False
        Guna2ProgressBar1.Visible = True
        Install()
    End Sub

    Public Sub Install()
        Try
            Dim Path As String = "WallPaper" & Core.Helpers.Utils.RandomString(5)

            Dim SavePath As String = IO.Path.Combine(Core.Manage.Paths.CacheWallpaperPath, Path)

            If IO.Directory.Exists(SavePath) Then
                IO.Directory.Delete(SavePath)
            End If

            IO.Directory.CreateDirectory(SavePath)

            Core.ZipCore.ZipFileWithProgress.ExtractToDirectory(TargetFile, SavePath,
            New Progress(Of Double)(
     Sub(p)
         Dim Progress As Integer = Val(p * 100)
         If Progress <= 100 Then
             Guna2ProgressBar1.Value = Progress
         End If
     End Sub))

            Core.Helpers.Utils.Sleep(3)

            Dim ModernMessage As Guna.UI2.WinForms.Guna2MessageDialog = New Guna.UI2.WinForms.Guna2MessageDialog With {.Parent = Me, .Icon = Guna.UI2.WinForms.MessageDialogIcon.None,
                                          .Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK, .Style = Guna.UI2.WinForms.MessageDialogStyle.Dark, .Caption = "Package Installer", .Text = "The package was installed successfully."}

            If ModernMessage.Show = DialogResult.OK Then
                Me.Close()
            End If

            Me.Close()
        Catch ex As Exception
            Dim ModernMessage As Guna.UI2.WinForms.Guna2MessageDialog = New Guna.UI2.WinForms.Guna2MessageDialog With {.Parent = Me, .Icon = Guna.UI2.WinForms.MessageDialogIcon.Error,
                                           .Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK, .Style = Guna.UI2.WinForms.MessageDialogStyle.Dark, .Caption = "Package Installer", .Text = ex.Message}

            If ModernMessage.Show = DialogResult.OK Then
                Me.Close()
            End If
        End Try
    End Sub

End Class