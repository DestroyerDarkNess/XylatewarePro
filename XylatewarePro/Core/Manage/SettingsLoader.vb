Imports XylatewarePro.Core.Helpers.Utils

Namespace Core.Managed
    Public Class SettingsLoader

#Region " Properties "

        Public ReadOnly Property GifEngine As Gif
            <DebuggerStepThrough>
            Get
                Return Me.GetGif
            End Get
        End Property

        Public ReadOnly Property VideoEngine As Video
            <DebuggerStepThrough>
            Get
                Return Me.GetVideo
            End Get
        End Property

        Public ReadOnly Property WebEngine As Web
            <DebuggerStepThrough>
            Get
                Return Me.GetWeb
            End Get
        End Property

        Public ReadOnly Property ExecuteEngine As Execute
            <DebuggerStepThrough>
            Get
                Return Me.GetExecute
            End Get
        End Property

        Public ReadOnly Property AppLang As String
            <DebuggerStepThrough>
            Get
                Return ReadIni("Settings", "LangType", "English")
            End Get
        End Property

        Public ReadOnly Property AppAutoStart As Boolean
            <DebuggerStepThrough>
            Get
                Return ReadIni("Settings", "AutoStart", False)
            End Get
        End Property

        Public ReadOnly Property WallpaperAutoStart As Boolean
            <DebuggerStepThrough>
            Get
                Return ReadIni("Settings", "AutoStartWallpaper", False)
            End Get
        End Property

        Public ReadOnly Property CheckUpdates As Boolean
            <DebuggerStepThrough>
            Get
                Return ReadIni("Settings", "CheckUpdates", False)
            End Get
        End Property

        Public ReadOnly Property PauseWallpaper As Boolean
            <DebuggerStepThrough>
            Get
                Return ReadIni("Settings", "PauseWallpaper", False)
            End Get
        End Property

        Public ReadOnly Property MultiCore As Boolean
            <DebuggerStepThrough>
            Get
                Return ReadIni("Settings", "MultiCore", False)
            End Get
        End Property

#End Region

#Region " Enum "

        Public Enum Gif
            Xylateware = 0
            GDI = 1
            ManualDecoder = 2
        End Enum

        Public Enum Video
            LibVLCSharp = 0
            Windows_Media_Player = 1
            MVP = 2
        End Enum

        Public Enum Web
            EO_WebBrowser = 0
            MS_Edge = 1
        End Enum

        Public Enum Execute
            Auto_Detect = 0
            Create_Process = 1
            Shell_Execute = 2
        End Enum

#End Region

        Public Sub New()

        End Sub

        Private Function GetGif() As Gif
            Return CType(ReadIni("Settings", "GifEngine", 0), Gif)
        End Function

        Private Function GetVideo() As Video
            Return CType(ReadIni("Settings", "VideoEngine", 0), Video)
        End Function

        Private Function GetWeb() As Web
            Return CType(ReadIni("Settings", "WebEngine", 0), Web)
        End Function

        Private Function GetExecute() As Execute
            Return CType(ReadIni("Settings", "ExeEngine", 0), Execute)
        End Function


    End Class
End Namespace

