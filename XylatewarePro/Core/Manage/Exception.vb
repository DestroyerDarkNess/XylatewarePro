Namespace Core.Manage

    Public Class Exception

        Private Shared _ExceptionList As New List(Of Core.Model.ExceptionModel)
        Public Shared LogFile As String = IO.Path.Combine(Core.Manage.Paths.CacheTempPath, "Error.log")

        Public Shared ReadOnly Property ExceptionList As List(Of Core.Model.ExceptionModel)
            Get
                Return _ExceptionList
            End Get
        End Property

        Private Shared StringLogEx As List(Of String) = New List(Of String)

        Public Shared Sub WriteException(ByVal Excep As Core.Model.ExceptionModel)
            _ExceptionList.Add(Excep)

            If String.Equals(Excep.Ident, "MAINWINDOW.Xylon.PH", StringComparison.OrdinalIgnoreCase) Then

            Else
                Dim ErrorInfo As String = "ID: " & _ExceptionList.Count & " Ident: " & Excep.Ident & " Message: " & Excep.Message
                StringLogEx.Add(ErrorInfo)
                Dim FileWriteEx As Boolean = Core.Helpers.Utils.FileWriteText(LogFile, String.Join(Environment.NewLine, StringLogEx.ToArray()))

                '  Console.ForegroundColor = ConsoleColor.Red
                '  Console.WriteLine("ID: " & _ExceptionList.Count & " Ident: " & Excep.Ident & " Message: " & Excep.Message)
                '  Console.ForegroundColor = ConsoleColor.White
            End If

        End Sub

    End Class

End Namespace
