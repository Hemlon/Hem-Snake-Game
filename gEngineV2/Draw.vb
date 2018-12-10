Public Class Draw

    Public Shared Sub TextRotate(g As Graphics, ByVal text As String, ByVal font As Font, ByVal color As Color, ByVal location As Point, ByVal angle As Integer, ByVal offset As Point)
        g.TranslateTransform(location.X, location.Y)
        g.RotateTransform(angle)
        g.DrawString(text, font, New SolidBrush(color), offset)

        g.RotateTransform(-angle)
        g.TranslateTransform(-location.X, -location.Y)
    End Sub

    Public Shared Sub TextCirculate(g As Graphics, ByVal text As String, ByVal fontType As String, ByVal fontSize As Integer, ByVal color As Color, ByVal location As Point, ByVal r1 As Integer, ByVal r2 As Integer, ByVal angle As Integer)
        g.DrawString(text, New Font(fontType, fontSize), New SolidBrush(color), New Point(location.X + r1 * Math.Cos(angle * Math.PI / 180), location.Y + r2 * Math.Sin(angle * Math.PI / 180)))
    End Sub

    Public Shared Sub TextCirculate(g As Graphics, ByVal text As String, ByVal font As Font, ByVal color As Color, ByVal location As Point, ByVal r1 As Integer, ByVal r2 As Integer, ByVal angle As Integer)
        TextCirculate(g, text, font.ToString, font.Size, color, location, r1, r2, angle)
    End Sub

    Public Shared Sub TextCirculate(g As Graphics, ByVal text As String, ByVal font As Font, ByVal color As Color, ByVal location As Point, ByVal radius As Integer, ByVal angle As Integer)
        TextCirculate(g, text, font, color, location, radius, radius, angle)
    End Sub

    Public Shared Sub Snake(g As Graphics, o As Object)
        For i = 0 To o.body.count - 1
            Rect(g, o.Body(i))
        Next
    End Sub

    Public Shared Sub Rect(g As Graphics, o As Object)
        g.FillRectangle(New SolidBrush(o.BackColor), New Rectangle(o.Location, o.Size))
    End Sub


End Class
