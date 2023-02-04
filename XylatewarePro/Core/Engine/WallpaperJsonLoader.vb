Imports System.Web.Script.Serialization

Namespace Core.Engine

    Public Class WallpaperJsonLoader

#Region " Properties "

        Private ReadOnly WallpaperColllection As New List(Of WallpaperInfo)
        Public ReadOnly Property Wallpapers As List(Of WallpaperInfo)
            <DebuggerStepThrough>
            Get
                Return Me.WallpaperColllection
            End Get
        End Property

#End Region

#Region " Declare's "

        Public Shared Property WallpaperPath As String = Core.Manage.Paths.CacheWallpaperPath

#End Region

#Region " Enums "

        Public Enum StateLoaded
            Indeterminate = 0
            Loaded = 1
            Failed = 2
        End Enum

#End Region

        Public Sub New(Optional ByVal WPPath As String = "")

            If Not WPPath = "" Then
                WallpaperPath = WPPath
            End If

            Dim ExtensionsFolders As List(Of String) = FileDirSearcher.GetDirPaths(WallpaperPath, IO.SearchOption.TopDirectoryOnly).ToList

            For Each ExFolder As String In ExtensionsFolders

                Dim ChoEx As New WallpaperInfo(ExFolder)
                WallpaperColllection.Add(ChoEx)

            Next

        End Sub

#Region " Extension Data "

        <Serializable()>
        Public NotInheritable Class Manifest

#Region " ExProperties "

            Private FilePath As String = String.Empty
            Public Property FilePathJson As String
                <DebuggerStepThrough>
                Get
                    Return Me.FilePath
                End Get
                Set(value As String)
                    FilePath = value
                End Set
            End Property

#End Region

            ' // Required
            Public Property AppVersion As String = String.Empty
            Public Property Title As String = String.Empty
            Public Property Thumbnail As String = String.Empty
            Public Property Preview As String = String.Empty
            Public Property Desc As String = String.Empty
            Public Property Author As String = String.Empty
            Public Property License As String = String.Empty
            Public Property Contact As String = String.Empty
            Public Property Type As String = String.Empty
            Public Property action As String = String.Empty
            Public Property FileName As String = String.Empty
            Public Property Arguments As String = String.Empty
            Public Property IsAbsolutePath As Boolean = False

            Public Property LivelyProperties As String = String.Empty

            Public Sub New()

            End Sub

            Public Overrides Function ToString() As String
                Return New JavaScriptSerializer().Serialize(Me).ToString
            End Function

        End Class

        Public NotInheritable Class WallpaperInfo

#Region " Properties "

            '#Region " Icons "

            '            Private ReadOnly PreviewEx As Image = Nothing
            '            Public ReadOnly Property Preview As Image
            '                <DebuggerStepThrough>
            '                Get
            '                    Return Me.PreviewEx
            '                End Get
            '            End Property

            '            Private ReadOnly ThumbnailEx As Image = Nothing
            '            Public ReadOnly Property Thumbnail As Image
            '                <DebuggerStepThrough>
            '                Get
            '                    Return Me.ThumbnailEx
            '                End Get
            '            End Property

            '#End Region

            Private ReadOnly ExtensionPath As String
            Public ReadOnly Property FullPath As String
                <DebuggerStepThrough>
                Get
                    Return Me.ExtensionPath
                End Get
            End Property

            Private ReadOnly MainFolderEx As String
            Public ReadOnly Property MainFolder As String
                <DebuggerStepThrough>
                Get
                    Return Me.MainFolderEx
                End Get
            End Property

            Private ManifestData As Manifest
            Public ReadOnly Property ManifestJson As Manifest
                <DebuggerStepThrough>
                Get
                    Return Me.ManifestData
                End Get
            End Property

            Private LoadStateEx As StateLoaded = StateLoaded.Indeterminate
            Public ReadOnly Property LoadState As StateLoaded
                <DebuggerStepThrough>
                Get
                    Return Me.LoadStateEx
                End Get
            End Property


            Private ExeptionInfoEx As String = String.Empty
            Public ReadOnly Property ExeptionInfo As String
                <DebuggerStepThrough>
                Get
                    Return Me.ExeptionInfoEx
                End Get
            End Property

#End Region

#Region " Constructors "

            <DebuggerStepThrough>
            Private Sub New()

            End Sub

            <DebuggerStepThrough>
            Public Sub New(ByVal ExPath As String)

                Dim CheckLaunch As Boolean = IsFolder(ExPath)
                Dim FileCurrent As String = ExPath

                If CheckLaunch = True Then

                    Dim ExtensionFiles As List(Of String) = FileDirSearcher.GetFilePaths(ExPath, IO.SearchOption.TopDirectoryOnly).ToList

                    For Each FileEx As String In ExtensionFiles
                        Dim IsValidJson As Boolean = False

                        Select Case LCase(IO.Path.GetFileName(FileEx))
                            Case LCase("Manifest.json") : IsValidJson = True
                            Case LCase("LivelyInfo.json") : IsValidJson = True
                            Case Else
                                IsValidJson = False
                        End Select

                        If IsValidJson = True Then FileCurrent = FileEx : Exit For

                    Next

                End If

                If IO.File.Exists(FileCurrent) = True Then
                    ExtensionPath = IO.Path.GetDirectoryName(FileCurrent)

                    Try

                        Dim ExtensionVersionFolder As String = IO.Path.GetDirectoryName(FileCurrent)

                        MainFolderEx = ExtensionVersionFolder

                        Dim JsonCode As String = Core.Helpers.Utils.FileReadText(FileCurrent)

                        ManifestData = New JavaScriptSerializer().Deserialize(Of Manifest)(JsonCode)
                        ManifestData.FilePathJson = FileCurrent

                        If IO.File.Exists(IO.Path.Combine(ExtensionVersionFolder, ManifestData.FileName)) = True Then
                            LoadStateEx = StateLoaded.Loaded
                        Else
                            LoadStateEx = StateLoaded.Failed
                        End If

                    Catch ex As Exception
                        LoadStateEx = StateLoaded.Failed
                        Console.WriteLine(ex.Message)
                        ExeptionInfoEx += ex.Message & vbNewLine
                    End Try

                Else
                    LoadStateEx = StateLoaded.Failed
                End If

            End Sub

            Public Overrides Function ToString() As String
                Return New JavaScriptSerializer().Serialize(Me).ToString
            End Function

            Private Function IsFolder(ByVal RutePathStr As String) As Boolean
                Try
                    Return ((IO.File.GetAttributes(RutePathStr) And IO.FileAttributes.Directory) = IO.FileAttributes.Directory)
                Catch ex As Exception
                    Return False
                End Try
            End Function

#End Region

        End Class

#End Region

    End Class

End Namespace
