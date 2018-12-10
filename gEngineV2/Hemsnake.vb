Imports Hemsnake.Projectile
Imports System.IO


Public Class Hemsnake

    Declare Function GetAsyncKeyState Lib "user32.dll" (ByVal a As Int32) As Boolean
    Dim theType As Type = Type.GetType("Hemsnake.Projectile")
    Dim instance As New List(Of Projectile)
    Public Shadows Event paint(ByVal s As Graphics)
    Dim GDI As New GDI

    Dim refreshRate = 30
    Dim a = -0.01, k = 300, h = 500
    Dim r = 50
    Dim angle = 0
    Dim dispsize = New Size(700, 700)

    Dim player As New Snake(New Size(10, 10), Color.DarkGreen)
    Dim myfood As New RectObj(New Size(10, 10), Color.Pink)
    Dim enSnake As New List(Of Snake)
    Dim speed As Integer = 0
    Dim isGameOver As Boolean = False
    Dim isGameOverEnd As Boolean = False
    Dim gameOverSize As Integer = 0
    Dim highScore As Integer = 0
    Dim genEnemyTimer As New Timer
    Dim genEn As eventhandler = New EventHandler(AddressOf generate_Enemy)
    Dim datafile As String = "Hemsnake.dat"
    Dim isKeyDown As Boolean = False
    Dim gameMessages As New List(Of String)
    Dim gameMsgDelay As Integer = 300
    Dim gameMsgCount As Integer = 0
    Dim gameMsgCurrent As Integer = 0
    Dim Padding As Integer = 20

    'closing form
    Private Sub Form1_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        GDI.Close()
        save_File(datafile)
    End Sub

    'load once
    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Me.Size = dispsize
        Me.Text = "Hem Snake"
        read_File(datafile)
        initGame()
        initDispObj(dispsize)
        GDI.initGDI(Me, dispsize.Width, dispsize.Height)
        GDI.initRefreshTimer(refreshRate, True, AddressOf renderingLoop)
        GDI.gDisplay.Clear(Color.LightBlue)


        genEnemyTimer.Enabled = True
        genEnemyTimer.Interval = 10000

        AddHandler genEnemyTimer.Tick, genEn

    End Sub

    'all refresher!!
    Private Sub renderingLoop()
        refresh_tick()
        If Me.Disposing = False And Me.IsDisposed = False And Me.Visible = True Then
            Try
                RaiseEvent paint(GDI.bufferDisp)
                With GDI.gDisplay
                    .DrawImageUnscaled(GDI.backBuffer, 0, 0)
                End With
            Catch ex As Exception
            End Try
        End If
    End Sub

    Private Sub initDispObj(ByVal size As Size)
        Me.SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        Me.SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        Me.SetStyle(ControlStyles.UserPaint, False)
        Me.Size = size
        Me.ClientSize = size
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.FixedSingle
        Me.MinimumSize = size
        Me.MaximumSize = size
        ' Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.SetStyle(ControlStyles.FixedHeight, True)
        Me.SetStyle(ControlStyles.FixedWidth, False)
        Me.Update()
    End Sub

    Public Sub fdf()
        If player.Direction = 90 Or player.Direction = 270 Then
            If CBool(GetAsyncKeyState(Keys.Left)) = True Then
                player.Direction = 180

            ElseIf CBool(GetAsyncKeyState(Keys.Right)) = True Then
                player.Direction = 0
            End If

        ElseIf player.Direction = 0 Or player.Direction = 180 Then
            'if the keyboard key A is pressed move the picture box down by 2
            If CBool(GetAsyncKeyState(Keys.Up)) = True Then
                player.Direction = 90
            ElseIf CBool(GetAsyncKeyState(Keys.Down)) = True Then
                player.Direction = 270
            End If

        End If

    End Sub

    'Game Update
    Private Sub refresh_tick()
        If isGameOver = False Then

            'space bar sucks the food to the player snake
            If player.MP > 0 Then
                If CBool(GetAsyncKeyState(Keys.Space)) = True Then
                    player.Homing(myfood, player.Body(0), 50)
                    player.MP -= 1
                End If
            End If

            Dim i = 0


            While i < instance.Count
                If instance(i).checkBoundary(Me.Size) Then
                    'Me.Controls.RemoveAt(i)
                    instance.RemoveAt(i)
                    i -= 1
                End If
                i += 1
            End While

            For i = 0 To instance.Count - 1
                instance(i).gravityOn()
                instance(i).Location = New Point(instance(i).Location.X + instance(i).Hspeed, instance(i).Location.Y + instance(i).Vspeed)
                instance(i).boundary_bounce(instance(i), 0, 0, Me.Width, Me.Height)
            Next

            For i = 0 To instance.Count - 1
                For j = 0 To instance.Count - 1
                    If instance(i).Collide(instance(i), instance(j)) Then
                    End If
                Next
            Next

            If speed < player.MaxSpeed Then

                speed += 1

            Else
                'function to move the snake is a direction
                '   move_snake(player.Body, player.Direction)
                Dim randtemp

                player.Move()

                If player.CollideSelf() Then
                    player.HP -= 20
                    gameMsgCurrent = 2
                End If

                If player.OutsideBounds(Me.Bounds) Then
                    player.HP -= 10
                    gameMsgCurrent = 3
                End If

                If player.HP <= 0 Then
                    gameOver()
                End If

                Dim index = -1
                'enemy snake AI, randomly generate the a distance to move, and has a 50% of turning left and right
                For i = 0 To enSnake.Count - 1
                    randtemp = Rnd() * 100

                    If SnakeEngine.outside_room(enSnake(i).Body(enSnake(i).Body.Count - 1), Me.Bounds) = True And SnakeEngine.outside_room(enSnake(i).Body(0), Me.Bounds) = True Then
                        index = i
                    End If

                    If player.CollideSnake(enSnake(i)) Then
                        player.HP -= 1
                        gameMsgCurrent = 1
                    End If

                    If enSnake(i).distcount < enSnake(i).maxdist Then
                        enSnake(i).distcount += 1
                    Else

                        'don't turn or generate the next distance to move if outside the room.
                        '   If SnakeEngine.outside_room(enSnake(i).Body(0), Me.Bounds) = False Then

                        '50% chance of left turn or right turn.
                        If Not enSnake(i).OutsideBounds(Me.Bounds) Then
                            If randtemp < 50 Then
                                enSnake(i).Direction = SnakeEngine.turnleft(enSnake(i).Direction)
                            Else
                                enSnake(i).Direction = SnakeEngine.turnright(enSnake(i).Direction)
                            End If

                            enSnake(i).maxdist = Rnd() * 6

                            enSnake(i).maxdist += 6

                            enSnake(i).distcount = 0
                        End If
                    End If

                    'rainbow animation
                    rainbow(enSnake(i).Body)

                    'move the enemy snake
                    enSnake(i).Move()

                Next

                If index > 0 Then
                    enSnake.RemoveAt(index)
                End If


                speed = 0

                'Every 5 collisions we speed up game. speedup variable is used to track how many collision player has made
                'Maxspeed is the delay; it is decremented to speed up the snake. effectively decreasing the delay.

                If SnakeEngine.collision(player.Body(0), myfood) Then


                    player.Score += 10
                    player.HP += 10

                    If player.SpeedUp < 4 Then

                        player.SpeedUp += 1
                    Else

                        If player.MaxSpeed > 4 Then

                            player.MaxSpeed -= 1
                        End If

                        player.SpeedUp = 0

                        Dim se As New SnakeEngine
                        se.change_color(player.Body)

                        '  If player.MP < 100 Then
                        player.MP += 30
                        'End If

                        '     If player.MP > 100 Then
                        'player.MP = 100
                        'End If


                    End If

                    'add the tail
                    player.CreateTail()
                    'generate new location for food
                    SnakeEngine.rand_jump(myfood, Me.Bounds, Padding)
                    'regenerate new location for food if it spawns on the player.
                    SnakeEngine.rand_jump_on_collision(myfood, player, Me.Bounds, Padding)


                End If


            End If
        Else

            If isGameOverEnd Then
                If CBool(GetAsyncKeyState(Keys.Enter)) = True Then
                    Application.Restart()
                End If
                If CBool(GetAsyncKeyState(Keys.Escape)) Then
                    exitRequest()
                End If
            End If
        End If


    End Sub






    'globals for the rainbow color change function
    Dim rainbowcounter As Integer = 0
    Dim rainbowdelay As Integer = 0
    Public Sub rainbow(ByRef a)
        Dim colorlist(8) As Color
        colorlist(0) = Color.Red
        colorlist(1) = Color.Orange
        colorlist(2) = Color.Yellow
        colorlist(3) = Color.LightBlue
        colorlist(6) = Color.Blue
        colorlist(7) = Color.LightGreen
        colorlist(4) = Color.Green
        colorlist(5) = Color.DarkGreen
        colorlist(8) = Color.Violet
        For i = 0 To a.count - 2
            If rainbowdelay < 20 Then
                rainbowdelay += 1
            Else
                rainbowdelay = 0

                If rainbowcounter > 8 Then
                    rainbowcounter = 0
                Else
                    a(i).backcolor = colorlist(rainbowcounter)
                    rainbowcounter += 1
                End If
            End If
        Next
    End Sub



    'Draw stuff here
    Private Sub drawme(ByVal g As Graphics) Handles Me.paint
        g.Clear(Color.Black)

        For i = 0 To instance.Count - 1
            Draw.Rect(g, instance(i))
        Next

        For i = 0 To player.Body.Count - 1
            Draw.Rect(g, player.Body(i))
        Next

        If enSnake.Count > 0 Then
            For i = 0 To enSnake.Count - 1
                Draw.Snake(g, enSnake(i))
            Next
        End If

        If isGameOver = True Then

            If gameOverSize < 40 Then
                gameOverSize += 1
            Else
                isGameOverEnd = True
            End If
            g.DrawString("GAME OVER", New Font("Arial", gameOverSize), Brushes.LightBlue, New Point(Me.Width / 2 - gameOverSize * 4, Me.Height / 2 - gameOverSize * 2))

            If isGameOverEnd = True Then
                Dim size = 20
                g.DrawString("PRESS ENTER TO RESTART", New Font("Arial", size), Brushes.White, New Point(Me.Width / 2 - size * 4, Me.Height / 2 - size * 2 + 40))
            End If

        End If

        If isGameOver = False Then
            angle += 5

            If angle > 360 Then
                angle = 0
            End If
        End If



        '  Draw.TextRotate(g, "HEM WORLD", New Font("Arial", 15), Color.DarkOrange, New Point(90, 80), angle, New Size(-70, -20))
        g.DrawString(player.Score, New Font("Arial", 15), Brushes.White, New Point(Me.Width - 60, 20))
        '  drawTextRotate(g, "HIGH SCORE", New Font("Arial", 15), Color.Red, New Point(Me.Width / 2, Me.Height / 2), angle, New Point(-30, -20))
        Draw.TextCirculate(g, "HIGH SCORE " + highScore.ToString, New Font("Arial", 13), Color.Red, New Point(50, 50 - 20), 20, 10, angle)


        If gameMsgCount <= gameMsgDelay Then
            gameMsgCount += 1
        Else
            gameMsgCurrent += 1
            gameMsgCount = 0
        End If

        If gameMsgCurrent > gameMessages.Count - 1 Then
            gameMsgCurrent = 0
        End If

        g.DrawString("MR HEM'S SNAKE GAME", New Font("Arial", 15), Brushes.Yellow, New Point(Me.Width / 2 - 15 * 8 + 10 * Math.Sin(angle * Math.PI / 180), 20))
        g.DrawString(gameMessages(gameMsgCurrent), New Font("Arial", 13), Brushes.White, New Point(Me.Width - 300, Me.Height - 70))

        g.FillRectangle(Brushes.Red, New Rectangle(Me.Width - 100, 50, 40 * player.HP / player.HPMax, 5))
        g.FillRectangle(Brushes.Blue, New Rectangle(Me.Width - 100, 55, 40 * player.MP / player.MPMax, 5))

        '   Next
        Draw.Rect(g, myfood)
        '   g.FillRectangle(New SolidBrush(myfood.BackColor), New Rectangle(myfood.Location, myfood.Size))

        Dim pts As New List(Of Point)
        pts.Clear()
        For x = 0 To 1000 Step 1
            pts.Add(New Point(x, Mathlib.QuadTP(x, a, k, h)))
        Next
        '    g.DrawCurve(New Pen(Brushes.Black, 5), pts.ToArray)

        '   Me.Text = instance.Count


    End Sub

    Private Sub gameOver()
        isGameOver = True
        gameOverSize = 0
        RemoveHandler genEnemyTimer.Tick, genEn
    End Sub

    Private Sub me_keydown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown

        Dim r As New Random
        If e.KeyCode = Keys.A Then
            instance.Add(DirectCast(Activator.CreateInstance(theType, True), Projectile))
            '  Me.Controls.Add(instance(instance.Count - 1))
            Dim y = r.Next(Me.Height - 60, Me.Height - 50)


            '  instance(instance.Count - 1).Location = New Point(r.Next(35, 45), y)
            instance(instance.Count - 1).Speed = 1.5 * r.Next(12, 14) - 5
            instance(instance.Count - 1).Location = player.Body(0).Location
            If player.Direction = 270 Then
                instance(instance.Count - 1).Direction = 90
            End If

            If player.Direction = 0 Then
                instance(instance.Count - 1).Direction = 0
            End If

            If player.Direction = 180 Then
                instance(instance.Count - 1).Direction = 180
            End If

            If player.Direction = 90 Then
                instance(instance.Count - 1).Direction = 270
            End If

            ' instance(instance.Count - 1). revirpat
            'instance(instance.Count - 1).Gravity = 4
            'instance(instance.Count - 1).Direction = -1 * r.Next(65, 80)
            instance(instance.Count - 1).BackColor = Color.FromArgb(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255))
        End If

        If isKeyDown = False Then
            If player.Direction = 90 Or player.Direction = 270 Then
                If e.KeyCode = Keys.Left Then
                    player.Direction = 180
                ElseIf e.KeyCode = Keys.Right Then
                    player.Direction = 0
                End If
            ElseIf player.Direction = 0 Or player.Direction = 180 Then
                'if the keyboard key A is pressed move the picture box down by 2
                If e.KeyCode = Keys.Up Then
                    player.Direction = 90
                ElseIf e.KeyCode = Keys.Down Then
                    player.Direction = 270
                End If
            End If
            isKeyDown = True
        End If



        'press esc to exit the game
        If CBool(GetAsyncKeyState(Keys.Escape)) Then
            exitRequest()
        End If

    End Sub

    Private Sub exitRequest()
        GDI.refresh.Enabled = False

        Dim response = MsgBox("EXIT?", MsgBoxStyle.YesNo, "ARE YOU SURE?")

        If response = vbYes Then
            'savefile()
            Me.Close()
        Else
            GDI.refresh.Enabled = True
        End If
    End Sub

    Private Sub me_keyup(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        isKeyDown = False
    End Sub

    Public Sub initGame()
        player.Score = 0
        player.MaxSpeed = 4
        player.SpeedUp = 0
        Randomize()
        'create head
        player.Body.Add(New RectObj(New Size(10, 10), New Point(300, 150), Color.DarkGreen))
        player.Direction = SnakeEngine.randDirection
        'create some starting body pieces
        For i = 0 To 3
            player.CreateTail()
        Next

        'intialise food object
        myfood = New RectObj(New Size(10, 10), New Point(200, 200), Color.Pink)
        SnakeEngine.rand_jump(myfood, Me.Bounds, Padding)
        SnakeEngine.rand_jump_on_collision(myfood, player, Me.Bounds, Padding)
        player.HPMax = 100
        player.MPMax = 100
        player.HP = 100
        player.MP = 100

        Me.Location = New Point(Screen.PrimaryScreen.Bounds.Width / 2 - Me.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2 - Me.Height / 2)

        gameMessages.Add("Hold Space for Suction Power!")
        gameMessages.Add("Careful! Other snakes hurt")
        gameMessages.Add("Don't hit yourself too often")
        gameMessages.Add("Going outside is painful...")
        gameMessages.Add("Don't worry, it gets faster...")
        gameMessages.Add("Press ESC to Pause/Exit")

    End Sub

    'every 30 seconds a new snake spawns up to a maximum of 3
    Public Sub generate_Enemy(ByVal sender As System.Object, ByVal e As System.EventArgs)

        If enSnake.Count - 1 <= 1 Then
            enSnake.Add(New Snake(New Size(10, 10), Color.Blue))
            SnakeEngine.newSnake(enSnake(enSnake.Count - 1), 20, New Size(10, 10), New Point(10, 250), 30, 0)
        End If

    End Sub

    Private Sub save_File(ByVal filename As String)
        If highScore < player.Score Then
            Using fo As BinaryWriter = New BinaryWriter(New FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
                fo.Write(player.Score)
            End Using
        End If
    End Sub

    Private Sub read_File(ByVal filename As String)
        If File.Exists(filename) Then
            Using fi As BinaryReader = New BinaryReader(New FileStream(filename, FileMode.Open, FileAccess.Read))
                highScore = fi.ReadInt32()
            End Using
        End If

    End Sub

End Class
