Public Class Snake
    Inherits RectObj
    Implements IDisposable

    Public Body As New List(Of RectObj) 'main body of the 
    Public MaxSpeed As Integer 'refresh speed delay
    Public Direction As Integer 'direction of object
    Public Score As Integer 'score
    Public SpeedUp As Integer 'trackup food collision
    '  Public bodylength As Integer 'stores current bodylength
    Public maxdist As Integer 'stores current distance to cover
    Public distcount As Integer 'distance counter used to count up to maxdist
    Private _hp As Integer
    Private _mp As Integer
    Public HPMax As Integer
    Public MPMax As Integer

    Public Sub New(ByVal a As Size, ByVal b As Color)
        MyBase.New(a, b)
    End Sub

    Public Sub New(ByVal a As Size, ByVal b As Point, ByVal c As Color)
        MyBase.New(a, b, c)
    End Sub

    Public Property HP() As Integer
        Get
            Return _hp
        End Get
        Set(value As Integer)
            _hp = value
            If value < 0 Then
                _hp = 0
            End If

            If value > HPMax Then
                _hp = HPMax
            End If
        End Set
    End Property

    Public Property MP
        Get
            Return _mp
        End Get
        Set(value)
            _mp = value

            If value < 0 Then
                _mp = 0
            End If

            If value > MPMax Then
                _mp = MPMax
            End If
        End Set
    End Property

    Public Sub CreateTail()
        SnakeEngine.createTail(Me)
    End Sub

    Public Sub Move()
        SnakeEngine.move_snake(Me.Body, Direction)
    End Sub

    Public Sub Homing(ByRef obj As Object, ByVal target As Object, ByVal speed As Integer)
        SnakeEngine.homing(obj, target, speed)
    End Sub

    'if outside room gameover
    Public Function OutsideBounds(ByVal bounds) As Boolean
        If SnakeEngine.outside_room(Body(0), bounds) = True Then
            Return True
            Console.WriteLine("outside")
        Else
            Return False
        End If
    End Function

    'if player body parts collides with enemy snake body parts gameover.
    Public Function CollideSnake(ByVal snake As Snake)
        Dim ans = False
        For j = 0 To Body.Count - 1
            For k = 0 To snake.Body.Count - 1
                If SnakeEngine.collision(Body(j), snake.Body(k)) = True Then
                    ans = True
                    Console.WriteLine("you were killed")
                End If
            Next
        Next
        Return ans
    End Function

    'if player head collide with it's body, game over.
    'only needs to check after 3rd body piece. It is impossible to self collide if you are only 4 pieces long
    Public Function CollideSelf()
        Dim ans = False
        For i = 2 To Body.Count - 1
            If Body.Count - 1 > 4 Then
                If SnakeEngine.collision(Body(0), Body(i)) = True Then
                    Console.WriteLine("Self Collision")
                    ans = True
                End If
            End If
        Next
        Return ans
    End Function

End Class
