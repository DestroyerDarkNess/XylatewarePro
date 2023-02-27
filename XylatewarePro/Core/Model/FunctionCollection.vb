

Imports System.Linq.Expressions
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports XylatewarePro.Core.ZipCore

Namespace Core.Manage

    Public NotInheritable Class Remix_Injector

        'Public Shared Async Function Start() As Task(Of Boolean)
        '    Try

        '        Core.Manage.Instances.MainUI = New MainUI

        '        Return Await Core.Manage.Instances.MainUI.GUI

        '    Catch ex As System.Exception
        '        Dim NewExcep As New Core.Model.ExceptionModel
        '        NewExcep.Ident = "Remix_Injector".ToUpper & "." & ex.Source
        '        NewExcep.Message = ex.Message
        '        NewExcep.Stack = ex.StackTrace

        '        Core.Manage.Exception.WriteException(NewExcep)
        '        Return False
        '    End Try
        'End Function


    End Class

    Public NotInheritable Class Config

        Public Shared Sub Load()

            '   Core.SettingsManager.SetSettingsPath(Core.Manage.Instances.DIH_FolderData)
        End Sub

        Public Shared Function Reset() As Boolean
            Try
                ' IO.File.Delete(Core.SettingsManager.FilePath)
                Return True
            Catch ex As System.Exception
                Return False
            End Try
        End Function

    End Class

    Public NotInheritable Class Engines

        Public Shared Sub Load()

            EO.WebBrowser.Runtime.AddLicense("Kb114+30EO2s3OmxGeCm3MGz8M5nzunz7fGo7vf2HaF3s7P9FOKe5ff2EL112PD9GvZ3s+X1D5+t8PT26KF+xrLUE/Go5Omzy5+v3PYEFO6ntKbC461pmaTA6bto2PD9GvZ3s/MDD+SrwPL3Gp+d2Pj26KFpqbPC3a5rp7XIzZ+v3PYEFO6ntKbC46FotcAEFOan2PgGHeR36d7SGeWawbMKFOervtrI9eBysO3XErx2s7MEFOan2PgGHeR3s7P9FOKe5ff26XXj7fQQ7azcws0X6Jzc8gQQyJ21tMbbtnCttcbcs3Wm8PoO5Kfq6doP")
            EO.WebEngine.Engine.Default.Options.AllowProprietaryMediaFormats()

            'If My.Settings.ConsoleDebug = True Then
            '    Dim DebugConsole As ConsoleHostV2 = New ConsoleHostV2
            '    Dim ConsoleIsHandle As Boolean = DebugConsole.Unsecure_Initialize() 'DebugConsole.Initialize
            '    If ConsoleIsHandle = False Then Process.GetCurrentProcess.Kill()
            'End If

        End Sub

    End Class

    Public NotInheritable Class IOProcess

        Public Shared Sub Load()


            Console.ForegroundColor = ConsoleColor.Green
            Console.WriteLine("Loaded All Libs.")
            Console.ForegroundColor = ConsoleColor.White

            Core.Helpers.Utils.Sleep(1)

        End Sub

    End Class

    Public NotInheritable Class IHelper


        Public Shared Async Sub Load(Optional ByVal progressBar As Guna.UI2.WinForms.Guna2ProgressBar = Nothing)


        End Sub

        Private Shared Sub DownloadEndSybols()
            Console.WriteLine("Symbols Loaded!")
        End Sub

    End Class


End Namespace
