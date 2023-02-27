Public Class Settings

#Region " Declare "

    Private PageTarget As Core.Manage.PagedList(Of Core.Engine.WallpaperJsonLoader.WallpaperInfo) = Nothing
    Private ListenerEngine As New Core.Manage.ControlLister With {.OrientationControls = Orientation.Horizontal, .Margen = New Point(20, 10)}

    Public Shared MobileUserAgent As String = "Mozilla/5.0 (Linux; Android 5.0; SM-G900P Build/LRX21T) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/72.0.3626.109 Mobile Safari/537.36"
    Public Shared PCUserAgent As String = "Mozilla/5.0 (Windows NT 10.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.114 Safari/537.36"


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

    Dim GuiLoaded As Boolean = False

    Private Sub Dashboard_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        StartUI()
    End Sub

    Public Sub StartUI()
        Try
            Guna2ToggleSwitch1.Checked = Program.AppInStartup
            Dim TaskStyle As Integer = Core.Helpers.Utils.ReadIni("Settings", "TaskbarStyle", 0)
            TaskbarX.TaskbarStyle.TaskbarStyler(TaskStyle)
            Guna2ComboBox1.SelectedIndex = TaskStyle
            Guna2ComboBox7.SelectedIndex = Core.Helpers.Utils.ReadIni("Settings", "YoutubePlayer", 0)
            Guna2ComboBox6.SelectedIndex = Core.Helpers.Utils.ReadIni("Settings", "YoutubeBars", 0)
            Guna2ComboBox5.SelectedIndex = Core.Helpers.Utils.ReadIni("Settings", "SysEmbed", 0)
            Guna2ComboBox4.SelectedIndex = Core.Helpers.Utils.ReadIni("Settings", "GifEngine", 0)
            Guna2ComboBox2.SelectedIndex = Core.Helpers.Utils.ReadIni("Settings", "VideoEngine", 0)
            Guna2ComboBox3.SelectedIndex = Core.Helpers.Utils.ReadIni("Settings", "WebEngine", 0)
            '   Guna2ComboBox4.SelectedIndex = Core.Helpers.Utils.ReadIni("Settings", "ExeEngine", 0)
        Catch ex As Exception
            Reset()
        End Try

        GuiLoaded = True
    End Sub

    Public Sub LoadSettingsNoUI()
        Dim TaskStyle As Integer = Core.Helpers.Utils.ReadIni("Settings", "TaskbarStyle", 0)
        TaskbarX.TaskbarStyle.TaskbarStyler(TaskStyle)
    End Sub

    Private Sub Reset()
        Core.Helpers.Utils.WriteIni("Settings", "YoutubeBars", 0)
        Core.Helpers.Utils.WriteIni("Settings", "SysEmbed", 0)
        Core.Helpers.Utils.WriteIni("Settings", "GifEngine", 0)
        Core.Helpers.Utils.WriteIni("Settings", "VideoEngine", 0)
        Core.Helpers.Utils.WriteIni("Settings", "WebEngine", 0)
        Guna2ComboBox7.SelectedIndex = Core.Helpers.Utils.ReadIni("Settings", "YoutubePlayer", 0)
        Guna2ComboBox6.SelectedIndex = Core.Helpers.Utils.ReadIni("Settings", "YoutubeBars", 0)
        Guna2ComboBox5.SelectedIndex = Core.Helpers.Utils.ReadIni("Settings", "SysEmbed", 0)
        Guna2ComboBox4.SelectedIndex = Core.Helpers.Utils.ReadIni("Settings", "GifEngine", 0)
        Guna2ComboBox2.SelectedIndex = Core.Helpers.Utils.ReadIni("Settings", "VideoEngine", 0)
        Guna2ComboBox3.SelectedIndex = Core.Helpers.Utils.ReadIni("Settings", "WebEngine", 0)
    End Sub


    Private Sub Guna2ToggleSwitch1_CheckedChanged(sender As Object, e As EventArgs) Handles Guna2ToggleSwitch1.CheckedChanged
        If GuiLoaded = True Then
            If XylatewarePro.Core.Helpers.Utils.IsAdmin() = False Then
                Dim OpenEx As Boolean = Core.Helpers.Utils.OpenAsAdmin(Application.ExecutablePath, "-startup " & Guna2ToggleSwitch1.Checked.ToString.ToLower)
            Else
                Dim OpenEx As Boolean = Program.AppInStartup(Guna2ToggleSwitch1.Checked)
            End If
        End If
    End Sub

    Private Sub Guna2ComboBox4_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Guna2ComboBox4.SelectedIndexChanged
        If GuiLoaded = True Then
            Core.Helpers.Utils.WriteIni("Settings", "GifEngine", Guna2ComboBox4.SelectedIndex)
        End If
    End Sub

    Private Sub Guna2ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Guna2ComboBox2.SelectedIndexChanged
        If GuiLoaded = True Then
            Core.Helpers.Utils.WriteIni("Settings", "VideoEngine", Guna2ComboBox2.SelectedIndex)
        End If
    End Sub

    Private Sub Guna2ComboBox3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Guna2ComboBox3.SelectedIndexChanged
        If GuiLoaded = True Then
            Core.Helpers.Utils.WriteIni("Settings", "WebEngine", Guna2ComboBox3.SelectedIndex)
            If Guna2ComboBox3.SelectedIndex = 2 Then
                AppNotify.MakeDialog(Core.Manage.Instances.MainUI, "You need to Install Microsoft's Webview2 Runtime: " & "https://developer.microsoft.com/en-us/microsoft-edge/webview2/", Color.Orange)
            End If
        End If
    End Sub

    Private Sub Guna2ComboBox5_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Guna2ComboBox5.SelectedIndexChanged
        If GuiLoaded = True Then
            Core.Helpers.Utils.WriteIni("Settings", "SysEmbed", Guna2ComboBox5.SelectedIndex)
        End If
    End Sub

    Private Sub Guna2ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Guna2ComboBox1.SelectedIndexChanged
        If GuiLoaded = True Then
            Core.Helpers.Utils.WriteIni("Settings", "TaskbarStyle", Guna2ComboBox1.SelectedIndex)
            TaskbarX.TaskbarStyle.TaskbarStyler(Guna2ComboBox1.SelectedIndex)
        End If
    End Sub

    Private Sub Guna2ComboBox6_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Guna2ComboBox6.SelectedIndexChanged
        If GuiLoaded = True Then
            Core.Helpers.Utils.WriteIni("Settings", "YoutubeBars", Guna2ComboBox6.SelectedIndex)
        End If
    End Sub

    Private Sub Guna2ComboBox7_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Guna2ComboBox7.SelectedIndexChanged
        If GuiLoaded = True Then
            Core.Helpers.Utils.WriteIni("Settings", "YoutubePlayer", Guna2ComboBox7.SelectedIndex)
        End If
    End Sub

End Class