Public Class Youtube

#Region " Declare "

#End Region

    Public Sub New()
        SetStyle(ControlStyles.SupportsTransparentBackColor, True)
        '  This call Is required by the designer.
        InitializeComponent()

        '   Add any initialization after the InitializeComponent() call.
        Me.BackColor = Color.Transparent
    End Sub

    Private Sub Dashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Dashboard_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        For i As Integer = 0 To 5
            WebView1.LoadUrl("https://www.youtube.com/")
            Core.Helpers.Utils.Sleep(1)
        Next
    End Sub

    Public Sub StartUI()
        WebView1.LoadUrl("https://www.youtube.com/")
    End Sub

    Public Sub Search(ByVal UrlParser As String)

        If UrlParser = "" Then
            If WebView1.Url = "" Then
                StartUI()
            Else
                WebView1.Reload()
            End If
        ElseIf UrlParser.StartsWith("http") = True Or UrlParser.StartsWith("www") = True Then
            Dim HostAddres As String = LCase(New Uri(UrlParser).Host)

            If HostAddres = "m.youtube.com" Or HostAddres = "www.youtube.com" Or HostAddres = "www.youtube.be" Or HostAddres = "consent.youtube.com" Then
                WebView1.Url = UrlParser
            Else
                WebView1.Url = "https://www.youtube.com/results?search_query=" & UrlParser
            End If
        Else
            WebView1.Url = "https://www.youtube.com/results?search_query=" & UrlParser
        End If

    End Sub


#Region " Browser Func "

    Dim IsDialog As Boolean = False
    Dim VideoTitle As String = String.Empty

    Private Sub WebView1_NewWindow(sender As Object, e As EO.WebBrowser.NewWindowEventArgs) Handles WebView1.NewWindow
        Dim HostAddres As String = LCase(New Uri(e.TargetUrl).Host)

        If HostAddres = "m.youtube.com" Or HostAddres = "www.youtube.com" Or HostAddres = "www.youtube.be" Or HostAddres = "consent.youtube.com" Then
            WebView1.Url = e.TargetUrl
        Else
            AppNotify.MakeDialog(Me, "Only youtube domain is accepted.", Color.OrangeRed)
            e.Accepted = False
        End If

    End Sub


    Dim CuurrentPageLoaded As String = String.Empty
    Dim OpenMiniature As Boolean = False
    Dim SkipLoop As String = String.Empty
    Dim LoopCount As Integer = 0

    Public Sub PauseVideo()
        WebView1.EvalScript(Core.JavaScripts.PauseVideo)
        WebView1.EvalScript(Core.JavaScripts.PauseVideo2)
    End Sub

    Private Sub WebView1_UrlChanged(sender As Object, e As EventArgs) Handles WebView1.UrlChanged

        Dim Adress As String = WebView1.Url
        '  Core.Manage.Instances.MainUI.Guna2TextBox1.Text = Adress
        Dim IDAddres As String = Core.Helpers.Utils.getID(Adress).ToString
        If Not IDAddres = String.Empty Then

            Dim MPreview As WallPaperPreview = New WallPaperPreview With {.Visible = False, .Dock = DockStyle.Fill}
            MPreview.Youtube = Adress
            Core.Manage.Instances.MainUI.MediaPreview(True, MPreview)

        End If

        If Adress = "about:blank" Then

            StartUI()

        Else

            If Not Adress = String.Empty Then
                Dim HostAddres As String = New Uri(WebView1.Url).Host

                If HostAddres = "m.youtube.com" Or HostAddres = "www.youtube.com" Or HostAddres = "www.youtube.be" Then

                    If WebView1.CustomUserAgent = Settings.MobileUserAgent Then
                        Dim UserAgent As String = Settings.PCUserAgent

                        WebView1.CustomUserAgent = UserAgent
                        WebView1.Reload()
                    End If

                    If LCase(Adress).Contains("#dialog") = True Then
                        IsDialog = True
                    ElseIf LCase(Adress).Contains("#menu") = True Then
                        IsDialog = True
                    ElseIf LCase(Adress).Contains("#searching") = True Then
                        IsDialog = True
                    Else
                        IsDialog = False
                    End If

                    If IsDialog = False Then
                        If LCase(Adress).Contains("watch") = True Then


                        End If

                    End If

                ElseIf HostAddres = "accounts.google.com" Then

                    If WebView1.CustomUserAgent = Settings.PCUserAgent Then
                        Dim UserAgent As String = Settings.MobileUserAgent

                        WebView1.CustomUserAgent = UserAgent
                        WebView1.Reload()
                    End If

                ElseIf HostAddres = "consent.youtube.com" Then

                    AcceptCookie = True

                Else

                    AppNotify.MakeDialog(Me, "Only youtube domain is accepted.", Color.OrangeRed)
                    WebView1.StopLoad()

                End If
            End If
        End If

    End Sub

    Dim AcceptCookie As Boolean = False

    Private Sub WebView1_LoadCompleted(sender As Object, e As EO.WebBrowser.LoadCompletedEventArgs) Handles WebView1.LoadCompleted
        On Error Resume Next

        Dim Adress As String = WebView1.Url

        If LCase(Adress).Contains("watch") = True Then

            Dim MPreview As WallPaperPreview = New WallPaperPreview With {.Visible = False, .Dock = DockStyle.Fill}
            MPreview.Youtube = Adress
            Core.Manage.Instances.MainUI.MediaPreview(True, MPreview)

            '    WebView1.EvalScript(Core.JavaScripts.TitleExtractor)

            'If ReadIni("Settings", "Adsblock1", False) = True Then
            '    WebView1.EvalScript(Core.JavaScripts.AdsBlock)
            'End If

            'If ReadIni("Settings", "Adsblock3", False) = True Then
            '    WebView1.EvalScript(Core.JavaScripts.AdsBlock3)
            'End If

            'If ReadIni("Settings", "SkipAds", False) = True Then
            '    WebView1.EvalScript(Core.JavaScripts.NoGoogleAds)
            '    WebView1.EvalScript(Core.JavaScripts.SkipYoutubeAds)
            '    WebView1.EvalScript(Core.JavaScripts.SkipYoutubeAds2)
            'End If

            'If ReadIni("Settings", "BypassAds", False) = True Then
            '    WebView1.EvalScript(Core.JavaScripts.BypassYoutubeAds)
            'End If

            WebView1.EvalScript(Core.JavaScripts.Unmute)

        End If

        Dim doc As EO.WebBrowser.DOM.Document = WebView1.GetDOMWindow().document

        If OpenMiniature = True Then
            VideoTitle = doc.title.Replace(" - YouTube", "")
        End If

        If AcceptCookie = True Then

            WebView1.EvalScript(Core.JavaScripts.UBlock)
            WebView1.EvalScript(Core.JavaScripts.AcceptCookies)

            AcceptCookie = False
        End If

    End Sub


    Private Sub WebView1_LoadFailed(sender As Object, e As EO.WebBrowser.LoadFailedEventArgs) Handles WebView1.LoadFailed
        Dim ErrorMessage As String = e.ErrorMessage
        Dim ErrorCode As String = e.ErrorCode

        If Not ErrorCode = "-3" Then
            AppNotify.MakeDialog(Me, ErrorMessage, Color.Red)
        End If

    End Sub

#End Region

End Class