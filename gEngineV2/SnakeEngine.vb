Public Class SnakeEngine

    'turn left function used with enemy AI
    Public Shared Function turnleft(ByVal direct) As Integer
        direct += 90

        If direct >= 360 Then
            direct = 0
        End If
        Return direct

    End Function

    'turn right function used with enemy AI
    Public Shared Function turnright(ByVal direct) As Integer
        direct -= 90

        If direct <= -90 Then
            direct = 270
        End If
        Return direct
    End Function

    'move snake sub
    Public Shared Sub move_snake(ByRef obj As List(Of RectObj), ByVal direction As Integer)
        Dim length = obj.Count - 1
        'creep body piece function. This is the main loop to propagate the body parts
        For i = 0 To length - 1
            obj(length - i).Location = obj(length - i - 1).Location
        Next

        'change direction and speed
        'this part increments the head of the snake by 10 in the proper direction.
        'the way this game is designed, it needs to move the head at the same length of the head.

        If direction = 90 Then
            obj(0).Top -= 10
        End If

        If direction = 180 Then
            obj(0).Left -= 10
        End If


        If direction = 0 Then
            obj(0).Left += 10
        End If

        If direction = 270 Then
            obj(0).Top += 10
        End If


    End Sub

    'randomnly jump in the form maximum is x is 580, maximum y is 360
    Public Shared Sub rand_jump(ByRef a As RectObj, ByVal bounds As Rectangle, ByVal padding As Integer)
        Dim rand As New Random


        Dim x As Integer = 0
        Dim y As Integer = 0
        Dim temp As String
        Dim tt As Integer = 0

        temp = (bounds.Width - 2 * padding).ToString

        For i = 0 To temp.Length - 2
            temp = temp.Remove(temp.ToCharArray.Count - 1)
            tt = CInt(temp.ToString.Last.ToString)
            x += rand.Next(0, tt) * Math.Pow(10, i + 1)
        Next

        temp = (bounds.Height - 2 * padding).ToString

        For i = 0 To temp.Length - 2
            temp = temp.Remove(temp.ToCharArray.Count - 1)
            tt = CInt(temp.ToString.Last.ToString)
            y += rand.Next(0, tt) * Math.Pow(10, i + 1)
        Next


        'generates a number between 0 and 580 in steps of 10
        ' temp = Rnd() * 5
        '  x = temp * 100
        ' temp = Rnd() * 8
        ' x += temp * 10



        'generates a number between 0 and 360 in steps of 10
        ' temp = Rnd() * 3
        '  y = temp * 100
        '  temp = Rnd() * 6
        '  y += temp * 10

        a.Location = New Point(padding + x, padding + y)

        '    myangle.Text = x & "," & y

    End Sub

    'this function regenerates a new location if it collides with a snake.
    Public Shared Sub rand_jump_on_collision(ByRef a As RectObj, ByRef b As Snake, ByVal bounds As Rectangle, padding As Integer)
        Dim ans As Boolean = True

        While ans = True
            For i = 0 To b.Body.Count - 1
                ans = collision(a, b.Body(i))

                If ans = True Then
                    Exit For
                End If
            Next

            If ans = True Then

                rand_jump(a, bounds, padding)

            End If
        End While

    End Sub

    'collision function for two picture box. If collision is true, it returns true
    Public Shared Function collision(ByVal a As RectObj, ByRef b As RectObj) As Boolean

        Dim ans = False

        If a.Left + 9 > b.Left And a.Left + 9 < b.Left + 10 And a.Top + 1 > b.Top And a.Top + 1 < b.Top + 10 Then
            ans = True
        End If

        If a.Left + 9 > b.Left And a.Left + 9 < b.Left + 10 And a.Top + 9 > b.Top And a.Top + 9 < b.Top + 10 Then
            ans = True
        End If

        If a.Left + 1 > b.Left And a.Left + 1 < b.Left + 10 And a.Top + 1 > b.Top And a.Top + 1 < b.Top + 10 Then
            ans = True
        End If

        If a.Left + 1 > b.Left And a.Left + 1 < b.Left + 10 And a.Top + 9 > b.Top And a.Top + 9 < b.Top + 10 Then
            ans = True
        End If
        Return ans

    End Function

    'outside room , if picturebox outside of room return false
    Public Shared Function outside_room(ByVal a As RectObj, ByVal bounds As Rectangle) As Boolean

        Dim ans = True

        If a.Left + 9 > 0 And a.Left + 9 < bounds.Width And a.Top + 1 > 0 And a.Top + 1 < 0 + bounds.Height Then
            ans = False
        End If

        If a.Left + 9 > 0 And a.Left + 9 < 0 + bounds.Width And a.Top + 9 > 0 And a.Top + 9 < 0 + bounds.Height Then
            ans = False
        End If

        If a.Left + 1 > 0 And a.Left + 1 < 0 + bounds.Width And a.Top + 1 > 0 And a.Top + 1 < 0 + bounds.Height Then
            ans = False
        End If

        If a.Left + 1 > 0 And a.Left + 1 < 0 + bounds.Width And a.Top + 9 > 0 And a.Top + 9 < 0 + bounds.Height Then
            ans = False
        End If

        Return ans

    End Function

    'randomly change color of object
    Private colorcounter As Integer

    Public Sub change_color(ByRef a)

        Dim mycolor As Color
        mycolor = Color.FromArgb(Rnd() * 255, Rnd() * 255, Rnd() * 255)

        If colorcounter < 6 Then
            colorcounter += 1
        Else
            colorcounter = 0
        End If

        For i = 0 To a.count - 1
            'a(i).backcolor = colorlist(colorcounter)
            a(i).backcolor = mycolor
        Next

    End Sub

    'rainbow function requires 2 variables
    'rainbowdelay is controls the speed of color change.
    'rainbowcounter is used to index into the colorlist array
    'homing function object a will move to object b at speed

    Public Shared Sub homing(ByRef a As Object, ByRef b As Object, ByVal speed As Double)
        Dim angle, m, temp As Double

        'calculate gradient between two points
        Dim run As Double = (b.Left - a.Left)
        If Not run = 0 Then
            m = (b.Top - a.Top) / run

            'inverse tan to get as angle
            angle = Math.Atan(m)
            'compensates for correct quadrants
            If (b.Left - a.Left) < 0 Then
                temp = -1
            Else
                temp = 1
            End If
            'move object based on angle
            a.Left += temp * Math.Sqrt(speed) * Math.Cos(angle)
            a.Top += temp * Math.Sqrt(speed) * Math.Sin(angle)
            'myangle.Text = angle * 180 / Math.PI
        End If

    End Sub

    Public Shared Sub createTail(ByRef a As Snake)
        Dim len
        Dim height, width As Single
        Dim endIndex = a.Body.Count

        If endIndex < 100 Then
            len = CSng(a.Width)

            'this part will generate the new body piece behind the last body piece, according to which direction is it moving.
            If endIndex < 2 Then
                If a.Direction = 0 Then
                    height = 0
                    width = -1 * len
                ElseIf a.Direction = 90 Then
                    width = 0
                    height = len
                ElseIf a.Direction = 180 Then
                    height = 0
                    width = len
                ElseIf a.Direction = 270 Then
                    width = 0
                    height = -1 * len
                End If
            Else

                If a.Body(endIndex - 2).Left - a.Body(endIndex - 1).Left > 0 Then
                    height = 0
                    width = len
                ElseIf a.Body(endIndex - 2).Left - a.Body(endIndex - 1).Left < 0 Then
                    height = 0
                    width = -1 * len
                ElseIf a.Body(endIndex - 2).Top - a.Body(endIndex - 1).Top > 0 Then
                    width = 0
                    height = len
                ElseIf a.Body(endIndex - 2).Top - a.Body(endIndex - 1).Top < 0 Then
                    width = 0
                    height = -1 * len
                End If

            End If


            Dim loc As Point = New Point(a.Body(endIndex - 1).Left + width, a.Body(endIndex - 1).Top + height)
            a.Body.Add(New RectObj(New Size(10, 10), loc, Color.DarkGreen))
            '   Controls.Add(a.body(a.bodylength))
        End If

    End Sub
    'generates a new snake
    Public Shared Sub newSnake(ByRef snakeInstance As Snake, ByVal length As Integer, ByVal size As Size, ByVal location As Point, ByVal maxdist As Integer, ByVal direction As Integer)


        snakeInstance.Body.Add(New RectObj(size, Color.Purple))
        snakeInstance.Body(0).Location = location
        snakeInstance.maxdist = maxdist
        snakeInstance.distcount = 0

        For i = 1 To length - 1
            snakeInstance.Body.Add(New RectObj(size, Color.Purple))
            snakeInstance.Body(i).Location = New Point(0 - i * size.Width, location.Y)
        Next

        snakeInstance.Direction = direction
    End Sub

    Public Shared Function randDirection() As Integer
        Dim randNum As New Random
        Return randNum.Next(0, 3) * 90
    End Function

End Class
