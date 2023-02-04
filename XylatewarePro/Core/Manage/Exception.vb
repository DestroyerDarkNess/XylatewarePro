Namespace Core.Manage

    Public Class Exception

        Private Shared _ExceptionList As New List(Of Core.Model.ExceptionModel)

        Public Shared ReadOnly Property ExceptionList As List(Of Core.Model.ExceptionModel)
            Get
                Return _ExceptionList
            End Get
        End Property

        Public Shared Sub WriteException(ByVal Excep As Core.Model.ExceptionModel)
            _ExceptionList.Add(Excep)

            If String.Equals(Excep.Ident, "MAINWINDOW.Xylon.PH", StringComparison.OrdinalIgnoreCase) Then

            Else
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine("ID: " & _ExceptionList.Count & " Ident: " & Excep.Ident & " Message: " & Excep.Message)
                Console.ForegroundColor = ConsoleColor.White
            End If

        End Sub

    End Class

End Namespace
