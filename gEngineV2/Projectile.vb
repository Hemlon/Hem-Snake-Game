Public Class Projectile

    Public Path As New List(Of Point)
    Private _speed As Single = 3
    Private _direction As Single = -45
    Public Gravity As Single = 0.5
    Public Vspeed As Single
    Public Hspeed As Single
    Private Width As Integer
    Private Height As Integer
    Private Top As Integer
    Private Left As Integer
    Private _size As Size
    Private _location As Point
    Public BackColor As Color
    Event Click()


    Public Sub New()
        Size = New Size(10, 10)
        Width = Size.Width
        Height = Size.Height
        Location = New Point(0, 0)
        Top = Location.Y
        Left = Location.X
        BackColor = Color.Red
        calcSpeeds()
    End Sub

    Public Property Size() As Size
        Get
            Return _size
        End Get
        Set(value As Size)
            _size = New Size(value.Width, value.Height)
            Width = value.Width
            Height = value.Height
        End Set
    End Property

    Public Property Location() As Point
        Get
            Return _location
        End Get
        Set(value As Point)
            _location = New Point(value.X, value.Y)
            Top = value.Y
            Left = value.X
        End Set
    End Property

    Public Property Direction()
        Get
            Return _direction
        End Get
        Set(value)
            _direction = value
            calcSpeeds()
        End Set
    End Property

    Public Property Speed()
        Get
            Return _speed
        End Get
        Set(value)
            _speed = value
            calcSpeeds()
        End Set
    End Property

    Private Sub calcSpeeds()
        vspeed = _speed * Math.Sin(direction * Math.PI / 180)
        hspeed = _speed * Math.Cos(direction * Math.PI / 180)
    End Sub


    Public Overridable Sub click_me(sender As Object, e As MouseEventArgs)

    End Sub

    Public Function checkBoundary(ByVal rect As Size) As Boolean
        If MyClass.Location.X > rect.Width Or MyClass.Location.Y > rect.Height Then
            Return True
        Else
            Return False
        End If
    End Function

    Private gravDelay = 10

    Private gravCount = 0
    Public Sub gravityOn()
        If gravCount < gravDelay Then
            gravCount += 1
        Else
            MyClass.vspeed += gravity
            gravCount = 0
        End If
    End Sub

    Public Function Collide(ByVal pbxplayer As Projectile, ByRef btnhit As Projectile)
        Dim collision = False
        'collision top y 0 to 10 and x 10 to 70 
        Dim colwid = pbxplayer.Width * 2
        If pbxplayer.Top <= (btnhit.Top + btnhit.Size.Height / 2) And pbxplayer.Top + pbxplayer.Size.Height >= (btnhit.Top) Then
            If pbxplayer.Left < (btnhit.Left + btnhit.Size.Width - colwid) And pbxplayer.Left + 20 > (btnhit.Left + colwid) Then
                Bounce(0)
                collision = True
            End If
        End If
        'collision top y 30 to 40 and x 10 to 70 
        If pbxplayer.Top <= (btnhit.Top + btnhit.Size.Height) And pbxplayer.Top >= (btnhit.Top + btnhit.Size.Height / 2) Then
            If pbxplayer.Left < (btnhit.Left + +btnhit.Size.Width - colwid) And pbxplayer.Left + pbxplayer.Size.Width > (btnhit.Left + colwid) Then
                Bounce(0)
                collision = True
            End If
        End If
        'x 70 to 80 and x 
        If pbxplayer.Left <= (btnhit.Left + btnhit.Size.Width) And pbxplayer.Left + pbxplayer.Size.Width >= (btnhit.Left + btnhit.Size.Width - colwid) Then
            If pbxplayer.Top < (btnhit.Top + btnhit.Size.Height) And pbxplayer.Top + pbxplayer.Size.Height > btnhit.Top Then
                Bounce(90)
                collision = True
            End If

        End If
        'x 0 to 10 and y 10 to 30
        If pbxplayer.Left <= (btnhit.Left + colwid) And pbxplayer.Left + pbxplayer.Size.Width >= (btnhit.Left) Then
            If pbxplayer.Top < (btnhit.Top + btnhit.Size.Height) And pbxplayer.Top + pbxplayer.Size.Height > btnhit.Top Then
                Bounce(90)
                collision = True
            End If

        End If

        Return collision
    End Function



    Public Function collide(ByVal objHit As Projectile)
        '    Dim m = Me
        Return collide(Me, objHit)

    End Function


    Public Sub Bounce(ByVal slope As Double, ByVal inelasticdeci As Double)

        If slope = 0 Then
            Vspeed -= inelasticdeci * Vspeed
            Vspeed = -1 * Vspeed
            '  x_speed = -1 * y_speed
        ElseIf slope = 90 Then
            ' Hspeed -= 0.1 * Hspeed
            Hspeed = -1 * Hspeed
        End If

    End Sub

    Public Sub Bounce(ByVal slope As Double)
        Bounce(slope, 0.1)
    End Sub

    Dim padding = 10
    Public Sub boundary_bounce(ByRef gameObj As Projectile, xmin As Integer, ymin As Integer, xmax As Integer, ymax As Integer)

        If gameObj.Top <= ymin + gameObj.Size.Height * 2 + padding Then
            Bounce(0)
        End If

        If gameObj.Top >= ymax - gameObj.Size.Height * 2 - padding Then
            Bounce(0)
        End If

        If gameObj.Left >= xmax - gameObj.Size.Width * 2 - padding Then
            Bounce(90)
        End If

        If gameObj.Left <= xmin + gameObj.Size.Height * 2 + padding Then
            Bounce(90)
        End If

  

    End Sub


End Class
