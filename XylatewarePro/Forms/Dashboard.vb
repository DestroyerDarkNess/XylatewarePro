Public Class Dashboard

#Region " Declare "

    Private PageTarget As Core.Manage.PagedList(Of Core.Engine.WallpaperJsonLoader.WallpaperInfo) = Nothing
    Private ListenerEngine As New Core.Manage.ControlLister With {.OrientationControls = Orientation.Horizontal, .Margen = New Point(20, 10)}

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
        ' Panel1.BackColor = Color.FromArgb(80, 28, 29, 33)
        'Panel3.BackColor = Color.FromArgb(80, 21, 21, 21)
    End Sub

    Dim ProcessListener As Boolean = False


    Public Sub ListWallpalpers(Optional ByVal Search As String = "", Optional ByVal Back As Boolean = False)

        If ProcessListener = False Then
            ProcessListener = True
            PanelFX1.Controls.Clear()

            Dim SortItem As Integer = Core.Manage.Instances.MainUI.Guna2ComboBox1.SelectedIndex
            If Back = True Then Guna2TextBox1.Text = 1
            Dim PageIndex As Integer = Val(Guna2TextBox1.Text)

            Dim Asynctask As New Task(New Action(Async Sub()

                                                     Dim WallPs As Core.Engine.WallpaperJsonLoader = New Core.Engine.WallpaperJsonLoader

                                                     Dim WallpapersArray As List(Of Core.Engine.WallpaperJsonLoader.WallpaperInfo) = WallPs.Wallpapers

                                                     Dim TempWallpapersArray As New List(Of Core.Engine.WallpaperJsonLoader.WallpaperInfo)

                                                     For Each WallInfo As Core.Engine.WallpaperJsonLoader.WallpaperInfo In WallpapersArray

                                                         If WallInfo.LoadState = Core.Engine.WallpaperJsonLoader.StateLoaded.Loaded Then

                                                             If String.IsNullOrEmpty(Search) = False Then
                                                                 If LCase(WallInfo.ManifestJson.Title).Contains(LCase(Search)) = False Then Continue For
                                                             End If

                                                             Select Case SortItem
                                                                 Case 0
                                                                     TempWallpapersArray.Add(WallInfo)
                                                                 Case 1
                                                                     If Core.Helpers.Utils.IsVideoFormat(WallInfo.ManifestJson.FileName) = True Then TempWallpapersArray.Add(WallInfo)
                                                                 Case 2
                                                                     If Core.Helpers.Utils.IsImageFormat(WallInfo.ManifestJson.FileName) = True Then TempWallpapersArray.Add(WallInfo)
                                                                 Case 3
                                                                     If Core.Helpers.Utils.IsGifFormat(WallInfo.ManifestJson.FileName) = True Then TempWallpapersArray.Add(WallInfo)
                                                                 Case 4
                                                                     If Core.Helpers.Utils.IsWebFormat(WallInfo.ManifestJson.FileName) = True Then TempWallpapersArray.Add(WallInfo)
                                                             End Select
                                                         End If

                                                     Next

                                                     '   WallpapersArray.Clear()

                                                     If Me.PageTarget IsNot Nothing Then Me.PageTarget.Clear()

                                                     Me.PageTarget = Core.Manage.PagedList(Of Core.Engine.WallpaperJsonLoader.WallpaperInfo).Create(TempWallpapersArray.AsQueryable, PageIndex, 12)

                                                     Me.BeginInvoke(Sub()
                                                                        MaximunPage.Text = "/ " & PageTarget.TotalPages
                                                                    End Sub)

                                                     Dim ID As Integer = 0

                                                     For Each WallInfo As Core.Engine.WallpaperJsonLoader.WallpaperInfo In Me.PageTarget.ToList

                                                         Me.BeginInvoke(Sub()
                                                                            Me.BeginInvoke(Sub()
                                                                                               Dim NewProc As New PreviewCon With {.Name = ID, .WallpaperInfo = WallInfo, .Visible = False}
                                                                                               NewProc.Name = ID

                                                                                               ListenerEngine.Add(PanelFX1, NewProc, True)
                                                                                           End Sub)
                                                                            ID += 1
                                                                        End Sub)

                                                     Next

                                                     Me.BeginInvoke(Sub()
                                                                        UpdatePaginate()
                                                                    End Sub)

                                                     ProcessListener = False
                                                 End Sub), TaskCreationOptions.PreferFairness)
            Asynctask.Start()

        End If

    End Sub

    Private Sub Guna2Button3_Click(sender As Object, e As EventArgs) Handles Guna2Button3.Click
        Dim Page As Integer = Val(Guna2TextBox1.Text)
        Page += 1
        Guna2TextBox1.Text = Page
        ListWallpalpers(Core.Manage.Instances.MainUI.Guna2TextBox1.Text)
    End Sub

    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        Dim Page As Integer = Val(Guna2TextBox1.Text)
        If Page > 1 Then Page -= 1
        Guna2TextBox1.Text = Page
        ListWallpalpers(Core.Manage.Instances.MainUI.Guna2TextBox1.Text)
    End Sub

    Private Sub Guna2Button2_Click(sender As Object, e As EventArgs) Handles Guna2Button2.Click
        Guna2TextBox1.Text = 1
        ListWallpalpers(Core.Manage.Instances.MainUI.Guna2TextBox1.Text)
    End Sub

    Private Sub Guna2Button4_Click(sender As Object, e As EventArgs) Handles Guna2Button4.Click
        If PageTarget Is Nothing Then Exit Sub
        Guna2TextBox1.Text = PageTarget.TotalPages
        ListWallpalpers(Core.Manage.Instances.MainUI.Guna2TextBox1.Text)
    End Sub

    Private Sub Guna2TextBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles Guna2TextBox1.KeyDown
        If e.KeyCode = Keys.Enter Then
            If String.IsNullOrWhiteSpace(Guna2TextBox1.Text) = True Then
                Guna2TextBox1.Text = 1
            End If
            ListWallpalpers(Core.Manage.Instances.MainUI.Guna2TextBox1.Text)
        End If
    End Sub

    Private Sub Guna2TextBox1_TextChanged(sender As Object, e As EventArgs) Handles Guna2TextBox1.TextChanged
        UpdatePaginate()
    End Sub

    Private Sub UpdatePaginate()
        Dim Page As Integer = Val(Guna2TextBox1.Text)
        If PageTarget Is Nothing Then Exit Sub

        If Page = 1 Then
            Guna2Button2.Visible = False
            Guna2Button1.Visible = False
            If PageTarget.TotalPages > 1 Then
                Guna2Button3.Visible = True
                Guna2Button4.Visible = True
            End If
        ElseIf Page >= PageTarget.TotalPages Then
            Page = PageTarget.TotalPages
            Guna2Button3.Visible = False
            Guna2Button4.Visible = False
            If PageTarget.TotalPages > 1 Then
                Guna2Button2.Visible = True
                Guna2Button1.Visible = True
            End If
        Else
            Guna2Button1.Visible = True
            Guna2Button2.Visible = True
            Guna2Button3.Visible = True
            Guna2Button4.Visible = True
        End If

    End Sub

End Class