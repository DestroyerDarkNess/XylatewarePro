Public Class CrashDialog

    Public Shared HeaderText As String = <a><![CDATA[
'------------------------------
   + %Header%
'------------------------------
]]></a>.Value


#Region " Properties "

    Private ErrorMessageEx As Exception = Nothing
    Public Property ErrorMessage As Exception
        <DebuggerStepThrough>
        Get
            Return Me.ErrorMessageEx
        End Get
        Set(value As Exception)
            ErrorMessageEx = value
        End Set
    End Property

#End Region

    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        Environment.Exit(0)
    End Sub

    Private Sub Guna2Button2_Click(sender As Object, e As EventArgs) Handles Guna2Button2.Click
        Environment.Exit(0)
    End Sub

    Private Sub CrashDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Core.Engine.DesktopEmbeder.RefreshDesktop()
        Dim Parent As String = String.Empty
        Try
            Parent = "Parent: " & Me.ParentForm.Name.ToString & vbNewLine
        Catch ex As Exception

        End Try

        Me.Guna2TextBox1.Text = Parent & MakeHeader("Message") & vbNewLine & ErrorMessageEx.Message & vbNewLine & vbNewLine & MakeHeader("Source") & vbNewLine & ErrorMessageEx.Source & vbNewLine & vbNewLine
    End Sub

    Private Function MakeHeader(ByVal Title As String) As String
        Return HeaderText.Replace("%Header%", Title)
    End Function

End Class