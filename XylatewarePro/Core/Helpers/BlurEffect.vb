Imports System.Drawing.Imaging

Namespace Core.Helpers
    Public Class BlurEffect

        Public Shared Sub BlurBitmap(ByRef image As Bitmap, Optional ByVal BlurForce As Integer = 2)
            Dim g As Graphics = Graphics.FromImage(image)
            Dim att As New ImageAttributes
            Dim m As New ColorMatrix
            m.Matrix33 = 0.4
            att.SetColorMatrix(m)
            For x = -1 To BlurForce
                For y = -1 To BlurForce
                    g.DrawImage(image, New Rectangle(x, y, image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, att)
                Next
            Next
            att.Dispose()
            g.Dispose()
        End Sub

    End Class
End Namespace

