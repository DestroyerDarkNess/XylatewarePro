Namespace Core.Manage
    Public Class Instances

        Public Shared MainUI As MainUI = Nothing
        Public Shared LastEngine As Process = Nothing
        Public Shared FileVer As String = FileVersionInfo.GetVersionInfo(Application.ExecutablePath).FileVersion
        Public Shared UniversalRGBColor As Color = Color.White

        ' Token: 0x04000201 RID: 513
        Public Shared AppName As String = "Xylateware"

        ' Token: 0x04000202 RID: 514
        Public Shared Extension As String = ".xypkg"

        Public Shared SilentMode As Boolean = False


    End Class
End Namespace

