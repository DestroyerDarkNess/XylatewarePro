Imports System.Drawing.Imaging

Public Class AppNotify
    Implements IDisposable

    Public Sub New()
        MyBase.CreateParams.ExStyle = MyBase.CreateParams.ExStyle Or &H20
        SetStyle(ControlStyles.SupportsTransparentBackColor, True)
        pbox = New PictureBox()

        pbox.BorderStyle = BorderStyle.None
        AddHandler pbox.Paint, AddressOf pbox_Paint
        fadeTimer = New Timer()
        fadeTimer.Interval = 15
        AddHandler fadeTimer.Tick, New EventHandler(AddressOf fadeTimer_Tick)
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Public Overrides Property Text As String
        Get
            Return Label1.Text
        End Get
        Set(value As String)
            Label1.Text = value
            Me.Width = Me.Width + Label1.Width '+ 23
        End Set
    End Property

    Private Delay As Integer = 5000

    Public Property MillisecondDelay As Integer
        Get
            Return Delay
        End Get
        Set(value As Integer)
            Delay = value
        End Set
    End Property

    Private ParentEx As Control = Nothing

    Public Overloads Sub Show(ByVal Parent As Control)

        Parent.BeginInvoke(Sub()
                               ParentEx = Parent
                               Parent.Controls.Add(Me)
                               Dim X As Integer = Val((Parent.Width / 2) - (Me.Width / 2))
                               Dim Y As Integer = Val((Parent.Height - Me.Height) - 50)
                               '  Debug.WriteLine("X=" & Me.Height & "    -    X=" & Parent.Height & " R=" & Y)
                               '   Debug.WriteLine("X=" & X & " Y= " & Y & "    -    X=" & Parent.Width & " Y=" & Parent.Height)
                               Me.Location = New Point(X, Y)
                               Me.BringToFront()
                               Label1.AutoSize = False
                               Label1.Dock = DockStyle.Fill
                               Me.Visible = True
                               Core.Helpers.Utils.Sleep(Delay, Core.Helpers.Utils.Measure.Milliseconds)
                               FadeOut(True)
                           End Sub)
    End Sub


    Public Shared Sub MakeDialog(ByVal Parent As Control, ByVal Text As String, Optional ByVal BorderColor As Color = Nothing)
        If BorderColor = Nothing Then BorderColor = Color.SpringGreen
        Dim Noty As New AppNotify
        Noty.Visible = False
        Noty.Text = Text
        Noty.Guna2Panel1.BorderColor = BorderColor
        Noty.Show(Parent)
    End Sub

    Public ReadOnly Property Faded As Boolean
        Get
            Return blend < 0.5F
        End Get
    End Property

    Public Sub FadeIn()
        stopFade(False)
        createBitmaps()
        startFade(1)
    End Sub

    Public Sub FadeOut(ByVal disposeWhenDone As Boolean)
        stopFade(False)
        createBitmaps()
        disposeOnComplete = disposeWhenDone
        startFade(-1)
    End Sub

    Private Sub createBitmaps()
        Try
            bmpBack = New Bitmap(Me.ClientSize.Width, Me.ClientSize.Height)

            Using gr = Graphics.FromImage(bmpBack)
                gr.Clear(Me.Parent.BackColor)
            End Using

            bmpFore = New Bitmap(bmpBack.Width, bmpBack.Height)
            Me.DrawToBitmap(bmpFore, Me.ClientRectangle)
        Catch ex As Exception
            Try
                ParentEx.Controls.Remove(Me)
            Catch exa As Exception

            End Try

            Me.Dispose()
        End Try
    End Sub

    Private Sub fadeTimer_Tick(ByVal sender As Object, ByVal e As EventArgs)
        blend += blendDir * 0.02F
        Dim done As Boolean = False

        If blend < 0 Then
            done = True
            blend = 0
        End If

        If blend > 1 Then
            done = True
            blend = 1
        End If

        If done Then
            stopFade(True)
        Else
            pbox.Invalidate()
        End If
    End Sub

    Private Sub pbox_Paint(ByVal sender As Object, ByVal e As PaintEventArgs)
        Dim rc As Rectangle = New Rectangle(0, 0, pbox.Width, pbox.Height)
        Dim cm As ColorMatrix = New ColorMatrix()
        Dim ia As ImageAttributes = New ImageAttributes()
        cm.Matrix33 = blend
        ia.SetColorMatrix(cm)
        e.Graphics.DrawImage(bmpFore, rc, 0, 0, bmpFore.Width, bmpFore.Height, GraphicsUnit.Pixel, ia)
        cm.Matrix33 = 1.0F - blend
        ia.SetColorMatrix(cm)
        e.Graphics.DrawImage(bmpBack, rc, 0, 0, bmpBack.Width, bmpBack.Height, GraphicsUnit.Pixel, ia)
    End Sub

    Private Sub stopFade(ByVal complete As Boolean)
        fadeTimer.Enabled = False

        If complete Then

            If Not Faded Then
                Me.Controls.Remove(pbox)
            ElseIf disposeOnComplete Then
                ParentEx.Controls.Remove(Me)
                'stopFade(False)
                'pbox.Dispose()
                'fadeTimer.Dispose()

                Me.Dispose()
            End If
        End If

        If bmpBack IsNot Nothing Then
            bmpBack.Dispose()
            bmpBack = Nothing
        End If

        If bmpFore IsNot Nothing Then
            bmpFore.Dispose()
            bmpFore = Nothing
        End If
    End Sub

    Private Sub startFade(ByVal dir As Integer)
        Me.Controls.Add(pbox)
        Me.Controls.SetChildIndex(pbox, 0)
        blendDir = dir
        fadeTimer.Enabled = True
        fadeTimer_Tick(Me, EventArgs.Empty)
    End Sub

    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        If Not DesignMode Then FadeIn()
    End Sub

    Protected Overrides Sub OnResize(ByVal eventargs As EventArgs)
        pbox.Size = Me.ClientSize
        MyBase.OnResize(eventargs)
    End Sub


    'Overloads Sub Dispose(ByVal disposing As Boolean) 'Implements IDisposable.Dispose
    '    If disposing Then
    '        stopFade(False)
    '        pbox.Dispose()
    '        fadeTimer.Dispose()
    '    End If

    '    MyBase.Dispose(disposing)
    'End Sub

    Private pbox As PictureBox
    Private fadeTimer As Timer
    Private bmpBack, bmpFore As Bitmap
    Private blend As Single
    Private blendDir As Integer = 1

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click
        FadeOut(True)
    End Sub

    Private Sub AppNotify_Load(sender As Object, e As EventArgs) Handles Me.Load

    End Sub

    Private disposeOnComplete As Boolean

End Class
