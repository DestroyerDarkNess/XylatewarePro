Imports System.Runtime.InteropServices
Imports System.Text
Imports Microsoft.Win32

Namespace Core.Engine

    Public Class DesktopEmbeder

#Region " Pinvoke "

        <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
        Public Shared Function SendMessageTimeout(ByVal windowHandle As IntPtr, ByVal Msg As UInteger, ByVal wParam As IntPtr, ByVal lParam As IntPtr, ByVal flags As SendMessageTimeoutFlags, ByVal timeout As UInteger, <System.Runtime.InteropServices.Out()> ByRef result As IntPtr) As IntPtr
        End Function

        <DllImport("user32.dll")>
        Public Shared Function EnumWindows(ByVal lpEnumFunc As EnumWindowsProc, ByVal lParam As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

        Public Delegate Function EnumWindowsProc(ByVal hwnd As IntPtr, ByVal lParam As IntPtr) As Boolean

        <DllImport("user32.dll", SetLastError:=True)>
        Public Shared Function FindWindow(ByVal lpClassName As String, ByVal lpWindowName As String) As IntPtr
        End Function

        <DllImport("user32.dll", SetLastError:=True)>
        Public Shared Function FindWindowEx(ByVal parentHandle As IntPtr, ByVal childAfter As IntPtr, ByVal className As String, ByVal windowTitle As IntPtr) As IntPtr
        End Function

        <DllImport("user32.dll", SetLastError:=True)>
        Public Shared Function SetParent(ByVal hWndChild As IntPtr, ByVal hWndNewParent As IntPtr) As IntPtr
        End Function

        <DllImport("user32.dll", SetLastError:=False)>
        Private Shared Function GetDesktopWindow() As IntPtr
        End Function

        <DllImport("user32.dll", CharSet:=CharSet.Auto)>
        Public Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As Integer, ByVal wParam As Integer, ByRef lParam As IntPtr) As IntPtr
        End Function

        <DllImport("user32.dll", EntryPoint:="SetWindowPos")>
        Private Shared Function SetWindowPos(ByVal hWnd As Integer, ByVal hWndInsertAfter As Integer, ByVal X As Integer, ByVal Y As Integer, ByVal cx As Integer, ByVal cy As Integer, ByVal uFlags As UInteger) As Boolean
        End Function

        <DllImport("user32.dll")>
        Private Shared Function ShowWindow(ByVal hWnd As System.IntPtr, ByVal nCmdShow As Integer) As Boolean
        End Function

        <DllImport("user32", CharSet:=CharSet.Auto)>
        Public Shared Function SystemParametersInfo(
            ByVal intAction As Integer,
            ByVal intParam As Integer,
            ByVal strParam As String,
            ByVal intWinIniFlag As Integer) As Integer
        End Function

        <DllImport("user32.dll", SetLastError:=True)>
        Private Shared Function GetWindowInfo(ByVal hwnd As IntPtr, ByRef pwi As WINDOWINFO) As Boolean
        End Function

#End Region

#Region " Enum "

        <Flags>
        Public Enum SendMessageTimeoutFlags As UInteger
            SMTO_NORMAL = &H0
            SMTO_BLOCK = &H1
            SMTO_ABORTIFHUNG = &H2
            SMTO_NOTIMEOUTIFNOTHUNG = &H8
            SMTO_ERRORONEXIT = &H20
        End Enum

        <StructLayout(LayoutKind.Sequential)>
        Public Structure RECT
            Private _Left As Integer
            Private _Top As Integer
            Private _Right As Integer
            Private _Bottom As Integer
        End Structure

        <StructLayout(LayoutKind.Sequential)>
        Structure WINDOWINFO
            Public cbSize As UInteger
            Public rcWindow As RECT
            Public rcClient As RECT
            Public dwStyle As UInteger
            Public dwExStyle As UInteger
            Public dwWindowStatus As UInteger
            Public cxWindowBorders As UInteger
            Public cyWindowBorders As UInteger
            Public atomWindowType As UShort
            Public wCreatorVersion As UShort

            Public Sub New(ByVal filler As Boolean?)
                Me.New()
                cbSize = CType((Marshal.SizeOf(GetType(WINDOWINFO))), UInt32)
            End Sub
        End Structure

        Public Enum WindowStyle
            WS_BORDER = &H800000
            WS_CAPTION = &HC00000
            WS_CHILD = &H40000000
            WS_CLIPCHILDREN = &H2000000
            WS_CLIPSIBLINGS = &H4000000
            WS_DISABLED = &H8000000
            WS_DLGFRAME = &H400000
            WS_GROUP = &H20000
            WS_HSCROLL = &H100000
            WS_MAXIMIZE = &H1000000
            WS_MAXIMIZEBOX = &H10000
            WS_MINIMIZE = &H20000000
            WS_MINIMIZEBOX = &H20000
            WS_OVERLAPPED = &H0
            WS_OVERLAPPEDWINDOW = WS_OVERLAPPED Or WS_CAPTION Or WS_SYSMENU Or WS_SIZEFRAME Or WS_MINIMIZEBOX Or WS_MAXIMIZEBOX
            WS_SIZEFRAME = &H40000
            WS_SYSMENU = &H80000
            WS_TABSTOP = &H10000
            WS_VISIBLE = &H10000000
            WS_VSCROLL = &H200000
        End Enum

#End Region

#Region " Properties "

        Private ServiceHandleEx As IntPtr = IntPtr.Zero
        Public ReadOnly Property ServiceHandle As IntPtr
            <DebuggerStepThrough>
            Get
                Return Me.ServiceHandleEx
            End Get
        End Property

#End Region

#Region " Const "

        Private Const SW_SHOWNOACTIVATE As Integer = 4
        Private Const HWND_TOPMOST As Integer = -1
        Private Const SWP_NOACTIVATE As UInteger = &H10
        Private Const SPI_SETDESKWALLPAPER As UInteger = 20
        Private Const SPIF_UPDATEINIFILE As UInteger = &H1
        Private Const WM_COMMAND As Integer = &H111

#End Region


#Region " Constructor "

        Public Sub New(Optional ByVal ManualParentHandle As Integer = 0)
            If ManualParentHandle = 0 Then
                ServiceHandleEx = GetServiceHandle()
            Else
                ServiceHandleEx = ManualParentHandle
            End If
        End Sub

#End Region

#Region " Public Methods "

        Public Sub EmbedControl(ByVal TargetControl As Control, Optional Show As Boolean = False)
            If ServiceHandleEx = IntPtr.Zero Then
                ServiceHandleEx = GetServiceHandle()
                If ServiceHandleEx = IntPtr.Zero Then
                    Throw New Exception(" Service Handle not Found Info: " & ExMessage)
                End If
            Else
                SetParent(TargetControl.Handle, ServiceHandleEx)
                Realse()
                If Show = True Then
                    TargetControl.Show()  ' ShowInactiveTopmost(TargetControl)
                End If
            End If
        End Sub

        Public Sub EmbedByHandle(ByVal Handle As IntPtr, Optional ByVal FullScreen As Boolean = False)
            If ServiceHandleEx = IntPtr.Zero Then
                ServiceHandleEx = GetServiceHandle()
                If ServiceHandleEx = IntPtr.Zero Then
                    Throw New Exception(" Service Handle not Found Info: " & ExMessage)
                End If
            Else
                For i As Integer = 0 To 5
                    SetParent(Handle, ServiceHandleEx)
                    If FullScreen = True Then
                        SetWindowStyle.SetWindowStyle(Handle, SetWindowStyle.WindowStyles.WS_BORDER)
                        SetWindowState.SetWindowState(Handle, SetWindowState.WindowState.Maximize)
                    End If
                    Realse()
                    ' Core.Helpers.Utils.Sleep(2)
                Next
            End If
        End Sub

        Public Sub SetManualParent(ByVal Handle As IntPtr)
            ServiceHandleEx = Handle
        End Sub

        Public Shared Sub RefreshDesktop()
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, Nothing, SPIF_UPDATEINIFILE)
        End Sub

        Public Shared Sub SetImage(ByVal Filename As String)
            If IO.File.Exists(Filename) = True Then
                SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, Filename, SPIF_UPDATEINIFILE)
            End If
        End Sub

        Public Shared Function IconsVisible() As Boolean
            Try
                Dim hWnd = GetDesktopListView()
                Dim info = New WINDOWINFO(Nothing)
                GetWindowInfo(hWnd, info)
                Return (info.dwStyle And WindowStyle.WS_VISIBLE) = WindowStyle.WS_VISIBLE
            Catch ex As Exception
                Return True
            End Try
        End Function

        Public Shared Sub ToggleDesktopIcons()
            Dim toggleDesktopCommand = New IntPtr(&H7402)
            SendMessage(GetDesktopSHELLDLL_DefView(), WM_COMMAND, toggleDesktopCommand, IntPtr.Zero)
        End Sub

#End Region

#Region " Private Methods "

        Public Shared Property ExMessage As String = String.Empty

        Private Shared Function GetDesktopSHELLDLL_DefView() As IntPtr
            Try
                Dim hShellViewWin = IntPtr.Zero
                Dim hWorkerW = IntPtr.Zero
                Dim hProgman = FindWindow("Progman", "Program Manager")
                Dim hDesktopWnd = GetDesktopWindow()

                If hProgman <> IntPtr.Zero Then
                    hShellViewWin = FindWindowEx(hProgman, IntPtr.Zero, "SHELLDLL_DefView", Nothing)

                    If hShellViewWin = IntPtr.Zero Then

                        Do
                            hWorkerW = FindWindowEx(hDesktopWnd, hWorkerW, "WorkerW", Nothing)
                            hShellViewWin = FindWindowEx(hWorkerW, IntPtr.Zero, "SHELLDLL_DefView", Nothing)
                        Loop While hShellViewWin = IntPtr.Zero AndAlso hWorkerW <> IntPtr.Zero
                    End If
                End If

                Return hShellViewWin
            Catch ex As Exception
                ExMessage = ex.Message
                Return IntPtr.Zero
            End Try
        End Function

        Private Shared Function GetDesktopListView() As IntPtr
            Try
                Dim hDesktopListView As IntPtr = IntPtr.Zero
                Dim hShellViewWin = IntPtr.Zero
                Dim hWorkerW = IntPtr.Zero
                Dim hProgman = FindWindow("Progman", "Program Manager")
                Dim hDesktopWnd = GetDesktopWindow()

                If hProgman <> IntPtr.Zero Then
                    hShellViewWin = FindWindowEx(hProgman, IntPtr.Zero, "SHELLDLL_DefView", Nothing)

                    If hShellViewWin = IntPtr.Zero Then

                        Do
                            hWorkerW = FindWindowEx(hDesktopWnd, hWorkerW, "WorkerW", Nothing)
                            hShellViewWin = FindWindowEx(hWorkerW, IntPtr.Zero, "SHELLDLL_DefView", Nothing)
                        Loop While hShellViewWin = IntPtr.Zero AndAlso hWorkerW <> IntPtr.Zero
                    End If

                    hDesktopListView = FindWindowEx(hShellViewWin, Nothing, "SysListView32", Nothing)

                End If

                Return hDesktopListView
            Catch ex As Exception
                ExMessage = ex.Message
                Return IntPtr.Zero
            End Try
        End Function

        Public Shared Function GetServiceHandle() As IntPtr
            Try
                Dim progman As IntPtr = Engine.Desktop.W32.FindWindow("Progman", Nothing)
                Dim result As IntPtr = IntPtr.Zero
                Engine.Desktop.W32.SendMessageTimeout(progman, &H52C, New IntPtr(0), IntPtr.Zero, Engine.Desktop.W32.SendMessageTimeoutFlags.SMTO_NORMAL, 1000, result)

                Dim workerw As IntPtr = IntPtr.Zero

                Engine.Desktop.W32.EnumWindows(New Engine.Desktop.W32.EnumWindowsProc(Function(tophandle, topparamhandle)
                                                                                          Dim p As IntPtr = Engine.Desktop.W32.FindWindowEx(tophandle, IntPtr.Zero, "SHELLDLL_DefView", IntPtr.Zero)

                                                                                          If p <> IntPtr.Zero Then
                                                                                              workerw = Engine.Desktop.W32.FindWindowEx(IntPtr.Zero, tophandle, "WorkerW", IntPtr.Zero)
                                                                                          End If

                                                                                          Return True
                                                                                      End Function), IntPtr.Zero)

                If workerw = IntPtr.Zero Then
                    ExMessage = "SHELLDLL_DefView Not Found, Make sure your system is windows 8 or higher."
                    Return IntPtr.Zero
                Else
                    Return workerw
                End If

            Catch ex As Exception
                ExMessage = ex.Message
                Return IntPtr.Zero
            End Try
        End Function

        Private Sub Realse()
            Dim dc As IntPtr = Engine.Desktop.W32.GetDCEx(ServiceHandleEx, IntPtr.Zero, CType(&H403, Engine.Desktop.W32.DeviceContextValues))

            If dc <> IntPtr.Zero Then
                Core.Engine.Desktop.W32.ReleaseDC(ServiceHandleEx, dc)
            End If
        End Sub

        Private Sub ShowInactiveTopmost(ByVal frm As System.Windows.Forms.Form)
            ShowWindow(frm.Handle, SW_SHOWNOACTIVATE)
            SetWindowPos(frm.Handle.ToInt32(), HWND_TOPMOST, frm.Left, frm.Top, frm.Width, frm.Height, SWP_NOACTIVATE)
        End Sub

#End Region

        Public Class IContent
            Public Property Hex As String = String.Empty
            Public Property ByteIndex As Integer = 0
            Public Property Enabled As Boolean = False
        End Class

        'Animate controls and elements inside windows

        Private Shared MaskString As String = "90 12 03 80 %id% 00 00 00"

        Public Shared Sub AnimateControls(ByVal Enabled As Boolean)
            Dim registryKey1 As RegistryKey = Registry.CurrentUser.OpenSubKey("Control Panel\Desktop", True)

            Dim objectValue1 As Object = registryKey1.GetValue("UserPreferencesMask")

            Dim GetContent As IContent = GetIndexByte(objectValue1)

            Dim ToInject As String = MaskString

            If Enabled = True Then
                ToInject = MaskString.Replace("%id%", "12")
            Else
                ToInject = MaskString.Replace("%id%", "10")
            End If

            'Dim ArrayHex() As String = GetContent.Hex.Split(" ")
            'ArrayHex(GetContent.ByteIndex) = ToInject
            'Debug.WriteLine("Enablin2g")

            'Dim Perform As String = String.Join(" ", ArrayHex)
            Debug.WriteLine("Perform " & ToInject)

            Dim InjectNewBytes() As Byte = StringToByteArray(ToInject)

            registryKey1.SetValue("UserPreferencesMask", InjectNewBytes)
        End Sub

        Public Shared Function GetIndexByte(ByVal bytes_Input As Byte()) As IContent
            Dim NewContent As New IContent
            Dim strTemp As New System.Text.StringBuilder(bytes_Input.Length * 2)
            Dim OldByte As String = String.Empty
            Dim Index As Integer = -1
            For Each b As Byte In bytes_Input
                Dim CurrentByte As String = Conversion.Hex(b)
                strTemp.Append(CurrentByte & " ")
                If CurrentByte = 0 Then
                    If NewContent.ByteIndex = 0 Then
                        NewContent.Enabled = (Index = "12")
                        NewContent.ByteIndex = Index
                    End If
                Else
                    OldByte = Index
                    Index += 1
                End If
            Next

            NewContent.Hex = strTemp.ToString()

            Return NewContent
        End Function

        Public Shared Function StringToByteArray(s As String) As Byte()
            ' remove any spaces from, e.g. "A0 20 34 34"
            s = s.Replace(" "c, "")
            ' make sure we have an even number of digits
            If (s.Length And 1) = 1 Then
                Throw New FormatException("Odd string length when even string length is required.")
            End If

            ' calculate the length of the byte array and dim an array to that
            Dim nBytes = s.Length \ 2
            Dim a(nBytes - 1) As Byte

            ' pick out every two bytes and convert them from hex representation
            For i = 0 To nBytes - 1
                a(i) = Convert.ToByte(s.Substring(i * 2, 2), 16)
            Next

            Return a

        End Function


    End Class

End Namespace
