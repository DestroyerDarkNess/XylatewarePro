Imports System.Text.RegularExpressions
Imports System.Net
Imports System.IO
Imports System.Runtime.InteropServices

Namespace Core.Helpers
    Public Class Utils

#Region " [ColorCode] Color Code "

        ' [ColorCode] Color Code
        '
        ' // By Elektro H@cker
        '
        ' Instructions:
        ' 1. Add a reference to ColorCode.dll
        '
        ' Examples:
        ' HtmlTextBox1.Text = Color_Code(IO.File.ReadAllText("c:\Code.vb"), ColorCode.Languages.VbDotNet)
        ' HtmlTextbox1.Text = Color_Code(IO.File.ReadAllText("c:\Code.cs"), ColorCode.Languages.CSharp)

        '  Private Function Color_Code(ByVal Code As String, ByVal Language As ColorCode.ILanguage) As String
        '      Return New ColorCode.CodeColorizer().Colorize(Code, Language)
        '  End Function

#End Region

#Region " Resize Image "

        ' [ Save Resize Image Function ]
        '
        ' Examples :
        '
        ' PictureBox1.Image = Resize_Image(System.Drawing.Image.FromFile("C:\Image.png"), 256, 256)

        Public Shared Function Resize_Image(ByVal img As Image, ByVal Width As Int32, ByVal Height As Int32) As Bitmap
            Dim Bitmap_Source As New Bitmap(img)
            Dim Bitmap_Dest As New Bitmap(CInt(Width), CInt(Height))
            Dim Graphic As Graphics = Graphics.FromImage(Bitmap_Dest)
            Graphic.DrawImage(Bitmap_Source, 0, 0, Bitmap_Dest.Width + 1, Bitmap_Dest.Height + 1)
            Return Bitmap_Dest
        End Function

        Public Shared Random As New Random
        Public Shared Function RandomString(ByVal length As Integer) As String
            Const chars As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"
            Return New String(Enumerable.Repeat(chars, length).[Select](Function(s) s(Random.[Next](s.Length))).ToArray())
        End Function

#End Region


        Public Shared Function IsAdmin() As Boolean
            Try
                Dim Identity As System.Security.Principal.WindowsIdentity = System.Security.Principal.WindowsIdentity.GetCurrent()
                Dim Principal As System.Security.Principal.WindowsPrincipal = New System.Security.Principal.WindowsPrincipal(Identity)
                Dim IsElevated As Boolean = Principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator)
                Return IsElevated
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Shared Function OpenAsAdmin(ByVal FilePth As String, Optional ByVal Argument As String = "", Optional ByVal Hidden As Boolean = True) As Boolean
            Try
                Dim procStartInfo As New ProcessStartInfo
                Dim procExecuting As New Process

                Dim WindowStyle As ProcessWindowStyle = ProcessWindowStyle.Hidden

                If Hidden = False Then
                    WindowStyle = ProcessWindowStyle.Normal
                End If

                With procStartInfo
                    .UseShellExecute = True
                    .FileName = FilePth
                    .Arguments = Argument
                    .WindowStyle = WindowStyle
                    .Verb = "runas"
                End With

                procExecuting = Process.Start(procStartInfo)
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Shared Function OpenAsHidden(ByVal FilePth As String, Optional ByVal Argument As String = "") As Process
            Try
                Dim procStartInfo As New ProcessStartInfo
                Dim procExecuting As New Process

                With procStartInfo
                    .FileName = FilePth
                    .Arguments = Argument
                    .WindowStyle = ProcessWindowStyle.Hidden
                End With

                Return Process.Start(procStartInfo)

            Catch ex As Exception
                Return Nothing
            End Try
        End Function

        Public Shared Function IsFolder(ByVal path As String) As Boolean
            Return ((IO.File.GetAttributes(path) And IO.FileAttributes.Directory) = IO.FileAttributes.Directory)
        End Function

        Public Shared Function DownloadImage(url As String) As Image
            Try
                Dim httpWebRequest = DirectCast(System.Net.WebRequest.Create(url), System.Net.HttpWebRequest)
                Dim httpWebResponse = DirectCast(httpWebRequest.GetResponse(), System.Net.HttpWebResponse)
                If (httpWebResponse.StatusCode <> System.Net.HttpStatusCode.OK AndAlso httpWebResponse.StatusCode <> System.Net.HttpStatusCode.Moved AndAlso httpWebResponse.StatusCode <> System.Net.HttpStatusCode.Redirect) OrElse Not httpWebResponse.ContentType.StartsWith("image", StringComparison.OrdinalIgnoreCase) Then
                    Return Nothing
                End If
                Using stream = httpWebResponse.GetResponseStream()
                    Return Image.FromStream(stream)
                End Using
            Catch ex As Exception
                Return Nothing
            End Try
        End Function

        Public Shared FileManagerEx As String = String.Empty

        Public Shared Function FileWriteText(ByVal FileDir As String, Optional ByVal ContentText As String = "") As Boolean
            Try
                Dim swEx As New IO.StreamWriter(FileDir, False)
                swEx.Write(ContentText)
                swEx.Close()
                Return True
            Catch ex As Exception
                FileManagerEx = ex.Message
                Return False
            End Try
        End Function


        Public Shared Function FileReadText(ByVal FileDir As String) As String
            Try
                Dim swEx As New IO.StreamReader(FileDir, False)
                Dim ReadAllText As String = swEx.ReadToEnd
                swEx.Close()
                Return ReadAllText
            Catch ex As Exception
                FileManagerEx = ex.Message
                Return String.Empty
            End Try
        End Function

#Region " CheckExtensions "

        Public Shared Function IsVideoFormat(ByVal Filename As String) As Boolean
            Try
                Select Case Filename.Split(".").LastOrDefault.ToUpper
                    Case "MP4" : Return True
                    Case "AVI" : Return True
                    Case "WEBM" : Return True
                    Case "MKV" : Return True
                    Case "FLV" : Return True
                    Case "WMV" : Return True
                    Case "AMV" : Return True
                    Case "M4P" : Return True
                    Case "M4V" : Return True
                    Case "3GP" : Return True
                    Case Else
                        Return False
                End Select
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Shared Function IsImageFormat(ByVal Filename As String) As Boolean
            Try
                Select Case Filename.Split(".").LastOrDefault.ToUpper
                    Case "JPEG" : Return True
                    Case "JFIF" : Return True
                    Case "BMP" : Return True
                    Case "PNG" : Return True
                    Case "APNG" : Return True
                    Case "AVIF" : Return True
                    Case "JPG" : Return True
                    Case "SVG" : Return True
                    Case "WEBP" : Return True
                    Case "TIFF" : Return True
                    Case "TIF" : Return True
                    Case Else
                        Return False
                End Select
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Shared Function IsGifFormat(ByVal Filename As String) As Boolean
            Try
                Select Case Filename.Split(".").LastOrDefault.ToUpper
                    Case "GIF" : Return True
                    Case Else
                        Return False
                End Select
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Shared Function IsWebFormat(ByVal Filename As String) As Boolean
            Try
                Select Case Filename.Split(".").LastOrDefault.ToUpper
                    Case "HTML" : Return True
                    Case "ASP" : Return True
                    Case "PHP" : Return True
                    Case "CSS" : Return True
                    Case "DHTML" : Return True
                    Case "CSHTML" : Return True
                    Case Else
                        Return False
                End Select
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Shared Function IsExecutableFormat(ByVal Filename As String) As Boolean
            Try
                Select Case Filename.Split(".").LastOrDefault.ToUpper
                    Case "EXE" : Return True
                    Case Else
                        Return False
                End Select
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Shared Function IsScrpitFormat(ByVal Filename As String) As Boolean
            Try
                Select Case Filename.Split(".").LastOrDefault.ToUpper
                    Case "BAT" : Return True
                    Case "CMD" : Return True
                    Case "VBS" : Return True
                    Case "WSF" : Return True
                    Case "JS" : Return True
                    Case "PS1" : Return True
                    Case "HTA" : Return True
                    Case Else
                        Return False
                End Select
            Catch ex As Exception
                Return False
            End Try
        End Function

#End Region

#Region " Center Form To Desktop "

        ' [ Center Form To Desktop ]
        '
        ' // By Elektro H@cker
        '
        ' Examples :
        ' Center_Form_To_Desktop(Me)

        Public Shared Sub Center_Form_To_Desktop(ByVal Form As Form)
            Dim Desktop_RES As System.Windows.Forms.Screen = System.Windows.Forms.Screen.PrimaryScreen
            Form.Location = New Point((Desktop_RES.Bounds.Width - Form.Width) / 2, (Desktop_RES.Bounds.Height - Form.Height) / 2)
        End Sub

#End Region

        Public Shared Function OpenMultiFile(Optional ByVal Filter As String = "All Files|*.*") As List(Of String)
            Dim OpenFileDialog1 As New OpenFileDialog
            OpenFileDialog1.FileName = ""
            OpenFileDialog1.Multiselect = True
            '  OpenFileDialog1.InitialDirectory = "c:\"
            OpenFileDialog1.Title = "Select file"
            OpenFileDialog1.Filter = Filter
            Dim ListFiles As New List(Of String)

            If Not OpenFileDialog1.ShowDialog() = DialogResult.Cancel Then
                ListFiles.AddRange(OpenFileDialog1.FileNames)
                Return ListFiles
            End If

            Return Nothing

        End Function

        Public Shared Function OpenFile(Optional ByVal Filter As String = "All files|*.*") As String
            Dim OpenFileDialog1 As New OpenFileDialog
            OpenFileDialog1.Multiselect = False
            ' OpenFileDialog1.DefaultExt = "txt"
            OpenFileDialog1.FileName = ""
            '  OpenFileDialog1.InitialDirectory = "c:\"
            OpenFileDialog1.Title = "Select file"
            OpenFileDialog1.Filter = Filter

            If Not OpenFileDialog1.ShowDialog() = DialogResult.Cancel Then
                Return OpenFileDialog1.FileName
            End If

            Return Nothing

        End Function

        Public Shared Function SaveFile(Optional ByVal Filter As String = "All files|*.*") As String
            Dim SaveFileDialog1 As New SaveFileDialog
            ' OpenFileDialog1.DefaultExt = "txt"
            SaveFileDialog1.FileName = ""
            '  OpenFileDialog1.InitialDirectory = "c:\"
            SaveFileDialog1.Title = "Select file"
            SaveFileDialog1.Filter = Filter

            If Not SaveFileDialog1.ShowDialog() = DialogResult.Cancel Then
                Return SaveFileDialog1.FileName
            End If

            Return Nothing

        End Function


        Public Shared Async Function DownloadImageAsync(ByVal UrlImg As String) As Task(Of Image)
            Try
                Dim WebpImageData() As Byte = New System.Net.WebClient().DownloadData(UrlImg)
                Dim Webpstream As IO.MemoryStream = New IO.MemoryStream(WebpImageData)
                Dim ToBitmap As Bitmap = New Bitmap(Webpstream)
                Return ToBitmap
            Catch ex As Exception
                Exeptions = ex.Message
                Return Nothing
            End Try
        End Function

#Region " My Application Is Already Running "

        ' [ My Application Is Already Running Function ]
        '
        ' // By Elektro H@cker
        '
        ' Examples :
        ' MsgBox(My_Application_Is_Already_Running)
        ' If My_Application_Is_Already_Running() Then Application.Exit()

        Public Declare Function CreateMutexA Lib "Kernel32.dll" (ByVal lpSecurityAttributes As Integer, ByVal bInitialOwner As Boolean, ByVal lpName As String) As Integer
        Public Declare Function GetLastError Lib "Kernel32.dll" () As Integer

        Public Shared Function My_Application_Is_Already_Running() As Boolean
            'Attempt to create defualt mutex owned by process
            CreateMutexA(0, True, Process.GetCurrentProcess().MainModule.ModuleName.ToString)
            Return (GetLastError() = 183) ' 183 = ERROR_ALREADY_EXISTS
        End Function

#End Region

        Public Shared Exeptions As String = String.Empty

        Public Shared Sub AddDragger(ByVal cControl As Control)
            Dim NewDragC As New Guna.UI2.WinForms.Guna2DragControl With {.TargetControl = cControl}
        End Sub

        Public Shared Function GetNum(value As String) As Integer
            Dim text As String = String.Empty
            Dim matchCollection As MatchCollection = Regex.Matches(value, "\d+")
            Dim enumerator As IEnumerator = matchCollection.GetEnumerator()
            While enumerator.MoveNext()
                Dim match As Match = CType(enumerator.Current, Match)
                text += match.ToString()
            End While
            Return Convert.ToInt32(text)
        End Function

        Public Shared Function getID(ByVal url As String) As String 'Function by Stack Overflow Forum. 
            Try
                Dim myMatches As System.Text.RegularExpressions.Match
                Dim MyRegEx As New System.Text.RegularExpressions.Regex("youtu(?:\.be|be\.com)/(?:.*v(?:/|=)|(?:.*/)?)([a-zA-Z0-9-_]+)", RegexOptions.IgnoreCase) 'This is where the magic happens/SHOULD work on all normal youtube links including youtu.be
                myMatches = MyRegEx.Match(url)
                If myMatches.Success = True Then
                    Return myMatches.Groups(1).Value
                Else
                    Return String.Empty
                End If
            Catch ex As Exception
                Return String.Empty ' Return ex.ToString
            End Try
        End Function

        Public Declare Auto Function GetAsyncKeyState Lib "user32.dll" (vKey As IntPtr) As IntPtr

        Public Shared Function CheckKeyDown(vKey As Keys) As Boolean
            Return 0L <> (CLng(GetAsyncKeyState(CType(CLng(vKey), IntPtr))) And 32768L)
        End Function

#Region "Setttins"

        Declare Function GetPrivateProfileStringA Lib "kernel32" (ByVal lpAppName As String, ByVal lpKeyName As String, ByVal lpDefault As String, ByVal lpReturnedString As System.Text.StringBuilder, ByVal nSize As Integer, ByVal lpFileName As String) As Integer
        Declare Function WritePrivateProfileStringA Lib "kernel32" (ByVal lpAppName As String, ByVal lpKeyName As String, ByVal lpString As String, ByVal lpFileName As String) As Integer

        Private Shared IniFile As String = Core.Manage.Paths.CachePath & "\Settings.ini"


        Public Shared Function ReadIni(ByVal Section As String, ByVal Key As String, Optional ByVal DefaultValue As String = Nothing) As String
            Dim buffer As New System.Text.StringBuilder(260)
            GetPrivateProfileStringA(Section, Key, DefaultValue, buffer, buffer.Capacity, IniFile)
            Return buffer.ToString
        End Function

        Public Shared Function WriteIni(ByVal Section As String, ByVal Key As String, ByVal Value As String) As Boolean
            Return (WritePrivateProfileStringA(Section, Key, Value, IniFile) <> 0)
        End Function

#End Region

#Region " FixRounded "

        <DllImport("Gdi32.dll", EntryPoint:="CreateRoundRectRgn")>
        Private Shared Function CreateRoundRectRgn(ByVal nLeftRect As Integer, ByVal nTopRect As Integer, ByVal nRightRect As Integer, ByVal nBottomRect As Integer, ByVal nWidthEllipse As Integer, ByVal nHeightEllipse As Integer) As IntPtr
        End Function

        Public Shared Sub RoundedBorders(ByVal FormTarget As Form, ByVal Rounded As Size)
            Try
                FormTarget.AllowTransparency = True
                FormTarget.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, FormTarget.Width, FormTarget.Height, Rounded.Width, Rounded.Height))
            Catch ex As Exception

            End Try
        End Sub


#End Region

#Region " Is Connectivity Avaliable? function "

        ' [ Is Connectivity Avaliable? Function ]
        '
        ' // By Elektro H@cker
        '
        ' Examples :
        ' MsgBox(Is_Connectivity_Avaliable())
        ' While Not Is_Connectivity_Avaliable() : Application.DoEvents() : End While

        Private Function Is_Connectivity_Avaliable()

            Dim WebSites() As String = {"Google.com", "Facebook.com", "Microsoft.com"}

            If My.Computer.Network.IsAvailable Then
                For Each WebSite In WebSites
                    Try
                        My.Computer.Network.Ping(WebSite)
                        Return True ' Network connectivity is OK.
                    Catch : End Try
                Next
                Return False ' Network connectivity is down.
            Else
                Return False ' No network adapter is connected.
            End If

        End Function

#End Region

#Region " Sleep "

        ' [ Sleep ]
        '
        ' // By Elektro H@cker
        '
        ' Examples :
        ' Sleep(5) : MsgBox("Test")
        ' Sleep(5, Measure.Seconds) : MsgBox("Test")

        Public Enum Measure
            Milliseconds = 1
            Seconds = 2
            Minutes = 3
            Hours = 4
        End Enum

        Public Shared Sub Sleep(ByVal Duration As Int64, Optional ByVal Measure As Measure = Measure.Seconds)

            Dim Starttime = DateTime.Now

            Select Case Measure
                Case Measure.Milliseconds : Do While (DateTime.Now - Starttime).TotalMilliseconds < Duration : Application.DoEvents() : Loop
                Case Measure.Seconds : Do While (DateTime.Now - Starttime).TotalSeconds < Duration : Application.DoEvents() : Loop
                Case Measure.Minutes : Do While (DateTime.Now - Starttime).TotalMinutes < Duration : Application.DoEvents() : Loop
                Case Measure.Hours : Do While (DateTime.Now - Starttime).TotalHours < Duration : Application.DoEvents() : Loop
                Case Else
            End Select

        End Sub

#End Region

#Region " Base64 Functions "

        Public Shared Function ConvertImageToBase64String(ByVal ImageL As Image) As String
            Try
                Using ms As New MemoryStream()
                    ImageL.Save(ms, System.Drawing.Imaging.ImageFormat.Png) 'We load the image from first PictureBox in the MemoryStream
                    Dim obyte = ms.ToArray() 'We tranform it to byte array..

                    Return Convert.ToBase64String(obyte) 'We then convert the byte array to base 64 string.
                End Using
            Catch ex As Exception
                Return String.Empty
            End Try
        End Function

        Public Shared Function ConvertBase64StringToImage(ByVal base64 As String) As Image
            Try
                Dim ImageBytes As Byte() = ConvertBase64ToByteArray(base64)
                Using ms As New MemoryStream(ImageBytes)
                    Dim image As Image = System.Drawing.Image.FromStream(ms)
                    Return image
                End Using
            Catch ex As Exception
                Return Nothing
            End Try
        End Function

        Public Shared Function ConvertBase64ToByteArray(ByVal base64 As String) As Byte()
            Return Convert.FromBase64String(base64) 'Convert the base64 back to byte array.
        End Function

        'Here's the part of your code (which works)
        Public Shared Function ConvertbByteToImage(ByVal BA As Byte()) As Image
            Try
                Dim ms As MemoryStream = New MemoryStream(BA)
                Dim image = System.Drawing.Image.FromStream(ms)
                Return image
            Catch ex As Exception
                Return Nothing
            End Try
        End Function

#End Region

#Region " CenterForm function "

        Public Shared Function CenterForm(ByVal ParentForm As Form, ByVal Form_to_Center As Form, ByVal Form_Location As Point) As Point
            Dim FormLocation As New Point
            FormLocation.X = (ParentForm.Left + (ParentForm.Width - Form_to_Center.Width) / 2) ' set the X coordinates.
            FormLocation.Y = (ParentForm.Top + (ParentForm.Height - Form_to_Center.Height) / 2) ' set the Y coordinates.
            Return FormLocation ' return the Location to the Form it was called from.
        End Function

        Public Shared Function CenterControl(ByVal ParentForm As Control, ByVal Form_to_Center As Control, ByVal Form_Location As Point) As Point
            Dim FormLocation As New Point
            FormLocation.X = (ParentForm.Left + (ParentForm.Width - Form_to_Center.Width) / 2) ' set the X coordinates.
            FormLocation.Y = (ParentForm.Top + (ParentForm.Height - Form_to_Center.Height) / 2) ' set the Y coordinates.
            Return FormLocation ' return the Location to the Form it was called from.
        End Function

#End Region

        Public Shared Function GetDataPage(ByVal Url As String) As String
            Try
                Dim UrlHost As String = New Uri(Url).Host
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12
                Dim cookieJar As CookieContainer = New CookieContainer()
                Dim request As HttpWebRequest = CType(WebRequest.Create(Url), HttpWebRequest)
                request.UseDefaultCredentials = True
                request.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials
                request.CookieContainer = cookieJar
                request.Accept = "text/html, application/xhtml+xml, */*"
                request.Referer = "https://" + UrlHost + "/"
                request.Headers.Add("Accept-Language", "en-GB")
                request.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; Trident/6.0)"
                request.Host = UrlHost
                Dim response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
                Dim htmlString As String = String.Empty

                Using reader = New StreamReader(response.GetResponseStream())
                    htmlString = reader.ReadToEnd()
                End Using

                Return htmlString
            Catch ex As Exception
                Return String.Empty
            End Try
        End Function

    End Class
End Namespace

