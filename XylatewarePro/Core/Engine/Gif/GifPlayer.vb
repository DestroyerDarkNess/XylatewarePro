' ***********************************************************************
' Author   : Destroyer
' Modified : 11-November-2015
' Github   : https://github.com/DestroyerDarkNess
' ***********************************************************************
' <copyright file="GifPlayer.vb" company="S4Lsalsoft">
'     Copyright (c) S4Lsalsoft. All rights reserved.
' </copyright>
' ***********************************************************************
Imports VB = Microsoft.VisualBasic


Imports System.ComponentModel

''' ----------------------------------------------------------------------------------------------------
''' <summary>
''' A device insertion and removal monitor.
''' </summary>
''' ----------------------------------------------------------------------------------------------------
Public Class GifPlayer : Inherits NativeWindow : Implements IDisposable


#Region " Constructor "

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Initializes a new instance of <see cref="GifPlayer"/> class.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <DebuggerStepThrough>
    Public Sub New(ByVal GifImage As Image, Optional ByVal Repeat As Boolean = False, Optional ByVal LoadAsyncFrames As Boolean = False)

        Me.events = New EventHandlerList
        Me.CurrentGif = GifImage
        Me.RepeatGif = Repeat

        If LoadAsyncFrames = True Then
            '  Dim Asynctask As New Task(New Action(Async Sub()
            If RunModeEx = ProcessMode.Defaul Then
                If FramesLoaded = False Then
                    GifFramerates = GetFrames(CurrentGif).ToList
                    FramesLoaded = True
                End If
            End If
            '      End Sub), TaskCreationOptions.PreferFairness)
            ' Asynctask.Start()

        End If


    End Sub

#End Region

#Region " Properties "

    Public FramesLoaded As Boolean = False

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Framerate In Millisecons
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    Public Property RunMode As ProcessMode
        <DebuggerStepThrough>
        Get
            Return Me.RunModeEx
        End Get
        Set(value As ProcessMode)
            Me.RunModeEx = value
        End Set
    End Property

    Private RunModeEx As ProcessMode = ProcessMode.Defaul

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Framerate In Millisecons
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    Public Property Framerate As Integer
        <DebuggerStepThrough>
        Get
            Return Me.FramerateEx
        End Get
        Set(value As Integer)
            If value < 40 Then
                value = 40
            End If
            Me.FramerateEx = value
        End Set
    End Property

    Private FramerateEx As Integer = 1

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Gets a value that determines whether the monitor is running.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    Public ReadOnly Property IsRunning As Boolean
        <DebuggerStepThrough>
        Get
            Return Me.isRunningB
        End Get
    End Property
    Private isRunningB As Boolean

#End Region

    Private WithEvents TimeWatcher As Timer = New Timer With {.Enabled = False, .Interval = Me.FramerateEx}

    Public Sub SetInterval(ByVal Millisecons As Integer)
        TimeWatcher.Interval = Millisecons
    End Sub

#Region " Events "

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' A list of event delegates.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    Private ReadOnly events As EventHandlerList

    Private CurrentGif As Image
    Private RepeatGif As Boolean

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Occurs when a drive is inserted, removed, or changed.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    Public Custom Event GifplayerStatusChanged As EventHandler(Of GifPlayerStatusChangedEventArgs)

        <DebuggerNonUserCode>
        <DebuggerStepThrough>
        AddHandler(ByVal value As EventHandler(Of GifPlayerStatusChangedEventArgs))
            Me.events.AddHandler("GifPlayerStatusChangedEvent", value)
        End AddHandler

        <DebuggerNonUserCode>
        <DebuggerStepThrough>
        RemoveHandler(ByVal value As EventHandler(Of GifPlayerStatusChangedEventArgs))
            Me.events.RemoveHandler("GifPlayerStatusChangedEvent", value)
        End RemoveHandler

        <DebuggerNonUserCode>
        <DebuggerStepThrough>
        RaiseEvent(ByVal sender As Object, ByVal e As GifPlayerStatusChangedEventArgs)
            Dim handler As EventHandler(Of GifPlayerStatusChangedEventArgs) =
                DirectCast(Me.events("GifPlayerStatusChangedEvent"), EventHandler(Of GifPlayerStatusChangedEventArgs))

            If (handler IsNot Nothing) Then
                handler.Invoke(sender, e)
            End If
        End RaiseEvent

    End Event

#End Region

    Private GifFramerates As New List(Of GifDecoderManager.Frame)
    Private FramerateCounter As Integer = 0

    Private LoadedFrames As FrameProcessState = FrameProcessState.Defaul

    'Def

    Dim gif As Image = Nothing
    Dim fd As Imaging.FrameDimension = Nothing
    Dim frameCount As Integer = Nothing
    Dim times() As Byte = Nothing
    Dim LocalCounter As Integer = 0
    Dim MaxLocalCounter As Integer = 0

    Private Sub TimeWatcher_Tick(sender As Object, e As EventArgs) Handles TimeWatcher.Tick
        On Error Resume Next

        If RunModeEx = ProcessMode.Runtime Then

            If LoadedFrames = FrameProcessState.Defaul Then

                gif = CurrentGif
                fd = New Imaging.FrameDimension(gif.FrameDimensionsList()(0))
                frameCount = gif.GetFrameCount(fd)
                times = gif.GetPropertyItem(&H5100).Value
                MaxLocalCounter = (frameCount - 1)

                LoadedFrames = FrameProcessState.Getting

            ElseIf LoadedFrames = FrameProcessState.Full Then

                If FramerateCounter = (GifFramerates.Count - 1) Then

                    If Me.RepeatGif = True Then
                        FramerateCounter = 0
                    Else
                        TimeWatcher.Stop()
                    End If

                Else

                    Dim FrameEx As GifDecoderManager.Frame = GifFramerates(FramerateCounter)

                    If FrameEx.Bitmap IsNot Nothing Then

                        Me.OnGifPlayerStatusChanged(New GifPlayerStatusChangedEventArgs(FrameEx))

                    End If

                    FramerateCounter += 1

                End If

            End If

            If LoadedFrames = FrameProcessState.Getting Then

                Dim frames(frameCount) As GifDecoderManager.Frame

                If frameCount > 1 Then

                    If LocalCounter = MaxLocalCounter Then

                        LoadedFrames = FrameProcessState.Full

                    Else

                        gif.SelectActiveFrame(fd, LocalCounter)
                        Dim length As Integer = BitConverter.ToInt32(times, 4 * LocalCounter) * 10
                        Dim NewFrame As GifDecoderManager.Frame = New GifDecoderManager.Frame(length, New Bitmap(gif))
                        GifFramerates.Add(NewFrame)
                        Me.OnGifPlayerStatusChanged(New GifPlayerStatusChangedEventArgs(NewFrame))

                        Application.DoEvents()

                        LocalCounter += 1

                    End If

                Else

                    LoadedFrames = FrameProcessState.Defaul
                    TimeWatcher.Stop()

                End If


            End If

        ElseIf RunModeEx = ProcessMode.Defaul Then

            If FramerateCounter = (GifFramerates.Count - 1) Then

                If Me.RepeatGif = True Then
                    FramerateCounter = 0
                Else
                    TimeWatcher.Stop()
                End If

            Else

                Dim FrameEx As GifDecoderManager.Frame = GifFramerates(FramerateCounter)

                If FrameEx.Bitmap IsNot Nothing Then

                    Me.OnGifPlayerStatusChanged(New GifPlayerStatusChangedEventArgs(FrameEx))

                End If

                FramerateCounter += 1

            End If

        End If

    End Sub

    Public Sub wait(ByVal seconds As Single)
        Static start As Single
        start = VB.Timer()
        Do While VB.Timer() < start + seconds
            System.Windows.Forms.Application.DoEvents()
        Loop
    End Sub

#Region " Event Invocators "

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Raises <see cref="GifplayerStatusChanged"/> event.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <param name="e">
    ''' The <see cref="GifplayerStatusChangedEventArgs"/> instance containing the event data.
    ''' </param>
    ''' ----------------------------------------------------------------------------------------------------
    <DebuggerStepThrough>
    Protected Overridable Sub OnGifPlayerStatusChanged(ByVal e As GifPlayerStatusChangedEventArgs)

        RaiseEvent GifplayerStatusChanged(Me, e)

    End Sub

#End Region

#Region " Events Data "

#Region " DriveStatusChangedEventArgs "

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Contains the event-data of a <see cref="GifPlayerStatusChanged"/> event.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    Public NotInheritable Class GifPlayerStatusChangedEventArgs : Inherits EventArgs

#Region " Properties "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the device event that occurred.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' The drive info.
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
        Public ReadOnly Property FrameData As GifDecoderManager.Frame
            <DebuggerStepThrough>
            Get
                Return Me.FrameDataB
            End Get
        End Property
        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' ( Backing field )
        ''' The device event that occurred.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private ReadOnly FrameDataB As GifDecoderManager.Frame

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Prevents a default instance of the <see cref="GifPlayerStatusChangedEventArgs"/> class from being created.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerNonUserCode>
        Private Sub New()
        End Sub

        <DebuggerStepThrough>
        Public Sub New(ByVal FrameData As GifDecoderManager.Frame)

            Me.FrameDataB = FrameData

        End Sub

#End Region

    End Class

#End Region

#Region " Enumerations "

    Public Enum FrameProcessState As Integer

        Defaul = 0

        Getting = 1

        Full = 2

    End Enum

    Public Enum ProcessMode As Integer

        Defaul = 0

        Runtime = 1

    End Enum

#End Region

#End Region

#Region " Private Methods "

    Private Function GetFrames(ByVal gif As Image) As GifDecoderManager.Frame()
        Return GifDecoderManager.GetFrames(gif)
    End Function

#End Region

#Region " Public Methods "

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Starts monitoring.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <exception cref="Exception">
    ''' Monitor is already running.
    ''' </exception>
    ''' ----------------------------------------------------------------------------------------------------
    <DebuggerStepThrough>
    Public Overridable Sub Start()

        If (Me.Handle = IntPtr.Zero) Then
            MyBase.CreateHandle(New CreateParams)

        End If

        If RunModeEx = ProcessMode.Defaul Then
            If FramesLoaded = False Then
                GifFramerates = GetFrames(CurrentGif).ToList
                FramesLoaded = True
            End If
        End If

        Me.TimeWatcher.Start()
        Me.isRunningB = True

    End Sub



    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Stops monitoring.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <exception cref="Exception">
    ''' Monitor is already stopped.
    ''' </exception>
    ''' ----------------------------------------------------------------------------------------------------
    <DebuggerStepThrough>
    Public Overridable Sub [Stop]()

        Me.isRunningB = False
        Me.TimeWatcher.Stop()

    End Sub

    Public Sub DestroyPlayer()
        MyBase.DestroyHandle()
    End Sub

    Public Sub ChangeImage(ByVal GifImage As Image, Optional ByVal Repeat As Boolean = False)
        Me.isRunningB = False
        Me.TimeWatcher.Stop()
        GifFramerates.Clear()
        Me.CurrentGif = GifImage
        Me.RepeatGif = Repeat
        If RunModeEx = ProcessMode.Defaul Then
            GifFramerates = GetFrames(CurrentGif).ToList
        End If
        Me.TimeWatcher.Start()
        Me.isRunningB = True
    End Sub


#End Region

#Region " IDisposable Implementation "

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' To detect redundant calls when disposing.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    Private isDisposed As Boolean

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Releases all the resources used by this instance.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <DebuggerStepThrough>
    Public Sub Dispose() Implements IDisposable.Dispose

        Me.Dispose(isDisposing:=True)
        GC.SuppressFinalize(obj:=Me)

    End Sub

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    ''' Releases unmanaged and - optionally - managed resources.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <param name="isDisposing">
    ''' <see langword="True"/>  to release both managed and unmanaged resources; 
    ''' <see langword="False"/> to release only unmanaged resources.
    ''' </param>
    ''' ----------------------------------------------------------------------------------------------------
    <DebuggerStepThrough>
    Protected Overridable Sub Dispose(ByVal isDisposing As Boolean)

        If (Not Me.isDisposed) AndAlso (isDisposing) Then

            Me.events.Dispose()
            Me.Stop()

        End If

        Me.isDisposed = True

    End Sub

#End Region

End Class

Public Class GifDecoderManager

#Region " Frame Data "

    Public Class Frame
        Public Property MilliSecondDuration As Integer
        Public Property Bitmap As Image
        Public Sub New(ByVal duration As Integer, ByVal img As Bitmap)
            Me.MilliSecondDuration = duration
            Me.Bitmap = img
        End Sub
    End Class

#End Region

    Public Shared Function GetFrames(ByVal gif As Image) As Frame()
        Dim fd As New Imaging.FrameDimension(gif.FrameDimensionsList()(0))
        Dim frameCount As Integer = gif.GetFrameCount(fd)
        Dim frames(frameCount) As Frame

        If frameCount > 1 Then
            Dim times() As Byte = gif.GetPropertyItem(&H5100).Value
            For i As Integer = 0 To frameCount - 1
                Try
                    gif.SelectActiveFrame(fd, i)
                    Dim length As Integer = BitConverter.ToInt32(times, 4 * i) * 10
                    Dim Bmap As Bitmap = New Bitmap(gif)
                    frames(i) = New Frame(length, Bmap)
                Catch ex As Exception

                End Try
            Next
        End If

        Return frames
    End Function

    Public Shared Function GetFrames(ByVal gif As Image, Optional FrameIndex As Integer = 0) As Frame
        Dim fd As New Imaging.FrameDimension(gif.FrameDimensionsList()(0))
        Dim frameCount As Integer = gif.GetFrameCount(fd)
        Dim frames As Frame = Nothing

        If frameCount > 1 Then
            Dim times() As Byte = gif.GetPropertyItem(&H5100).Value
            gif.SelectActiveFrame(fd, FrameIndex)
            Dim length As Integer = BitConverter.ToInt32(times, 4 * FrameIndex) * 10
            Dim Bmap As Bitmap = New Bitmap(gif)
            frames = New Frame(length, Bmap)
        End If

        Return frames
    End Function

End Class


