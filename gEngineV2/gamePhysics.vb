Public Class gamePhysics

    Private y_speed, x_speed, x_pos, y_pos, angle As Single
    Private collision As Boolean
    Public velocity As Single

    Property yspeed() As Single
        Get
            Return y_speed
        End Get
        Set(ByVal value As Single)
            y_speed = value
        End Set
    End Property

    Property xspeed() As Single
        Get
            Return x_speed
        End Get
        Set(ByVal value As Single)
            x_speed = value
        End Set
    End Property

    Property speed() As Single
        Get
            Return velocity
        End Get
        Set(ByVal value As Single)
            velocity = value
        End Set
    End Property

    Property direction() As Single
        Get
            Return angle
        End Get
        Set(ByVal value As Single)
            angle = value

        End Set
    End Property

    Property Collide() As Boolean
        Get
            Return collision
        End Get
        Set(ByVal value As Boolean)
            collision = value
        End Set
    End Property

    Property y As Single
        Get
            Return y_pos
        End Get
        Set(ByVal value As Single)
            y_pos = value

        End Set
    End Property

    Property x As Single
        Get
            Return x_pos
        End Get
        Set(ByVal value As Single)
            x_pos = value

        End Set
    End Property

    Public Sub free_move(ByRef gameObj)

        y_pos = y_pos + y_speed
        x_pos = x_pos + x_speed

        gameObj.top = y_pos
        gameObj.left = x_pos
    End Sub

    Public Sub Bounce(ByVal slope As Double)

        If slope = 0 Then
            y_speed = -1 * y_speed
            '  x_speed = -1 * y_speed
        ElseIf slope = 90 Then
            x_speed = -1 * x_speed
        End If

    End Sub

    Public Sub calc_speed()

        y_speed = velocity * -1 * Math.Sin(angle * Math.PI / 180)
        x_speed = velocity * Math.Cos(angle * Math.PI / 180)

    End Sub

    Public Sub calc_speed(ByVal velocity, ByVal angle)

        y_speed = velocity * -1 * Math.Sin(angle * Math.PI / 180)
        x_speed = velocity * Math.Cos(angle * Math.PI / 180)

    End Sub

    Public Sub boundary_bounce(ByRef gameObj, xmin, ymin, xmax, ymax)

        If gameObj.Top <= ymin Then
            Bounce(0)
        End If

        If gameObj.Left >= xmax - gameObj.size.width Then
            Bounce(90)
        End If

        If gameObj.Left <= xmin Then
            Bounce(90)
        End If

        If gameObj.Top >= ymax - gameObj.size.height * 2 Then
            Bounce(0)
        End If

    End Sub

    Public Sub obj_collision(ByRef pbxplayer, ByRef btnhit)
        collision = False
        'collision top y 0 to 10 and x 10 to 70 
        If pbxplayer.Top <= (btnhit.Top + btnhit.size.height / 2) And pbxplayer.Top + pbxplayer.size.height >= (btnhit.Top) Then
            If pbxplayer.Left < (btnhit.Left + btnhit.size.width - 10) And pbxplayer.Left + 80 > (btnhit.Left + 10) Then
                Bounce(0)
                collision = True
            End If
        End If
        'collision top y 30 to 40 and x 10 to 70 
        If pbxplayer.Top <= (btnhit.Top + btnhit.size.height) And pbxplayer.Top >= (btnhit.Top + btnhit.size.height / 2) Then
            If pbxplayer.Left < (btnhit.Left + +btnhit.size.width - 10) And pbxplayer.Left + pbxplayer.size.width > (btnhit.Left + 10) Then
                Bounce(0)
                collision = True
            End If
        End If
        'x 70 to 80 and x 
        If pbxplayer.Left <= (btnhit.Left + btnhit.size.width) And pbxplayer.Left + pbxplayer.size.width >= (btnhit.Left + btnhit.size.width - 10) Then
            If pbxplayer.Top < (btnhit.Top + btnhit.size.height) And pbxplayer.Top + pbxplayer.size.height > btnhit.Top Then
                Bounce(90)
                collision = True
            End If

        End If
        'x 0 to 10 and y 10 to 30
        If pbxplayer.Left <= (btnhit.Left + 10) And pbxplayer.Left + pbxplayer.size.width >= (btnhit.Left) Then
            If pbxplayer.Top < (btnhit.Top + btnhit.size.height) And pbxplayer.Top + pbxplayer.size.height > btnhit.Top Then
                Bounce(90)
                collision = True
            End If

        End If

    End Sub


End Class
