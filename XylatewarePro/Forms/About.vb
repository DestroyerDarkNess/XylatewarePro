Public Class About

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
        Label2.Text = Core.Manage.Instances.FileVer
    End Sub

    Private Sub Guna2Button2_Click(sender As Object, e As EventArgs) Handles Guna2Button2.Click
        Process.Start("https://www.reddit.com/r/Xylateware/")
    End Sub

    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        Process.Start("https://discord.gg/aE9Nv7bhfr")
    End Sub

    Private Sub Guna2Button3_Click(sender As Object, e As EventArgs) Handles Guna2Button3.Click
        Process.Start("https://www.paypal.com/paypalme/salvadorKrilewski")
    End Sub

    Private Sub Guna2Button4_Click(sender As Object, e As EventArgs) Handles Guna2Button4.Click
        Clipboard.SetText("0xb24864A01Ba4603b754C9b8E0dFb7F18B44c4503")
        AppNotify.MakeDialog(Me, "Adress Copied!", Color.Yellow)
    End Sub

End Class