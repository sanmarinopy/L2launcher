Imports System.IO
Imports System.Diagnostics
Imports System.Net.Http
Imports System.IO.Compression
Imports System.Threading.Tasks
Imports System.Net.Sockets
Imports System.Timers

Public Class Mainform
    ' Configuración del servidor
    Private ReadOnly gameServerIP As String = "127.0.0.1"
    Private ReadOnly updateBaseUrl As String = "http://localhost//updater/"
    Private ReadOnly manifestUrl As String = "http://localhost//updater/manifest.txt"
    Private ReadOnly newsUrl As String = "http://localhost//news/news.txt"

    ' Puertos para verificación
    Private ReadOnly LOGIN_SERVER_PORT As Integer = 2106
    Private ReadOnly GAME_SERVER_PORT As Integer = 7777

    ' Variables de estado
    Private isUpdating As Boolean = False
    Private totalFiles As Integer = 0
    Private processedFiles As Integer = 0
    Private httpClient As HttpClient
    Private serverCheckTimer As Timer

    ' ==================== EVENTOS PRINCIPALES ====================

    Private Async Sub Mainform_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Inicializar HttpClient
        httpClient = New HttpClient()
        httpClient.Timeout = TimeSpan.FromMinutes(5)

        ' Configurar imágenes iniciales
        InitializeButtons()

        ' Configurar barra de progreso
        SetupProgressBar()

        ' Cargar noticias
        Await LoadNewsAsync()

        ' Configurar y iniciar el timer de verificación de servidores
        SetupServerCheckTimer()

        ' Verificar estado inicial de los servidores
        Await CheckServerStatus()
    End Sub

    ' ==================== VERIFICACIÓN DE SERVIDORES ====================

    Private Sub SetupServerCheckTimer()
        serverCheckTimer = New Timer(60000) ' 60 segundos
        AddHandler serverCheckTimer.Elapsed, AddressOf ServerCheckTimer_Elapsed
        serverCheckTimer.AutoReset = True
        serverCheckTimer.Enabled = True
    End Sub

    Private Async Sub ServerCheckTimer_Elapsed(sender As Object, e As ElapsedEventArgs)
        Await CheckServerStatus()
    End Sub

    Private Async Function CheckServerStatus() As Task
        ' Verificar estado del LoginServer (puerto 2106)
        Dim isLoginServerOnline As Boolean = Await CheckPortStatus(gameServerIP, LOGIN_SERVER_PORT)
        UpdateServerStatusIndicator(btnLoginStatus, isLoginServerOnline, "Login Server")

        ' Verificar estado del GameServer (puerto 7777)
        Dim isGameServerOnline As Boolean = Await CheckPortStatus(gameServerIP, GAME_SERVER_PORT)
        UpdateServerStatusIndicator(btnGameStatus, isGameServerOnline, "Game Server")
    End Function

    Private Async Function CheckPortStatus(ip As String, port As Integer) As Task(Of Boolean)
        Try
            Using client As New TcpClient()
                Dim connectTask As Task = client.ConnectAsync(ip, port)
                Dim timeoutTask As Task = Task.Delay(5000) ' Timeout de 5 segundos

                Dim completedTask As Task = Await Task.WhenAny(connectTask, timeoutTask)

                If completedTask Is connectTask AndAlso client.Connected Then
                    client.Close()
                    Return True
                Else
                    Return False
                End If
            End Using
        Catch
            Return False
        End Try
    End Function

    Private Sub UpdateServerStatusIndicator(button As Button, isOnline As Boolean, serverName As String)
        If button.InvokeRequired Then
            button.Invoke(Sub() UpdateServerStatusIndicator(button, isOnline, serverName))
        Else
            If isOnline Then
                button.BackgroundImage = My.Resources.green_circle
                button.Text = $"{serverName} - ONLINE"
                button.ForeColor = Color.White
            Else
                button.BackgroundImage = My.Resources.red_circle
                button.Text = $"{serverName} - OFFLINE"
                button.ForeColor = Color.White
            End If
        End If
    End Sub

    ' ==================== CARGAR NOTICIAS ====================

    Private Async Function LoadNewsAsync() As Task
        Try
            rtbNews.Text = "Cargando noticias..."
            Dim newsContent As String = Await httpClient.GetStringAsync(newsUrl)

            If Not String.IsNullOrEmpty(newsContent) Then
                rtbNews.Text = newsContent
                ApplyNewsFormatting()
            Else
                rtbNews.Text = "No hay noticias disponibles en este momento."
            End If
        Catch ex As Exception
            rtbNews.Text = "No se pudieron cargar las noticias. Error: " & ex.Message
        End Try
    End Function

    Private Sub ApplyNewsFormatting()
        rtbNews.SelectAll()
        rtbNews.SelectionFont = New Font("Segoe UI", 10, FontStyle.Regular)
        rtbNews.SelectionColor = Color.White

        Dim lines As String() = rtbNews.Lines
        For i As Integer = 0 To lines.Length - 1
            If lines(i).StartsWith("[") AndAlso lines(i).EndsWith("]") Then
                rtbNews.Select(rtbNews.GetFirstCharIndexFromLine(i), lines(i).Length)
                rtbNews.SelectionFont = New Font("Segoe UI", 12, FontStyle.Bold)
                rtbNews.SelectionColor = Color.LightSkyBlue
            End If
        Next

        rtbNews.Select(0, 0)
    End Sub

    ' ==================== INICIALIZACIÓN ====================

    Private Sub InitializeButtons()
        btnPlay.BackgroundImage = My.Resources.jugar_normal
        btnClose.BackgroundImage = My.Resources.cerrar_normal
        btnMinimize.BackgroundImage = My.Resources.minimizar_normal
        btnUpdate.BackgroundImage = My.Resources.upgrade_normal
        btnDiscord.BackgroundImage = My.Resources.discord_normal
        btnIG.BackgroundImage = My.Resources.ig_normal
        btnUserpanel.BackgroundImage = My.Resources.user_normal

        ' Configurar botones de estado (inicialmente en gris o rojo)
        btnLoginStatus.BackgroundImage = My.Resources.red_circle
        btnGameStatus.BackgroundImage = My.Resources.red_circle
        btnLoginStatus.Text = "Login Server - VERIFICANDO..."
        btnGameStatus.Text = "Game Server - VERIFICANDO..."
        btnLoginStatus.ForeColor = Color.White
        btnGameStatus.ForeColor = Color.White
        btnLoginStatus.TextAlign = ContentAlignment.MiddleCenter
        btnGameStatus.TextAlign = ContentAlignment.MiddleCenter
        btnLoginStatus.FlatStyle = FlatStyle.Flat
        btnGameStatus.FlatStyle = FlatStyle.Flat
        btnLoginStatus.FlatAppearance.BorderSize = 0
        btnGameStatus.FlatAppearance.BorderSize = 0

    End Sub

    Private Sub SetupProgressBar()
        lblProgress.Text = "Listo para actualizar"
        lblProgress.ForeColor = Color.White
    End Sub

    ' ==================== SISTEMA DE ACTUALIZACIÓN ====================

    Private Async Sub BtnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        If isUpdating Then Return

        isUpdating = True
        ToggleControls(False)

        Try
            UpdateStatus("Descargando lista de actualizaciones...")
            Dim manifestContent As String = Await DownloadManifestAsync()

            UpdateStatus("Procesando lista de archivos...")
            Dim fileList As List(Of String) = ProcessManifest(manifestContent)
            totalFiles = fileList.Count
            processedFiles = 0

            For Each filePath In fileList
                UpdateStatus($"Actualizando: {Path.GetFileName(filePath)}")
                Dim success As Boolean = Await TryForceUpdateFileAsync(filePath)
                If success Then
                    processedFiles += 1
                    UpdateProgress()
                End If
            Next

            UpdateStatus("¡Actualización completada con éxito!", isSuccess:=True)
            MessageBox.Show("Todos los archivos han sido actualizados correctamente.",
                          "Actualización completada",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Information)
        Catch ex As HttpRequestException
            UpdateStatus($"Error de conexión: {ex.Message}", isError:=True)
            MessageBox.Show($"Error de conexión: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            UpdateStatus($"Error durante la actualización: {ex.Message}", isError:=True)
            MessageBox.Show($"Error durante la actualización: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            isUpdating = False
            ToggleControls(True)
        End Try
    End Sub

    Private Async Function DownloadManifestAsync() As Task(Of String)
        Dim response = Await httpClient.GetAsync(manifestUrl)
        response.EnsureSuccessStatusCode()
        Return Await response.Content.ReadAsStringAsync()
    End Function

    Private Function ProcessManifest(content As String) As List(Of String)
        Dim lines As String() = content.Split({vbCrLf}, StringSplitOptions.RemoveEmptyEntries)
        Dim fileList As New List(Of String)

        For Each line In lines
            If line.Contains(",") Then
                fileList.Add(line.Split(","c)(0).Trim())
            End If
        Next

        Return fileList
    End Function

    Private Async Function TryForceUpdateFileAsync(filePath As String) As Task(Of Boolean)
        Dim tempZipPath As String = Path.GetTempFileName()
        Dim downloadUrl As String = updateBaseUrl & filePath.Replace("\", "/") & ".zip"
        Dim destinationPath As String = Path.Combine(Application.StartupPath, filePath)
        Dim destinationDir As String = Path.GetDirectoryName(destinationPath)

        Directory.CreateDirectory(destinationDir)

        If File.Exists(destinationPath) Then
            If Not DeleteFileWithRetry(destinationPath) Then
                UpdateStatus($"No se pudo sobrescribir: {Path.GetFileName(filePath)}", isError:=True)
                Return False
            End If
        End If

        Try
            Using response = Await httpClient.GetAsync(downloadUrl)
                response.EnsureSuccessStatusCode()

                Using fileStream = File.Create(tempZipPath)
                    Await response.Content.CopyToAsync(fileStream)
                End Using
            End Using

            ZipFile.ExtractToDirectory(tempZipPath, destinationDir)
            File.Delete(tempZipPath)
            Return True
        Catch ex As Exception
            UpdateStatus($"Error al actualizar {filePath}: {ex.Message}", isError:=True)
            Return False
        End Try
    End Function

    Private Function DeleteFileWithRetry(filePath As String) As Boolean
        Dim attempts As Integer = 0
        While attempts < 3
            Try
                File.Delete(filePath)
                Return True
            Catch ex As Exception
                attempts += 1
                If attempts < 3 Then
                    UpdateStatus($"Reintentando borrar archivo (intento {attempts})...")
                    Threading.Thread.Sleep(1000)
                End If
            End Try
        End While
        Return False
    End Function

    ' ==================== BARRA DE PROGRESO ====================

    Private Sub UpdateProgress()
        If totalFiles > 0 Then
            lblProgress.Text = $"{processedFiles} de {totalFiles} archivos actualizados"
        End If
    End Sub

    Private Sub UpdateStatus(message As String, Optional isError As Boolean = False, Optional isSuccess As Boolean = False)
        lblProgress.Text = message
        If isError Then
            lblProgress.ForeColor = Color.Red
        ElseIf isSuccess Then
            lblProgress.ForeColor = Color.LightGreen
        Else
            lblProgress.ForeColor = Color.White
        End If
    End Sub

    ' ==================== BOTÓN PLAY ====================

    Private Sub BtnPlay_Click(sender As Object, e As EventArgs) Handles btnPlay.Click
        If isUpdating Then
            MessageBox.Show("Espere a que termine la actualización.",
                          "Actualización en progreso",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Warning)
            Return
        End If

        Try
            LaunchGame()
        Catch ex As Exception
            MessageBox.Show($"Error al iniciar el juego: {ex.Message}",
                          "Error",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LaunchGame()
        Dim executablePathExe As String = Path.Combine(Application.StartupPath, "system", "l2.exe")
        Dim executablePathBin As String = Path.Combine(Application.StartupPath, "system", "l2.bin")

        Dim gamePath As String = If(File.Exists(executablePathExe), executablePathExe,
                               If(File.Exists(executablePathBin), executablePathBin, Nothing))

        If gamePath Is Nothing Then
            Throw New FileNotFoundException("No se encontró el ejecutable del juego (l2.exe o l2.bin)")
        End If

        Dim startInfo As New ProcessStartInfo With {
            .FileName = gamePath,
            .Arguments = $"IP={gameServerIP}",
            .WorkingDirectory = Path.GetDirectoryName(gamePath)
        }

        Process.Start(startInfo)
        Me.Close()
    End Sub

    ' ==================== EFECTOS HOVER ====================

    Private Sub BtnPlay_MouseEnter(sender As Object, e As EventArgs) Handles btnPlay.MouseEnter
        btnPlay.BackgroundImage = My.Resources.jugar_hover
    End Sub

    Private Sub BtnPlay_MouseLeave(sender As Object, e As EventArgs) Handles btnPlay.MouseLeave
        btnPlay.BackgroundImage = My.Resources.jugar_normal
    End Sub

    Private Sub BtnUpdate_MouseEnter(sender As Object, e As EventArgs) Handles btnUpdate.MouseEnter
        btnUpdate.BackgroundImage = My.Resources.upgrade_hover
    End Sub

    Private Sub BtnUpdate_MouseLeave(sender As Object, e As EventArgs) Handles btnUpdate.MouseLeave
        btnUpdate.BackgroundImage = My.Resources.upgrade_normal
    End Sub

    Private Sub BtnClose_MouseEnter(sender As Object, e As EventArgs) Handles btnClose.MouseEnter
        btnClose.BackgroundImage = My.Resources.cerrar_hover
    End Sub

    Private Sub BtnClose_MouseLeave(sender As Object, e As EventArgs) Handles btnClose.MouseLeave
        btnClose.BackgroundImage = My.Resources.cerrar_normal
    End Sub

    Private Sub BtnMinimize_MouseEnter(sender As Object, e As EventArgs) Handles btnMinimize.MouseEnter
        btnMinimize.BackgroundImage = My.Resources.minimizar_hover
    End Sub

    Private Sub BtnMinimize_MouseLeave(sender As Object, e As EventArgs) Handles btnMinimize.MouseLeave
        btnMinimize.BackgroundImage = My.Resources.minimizar_normal
    End Sub

    ' ==================== FUNCIONES DE VENTANA ====================

    Private Sub BtnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        If isUpdating Then
            If MessageBox.Show("Hay una actualización en progreso. ¿Desea salir de todos modos?",
                             "Confirmar",
                             MessageBoxButtons.YesNo,
                             MessageBoxIcon.Warning) = DialogResult.No Then
                Return
            End If
        End If
        Me.Close()
    End Sub

    Private Sub BtnMinimize_Click(sender As Object, e As EventArgs) Handles btnMinimize.Click
        Me.WindowState = FormWindowState.Minimized
    End Sub

    ' ==================== MOVIMIENTO DE VENTANA ====================

    Private isDragging As Boolean = False
    Private mouseOffset As Point

    Private Sub Mainform_MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown
        If e.Button = MouseButtons.Left Then
            isDragging = True
            mouseOffset = New Point(e.X, e.Y)
        End If
    End Sub

    Private Sub Mainform_MouseMove(sender As Object, e As MouseEventArgs) Handles Me.MouseMove
        If isDragging Then
            Dim newLocation As Point = Me.PointToScreen(New Point(e.X, e.Y))
            newLocation.Offset(-mouseOffset.X, -mouseOffset.Y)
            Me.Location = newLocation
        End If
    End Sub

    Private Sub Mainform_MouseUp(sender As Object, e As MouseEventArgs) Handles Me.MouseUp
        isDragging = False
    End Sub

    ' ==================== UTILIDADES ====================

    Private Sub ToggleControls(enabled As Boolean)
        btnPlay.Enabled = enabled
        btnUpdate.Enabled = enabled
        btnMinimize.Enabled = enabled
        btnClose.Enabled = enabled
    End Sub

    ' ==================== EVENTOS PARA BOTÓN USERPANEL ====================

    Private Sub BtnUserpanel_MouseEnter(sender As Object, e As EventArgs) Handles btnUserpanel.MouseEnter
        btnUserpanel.BackgroundImage = My.Resources.user_hover
    End Sub

    Private Sub BtnUserpanel_MouseLeave(sender As Object, e As EventArgs) Handles btnUserpanel.MouseLeave
        btnUserpanel.BackgroundImage = My.Resources.user_normal
    End Sub

    Private Sub BtnUserpanel_Click(sender As Object, e As EventArgs) Handles btnUserpanel.Click
        OpenURL("https://www.yourlink/login.php")
    End Sub

    Private Sub OpenURL(url As String)
        Try
            Process.Start(New ProcessStartInfo With {
            .FileName = url,
            .UseShellExecute = True
        })
        Catch ex As Exception
            MessageBox.Show($"No se pudo abrir el panel de usuario: {ex.Message}",
                      "Error",
                      MessageBoxButtons.OK,
                      MessageBoxIcon.Error)
        End Try
    End Sub


    ' ==================== EVENTOS PARA BOTONES DE REDES SOCIALES ====================

    Private Sub BtnDiscord_MouseEnter(sender As Object, e As EventArgs) Handles btnDiscord.MouseEnter
        btnDiscord.BackgroundImage = My.Resources.discord_hover
    End Sub

    Private Sub BtnDiscord_MouseLeave(sender As Object, e As EventArgs) Handles btnDiscord.MouseLeave
        btnDiscord.BackgroundImage = My.Resources.discord_normal
    End Sub

    Private Sub BtnIG_MouseEnter(sender As Object, e As EventArgs) Handles btnIG.MouseEnter
        btnIG.BackgroundImage = My.Resources.ig_hover
    End Sub

    Private Sub BtnIG_MouseLeave(sender As Object, e As EventArgs) Handles btnIG.MouseLeave
        btnIG.BackgroundImage = My.Resources.ig_normal
    End Sub

    Private Sub OpenSocialMedia(sender As Object, e As EventArgs) Handles btnDiscord.Click, btnIG.Click
        Dim url As String = ""

        If sender Is btnDiscord Then
            url = "https://yourdiscordlink"
        ElseIf sender Is btnIG Then
            url = "https://youriglink"
        End If

        If Not String.IsNullOrEmpty(url) Then
            Try
                Process.Start(New ProcessStartInfo With {
                    .FileName = url,
                    .UseShellExecute = True
                })
            Catch ex As Exception
                MessageBox.Show($"No se pudo abrir el enlace: {ex.Message}",
                              "Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error)
            End Try
        End If
    End Sub


    ' ==================== LIMPIEZA ====================

    Private Sub Mainform_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        serverCheckTimer?.Dispose()
        httpClient?.Dispose()
    End Sub

    Private Sub txtPartner_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub rtbNews_TextChanged(sender As Object, e As EventArgs) Handles rtbNews.TextChanged

    End Sub
End Class