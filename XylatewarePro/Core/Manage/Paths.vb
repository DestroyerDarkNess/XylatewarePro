Namespace Core.Manage

    Public Class Paths

        Public Shared ReadOnly CachePath = IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "AppData\Local\Xylateware\")

        Public Shared ReadOnly CacheWallpaperPath = IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "AppData\Local\Xylateware\Wallpapers")

        Public Shared ReadOnly CacheTempPath = IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "AppData\Local\Xylateware\Temp")

        Public Shared Sub CreateCache()
            If IO.Directory.Exists(CachePath) = False Then
                IO.Directory.CreateDirectory(CachePath)
            End If
            If IO.Directory.Exists(CacheWallpaperPath) = False Then
                IO.Directory.CreateDirectory(CacheWallpaperPath)
            End If
            If IO.Directory.Exists(CacheTempPath) = False Then
                IO.Directory.CreateDirectory(CacheTempPath)
            End If
        End Sub

    End Class

End Namespace

