
Imports XylatewarePro.Core.Manage

Namespace Core.Model
    Friend Class Startup
        Public Shared Property Configuration As Config


        Private Shared ReadOnly Property StartupTasks As List(Of StartupTask) = New List(Of StartupTask)() From {
         New StartupTask(Sub() Config.Load(), "Loading Config..."),
         New StartupTask(Sub() Engines.Load(), "Loading Engine..."),
         New StartupTask(Sub() IOProcess.Load(), "Loading IO Processes...")
        }
        'New StartupTask(Sub() Symbols.Load(), "Loading Symbols...")

        Private Delegate Sub Update()

        Public Shared Sub Start(ByVal progressBar As Guna.UI2.WinForms.Guna2ProgressBar, Optional ByVal StatusLabel As Label = Nothing)
            StartupTasks.Add(New StartupTask(Sub() IHelper.Load(progressBar), "Loading Helpers..."))

            Dim updateMaximum As Update = Sub()
                                              progressBar.Maximum = StartupTasks.Count
                                          End Sub

            Dim updateValue As Update = Sub()
                                            progressBar.Value += 1
                                            System.Threading.Thread.Sleep(500)
                                        End Sub

            progressBar.Invoke(updateMaximum)

            For Each TaskStart As StartupTask In StartupTasks
                Dim ThreadAction As Action = TaskStart.Action

                StatusLabel?.Invoke(Sub()
                                        StatusLabel.ForeColor = Color.White
                                        StatusLabel.Text = TaskStart.Description
                                    End Sub)

                Task.Run(ThreadAction).Wait()

                StatusLabel?.Invoke(Sub() StatusLabel.ForeColor = Color.DodgerBlue)
                progressBar?.Invoke(updateValue)
            Next

        End Sub

        Public Class StartupTask
            Public ReadOnly Property Action As Action
            Public ReadOnly Property Description As String

            Public Sub New(ByVal actionEx As Action, ByVal descriptionEx As String)
                Action = actionEx
                Description = descriptionEx
            End Sub
        End Class
    End Class
End Namespace
