
Imports System.Runtime.InteropServices
Imports Microsoft.Win32
Imports XylatewarePro.Core

NotInheritable Class Program

    <System.Runtime.InteropServices.DllImport("user32.dll")>
    Private Shared Function SetProcessDPIAware() As Boolean
    End Function

    Public Shared SplashFormEx As SplashForm = Nothing

    Public Shared Sub FirstChanceExceptionHandler(sender As Object, e As System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs)
        Dim ex As Exception = CType(e.Exception, Exception)
        Dim NewExcep As New Core.Model.ExceptionModel
        NewExcep.Ident = ex.Source
        NewExcep.Message = ex.Message
        NewExcep.Stack = ex.StackTrace

        Core.Manage.Exception.WriteException(NewExcep)
    End Sub

    Public Shared Sub CurrentDomain_UnhandledException(sender As Object, e As System.UnhandledExceptionEventArgs)
        Dim ex As Exception = CType(e.ExceptionObject, Exception)
        Dim NewExcep As New Core.Model.ExceptionModel
        NewExcep.Ident = ex.Source
        NewExcep.Message = ex.Message
        NewExcep.Stack = ex.StackTrace

        Core.Manage.Exception.WriteException(NewExcep)
    End Sub

    Private Shared Sub Application_Exception_Handler(ByVal sender As Object, ByVal e As System.Threading.ThreadExceptionEventArgs)
        Dim ex As Exception = CType(e.Exception, Exception)
        Dim NewExcep As New Core.Model.ExceptionModel
        NewExcep.Ident = ex.Source
        NewExcep.Message = ex.Message
        NewExcep.Stack = ex.StackTrace

        Core.Manage.Exception.WriteException(NewExcep)
    End Sub


    <STAThread>
    Friend Shared Sub Main()

        Core.Manage.Paths.CreateCache()

        AddHandler AppDomain.CurrentDomain.FirstChanceException, AddressOf FirstChanceExceptionHandler
        AddHandler AppDomain.CurrentDomain.UnhandledException, AddressOf CurrentDomain_UnhandledException

        Try : AddHandler Application.ThreadException, AddressOf Application_Exception_Handler
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException, False)
        Catch : End Try

        Dim ReOpenIsAdmin As Boolean = False

        If ExtensionInstalled() = False Then
            ReOpenIsAdmin = True
        End If


        If ReOpenIsAdmin = True Then
            If XylatewarePro.Core.Helpers.Utils.IsAdmin() = False Then
                Core.Helpers.Utils.OpenAsAdmin(Application.ExecutablePath)
                Environment.Exit(0)
            Else
                Core.Helpers.FileAssociations.EnsureAssociationsSet()
            End If
        End If

        If Core.Helpers.Utils.IsAdmin = False Then
            Core.Helpers.Utils.OpenAsAdmin(Application.ExecutablePath)
            Environment.Exit(0)
        End If

        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)

        SetProcessDPIAware()

        Dim CommandLineArgs As String() = Environment.GetCommandLineArgs

        Dim FastArgumentParser As Core.FastArgumentParser = New Core.FastArgumentParser()

        Dim FileA As IArgument = FastArgumentParser.Add("-install").SetDescription("Package Installer")
        Dim SilentA As IArgument = FastArgumentParser.Add("-silent").SetDescription("silent application start")
        Dim StartA As IArgument = FastArgumentParser.Add("-start").SetDescription("Start Wallpaper")
        Dim StartupA As IArgument = FastArgumentParser.Add("-startup").SetDescription("Start On Startup")
        Dim YoutubeEmbed As IArgument = FastArgumentParser.Add("-youtube").SetDescription("Start youtube embed wallpaper")

        FastArgumentParser.Parse(CommandLineArgs)

        If FileA.Value IsNot Nothing AndAlso IO.File.Exists(FileA.Value.FirstOrDefault) Then

            Dim PackInstaller As New PakageInstaller With {.TargetFile = FileA.Value.FirstOrDefault}
            PackInstaller.ShowDialog()
            Environment.Exit(0)

        ElseIf StartA.Value IsNot Nothing Then

            StartWallpaper(StartA.Value)

        ElseIf YoutubeEmbed.Value IsNot Nothing Then

            Dim DeskEmbeder As Core.Engine.DesktopEmbeder = New Core.Engine.DesktopEmbeder
            Dim SettingsReader As Core.Managed.SettingsLoader = New Core.Managed.SettingsLoader

            Dim SysEmbed As Integer = Core.Helpers.Utils.ReadIni("Settings", "SysEmbed", 0)

            Try
                Dim wallConfig As WallConfig = New WallConfig() With {.YoutubeID = YoutubeEmbed.Value.FirstOrDefault}

                Dim VideoEngine As Control = Nothing


                Select Case Core.Helpers.Utils.ReadIni("Settings", "YoutubePlayer", 0)
                    Case 0
                        VideoEngine = New YoutubePlayer With {.Url = YoutubeEmbed.Value.LastOrDefault}
                    Case 1
                        VideoEngine = New YoutubePlayerVLC(YoutubeEmbed.Value.LastOrDefault)
                    Case Else
                        VideoEngine = New YoutubePlayer With {.Url = YoutubeEmbed.Value.LastOrDefault}
                End Select

                If SysEmbed = 0 Then
                    DeskEmbeder.EmbedControl(VideoEngine, True)
                ElseIf SysEmbed = 1 Then
                    BiliUPDesktopTool.DesktopEmbeddedWindowHelper.DesktopEmbedWindow(VideoEngine)
                End If

                wallConfig.EmbedControl = VideoEngine

                Application.Run(wallConfig)
                Environment.Exit(0)

            Catch ex As Exception
                If ex.Message.Contains("SHELLDLL_DefView") Then
                    Dim SysError As New SysAnimationError
                    SysError.ShowDialog()
                Else
                    Dim ExDialog As New CrashDialog
                    ExDialog.ErrorMessage = ex
                    ExDialog.ShowDialog()
                End If


                Environment.Exit(0)
            End Try


        ElseIf StartupA.Value IsNot Nothing Then
            Dim ValBool As Boolean = CBool(StartupA.Value.FirstOrDefault)
            AppInStartup(ValBool)
            Environment.Exit(0)
        Else
            SplashFormEx = New SplashForm()
            Application.Run(SplashFormEx)

            Core.Manage.Instances.SilentMode = SilentA.Detected
            Core.Manage.Instances.MainUI = New MainUI

            Application.Run(Core.Manage.Instances.MainUI)
        End If
    End Sub

#Region " Extensions "

    Public Shared Function IsAnimatedEnabled()
        Try
            Dim registryKey1 As RegistryKey = Registry.CurrentUser.OpenSubKey("Control Panel\Desktop", False)
            Dim objectValue1 As Object = registryKey1.GetValue("UserPreferencesMask")
            Dim GetContent As Engine.DesktopEmbeder.IContent = Engine.DesktopEmbeder.GetIndexByte(objectValue1)
            Return GetContent.Enabled
        Catch ex As Exception
            Return False
        End Try
    End Function


    Public Shared Function ExtensionInstalled() As Boolean
        Try
            Dim result As Boolean = False

            Dim registryKey As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\Classes\.xypkg", False)
            Dim objectValue As Object = registryKey.GetValue("Xypkg_Editor_File")

            If objectValue Is Nothing Then
                result = False
            Else
                result = True
            End If
            Return result
        Catch ex As System.Exception
            Return False
        End Try
    End Function

    Public Shared Function AddInStartup() As Boolean
        Dim result As Boolean = False
        Try
            Dim registryKey As RegistryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Run", False)

            Dim objectValue As Object = registryKey.GetValue(Core.Manage.Instances.AppName)

            If objectValue Is Nothing Then
                result = False
            Else
                result = True
            End If

        Catch ex As System.Exception
            result = False
        End Try
        Return result
    End Function

    Public Shared Function AppInStartup(Optional Install As Boolean = True) As Boolean
        Dim result As Boolean = False
        Try
            Dim registryKey As RegistryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Run", True)
            If Install Then
                registryKey.SetValue(Core.Manage.Instances.AppName, """" & Application.ExecutablePath & """" & " -silent")
            Else
                registryKey.DeleteValue(Core.Manage.Instances.AppName, False)
            End If
            result = True
        Catch ex As System.Exception
            result = False
        End Try
        Return result
    End Function

    Public Shared Function GetVisualEffect() As Boolean
        Try
            Dim registryKey As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects", False)

            Dim objectValue As Object = registryKey.GetValue("VisualFXSetting")

            If objectValue Is Nothing Then
                Return False
            ElseIf Not objectValue = 3 Then
                Return False
            End If

            Dim registryKey1 As RegistryKey = Registry.CurrentUser.OpenSubKey("Control Panel\Desktop", False)

            Dim objectValue1 As Object = registryKey.GetValue("UserPreferencesMask")

            If objectValue Is Nothing Then
                Return False
            End If



        Catch ex As System.Exception
            Return False
        End Try
    End Function

#End Region

#Region " Wallpaper "

    Public Shared MVP_CL As String = <a><![CDATA[--volume=0 --loop-file --keep-open --geometry=0:0 --force-window=yes --no-window-dragging --cursor-autohide=no --stop-screensaver=no --keepaspect=no --no-osc --hwdec=auto-safe --screenshot-format=jpg "%File_Path%"]]></a>.Value

    <DllImport("user32.dll", SetLastError:=True)>
    Private Shared Function IsWindowVisible(ByVal hWnd As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

    Private Shared Sub StartWallpaper(ByVal WallpaperList As List(Of String))

        Dim DeskEmbeder As Core.Engine.DesktopEmbeder = New Core.Engine.DesktopEmbeder
        Dim SettingsReader As Core.Managed.SettingsLoader = New Core.Managed.SettingsLoader

        Dim SysEmbed As Integer = Core.Helpers.Utils.ReadIni("Settings", "SysEmbed", 0)

        Try

            Dim FilePath As String = IO.Path.GetFullPath(WallpaperList.FirstOrDefault)
            Core.Engine.DesktopEmbeder.RefreshDesktop()

            Dim JsonWallpaper As New Core.Engine.WallpaperJsonLoader.WallpaperInfo(FilePath)

            If JsonWallpaper.LoadState = Core.Engine.WallpaperJsonLoader.StateLoaded.Loaded Then

                Dim wallConfig As WallConfig = New WallConfig() With {.Wallpaper = JsonWallpaper}

                Dim FileNameEx As String = JsonWallpaper.ManifestJson.FileName

                If Core.Helpers.Utils.IsVideoFormat(FileNameEx) = True Then

                    If SettingsReader.VideoEngine = Core.Managed.SettingsLoader.Video.LibVLCSharp Then

                        Dim VideoEngine As New VideoForm With {.Wallpaper = JsonWallpaper}
                        If SysEmbed = 0 Then
                            DeskEmbeder.EmbedControl(VideoEngine, True)
                        ElseIf SysEmbed = 1 Then
                            BiliUPDesktopTool.DesktopEmbeddedWindowHelper.DesktopEmbedWindow(VideoEngine)
                        End If
                        wallConfig.EmbedControl = VideoEngine

                    ElseIf SettingsReader.VideoEngine = Core.Managed.SettingsLoader.Video.Windows_Media_Player Then

                        Dim WPFVideoForm As New WPFVideoEngine With {.Wallpaper = JsonWallpaper}
                        If SysEmbed = 0 Then
                            DeskEmbeder.EmbedControl(WPFVideoForm, True)
                        ElseIf SysEmbed = 1 Then
                            BiliUPDesktopTool.DesktopEmbeddedWindowHelper.DesktopEmbedWindow(WPFVideoForm)
                        End If
                        wallConfig.EmbedControl = WPFVideoForm

                    ElseIf SettingsReader.VideoEngine = Core.Managed.SettingsLoader.Video.MVP Then

                        Dim FileMedia As String = IO.Path.Combine(JsonWallpaper.MainFolder, JsonWallpaper.ManifestJson.FileName)
                        If IO.File.Exists(FileMedia) Then
                            Dim EnginePath As IO.FileInfo = New IO.FileInfo("plugins\mpv\mpv.exe")
                            Dim Argument As String = MVP_CL.Replace("%File_Path%", FileMedia)
                            Dim LastEngine As Process = Process.Start(EnginePath.FullName, Argument)
                            LastEngine.WaitForInputIdle()

                            For i As Integer = 0 To 10
                                If IsWindowVisible(LastEngine.MainWindowHandle) = True Then
                                    Exit For
                                End If
                                Core.Helpers.Utils.Sleep(1)
                            Next

                            If SysEmbed = 0 Then
                                DeskEmbeder.EmbedByHandle(LastEngine.MainWindowHandle, True)
                            ElseIf SysEmbed = 1 Then
                                BiliUPDesktopTool.DesktopEmbeddedWindowHelper.DesktopEmbedWindow(LastEngine.MainWindowHandle)
                            End If

                            wallConfig.EmbedProcess = LastEngine

                        End If


                    End If

                ElseIf Core.Helpers.Utils.IsWebFormat(FileNameEx) = True Then

                    If SettingsReader.WebEngine = Core.Managed.SettingsLoader.Web.EO_WebBrowser Then

                        Core.Manage.Engines.Load()

                        Dim WebEngine As New WebForms With {.Wallpaper = JsonWallpaper}
                        If SysEmbed = 0 Then
                            DeskEmbeder.EmbedControl(WebEngine, True)
                        ElseIf SysEmbed = 1 Then
                            BiliUPDesktopTool.DesktopEmbeddedWindowHelper.DesktopEmbedWindow(WebEngine)
                        End If
                        wallConfig.EmbedControl = WebEngine

                    ElseIf SettingsReader.WebEngine = Core.Managed.SettingsLoader.Web.MS_Edge Then

                        Dim EdgeEngine As New EdgeForm With {.Wallpaper = JsonWallpaper}
                        If SysEmbed = 0 Then
                            DeskEmbeder.EmbedControl(EdgeEngine, True)
                        ElseIf SysEmbed = 1 Then
                            BiliUPDesktopTool.DesktopEmbeddedWindowHelper.DesktopEmbedWindow(EdgeEngine)
                        End If
                        wallConfig.EmbedControl = EdgeEngine

                    End If

                ElseIf Core.Helpers.Utils.IsGifFormat(FileNameEx) = True Then

                    Dim EnginePlay As Core.Managed.SettingsLoader.Gif = SettingsReader.GifEngine

                    Dim GifEngine1 As New GifEngine With {.Wallpaper = JsonWallpaper, .EnginePlayer = EnginePlay}
                    If SysEmbed = 0 Then
                        DeskEmbeder.EmbedControl(GifEngine1, True)
                    ElseIf SysEmbed = 1 Then
                        BiliUPDesktopTool.DesktopEmbeddedWindowHelper.DesktopEmbedWindow(GifEngine1)
                    End If
                    wallConfig.EmbedControl = GifEngine1

                ElseIf Core.Helpers.Utils.IsExecutableFormat(FileNameEx) = True Then

                    Dim FileMedia As String = IO.Path.Combine(JsonWallpaper.MainFolder, JsonWallpaper.ManifestJson.FileName)
                    If IO.File.Exists(FileMedia) Then
                        Dim EnginePath As IO.FileInfo = New IO.FileInfo(FileMedia)
                        Dim LastEngine As Process = Process.Start(EnginePath.FullName)
                        LastEngine.WaitForInputIdle()

                        For i As Integer = 0 To 10
                            If IsWindowVisible(LastEngine.MainWindowHandle) = True Then
                                Exit For
                            End If
                            Core.Helpers.Utils.Sleep(1)
                        Next

                        If SysEmbed = 0 Then
                            DeskEmbeder.EmbedByHandle(LastEngine.MainWindowHandle, True)
                        ElseIf SysEmbed = 1 Then
                            BiliUPDesktopTool.DesktopEmbeddedWindowHelper.DesktopEmbedWindow(LastEngine.MainWindowHandle)
                        End If

                        wallConfig.EmbedProcess = LastEngine

                    End If

                Else
                    Throw New Exception("Format Not Found")
                End If

                Application.Run(wallConfig)
                Environment.Exit(0)

            Else

                MsgBox("Undefine Error :(")
                Environment.Exit(0)

            End If

        Catch ex As Exception

            If ex.Message.Contains("SHELLDLL_DefView") Then
                Dim SysError As New SysAnimationError
                SysError.ShowDialog()
            Else
                Dim ExDialog As New CrashDialog
                ExDialog.ErrorMessage = ex
                ExDialog.ShowDialog()
            End If


            Environment.Exit(0)
        End Try

    End Sub

#End Region

End Class