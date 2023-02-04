Public Class PreviewWallpaper


#Region " Properties "

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

#End Region

    Private Sub PreviewCon_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If InfoEx IsNot Nothing Then
            Guna2Panel1.BorderThickness = 0
            Guna2Panel1.FillColor = Color.FromArgb(80, 17, 17, 17)
            LoadImage()
        End If
    End Sub

    Private Sub Guna2Button5_Click(sender As Object, e As EventArgs) Handles Guna2Button5.Click
        LoadImage()
    End Sub

    Public Sub LoadImage()
        Dim Asynctask As New Task(New Action(Async Sub()
                                                 'DownLoadImage:
                                                 Dim ImageAwait As Image = Await Core.Helpers.Utils.DownloadImageAsync(InfoEx.Miniature)

                                                 If ImageAwait Is Nothing Then
                                                     ' GoTo DownLoadImage
                                                     Guna2Button5.Visible = True
                                                 End If

                                                 Try
                                                     Me.BeginInvoke(Sub()
                                                                        Guna2Panel1.BackgroundImage = ImageAwait
                                                                    End Sub)
                                                 Catch ex As Exception
                                                     Me.Visible = False
                                                     Exit Sub
                                                 End Try


                                             End Sub), TaskCreationOptions.PreferFairness)
        Asynctask.Start()
    End Sub

    Private Sub Mouse_Click(sender As Object, e As EventArgs) Handles Me.Click, Guna2Panel1.Click
        'DirectCast(Me.ParentForm, Store).WallpaperPreviewEx.StartPreview(Me)
        If Me.Guna2Panel1.BackgroundImage Is Nothing Then Exit Sub
        Dim MPreview As WallPaperPreview = New WallPaperPreview With {.Visible = False, .Dock = DockStyle.Fill}

        MPreview.PanelFX1.BackgroundImage = Me.Guna2Panel1.BackgroundImage.Clone
        Core.Helpers.BlurEffect.BlurBitmap(MPreview.PanelFX1.BackgroundImage)

        MPreview.Wallpaper = InfoEx
        Core.Manage.Instances.MainUI.MediaPreview(True, MPreview)
    End Sub

    Private Sub Home_MouseHover(sender As Object, e As EventArgs) Handles Me.MouseHover, Guna2Panel1.MouseHover
        DrawBorder(Color.DodgerBlue)
        Guna2Panel1.BorderThickness = 1
        Guna2Panel1.FillColor = Nothing
    End Sub

    Private Sub Home_MouseLeave(sender As Object, e As EventArgs) Handles Me.MouseLeave, Guna2Panel1.MouseLeave
        DrawBorder(Color.White)
        Guna2Panel1.BorderThickness = 0
        Guna2Panel1.FillColor = Color.FromArgb(80, 17, 17, 17)
    End Sub

    Private Sub DrawBorder(ByVal Color As Color)
        Guna2Panel1.BorderColor = Color
    End Sub

End Class
