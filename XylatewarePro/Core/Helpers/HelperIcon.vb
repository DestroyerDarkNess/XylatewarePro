Module HelperIcon

    <System.Runtime.CompilerServices.ExtensionAttribute()>
    Friend Function ToIcon(img As Bitmap, makeTransparent As Boolean, colorToMakeTransparent As Color) As Icon
        If makeTransparent Then
            img.MakeTransparent(colorToMakeTransparent)
        End If
        Dim hicon As IntPtr = img.GetHicon()
        Return Icon.FromHandle(hicon)
    End Function

End Module
