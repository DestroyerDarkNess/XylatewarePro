Imports System.IO
Imports System.Net

Namespace Core.Wallpaper
    Public Class WallHaven

#Region " Declare "

        Public HomeUrl As String = "https://wallhaven.cc/"

        Public Latest As String = HomeUrl & "latest"
        Public Hot As String = HomeUrl & "hot"
        Public Toplist As String = HomeUrl & "toplist"
        Public Random As String = HomeUrl & "random"

        Private ParentForm As Form = Nothing


#End Region

#Region " Constructor "

        Public Sub New(Optional Parent As Form = Nothing)
            ParentForm = Parent
        End Sub

#End Region

#Region " Public Methods "

        Public Function MakeSearch(ByVal Text As String, Optional ByVal Page As Integer = 1) As String
            Return HomeUrl & "search?q=" & Text.Replace(" ", "+") & "&page=" & Page
        End Function

        Public Function MakePageUrl(ByVal BaseUrl As String, Optional ByVal Page As Integer = 1) As String
            Return BaseUrl & "?page=" & Page
        End Function

        Public Function GetHomeImages(ByVal Source As String) As List(Of Info)
            Dim ListInfo As New List(Of Info)
            Dim HTMLParse As HtmlDocument = StringToHtmlDocument(Source)

            Dim Previewexa As HtmlElementCollection = HTMLParse.GetElementsByTagName("span")

            For Each Preview As HtmlElement In Previewexa

                Try
                    Dim PreviewUrl As String = Preview.GetElementsByTagName("a").Item(0).GetAttribute("href").ToString

                    Dim Minimized As String = Preview.GetElementsByTagName("img").Item(0).GetAttribute("src").ToString

                    If String.IsNullOrEmpty(PreviewUrl) = False Then

                        ListInfo.Add(New Info(Minimized, PreviewUrl))

                    End If

                Catch ex As Exception

                End Try

            Next

            If ListInfo.Count = 0 Then
                Return Nothing
            Else
                Return ListInfo
            End If
        End Function

        Public Function GetImages(ByVal Source As String) As List(Of Info)
            Dim ListInfo As New List(Of Info)
            Dim HTMLParse As HtmlDocument = StringToHtmlDocument(Source)

            Dim PreCollection As HtmlElementCollection = HTMLParse.GetElementsByTagName("li")

            For Each PreElement As HtmlElement In PreCollection

                Try

                    Dim PreviewUrl As String = PreElement.GetElementsByTagName("a").Item(0).GetAttribute("href").ToString
                    Dim Minimized As String = PreElement.GetElementsByTagName("img").Item(0).GetAttribute("data-src").ToString

                    If String.IsNullOrEmpty(PreviewUrl) = False Then

                        ListInfo.Add(New Info(Minimized, PreviewUrl))

                    End If

                Catch ex As Exception

                End Try

            Next

            If ListInfo.Count = 0 Then
                Return Nothing
            Else
                Return ListInfo
            End If
        End Function

        Public Function GetImageInfo(ByVal Source As String) As ImageInfo
            Try
                Dim HTMLParse As HtmlDocument = StringToHtmlDocument(Source)

                Dim WallDocument As HtmlElement = HTMLParse.GetElementById("wallpaper")

                Dim WallpaperUrl As String = WallDocument.GetAttribute("src")

                Dim WallpaperResX As String = Val(WallDocument.GetAttribute("data-wallpaper-width").ToString)
                Dim WallpaperResY As String = Val(WallDocument.GetAttribute("data-wallpaper-height").ToString)

                Dim WallpaerResolution As Size = New Size(WallpaperResX, WallpaperResY)

                Return New ImageInfo(WallpaperUrl, WallpaerResolution)
            Catch ex As Exception
                Return Nothing
            End Try
        End Function

        Public Property ErrInfo As String = String.Empty

        Public Function GetSourcePage(ByVal Url As String) As String
            Dim UrlHost As String = New Uri(Url).Host
            Try

                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12
                Dim cookieJar As CookieContainer = New CookieContainer()
                Dim request As HttpWebRequest = CType(WebRequest.Create(Url), HttpWebRequest)
                request.UseDefaultCredentials = True
                request.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials
                request.CookieContainer = cookieJar
                request.Accept = "text/html, application/xhtml+xml, */*"
                request.Referer = "https://" + UrlHost + "/"
                request.Headers.Add("Accept-Language", "en-GB")
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.114 Safari/537.36"
                request.Host = UrlHost
                Dim response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
                Dim htmlString As String = String.Empty

                Using reader = New StreamReader(response.GetResponseStream())
                    htmlString = reader.ReadToEnd()
                End Using

                Return htmlString
            Catch ex As Exception
                ErrInfo = ex.Message.Replace(UrlHost, "$WebEnv$")
                Return String.Empty
            End Try
        End Function


#End Region

#Region " Private Methods "

        ''' <summary>
        ''' Converts a <see cref="String"/> to an <see cref="HTMLDocument"/>.
        ''' </summary>
        ''' <param name="str">Indicates the string.</param>
        ''' <returns>The <see cref="HTMLDocument"/> object.</returns>
        Public Function StringToHtmlDocument(ByVal str As String) As HtmlDocument
            Dim Result As HtmlDocument = Nothing
            ParentForm.BeginInvoke(Sub()

                                       Using wb As New WebBrowser

                                           wb.ScriptErrorsSuppressed = True
                                           wb.DocumentText = ""
                                           wb.Document.OpenNew(replaceInHistory:=True)
                                           wb.Document.Write(str)
                                           Result = wb.Document

                                       End Using

                                   End Sub)

            For i As Integer = 0 To 2
                If Result IsNot Nothing Then
                    Exit For
                End If
                i -= 1
            Next

            Return Result

        End Function

#End Region

        Public Class Info

#Region " Properties "

            Public Property Name As String = String.Empty

            Private ReadOnly MiniatureUrl As String
            Public ReadOnly Property Miniature As String
                <DebuggerStepThrough>
                Get
                    Return Me.MiniatureUrl
                End Get
            End Property

            Private ReadOnly LinkerPreview As String
            Public ReadOnly Property Preview As String
                <DebuggerStepThrough>
                Get
                    Return Me.LinkerPreview
                End Get
            End Property

#End Region

#Region " Constructors "

            <DebuggerStepThrough>
            Public Sub New(ByVal Min As String, ByVal Pre As String)
                MiniatureUrl = Min
                LinkerPreview = Pre
            End Sub

#End Region

        End Class

        Public Class ImageInfo

#Region " Properties "

            Private ReadOnly DownloadEx As String
            Public ReadOnly Property DownloadUrl As String
                <DebuggerStepThrough>
                Get
                    Return Me.DownloadEx
                End Get
            End Property

            Private ReadOnly ImageSize As Size = New Size(0, 0)
            Public ReadOnly Property Resolution As Size
                <DebuggerStepThrough>
                Get
                    Return Me.ImageSize
                End Get
            End Property

#End Region

#Region " Constructors "

            <DebuggerStepThrough>
            Public Sub New(ByVal Url As String, ByVal ImageRes As Size)
                DownloadEx = Url
                ImageSize = ImageRes
            End Sub

#End Region

        End Class

    End Class
End Namespace
