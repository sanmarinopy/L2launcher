<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Mainform
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
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
        btnClose = New Button()
        btnMinimize = New Button()
        btnPlay = New Button()
        btnUpdate = New Button()
        lblProgress = New Label()
        rtbNews = New RichTextBox()
        btnDiscord = New Button()
        btnIG = New Button()
        btnLoginStatus = New Button()
        btnGameStatus = New Button()
        btnUserpanel = New Button()
        SuspendLayout()
        ' 
        ' btnClose
        ' 
        btnClose.BackColor = Color.Transparent
        btnClose.BackgroundImage = My.Resources.Resources.cerrar_normal
        btnClose.BackgroundImageLayout = ImageLayout.Zoom
        btnClose.FlatAppearance.BorderSize = 0
        btnClose.FlatAppearance.MouseDownBackColor = Color.Transparent
        btnClose.FlatAppearance.MouseOverBackColor = Color.Transparent
        btnClose.FlatStyle = FlatStyle.Flat
        btnClose.Location = New Point(1050, 12)
        btnClose.Name = "btnClose"
        btnClose.Size = New Size(58, 40)
        btnClose.TabIndex = 0
        btnClose.UseVisualStyleBackColor = False
        ' 
        ' btnMinimize
        ' 
        btnMinimize.BackColor = Color.Transparent
        btnMinimize.BackgroundImage = My.Resources.Resources.minimizar_normal
        btnMinimize.BackgroundImageLayout = ImageLayout.Zoom
        btnMinimize.FlatAppearance.BorderSize = 0
        btnMinimize.FlatAppearance.MouseDownBackColor = Color.Transparent
        btnMinimize.FlatAppearance.MouseOverBackColor = Color.Transparent
        btnMinimize.FlatStyle = FlatStyle.Flat
        btnMinimize.Location = New Point(1001, 12)
        btnMinimize.Name = "btnMinimize"
        btnMinimize.Size = New Size(58, 40)
        btnMinimize.TabIndex = 1
        btnMinimize.UseVisualStyleBackColor = False
        ' 
        ' btnPlay
        ' 
        btnPlay.BackColor = Color.Transparent
        btnPlay.BackgroundImage = My.Resources.Resources.jugar_normal
        btnPlay.BackgroundImageLayout = ImageLayout.Zoom
        btnPlay.FlatAppearance.BorderSize = 0
        btnPlay.FlatAppearance.MouseDownBackColor = Color.Transparent
        btnPlay.FlatAppearance.MouseOverBackColor = Color.Transparent
        btnPlay.FlatStyle = FlatStyle.Flat
        btnPlay.Location = New Point(27, 409)
        btnPlay.Name = "btnPlay"
        btnPlay.Size = New Size(231, 86)
        btnPlay.TabIndex = 2
        btnPlay.UseVisualStyleBackColor = False
        ' 
        ' btnUpdate
        ' 
        btnUpdate.BackColor = Color.Transparent
        btnUpdate.BackgroundImage = My.Resources.Resources.upgrade_normal
        btnUpdate.BackgroundImageLayout = ImageLayout.Zoom
        btnUpdate.FlatAppearance.BorderSize = 0
        btnUpdate.FlatAppearance.MouseDownBackColor = Color.Transparent
        btnUpdate.FlatAppearance.MouseOverBackColor = Color.Transparent
        btnUpdate.FlatStyle = FlatStyle.Flat
        btnUpdate.Location = New Point(52, 493)
        btnUpdate.Name = "btnUpdate"
        btnUpdate.Size = New Size(158, 61)
        btnUpdate.TabIndex = 3
        btnUpdate.UseVisualStyleBackColor = False
        ' 
        ' lblProgress
        ' 
        lblProgress.AutoSize = True
        lblProgress.BackColor = SystemColors.ActiveCaptionText
        lblProgress.Location = New Point(58, 561)
        lblProgress.Name = "lblProgress"
        lblProgress.Size = New Size(0, 20)
        lblProgress.TabIndex = 4
        ' 
        ' rtbNews
        ' 
        rtbNews.BackColor = SystemColors.InfoText
        rtbNews.BorderStyle = BorderStyle.None
        rtbNews.Font = New Font("Calibri Light", 9.0F)
        rtbNews.ForeColor = SystemColors.InactiveBorder
        rtbNews.Location = New Point(27, 82)
        rtbNews.Name = "rtbNews"
        rtbNews.ReadOnly = True
        rtbNews.Size = New Size(682, 310)
        rtbNews.TabIndex = 5
        rtbNews.Text = ""
        ' 
        ' btnDiscord
        ' 
        btnDiscord.BackColor = Color.Transparent
        btnDiscord.BackgroundImage = My.Resources.Resources.discord_normal
        btnDiscord.BackgroundImageLayout = ImageLayout.Zoom
        btnDiscord.FlatAppearance.BorderSize = 0
        btnDiscord.FlatAppearance.MouseDownBackColor = Color.Transparent
        btnDiscord.FlatAppearance.MouseOverBackColor = Color.Transparent
        btnDiscord.FlatStyle = FlatStyle.Flat
        btnDiscord.Location = New Point(1050, 396)
        btnDiscord.Name = "btnDiscord"
        btnDiscord.Size = New Size(66, 62)
        btnDiscord.TabIndex = 6
        btnDiscord.UseVisualStyleBackColor = False
        ' 
        ' btnIG
        ' 
        btnIG.BackColor = Color.Transparent
        btnIG.BackgroundImage = My.Resources.Resources.ig_normal
        btnIG.BackgroundImageLayout = ImageLayout.Zoom
        btnIG.FlatAppearance.BorderSize = 0
        btnIG.FlatAppearance.MouseDownBackColor = Color.Transparent
        btnIG.FlatAppearance.MouseOverBackColor = Color.Transparent
        btnIG.FlatStyle = FlatStyle.Flat
        btnIG.Location = New Point(1050, 464)
        btnIG.Name = "btnIG"
        btnIG.Size = New Size(66, 61)
        btnIG.TabIndex = 7
        btnIG.UseVisualStyleBackColor = False
        ' 
        ' btnLoginStatus
        ' 
        btnLoginStatus.BackColor = Color.Transparent
        btnLoginStatus.BackgroundImage = My.Resources.Resources.green_circle
        btnLoginStatus.BackgroundImageLayout = ImageLayout.Stretch
        btnLoginStatus.FlatAppearance.BorderSize = 0
        btnLoginStatus.FlatAppearance.MouseDownBackColor = Color.Transparent
        btnLoginStatus.FlatAppearance.MouseOverBackColor = Color.Transparent
        btnLoginStatus.FlatStyle = FlatStyle.Flat
        btnLoginStatus.Location = New Point(27, 36)
        btnLoginStatus.Name = "btnLoginStatus"
        btnLoginStatus.Size = New Size(244, 29)
        btnLoginStatus.TabIndex = 8
        btnLoginStatus.UseVisualStyleBackColor = False
        ' 
        ' btnGameStatus
        ' 
        btnGameStatus.BackColor = Color.Transparent
        btnGameStatus.BackgroundImage = My.Resources.Resources.red_circle
        btnGameStatus.BackgroundImageLayout = ImageLayout.Stretch
        btnGameStatus.FlatAppearance.BorderSize = 0
        btnGameStatus.FlatAppearance.MouseDownBackColor = Color.Transparent
        btnGameStatus.FlatAppearance.MouseOverBackColor = Color.Transparent
        btnGameStatus.FlatStyle = FlatStyle.Flat
        btnGameStatus.Location = New Point(277, 36)
        btnGameStatus.Name = "btnGameStatus"
        btnGameStatus.Size = New Size(244, 29)
        btnGameStatus.TabIndex = 9
        btnGameStatus.UseVisualStyleBackColor = False
        ' 
        ' btnUserpanel
        ' 
        btnUserpanel.BackColor = Color.Transparent
        btnUserpanel.BackgroundImage = My.Resources.Resources.user_normal
        btnUserpanel.BackgroundImageLayout = ImageLayout.Zoom
        btnUserpanel.FlatAppearance.BorderSize = 0
        btnUserpanel.FlatAppearance.MouseDownBackColor = Color.Transparent
        btnUserpanel.FlatAppearance.MouseOverBackColor = Color.Transparent
        btnUserpanel.FlatStyle = FlatStyle.Flat
        btnUserpanel.Location = New Point(1042, 321)
        btnUserpanel.Name = "btnUserpanel"
        btnUserpanel.Size = New Size(84, 69)
        btnUserpanel.TabIndex = 10
        btnUserpanel.UseVisualStyleBackColor = False
        ' 
        ' Mainform
        ' 
        AutoScaleDimensions = New SizeF(8.0F, 20.0F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = SystemColors.ButtonFace
        BackgroundImage = My.Resources.Resources.background
        BackgroundImageLayout = ImageLayout.Stretch
        ClientSize = New Size(1144, 624)
        Controls.Add(btnUserpanel)
        Controls.Add(btnGameStatus)
        Controls.Add(btnLoginStatus)
        Controls.Add(btnIG)
        Controls.Add(btnDiscord)
        Controls.Add(rtbNews)
        Controls.Add(lblProgress)
        Controls.Add(btnPlay)
        Controls.Add(btnMinimize)
        Controls.Add(btnClose)
        Controls.Add(btnUpdate)
        DoubleBuffered = True
        FormBorderStyle = FormBorderStyle.None
        Name = "Mainform"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Mainform"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents btnClose As Button
    Friend WithEvents btnMinimize As Button
    Friend WithEvents btnPlay As Button
    Friend WithEvents btnUpdate As Button
    Friend WithEvents lblProgress As Label
    Friend WithEvents rtbNews As RichTextBox
    Friend WithEvents btnDiscord As Button
    Friend WithEvents btnIG As Button
    Friend WithEvents btnLoginStatus As Button
    Friend WithEvents btnGameStatus As Button
    Friend WithEvents btnUserpanel As Button

End Class
