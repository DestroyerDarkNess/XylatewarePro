<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Youtube
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.Guna2HtmlToolTip1 = New Guna.UI2.WinForms.Guna2HtmlToolTip()
        Me.WebView1 = New EO.WebBrowser.WebView()
        Me.PanelFX1 = New XylatewarePro.PanelFX()
        Me.WebControl1 = New EO.WinForm.WebControl()
        Me.PanelFX1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Guna2HtmlToolTip1
        '
        Me.Guna2HtmlToolTip1.AllowLinksHandling = True
        Me.Guna2HtmlToolTip1.BackColor = System.Drawing.Color.FromArgb(CType(CType(18, Byte), Integer), CType(CType(19, Byte), Integer), CType(CType(23, Byte), Integer))
        Me.Guna2HtmlToolTip1.BorderColor = System.Drawing.Color.DodgerBlue
        Me.Guna2HtmlToolTip1.Font = New System.Drawing.Font("Segoe UI", 8.0!)
        Me.Guna2HtmlToolTip1.MaximumSize = New System.Drawing.Size(0, 0)
        Me.Guna2HtmlToolTip1.TitleForeColor = System.Drawing.Color.FromArgb(CType(CType(152, Byte), Integer), CType(CType(204, Byte), Integer), CType(CType(255, Byte), Integer))
        '
        'WebView1
        '
        Me.WebView1.ObjectForScripting = Nothing
        Me.WebView1.Url = "https://www.youtube.com/"
        '
        'PanelFX1
        '
        Me.PanelFX1.AutoScroll = True
        Me.PanelFX1.Controls.Add(Me.WebControl1)
        Me.PanelFX1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelFX1.DoubleBuffered = True
        Me.PanelFX1.Location = New System.Drawing.Point(0, 0)
        Me.PanelFX1.Name = "PanelFX1"
        Me.PanelFX1.PreventFlickering = True
        Me.PanelFX1.Size = New System.Drawing.Size(815, 495)
        Me.PanelFX1.TabIndex = 6
        '
        'WebControl1
        '
        Me.WebControl1.BackColor = System.Drawing.Color.White
        Me.WebControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.WebControl1.Location = New System.Drawing.Point(0, 0)
        Me.WebControl1.Name = "WebControl1"
        Me.WebControl1.Size = New System.Drawing.Size(815, 495)
        Me.WebControl1.TabIndex = 0
        Me.WebControl1.Text = "WebControl1"
        Me.WebControl1.WebView = Me.WebView1
        '
        'Youtube
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(28, Byte), Integer), CType(CType(29, Byte), Integer), CType(CType(33, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(815, 495)
        Me.Controls.Add(Me.PanelFX1)
        Me.ForeColor = System.Drawing.Color.White
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "Youtube"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Dashboard"
        Me.PanelFX1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Guna2HtmlToolTip1 As Guna.UI2.WinForms.Guna2HtmlToolTip
    Friend WithEvents PanelFX1 As PanelFX
    Friend WithEvents WebControl1 As EO.WinForm.WebControl
    Friend WithEvents WebView1 As EO.WebBrowser.WebView
End Class
