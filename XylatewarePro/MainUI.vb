Imports XylatewarePro.Core

Public Class MainUI

#Region " Declare "
    '
    Public Home As Dashboard = New Dashboard With {.TopLevel = False, .Visible = False, .Dock = DockStyle.Fill, .FormBorderStyle = FormBorderStyle.None}
    Public WallStore As Store = New Store With {.TopLevel = False, .Visible = False, .Dock = DockStyle.Fill, .FormBorderStyle = FormBorderStyle.None}
    Public Youtube As Youtube '= New Youtube With {.TopLevel = False, .Visible = False, .Dock = DockStyle.Fill, .FormBorderStyle = FormBorderStyle.None}
    Public Studio As Studio = New Studio With {.TopLevel = False, .Visible = False, .Dock = DockStyle.Fill, .FormBorderStyle = FormBorderStyle.None}
    Public Settings As Settings = New Settings With {.TopLevel = False, .Visible = False, .FormBorderStyle = FormBorderStyle.None}
    Public About As About = New About With {.TopLevel = False, .Visible = False, .Dock = DockStyle.Fill, .FormBorderStyle = FormBorderStyle.None}


    Private UIScrool As Core.Manage.ScrollManager = Nothing
    Private ListenerUI As New Core.Manage.ControlLister With {.OrientationControls = Orientation.Vertical, .Margen = New Point(0, 0)}

#End Region


    Private Sub MainUI_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Core.Manage.Instances.MainUI = Me
        If Core.Manage.Instances.SilentMode = True Then
            Me.ShowIcon = False
            Me.WindowState = FormWindowState.Minimized
        End If
    End Sub

    Private Sub MainUI_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        If Core.Manage.Instances.SilentMode = True Then
            Me.Hide()
        End If

        Me.WindowState = FormWindowState.Normal
        Me.ShowIcon = True

        '  Dim ModernMessage As Guna.UI2.WinForms.Guna2MessageDialog = New Guna.UI2.WinForms.Guna2MessageDialog With {.Parent = Me, .Icon = Guna.UI2.WinForms.MessageDialogIcon.None,
        '.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OKCancel, .Style = Guna.UI2.WinForms.MessageDialogStyle.Light, .Caption = "Title", .Text = " Message"}

        '  If ModernMessage.Show = DialogResult.OK Then

        '  End If
    End Sub

#Region " UI "

    Public Async Function GUI() As Task(Of Boolean)
        ' UIScrool = New Core.Manage.ScrollManager(PanelFX1, {Guna2VScrollBar1}, True)
        PanelFX1.Controls.Add(Home)
        PanelFX1.Controls.Add(WallStore)

        PanelFX1.Controls.Add(Studio)
        PanelFX1.Controls.Add(Settings)
        PanelFX1.Controls.Add(About)

        DarshboardButton.Checked = True

        PanelFX1.HorizontalScroll.Visible = False
        PanelFX1.VerticalScroll.Visible = False

        PanelFX1.AutoScroll = True
        PanelFX1.HorizontalScroll.Visible = False
        PanelFX1.VerticalScroll.Visible = True

        Return True
    End Function

    'Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs)
    '    Select Case ComboBox1.SelectedIndex
    '        Case 0
    '            _themer.Apply(Themer.Themes.None)
    '        Case 1
    '            _themer.Apply(Themer.Themes.Acrylic)
    '        Case 2
    '            _themer.Apply(Themer.Themes.AeroGlass)
    '        Case 3
    '            _themer.Apply(Themer.Themes.Transparent)
    '    End Select
    'End Sub


    Private Sub Guna2TextBox1_LostFocus(sender As Object, e As EventArgs) Handles Guna2TextBox1.LostFocus
        Guna2Button1.Visible = False
    End Sub

    Private Sub Guna2TextBox1_TextChanged(sender As Object, e As EventArgs) Handles Guna2TextBox1.TextChanged
        If String.IsNullOrEmpty(Guna2TextBox1.Text) = True Then
            Guna2Button1.Visible = False
        Else
            If Guna2TextBox1.Focused = True Then Guna2Button1.Visible = True
        End If
    End Sub

    Private Sub DarshboardButton_CheckedChanged(sender As Object, e As EventArgs) Handles DarshboardButton.CheckedChanged, StoreButton.CheckedChanged, YoutubeButton.CheckedChanged, StudioButton.CheckedChanged, SettingsButton.CheckedChanged, AboutButton.CheckedChanged

        Dim ButtonSelected As Guna.UI2.WinForms.Guna2Button = DirectCast(sender, Guna.UI2.WinForms.Guna2Button)

        If ButtonSelected.Checked = False Then Exit Sub

        If UIScrool IsNot Nothing Then UIScrool.Dispose()

        If ButtonSelected Is DarshboardButton Then
            Home.Visible = True
            Home.BringToFront()
            WallStore.Visible = False
            If Youtube IsNot Nothing Then Youtube.Visible = False
            Studio.Visible = False
            Settings.Visible = False
            About.Visible = False

            Guna2ComboBox1.Items.Clear()
            Guna2ComboBox1.Items.AddRange({"All Types", "Video", "Image", "Gif", "Web"})
            Guna2ComboBox1.SelectedIndex = 0
            Guna2ComboBox1.Visible = True
            UIScrool = New Core.Manage.ScrollManager(Home.PanelFX1, {Guna2VScrollBar1}, True)

        ElseIf ButtonSelected Is StoreButton Then
            WallStore.Visible = True
            WallStore.BringToFront()
            Home.Visible = False
            If Youtube IsNot Nothing Then Youtube.Visible = False
            Studio.Visible = False
            Settings.Visible = False
            About.Visible = False

            Guna2ComboBox1.Items.Clear()
            Guna2ComboBox1.Items.AddRange({"Home", "Latest", "Hot", "Toplist"})
            Guna2ComboBox1.SelectedIndex = 0
            Guna2ComboBox1.Visible = True
            UIScrool = New Core.Manage.ScrollManager(WallStore.PanelFX1, {Guna2VScrollBar1}, True)

        ElseIf ButtonSelected Is YoutubeButton Then

            If Youtube Is Nothing Then
                Dim ModernMessage As Guna.UI2.WinForms.Guna2MessageDialog = New Guna.UI2.WinForms.Guna2MessageDialog With {.Parent = Me, .Icon = Guna.UI2.WinForms.MessageDialogIcon.None,
              .Buttons = Guna.UI2.WinForms.MessageDialogButtons.OKCancel, .Style = Guna.UI2.WinForms.MessageDialogStyle.Dark, .Caption = "WebView Notify", .Text = "Do you want to Attach a Browser to the application to access YouTube?

This will consume more ram."}

                If ModernMessage.Show = DialogResult.OK Then
                    Youtube = New Youtube With {.TopLevel = False, .Visible = False, .Dock = DockStyle.Fill, .FormBorderStyle = FormBorderStyle.None}
                    PanelFX1.Controls.Add(Youtube)
                End If
            End If

            If Youtube IsNot Nothing Then
                Youtube.Visible = True
                Youtube.BringToFront()
                Home.Visible = False
                WallStore.Visible = False
                Studio.Visible = False
                Settings.Visible = False
                About.Visible = False

                Guna2ComboBox1.Items.Clear()
                Guna2ComboBox1.Visible = False

                UIScrool = New Core.Manage.ScrollManager(Youtube.PanelFX1, {Guna2VScrollBar1}, True)
            End If


        ElseIf ButtonSelected Is StudioButton Then
                Studio.Visible = True
                Studio.BringToFront()
                Home.Visible = False
                WallStore.Visible = False
                If Youtube IsNot Nothing Then Youtube.Visible = False
                Settings.Visible = False
                About.Visible = False

                Guna2ComboBox1.Items.Clear()
                Guna2ComboBox1.Visible = False

                UIScrool = New Core.Manage.ScrollManager(Studio.PanelFX1, {Guna2VScrollBar1}, True)

            ElseIf ButtonSelected Is SettingsButton Then
                Settings.Visible = True
                Settings.BringToFront()
                Home.Visible = False
                WallStore.Visible = False
                If Youtube IsNot Nothing Then Youtube.Visible = False
                Studio.Visible = False
                About.Visible = False

                Guna2ComboBox1.Items.Clear()
                Guna2ComboBox1.Visible = False

                UIScrool = New Core.Manage.ScrollManager(PanelFX1, {Guna2VScrollBar1}, True)

            ElseIf ButtonSelected Is AboutButton Then
                About.Visible = True
            About.BringToFront()
            Home.Visible = False
            WallStore.Visible = False
            If Youtube IsNot Nothing Then Youtube.Visible = False
            Studio.Visible = False
            Settings.Visible = False

            Guna2ComboBox1.Items.Clear()
            Guna2ComboBox1.Visible = False

            UIScrool = New Core.Manage.ScrollManager(WallStore.PanelFX1, {Guna2VScrollBar1}, True)

        End If

    End Sub

    Public Sub MediaPreview(ByVal Visible As Boolean, Optional ByVal Container As Control = Nothing)

        MediaContainer.Controls.Clear()

        If Container IsNot Nothing Then
            MediaContainer.Controls.Add(Container)
            Container.Visible = True
        End If

        MediaContainer.Visible = Visible
        If Visible = True Then
            Me.Width = 1152
        Else
            Me.Width = 916
        End If
    End Sub

    Private Sub Guna2TextBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles Guna2TextBox1.KeyDown
        If e.KeyCode = Keys.Enter Then
            WallStore.IsLoaded = False
            UpdateContent()
        End If
    End Sub

    Private Sub Guna2Button2_Click(sender As Object, e As EventArgs) Handles Guna2Button2.Click
        WallStore.IsLoaded = False
        UpdateContent()
    End Sub

    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        WallStore.IsLoaded = False
        UpdateContent()
    End Sub

    Private Sub Guna2ComboBox1_Click(sender As Object, e As EventArgs) Handles Guna2ComboBox1.Click
        WallStore.IsLoaded = False
    End Sub

    Private Sub Guna2ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Guna2ComboBox1.SelectedIndexChanged
        UpdateContent()
    End Sub

    Public Sub UpdateContent(Optional ByVal ForceUpdate As Boolean = False)
        If DarshboardButton.Checked = True Then
            Home.ListWallpalpers(Guna2TextBox1.Text, True)
        ElseIf StoreButton.Checked = True Then

            If WallStore.IsLoaded = False Then WallStore.ListWallpalpers(Guna2TextBox1.Text, True)

        ElseIf YoutubeButton.Checked = True Then
            If Youtube IsNot Nothing Then Youtube.Search(Guna2TextBox1.Text)
        End If
    End Sub

#End Region

    Private Sub ShowToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ShowToolStripMenuItem.Click
        ShowHideMenu()
    End Sub

    Private Sub NotifyIcon1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles NotifyIcon1.MouseDoubleClick
        ShowHideMenu()
    End Sub

    Private Sub Guna2ControlBox1_Click(sender As Object, e As EventArgs) Handles Guna2ControlBox1.Click
        ShowHideMenu()
    End Sub

    Private Sub ShowHideMenu()
        If Me.Visible = True Then
            Me.Hide()
            ShowToolStripMenuItem.Text = "Show"
        Else
            Me.Show()
            Me.WindowState = FormWindowState.Normal
            ShowToolStripMenuItem.Text = "Hide"
        End If
    End Sub

    Private Sub CloseWallpapersToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CloseWallpapersToolStripMenuItem.Click
        Core.Engine.DesktopEmbeder.RefreshDesktop()
        If Core.Manage.Instances.LastEngine IsNot Nothing Then
            Try
                Core.Manage.Instances.LastEngine.Kill()
            Catch ex As Exception

            End Try
        End If
        Core.Engine.DesktopEmbeder.RefreshDesktop()
    End Sub

    Private Sub EndToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles EndToolStripMenuItem1.Click
        Environment.Exit(0)
    End Sub

End Class