Public Class Store

#Region " Declare "

    Public WallpaperAPI As Core.Wallpaper.WallHaven = New Core.Wallpaper.WallHaven(Me)
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

    Private Sub Store_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Store_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        StartUI()
    End Sub

    Public Sub StartUI()
        ' Panel1.BackColor = Color.FromArgb(80, 28, 29, 33)
        'Panel3.BackColor = Color.FromArgb(80, 21, 21, 21)
    End Sub

    Private Sub Set_Progress(ByVal Value As Integer)
        Try
            Me.BeginInvoke(Sub()
                               Guna2ProgressBar1.Value = Value
                           End Sub)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub SetFailed(ByVal ProgressColor As Color, Optional ByVal VisibleContainer As Boolean = False)
        Try
            Me.BeginInvoke(Sub()
                               Guna2ProgressBar1.ProgressColor = ProgressColor
                               Guna2ProgressBar1.ProgressColor2 = ProgressColor
                               PanelFX1.Visible = VisibleContainer
                               Label1.Visible = True
                               Guna2Button5.Visible = True
                           End Sub)
            Application.DoEvents()
        Catch ex As Exception

        End Try
    End Sub

    Dim ProcessListener As Boolean = False

    Public Sub ListWallpalpers(Optional ByVal Search As String = "", Optional ByVal Back As Boolean = False)

        If ProcessListener = False Then
            ProcessListener = True
            PanelFX1.Controls.Clear()
            Guna2ProgressBar1.ProgressColor = Color.Cyan
            Guna2ProgressBar1.ProgressColor2 = Color.DeepSkyBlue
            Guna2ProgressBar1.Value = 0
            PanelFX1.Visible = True


            Dim SortItem As Integer = Core.Manage.Instances.MainUI.Guna2ComboBox1.SelectedIndex
            If Back = True Then Guna2TextBox1.Text = 1
            Dim WallPage As Integer = Val(Guna2TextBox1.Text)

            Dim Asynctask As New Task(New Action(Async Sub()


                                                     Dim UrlBase As String = WallpaperAPI.Random

                                                     If Search = "" Then
                                                         Select Case SortItem
                                                             Case 0 : UrlBase = WallpaperAPI.MakePageUrl(WallpaperAPI.Random, WallPage)
                                                             Case 1 : UrlBase = WallpaperAPI.MakePageUrl(WallpaperAPI.Latest, WallPage)
                                                             Case 2 : UrlBase = WallpaperAPI.MakePageUrl(WallpaperAPI.Hot, WallPage)
                                                             Case 3 : UrlBase = WallpaperAPI.MakePageUrl(WallpaperAPI.Toplist, WallPage)
                                                         End Select
                                                     Else

                                                         UrlBase = WallpaperAPI.MakeSearch(Search, WallPage)

                                                     End If

                                                     Set_Progress(20)

                                                     Dim HTML_Source As String = WallpaperAPI.GetSourcePage(UrlBase)

                                                     Set_Progress(40)


                                                     If HTML_Source = String.Empty Then

                                                         SetFailed(Color.Red)

                                                         ProcessListener = False

                                                         Me.BeginInvoke(Sub()
                                                                            Dim ModernMessage As Guna.UI2.WinForms.Guna2MessageDialog = New Guna.UI2.WinForms.Guna2MessageDialog With {.Parent = Core.Manage.Instances.MainUI, .Icon = Guna.UI2.WinForms.MessageDialogIcon.Error,
                                                                            .Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK, .Style = Guna.UI2.WinForms.MessageDialogStyle.Light, .Caption = "Web Error", .Text = WallpaperAPI.ErrInfo}
                                                                            ModernMessage.Show()
                                                                        End Sub)

                                                     Else


                                                         Set_Progress(50)

                                                         Dim GetImages As List(Of Core.Wallpaper.WallHaven.Info) = WallpaperAPI.GetImages(HTML_Source)

                                                         Set_Progress(60)

                                                         If GetImages Is Nothing Then

                                                             SetFailed(Color.Orange)
                                                             ProcessListener = False

                                                         Else

                                                             Set_Progress(80)

                                                             Dim ID As Integer = 0
                                                             For Each WallInfo As Core.Wallpaper.WallHaven.Info In GetImages

                                                                 Me.BeginInvoke(Sub()
                                                                                    Me.BeginInvoke(Sub()
                                                                                                       If Not Search = "" Then WallInfo.Name = Search

                                                                                                       Dim NewProc As New PreviewWallpaper With {.Name = ID, .Wallpaper = WallInfo, .Visible = False}
                                                                                                       NewProc.Name = ID

                                                                                                       ListenerEngine.Add(PanelFX1, NewProc, True)
                                                                                                   End Sub)
                                                                                    ID += 1
                                                                                End Sub)

                                                             Next

                                                             Set_Progress(100)
                                                             Core.Helpers.Utils.Sleep(500, Core.Helpers.Utils.Measure.Milliseconds)
                                                             Set_Progress(0)

                                                         End If

                                                     End If

                                                     IsLoaded = True

                                                     ProcessListener = False
                                                 End Sub), TaskCreationOptions.PreferFairness)
            Asynctask.Start()

        End If

    End Sub

    Public Property IsLoaded As Boolean = False


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

    Private Sub Guna2TextBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles Guna2TextBox1.KeyDown
        If e.KeyCode = Keys.Enter Then
            If String.IsNullOrWhiteSpace(Guna2TextBox1.Text) = True Then
                Guna2TextBox1.Text = 1
            End If
            ListWallpalpers(Core.Manage.Instances.MainUI.Guna2TextBox1.Text)
        End If
    End Sub

    Private Sub Guna2Button5_Click(sender As Object, e As EventArgs) Handles Guna2Button5.Click
        ListWallpalpers(Core.Manage.Instances.MainUI.Guna2TextBox1.Text)
    End Sub

End Class