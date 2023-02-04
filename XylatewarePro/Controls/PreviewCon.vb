Public Class PreviewCon


#Region " Properties "

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

#End Region

    Private Sub PreviewCon_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Panel1.BackColor = Color.FromArgb(80, 24, 24, 24)
        Load()
        Guna.UI2.WinForms.Helpers.GraphicsHelper.DrawLineShadow(Guna2Panel1, Color.Black, 100, 30, Guna.UI2.WinForms.Enums.VerHorAlign.HorizontalBottom)
    End Sub

    Public Sub Load()
        If WallInfo IsNot Nothing Then
            Label1.Text = WallInfo.ManifestJson.Title

            Dim ThumbnailEx As String = IO.Path.Combine(WallInfo.FullPath, WallInfo.ManifestJson.Thumbnail)

            If IO.File.Exists(ThumbnailEx) Then
                Guna2Panel1.BackgroundImage = Core.Helpers.Utils.Resize_Image(System.Drawing.Image.FromFile(ThumbnailEx), Me.Width, Me.Height)
            End If

            If Label1.Text = String.Empty Then
                Panel1.Visible = False
            End If
        End If
    End Sub


    Public Function GetExtension() As String
        Return IO.Path.GetFileName(WallInfo.ManifestJson.FileName).Split(".").LastOrDefault.ToUpper.ToString
    End Function

    Private Sub Mouse_Click(sender As Object, e As EventArgs) Handles Me.Click, Guna2Panel1.Click, Panel1.Click, Label1.Click
        ' DirectCast(Me.ParentForm, Home).PlayerPreviewEx.StartPreview(Me)
        Dim MPreview As WallPaperPreview = New WallPaperPreview With {.Visible = False, .Dock = DockStyle.Fill}
        MPreview.WallpaperInfo = WallInfo
        Core.Manage.Instances.MainUI.MediaPreview(True, MPreview)
    End Sub

    Private Sub Home_MouseHover(sender As Object, e As EventArgs) Handles Me.MouseHover, Guna2Panel1.MouseHover, Panel1.MouseHover, Label1.MouseHover
        DrawBorder(Color.DodgerBlue)

    End Sub

    Private Sub Home_MouseLeave(sender As Object, e As EventArgs) Handles Me.MouseLeave, Guna2Panel1.MouseLeave, Panel1.MouseLeave, Label1.MouseLeave
        DrawBorder(Color.White)

    End Sub


    Private Sub DrawBorder(ByVal Color As Color)
        Guna2Panel1.BorderColor = Color
    End Sub

    Private Sub ShowHideToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ShowHideToolStripMenuItem.Click
        Try
            IO.File.Delete(WallInfo.ManifestJson.FilePathJson)
            Core.Manage.Instances.MainUI.UpdateContent()
        Catch ex As Exception
            AppNotify.MakeDialog(Me, ex.Message, Color.Red)
        End Try
    End Sub

    Private Sub EndToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EndToolStripMenuItem.Click
        Process.Start("explorer.exe", "/select, " & WallInfo.ManifestJson.FilePathJson)
    End Sub

End Class
