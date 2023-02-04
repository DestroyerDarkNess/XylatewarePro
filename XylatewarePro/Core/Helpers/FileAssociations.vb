Imports System
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.CompilerServices
Imports Microsoft.Win32
Imports XylatewarePro.Core.Helpers

Namespace Core.Helpers

    Public Class FileAssociation
        ' Token: 0x17000198 RID: 408
        ' (get) Token: 0x06000624 RID: 1572 RVA: 0x00021BD4 File Offset: 0x0001FDD4
        ' (set) Token: 0x06000625 RID: 1573 RVA: 0x00021BDE File Offset: 0x0001FDDE
        Public Property Extension As String

        ' Token: 0x17000199 RID: 409
        ' (get) Token: 0x06000626 RID: 1574 RVA: 0x00021BE7 File Offset: 0x0001FDE7
        ' (set) Token: 0x06000627 RID: 1575 RVA: 0x00021BF1 File Offset: 0x0001FDF1
        Public Property ProgId As String

        ' Token: 0x1700019A RID: 410
        ' (get) Token: 0x06000628 RID: 1576 RVA: 0x00021BFA File Offset: 0x0001FDFA
        ' (set) Token: 0x06000629 RID: 1577 RVA: 0x00021C04 File Offset: 0x0001FE04
        Public Property FileTypeDescription As String

        ' Token: 0x1700019B RID: 411
        ' (get) Token: 0x0600062A RID: 1578 RVA: 0x00021C0D File Offset: 0x0001FE0D
        ' (set) Token: 0x0600062B RID: 1579 RVA: 0x00021C17 File Offset: 0x0001FE17
        Public Property ExecutableFilePath As String
    End Class

    ' Token: 0x0200008A RID: 138
    Public Class FileAssociations
        ' Token: 0x0600062D RID: 1581
        Private Declare Function SHChangeNotify Lib "Shell32.dll" (eventId As Integer, flags As Integer, item1 As IntPtr, item2 As IntPtr) As Integer


        ' Token: 0x0600062E RID: 1582 RVA: 0x00021C20 File Offset: 0x0001FE20
        Public Shared Sub EnsureAssociationsSet()
            Dim executablePath As String = Application.ExecutablePath

            Dim FA As New List(Of FileAssociation)

            Dim XyFA As New FileAssociation With {.Extension = ".xypkg", .ProgId = "Xypkg_Editor_File", .FileTypeDescription = "Xylateware Package", .ExecutableFilePath = executablePath}
            FA.Add(XyFA)

            FileAssociations.EnsureAssociationsSet(FA.ToArray)
        End Sub

        ' Token: 0x0600062F RID: 1583 RVA: 0x00021C74 File Offset: 0x0001FE74
        Public Shared Sub EnsureAssociationsSet(ParamArray associations As FileAssociation())
            Dim flag As Boolean = False
            For Each fileAssociation As FileAssociation In associations
                flag = flag Or FileAssociations.SetAssociation(fileAssociation.Extension, fileAssociation.ProgId, fileAssociation.FileTypeDescription, fileAssociation.ExecutableFilePath)
            Next
            Dim flag2 As Boolean = flag
            If flag2 Then
                FileAssociations.SHChangeNotify(134217728, 4096, IntPtr.Zero, IntPtr.Zero)
            End If
        End Sub

        ' Token: 0x06000630 RID: 1584 RVA: 0x00021CE4 File Offset: 0x0001FEE4
        Public Shared Function SetAssociation(extension As String, progId As String, fileTypeDescription As String, applicationFilePath As String) As Boolean
            Dim flag As Boolean = False
            flag = flag Or FileAssociations.SetKeyDefaultValue("Software\Classes\" + extension, progId)
            flag = flag Or FileAssociations.SetKeyDefaultValue("Software\Classes\" + progId, fileTypeDescription)
            Return flag Or FileAssociations.SetKeyDefaultValue(String.Format("Software\Classes\{0}\shell\open\command", progId), """" + applicationFilePath + """ -install ""%1""")
        End Function

        ' Token: 0x06000631 RID: 1585 RVA: 0x00021D44 File Offset: 0x0001FF44
        Private Shared Function SetKeyDefaultValue(keyPath As String, value As String) As Boolean
            Using registryKey As RegistryKey = Registry.CurrentUser.CreateSubKey(keyPath)
                Dim flag As Boolean = Operators.CompareString(TryCast(registryKey.GetValue(Nothing), String), value, False) <> 0
                If flag Then
                    registryKey.SetValue(Nothing, value)
                    Return True
                End If
            End Using
            Return False
        End Function

        ' Token: 0x0400029F RID: 671
        Private Const SHCNE_ASSOCCHANGED As Integer = 134217728

        ' Token: 0x040002A0 RID: 672
        Private Const SHCNF_FLUSH As Integer = 4096
    End Class
End Namespace
