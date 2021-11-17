Imports MetroFramework
Imports MetroFramework.Controls
Imports System.IO
Imports System.Linq

Imports Newtonsoft.Json
Imports System.Text.RegularExpressions
Imports System.Net
Imports System.Threading
Imports System.Text



Public Class Main
    Public selected_file As String

    Dim items_count As Integer = 0
    Public trd(10)
    Public y_location As Integer = 78
    Dim Top_panel As New Panel
    Public file_list As New List(Of String)
    Public Selectlected_button As Button
    Public Selected_index As Integer = 0
    Dim MetroPanel2_list_files As New ListBox
    Dim public_fanart As String
    Dim public_icon As String
    Dim all_button As New List(Of Control)
    Dim InitialDirectory As String = "c:\"

    Dim saved_file_name As String = ""
    Dim saved_project_name As String = ""

    Dim button_data As String

    Class SelectablePanel
        Inherits MetroPanel

        Const ScrollSmallChange As Integer = 40
        Const ScrollBigChange As Integer = 100
        Public Sub New()
            SetStyle(ControlStyles.Selectable, True)
            SetStyle(ControlStyles.UserMouse, True)
            TabStop = True
        End Sub

        Protected Overrides Function ProcessCmdKey(ByRef msg As Message, ByVal keyData As Keys) As Boolean
            'If Not Focused Then Return MyBase.ProcessCmdKey(msg, keyData)
            Dim p = AutoScrollPosition

            Select Case keyData
                Case Keys.Home
                    AutoScrollPosition = New Point(-p.X, 0)
                    Return True
                Case Keys.End
                    AutoScrollPosition = New Point(-p.X, 10000)
                    Return True

                Case Keys.Up
                    AutoScrollPosition = New Point(-p.X, -ScrollSmallChange - p.Y)



                    Return True
                Case Keys.Down

                    AutoScrollPosition = New Point(-p.X, ScrollSmallChange - p.Y)
                    Return True
                Case Keys.PageDown
                    AutoScrollPosition = New Point(-p.X, ScrollBigChange - p.Y)
                    Return True
                Case Keys.PageUp
                    AutoScrollPosition = New Point(-p.X, -ScrollBigChange - p.Y)
                    Return True
                Case Else
                    Return MyBase.ProcessCmdKey(msg, keyData)
            End Select
        End Function
    End Class
    Public Class Class1
        Implements IDisposable

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects).
                End If

                ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                ' TODO: set large fields to null.
            End If
            Me.disposedValue = True
        End Sub

        ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
        'Protected Overrides Sub Finalize()
        '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Su

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region
        Public n As String

    End Class
    Public Class Data_entry
        Implements IDisposable

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects).
                End If

                ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                ' TODO: set large fields to null.
            End If
            Me.disposedValue = True
        End Sub

        ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
        'Protected Overrides Sub Finalize()
        '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Su

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region


        Public plot As String = ""
        Public fanart As String = ""
        Public thumbnail As String = ""
        Public imdb As String = ""
        Public link As New List(Of String)

        Public season As String = "0"
        Public episode As String = "0"
        Public title As String = ""
        Public type As String = ""
        Public year As String = ""
        Public originaltitle As String = ""
        Public mediatype As String = ""
        Public genre As String = ""
        Public rating As String = ""
        Public director As String = ""
        Public duration As String = ""
        Public studios As String = ""
        Public writer As String = ""
        Public votes As String = ""
        Public dateadded As String = ""
        Public trailer As String = ""

        Public content As String = ""




    End Class

    Public all_data_list As New List(Of String)
    Public all_converted_list As New List(Of Data_entry)
    Dim MetroPanel1 As New SelectablePanel
    Dim MetroPanel2 As New MetroPanel
    Dim myButtons(20) As Button


    Dim button_font As Font = New Font("Nirmala UI", 10)


    Private isMouseDown As Boolean = False
    Private mouseOffset As Point

    ' Left mouse button pressed

    Public Function refresh_data_row(ByVal data_row, ByVal Parent, ByVal json_row)
        Dim found_link As Boolean = False


        data_row.title = Parent("title")
        If (data_row.title = Nothing) Then
            data_row.title = ""
        End If
        data_row.thumbnail = Parent("thumbnail")
        If (data_row.thumbnail = Nothing) Then
            data_row.thumbnail = ""
        End If
        data_row.fanart = Parent("fanart")
        If (data_row.fanart = Nothing) Then
            data_row.fanart = ""
        End If
        data_row.imdb = Parent("imdb")
        If (data_row.imdb = Nothing) Then
            data_row.imdb = ""
        End If
        data_row.season = Parent("season")
        If (data_row.season = Nothing) Then
            data_row.season = ""
        End If
        data_row.episode = Parent("episode")
        If (data_row.episode = Nothing) Then
            data_row.episode = ""
        End If
        data_row.type = Parent("type")
        If (data_row.type = Nothing) Then
            data_row.type = ""
        End If
        data_row.year = Parent("year")
        If (data_row.year = Nothing) Then
            data_row.year = ""
        End If
        data_row.originaltitle = Parent("originaltitle")

        Try
            If data_row.originaltitle.Length = 0 Then
                data_row.originaltitle = clean_name(data_row.title)
            End If
        Catch ex As Exception
            data_row.originaltitle = clean_name(data_row.title)
        End Try

        data_row.mediatype = Parent("mediatype")
        If (data_row.mediatype = Nothing) Then
            data_row.mediatype = ""
        End If
        data_row.genre = Parent("genre")
        If (data_row.genre = Nothing) Then
            data_row.genre = ""
        End If
        data_row.rating = Parent("rating")
        If (data_row.rating = Nothing) Then
            data_row.rating = ""
        End If
        data_row.director = Parent("director")
        If (data_row.director = Nothing) Then
            data_row.director = ""
        End If
        data_row.duration = Parent("duration")
        If (data_row.duration = Nothing) Then
            data_row.duration = ""
        End If
        data_row.studios = Parent("studios")
        If (data_row.studios = Nothing) Then
            data_row.studios = ""
        End If
        data_row.writer = Parent("writer")
        If (data_row.writer = Nothing) Then
            data_row.writer = ""
        End If
        data_row.votes = Parent("votes")
        If (data_row.votes = Nothing) Then
            data_row.votes = ""
        End If
        data_row.plot = Parent("plot")
        If (data_row.plot = Nothing) Then
            data_row.plot = ""
        End If
      
        data_row.dateadded = Parent("dateadded")
        If (data_row.dateadded = Nothing) Then
            data_row.dateadded = ""
        End If
        data_row.trailer = Parent("trailer")
        If (data_row.trailer = Nothing) Then
            data_row.trailer = ""
        End If
        data_row.content = Parent("content")
        If (data_row.content = Nothing) Then
            data_row.content = ""
        End If
        data_row.link.Clear()
        If Parent("link").GetType Is GetType(System.String) Then
            data_row.link.Add(Parent("link"))
        Else
            For Each links In Parent("link")
                data_row.link.Add(links)
                found_link = True
            Next
        End If



        json_row = JsonConvert.SerializeObject(data_row)
        Return json_row
    End Function
    ' MouseMove used to check if mouse cursor is moving
  




    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        ServicePointManager.SecurityProtocol = 3072

        With MetroPanel1
            .BackColor = Color.FromArgb(24, 30, 54)
            .Dock = DockStyle.Left
            .Size = New Point(186, 577)
            .AutoScroll = True
            .VerticalScroll.Enabled = True
            .VerticalScrollbar = True
            .VerticalScroll.Enabled = True
            .VerticalScrollbarSize = 10
            .UseCustomBackColor = True
            .UseCustomForeColor = True

            AddHandler .MouseDown, AddressOf MetroPanelClickEvent






        End With
        Me.Controls.Add(MetroPanel1)
        With MetroPanel2
            .BackColor = Color.FromArgb(24, 30, 54)
            .Dock = DockStyle.Left
            .Size = New Point(156, 577)
            .AutoScroll = True
            .VerticalScroll.Enabled = True
            .VerticalScrollbar = True
            .VerticalScroll.Enabled = True
            .VerticalScrollbarSize = 10
            .UseCustomBackColor = True
            .UseCustomForeColor = True







        End With
        Me.Controls.Add(MetroPanel2)


        With MetroPanel2_list_files
            .Dock = DockStyle.Left
            .BackColor = Color.FromArgb(24, 30, 54)
            .Font = button_font
            .ForeColor = Color.FromArgb(0, 156, 149)
            .Size = New Point(156, 577)
            AddHandler .Click, AddressOf ListBoxClickevent
            AddHandler .MouseDown, AddressOf MetroPanel2ClickEvent
        End With
        MetroPanel2.Controls.Add(MetroPanel2_list_files)


        update_list_items()


    End Sub
    Public Sub ListBoxClickevent(ByVal sender As ListBox, ByVal e As System.EventArgs) 'file lists
        Dim item_clicked As String
        Dim File_Text As String = ""
        Dim file_path As String = ""
        If sender.SelectedIndex = -1 Then
            Exit Sub

        End If

        item_clicked = sender.SelectedItem.ToString
        If item_clicked.Contains("- Add File...") Then
            OpenFileDialog1.InitialDirectory = InitialDirectory
            OpenFileDialog1.Filter = "txt files All files (*.*)|*.*|(*.json)|*.json"
            OpenFileDialog1.FileName = ""
            OpenFileDialog1.FilterIndex = 2
            OpenFileDialog1.RestoreDirectory = True

            ' Call ShowDialog.
            Dim result As DialogResult = OpenFileDialog1.ShowDialog()

            ' Test result.
            If result = Windows.Forms.DialogResult.OK Then
                file_path = OpenFileDialog1.FileName
                ' Get the file name.

                Try
                    ' Read in text.
                    File_Text = File.ReadAllText(file_path)



                Catch ex As Exception

                    ' Report an error.
                    Me.Text = "Error"

                End Try
            End If
           


            saved_file_name = Path.GetFileName(file_path)
            sender.Items.Add(Path.GetFileName(file_path))
            file_list.Add(file_path)
            selected_file = file_path
            'sender.SelectedIndex = sender.SelectedIndex + 1
            sender.SelectedItem = Path.GetFileName(file_path)
        Else
            file_path = file_list.Item(sender.SelectedIndex)
            saved_file_name = Path.GetFileName(file_list.Item(sender.SelectedIndex))
            File_Text = File.ReadAllText(file_list.Item(sender.SelectedIndex))
            selected_file = file_list.Item(sender.SelectedIndex)
        End If
        If file_path.Contains(".xml") Then
            clear_movie_list()
            Try
                ' Read in text.
                File_Text = File.ReadAllText(file_path)



            Catch ex As Exception

                ' Report an error.
                Me.Text = "Error"

            End Try
            parse_xml(File_Text, file_path)
        Else
            clear_movie_list()
            MetroPanel1.Enabled = False

            refresh_movie_list(File_Text)
            MetroPanel1.Enabled = True
        End If
        

    End Sub
    Public Sub update_buttons()
        Dim parent As New Newtonsoft.Json.Linq.JObject
        Dim json_row As String = ""
        Dim data_row As New Data_entry
        Dim precentage As Integer = 0
        Dim loding_text As New Label 'label Logo
        With loding_text
            .Location = New Point(50, 50)
            .BackColor = Color.FromArgb(24, 30, 54)
            .ForeColor = Color.FromArgb(0, 156, 149)
            .Text = "MicroJen"
            .Font = button_font

        End With
        Top_panel.Controls.Add(loding_text)
        loding_text.Text = "Clear List"


        Dim i As Integer = 0
        Dim max_items As Integer = 0


        For Each item In all_data_list
            max_items = all_data_list.Count
            parent = Newtonsoft.Json.Linq.JObject.Parse(item)


            loding_text.Text = "Loading " + precentage.ToString + "%"

            precentage = ((i * 100) / max_items)
            i += 1

            Application.DoEvents()


            data_row.title = parent("title")
            json_row = refresh_data_row(data_row, parent, json_row)
            add_button(json_row, data_row.title)

        Next
    End Sub

    Public Sub refresh_movie_list(ByVal File_Text As String)
        Dim parent As Newtonsoft.Json.Linq.JObject = Newtonsoft.Json.Linq.JObject.Parse(File_Text)

        Dim data_row As New Data_entry
        Dim json_row As String = ""
        Dim precentage As Integer = 0
        Dim loding_text As New Label 'label Logo

        With loding_text
            .Location = New Point(50, 50)
            .BackColor = Color.FromArgb(24, 30, 54)
            .ForeColor = Color.FromArgb(0, 156, 149)
            .Text = "MicroJen"
            .Font = button_font

        End With
        Top_panel.Controls.Add(loding_text)
        loding_text.Text = "Clear List"

        clear_movie_list()
        Dim i As Integer = 0
        Dim max_items As Integer = 0

        For Each item In parent("items")
            max_items += 1

        Next

        For Each item In parent("items")
            loding_text.Text = "Loading " + precentage.ToString + "%"

            precentage = ((i * 100) / max_items)
            i += 1

            Application.DoEvents()


            data_row.title = item("title")
            json_row = refresh_data_row(data_row, item, json_row)
            'add_button(json_row, data_row.title)
            all_data_list.Add(json_row)

        Next
        loding_text.Dispose()
        update_buttons()

    End Sub
    Public Sub clear_movie_list()
        Dim i As Integer

        For i = (MetroPanel1.Controls.Count - 1) To 0 Step -1
            MetroPanel1.Controls.RemoveAt(i)
        Next

        y_location = 78
        all_data_list.Clear()
        Selected_index = 0
        ComboBox1.Items.Clear()

    End Sub
    Public Sub Fanart_image_download(ByVal addr As String)
        Try
            PictureBox1.Image = Image.FromStream(New WebClient().OpenRead(addr))
        Catch ex As Exception
            PictureBox1.Image = My.Resources._404
        End Try
    End Sub
    Public Sub Icon_image_download(ByVal addr As String)
        Try
            PictureBox2.Image = Image.FromStream(New WebClient().OpenRead(addr))
        Catch ex As Exception
            PictureBox2.Image = My.Resources.broken_icon

        End Try
    End Sub
    Public Sub refresh_data_list()

        Dim addr As String
        Dim link_type
        Dim parent As Newtonsoft.Json.Linq.JObject = Newtonsoft.Json.Linq.JObject.Parse(all_data_list(button_data))
        ComboBox1.Items.Clear()

        RichTextBox1.Text = parent("plot")
        addr = parent("fanart")
        public_fanart = addr
        Timer2.Enabled = False

        Timer2.Enabled = True






        addr = parent("thumbnail")

        public_icon = addr

        TextBox1.Text = parent("title")
        TextBox2.Text = parent("thumbnail")
        TextBox3.Text = parent("fanart")
        RichTextBox2.Text = parent("plot")
        TextBox5.Text = parent("imdb")


        TextBox4.Text = parent("season")
        TextBox8.Text = parent("episode")
        TextBox6.Text = parent("type")
        TextBox18.Text = parent("year")

        TextBox11.Text = parent("originaltitle")
        TextBox7.Text = parent("mediatype")
        TextBox12.Text = parent("genre")
        TextBox9.Text = parent("rating")
        TextBox14.Text = parent("director")
        TextBox13.Text = parent("duration")
        TextBox15.Text = parent("studios")
        TextBox16.Text = parent("writer")
        TextBox10.Text = parent("votes")

        TextBox19.Text = parent("dateadded")
        TextBox17.Text = parent("trailer")
        link_type = False



        For Each links In parent("link")
            ComboBox1.Items.Add(links)
            link_type = True
        Next

        Try
            ComboBox1.SelectedIndex = 0
        Catch ex As Exception

        End Try
        RichTextBox3.Text = all_data_list(button_data)
        Selected_index = button_data
    End Sub

    Public Sub ButtonHoverEvent(ByVal sender As Button, ByVal e As System.EventArgs)

        button_data = sender.Tag
        Dim addr As String
        Dim link_type
        Dim parent As Newtonsoft.Json.Linq.JObject = Newtonsoft.Json.Linq.JObject.Parse(all_data_list(sender.Tag))
        ComboBox1.Items.Clear()
        ComboBox1.Text = ""
        RichTextBox1.Text = parent("plot")
        addr = parent("fanart")
        public_fanart = addr
        Timer2.Enabled = False
        Timer2.Enabled = True





        addr = parent("thumbnail")
        public_icon = addr


        TextBox1.Text = parent("title")
        TextBox2.Text = parent("thumbnail")
        TextBox3.Text = parent("fanart")
        RichTextBox2.Text = parent("plot")
        TextBox5.Text = parent("imdb")


        TextBox4.Text = parent("season")
        TextBox8.Text = parent("episode")
        TextBox6.Text = parent("type")
        TextBox18.Text = parent("year")

        TextBox11.Text = parent("originaltitle")
        TextBox7.Text = parent("mediatype")
        TextBox12.Text = parent("genre")
        TextBox9.Text = parent("rating")
        TextBox14.Text = parent("director")
        TextBox13.Text = parent("duration")
        TextBox15.Text = parent("studios")
        TextBox16.Text = parent("writer")
        TextBox10.Text = parent("votes")

        TextBox19.Text = parent("dateadded")
        TextBox17.Text = parent("trailer")
        TextBox20.Text = parent("content")
        link_type = False



        For Each links In parent("link")
            ComboBox1.Items.Add(links)
            link_type = True
        Next

        Try
            ComboBox1.SelectedIndex = 0
        Catch ex As Exception

        End Try
        RichTextBox3.Text = all_data_list(sender.Tag)
        RichTextBox2.Text = parent("plot").ToString

        Selected_index = sender.Tag
    End Sub


    Public Sub MetroPanel2ClickEvent(ByVal sender As Object, ByVal e As MouseEventArgs)
        If e.Button = MouseButtons.Right Then
            ContextMenuStrip2.Show(MousePosition)
        End If



    End Sub

    Public Sub MetroPanelClickEvent(ByVal sender As Object, ByVal e As MouseEventArgs)
        If e.Button = MouseButtons.Right Then
            ContextMenuStrip1.Show(MousePosition)
        End If



    End Sub

    Public Sub ButtonRightClickEvent(ByVal sender As Button, ByVal e As System.Windows.Forms.MouseEventArgs)
        If e.Button = MouseButtons.Right Then
            ContextMenuStrip1.Show(MousePosition)
        End If



    End Sub

    Private Function GetImageFromURL(ByVal url As String) As Image

        Dim retVal As Image = Nothing

        Try
            If Not String.IsNullOrEmpty(url) Then
                Dim req As System.Net.WebRequest = System.Net.WebRequest.Create(url.Trim)

                Using request As System.Net.WebResponse = req.GetResponse
                    Using stream As System.IO.Stream = request.GetResponseStream
                        retVal = New Bitmap(System.Drawing.Image.FromStream(stream))
                    End Using
                End Using
            End If

        Catch ex As Exception
            MessageBox.Show(String.Format("An error occurred:{0}{0}{1}", _
                                          vbCrLf, ex.Message), _
                                          "Exception Thrown", _
                                          MessageBoxButtons.OK, _
                                          MessageBoxIcon.Warning)

        End Try

        Return retVal

    End Function


    Public Sub add_button(ByVal button_data As String, ByVal button_name As String)
        Dim x As Integer = 20


        Dim test As Integer


        Dim xPos As Integer = 0

        Dim yPos As Integer = 0

        Dim regex_n2, matches_n2, matches_n3

        Dim new_color As String = ""




        Dim myButtons As New Button

        button_name = button_name.Replace("[/COLOR]", "")
        regex_n2 = New Regex("\[COLOR=(.+?)\]")
        matches_n2 = regex_n2.Matches(button_name)

        If matches_n2.count > 0 Then
            new_color = (matches_n2(0).Groups(1).Value)
            button_name = button_name.Replace(String.Format("[COLOR={0}]", new_color), "")

        Else
            regex_n2 = New Regex("\[COLOR (.+?)\]")
            matches_n3 = regex_n2.Matches(button_name)
            If matches_n3.count > 0 Then
                new_color = (matches_n3(0).Groups(1).Value)
                button_name = button_name.Replace(String.Format("[COLOR {0}]", new_color), "")
            Else

                test = 0
            End If

            End If


            With (myButtons)

                .Tag = Selected_index ' Tag of button

                .Width = MetroPanel1.Size.Width ' Width of button

                .Height = 40 ' Height of button


                .FlatStyle = FlatStyle.Flat
                .FlatAppearance.BorderSize = 0
                '.Dock = DockStyle.Top
                Dim b = DirectCast(myButtons, Button)
                b.FlatAppearance.MouseOverBackColor = Color.LightGray 'I AM STUCK HERE



                xPos = 0

                yPos = y_location

                .Font = button_font

                .BackColor = Color.Black
                If new_color <> "" Then
                    .ForeColor = Color.FromName(new_color)
                Else
                    .ForeColor = Color.White
                End If

                ' Location of button:

                .Left = xPos

                .Top = yPos


                ' Write English Character:

                .Text = button_name
                .TextImageRelation = TextImageRelation.TextBeforeImage

                ' Add buttons to a Panel:









                AddHandler .MouseHover, AddressOf ButtonHoverEvent
                AddHandler .MouseDown, AddressOf ButtonRightClickEvent


            End With
            y_location += 40 - 2
            MetroPanel1.Controls.Add(myButtons) ' Let panel hold the Buttons
            all_button.Add(myButtons)

            Selectlected_button = myButtons
            Selected_index += 1
    End Sub
    Public Sub update_list_items()
        Dim x As Integer = 20
        Dim y_location As Integer = 78


        Dim xPos As Integer = 0

        Dim yPos As Integer = 0

        For i As Integer = 0 To 19

            myButtons(i) = New Button

        Next





        Dim list_button As New Button
        Dim j As Integer
        j = 0

        'For i = 0 To 19

        '    With (myButtons(i))

        '        .Tag = i + 1 ' Tag of button

        '        .Width = MetroPanel1.Size.Width ' Width of button

        '        .Height = 40 ' Height of button


        '        .FlatStyle = FlatStyle.Flat
        '        .FlatAppearance.BorderSize = 0
        '        '.Dock = DockStyle.Top


        '        xPos = 0

        '        yPos = y_location

        '        .Font = button_font

        '        .BackColor = Color.FromArgb(0, 126, 249)
        '        .ForeColor = Color.White
        '        ' Location of button:

        '        .Left = xPos

        '        .Top = yPos


        '        ' Write English Character:

        '        .Text = "Button " + Str(i)
        '        .TextImageRelation = TextImageRelation.TextBeforeImage

        '        ' Add buttons to a Panel:
        '        MetroPanel1.Controls.Add(myButtons(i)) ' Let panel hold the Buttons








        '        ' AddHandler .Click, AddressOf ButtonClickEvent
        '        '  AddHandler .Leave, AddressOf ButtonLeaveEvent


        '    End With

        '    Dim tempButtons As New Button



        '    y_location += myButtons(i).Size.Height - 2

        'Next


        With (Top_panel)
            .BackColor = Color.FromArgb(24, 30, 54)
            .Size = New Point(299, 72)
            .Dock = DockStyle.Top


        End With

        MetroPanel2.Controls.Add(Top_panel) ' Let panel hold the Buttons

        Dim Logo_img As New PictureBox 'Logo



        With Logo_img
            .Image = My.Resources.logo
            .Size = New Point(41, 41)
            .Location = New Point(12, 16)
            .SizeMode = PictureBoxSizeMode.Zoom
            .BackColor = Color.Transparent
            .ForeColor = Color.FromArgb(0, 156, 149)

        End With
        SetStyle(ControlStyles.SupportsTransparentBackColor, True)
        Top_panel.Controls.Add(Logo_img)

        Dim logo_text As New Label 'label Logo
        With logo_text
            .Location = New Point(59, 30)
            .BackColor = Color.FromArgb(24, 30, 54)
            .ForeColor = Color.FromArgb(0, 156, 149)
            .Text = "Media List"
            .Font = button_font

        End With
        Top_panel.Controls.Add(logo_text)



    End Sub

    Private Sub MovieToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MovieToolStripMenuItem.Click
        Dim imdb_id As String
        Dim all_data

        Dim json_row As String = ""
        imdb_id = InputBox("Enter Imdb number")
        If imdb_id.Contains("tt") Then


            all_data = Imdb_Data.get_all_movie_data(imdb_id, "0", "1")
            all_data = all_data
            Dim data_row As New Data_entry
            data_row.title = all_data("title")
            json_row = refresh_data_row(data_row, all_data, json_row)

            all_data_list.Insert(Selected_index, json_row)

        End If
        clean_buttons()
        update_buttons()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Close()

    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.WindowState = FormWindowState.Minimized

    End Sub




    Private Sub ToolStripLabel1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub ToolStripLabel2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub MenuStrip1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MenuStrip1.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Left Then
            ' Get the new position
            mouseOffset = New Point(-e.X, -e.Y)
            ' Set that left button is pressed
            isMouseDown = True
        End If
    End Sub

    Private Sub MenuStrip1_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MenuStrip1.MouseMove
        If isMouseDown Then
            Dim mousePos As Point = Control.MousePosition
            ' Get the new form position
            mousePos.Offset(mouseOffset.X, mouseOffset.Y)
            Me.Location = mousePos
        End If
    End Sub

    Private Sub MenuStrip1_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MenuStrip1.MouseUp
        If e.Button = Windows.Forms.MouseButtons.Left Then
            isMouseDown = False
        End If
    End Sub

    Private Sub TitleToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TitleToolStripMenuItem.Click
        Dim imdb_id, all_data
        Dim json_row As String = ""
        imdb_id = InputBox("Enter Imdb number")
        all_data = Imdb_Data.get_all_movie_data(imdb_id, "0", "0")
        all_data = all_data
        Dim data_row As New Data_entry
        data_row.title = all_data("title")
        json_row = refresh_data_row(data_row, all_data, json_row)

        all_data_list.Insert(Selected_index, json_row)
        clean_buttons()
        update_buttons()

    End Sub

    Private Sub ListBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListBox1.SelectedIndexChanged

    End Sub

    Private Sub Label6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label6.Click

    End Sub

    Private Sub RichTextBox3_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles RichTextBox3.KeyPress
        fields_was_changed()
    End Sub

    Private Sub RichTextBox3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RichTextBox3.TextChanged

    End Sub

    Private Sub Button3_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        fields_was_changed()
        links_list.ShowDialog()

    End Sub

    Private Sub ComboBox1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles ComboBox1.KeyPress
        e.Handled = True
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged

    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Label23.Text = "Items Count:" + all_data_list.Count.ToString


    End Sub

    Private Sub RichTextBox2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles RichTextBox2.KeyPress
        fields_was_changed()
    End Sub

    Public Sub fields_was_changed()
        Button2.Enabled = True

    End Sub

    Private Sub TextBox19_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox19.KeyPress
        fields_was_changed()
    End Sub

    Private Sub TextBox19_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox19.TextChanged

    End Sub

    Private Sub TextBox17_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox17.KeyPress
        fields_was_changed()
    End Sub

    Private Sub TextBox17_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox17.TextChanged

    End Sub

    Private Sub TextBox16_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox16.KeyPress
        fields_was_changed()
    End Sub

    Private Sub TextBox16_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox16.TextChanged

    End Sub

    Private Sub TextBox15_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox15.KeyPress
        fields_was_changed()
    End Sub

    Private Sub TextBox15_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox15.TextChanged

    End Sub

    Private Sub TextBox14_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox14.KeyPress
        fields_was_changed()
    End Sub

    Private Sub TextBox14_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox14.TextChanged

    End Sub

    Private Sub TextBox13_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox13.KeyPress
        fields_was_changed()
    End Sub

    Private Sub TextBox13_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox13.TextChanged

    End Sub

    Private Sub TextBox18_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox18.KeyPress
        fields_was_changed()
    End Sub

    Private Sub TextBox18_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox18.TextChanged
        fields_was_changed()
    End Sub

    Private Sub TextBox9_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox9.KeyPress
        fields_was_changed()
    End Sub

    Private Sub TextBox9_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox9.TextChanged

    End Sub

    Private Sub TextBox10_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox10.KeyPress
        fields_was_changed()
    End Sub

    Private Sub TextBox10_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox10.TextChanged

    End Sub

    Private Sub TextBox12_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox12.KeyPress
        fields_was_changed()
    End Sub

    Private Sub TextBox12_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox12.TextChanged

    End Sub

    Private Sub TextBox11_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox11.KeyPress
        fields_was_changed()
    End Sub

    Private Sub TextBox11_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox11.TextChanged

    End Sub

    Private Sub TextBox7_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox7.KeyPress
        fields_was_changed()
    End Sub

    Private Sub TextBox7_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox7.TextChanged

    End Sub

    Private Sub TextBox6_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox6.KeyPress
        fields_was_changed()
    End Sub

    Private Sub TextBox6_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox6.TextChanged

    End Sub

    Private Sub TextBox8_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox8.KeyPress
        If Asc(e.KeyChar) <> 8 Then
            If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then
                e.Handled = True
            Else
                fields_was_changed()
            End If
        End If

    End Sub

    Private Sub TextBox8_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox8.TextChanged

    End Sub

    Private Sub TextBox4_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox4.KeyPress

        If Asc(e.KeyChar) <> 8 Then
            If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then
                e.Handled = True
            Else
                fields_was_changed()
            End If
        End If


    End Sub

    Private Sub TextBox4_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox4.TextChanged

    End Sub

    Private Sub TextBox5_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox5.KeyPress
        fields_was_changed()
    End Sub

    Private Sub TextBox5_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox5.TextChanged

    End Sub

    Private Sub TextBox3_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox3.KeyPress
        fields_was_changed()
    End Sub

    Private Sub TextBox3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox3.TextChanged

    End Sub

    Private Sub TextBox2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox2.KeyPress
        fields_was_changed()
    End Sub

    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox2.TextChanged

    End Sub

    Private Sub TextBox1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox1.KeyPress
        fields_was_changed()
    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged

    End Sub
    Public Function build_value_from_fields()
        Dim data_row As New Data_entry

        data_row.title = TextBox1.Text
        data_row.thumbnail = TextBox2.Text
        data_row.fanart = TextBox3.Text
        data_row.plot = RichTextBox2.Text
        data_row.imdb = TextBox5.Text


        data_row.season = TextBox4.Text
        data_row.episode = TextBox8.Text
        data_row.type = TextBox6.Text
        data_row.year = TextBox18.Text

        data_row.originaltitle = TextBox11.Text
        If data_row.originaltitle.Length = 0 Then
            data_row.originaltitle = clean_name(data_row.title)
        End If


        data_row.mediatype = TextBox7.Text
        data_row.genre = TextBox12.Text
        data_row.rating = TextBox9.Text
        data_row.director = TextBox14.Text
        data_row.duration = TextBox13.Text
        data_row.studios = TextBox15.Text
        data_row.writer = TextBox16.Text
        data_row.votes = TextBox10.Text

        data_row.dateadded = TextBox19.Text
        data_row.trailer = TextBox17.Text
        data_row.content = TextBox20.Text
        data_row.link.Clear()
        For Each item In ComboBox1.Items
            data_row.link.Add(item)
        Next


        Return data_row
    End Function

    Public Sub convert_list_to_json_list()
        Dim parent As New Newtonsoft.Json.Linq.JObject
        Dim complete_txt As New List(Of String)

        complete_txt.Add("{")

        For Each item In all_data_list

            parent = Newtonsoft.Json.Linq.JObject.Parse(item)
            For Each p_item In parent

            Next
        Next
    End Sub
    Public Function build_data_row(ByVal data_row, ByVal Parent)
        Dim found_link As Boolean = False


        data_row.title = Parent("title")

        data_row.thumbnail = Parent("thumbnail")



        data_row.fanart = Parent("fanart")
        data_row.imdb = Parent("imdb")
        data_row.season = Parent("season")
        data_row.episode = Parent("episode")
        data_row.type = Parent("type")
        data_row.year = Parent("year")
        data_row.plot = Parent("plot")
        data_row.originaltitle = Parent("originaltitle")
        data_row.mediatype = Parent("mediatype")
        data_row.genre = Parent("genre")
        data_row.rating = Parent("rating")
        data_row.director = Parent("director")
        data_row.duration = Parent("duration")
        data_row.studios = Parent("studios")
        data_row.writer = Parent("writer")
        data_row.votes = Parent("votes")
        data_row.dateadded = Parent("dateadded")
        data_row.trailer = Parent("trailer")
        data_row.content = Parent("content")
        data_row.link.Clear()
        If Parent("link").GetType Is GetType(System.String) Then
            data_row.link.Add(Parent("link"))
        Else
            For Each links In Parent("link")
                data_row.link.Add(links)
                found_link = True
            Next
        End If

        If (data_row.link.count) = 0 Then
            data_row.link.Add(" ")

        End If

        Return data_row
    End Function
    Public Function build_xml_data_row(ByVal Parent)
        Dim found_link As Boolean = False

        Dim xml_data As String = ""
        If Parent("type") = "dir" Then
            xml_data = xml_data + Environment.NewLine + "<dir>"

            xml_data = xml_data + Environment.NewLine + String.Format("<title>{0}", Parent("title")) + "</title>"
            If Parent("link").GetType Is GetType(System.String) Then
                xml_data = xml_data + Environment.NewLine + "<link>" + Parent("link") + "</link>"
            Else
                For Each links In Parent("link")
                    xml_data = xml_data + Environment.NewLine + "<link>" + links + "</link>"
                Next
            End If
            xml_data = xml_data + Environment.NewLine + String.Format("<thumbnail>{0}", Parent("thumbnail")) + "</thumbnail>"
            xml_data = xml_data + Environment.NewLine + String.Format("<fanart>{0}", Parent("fanart")) + "</fanart>"
            xml_data = xml_data + Environment.NewLine + "</dir>"
        Else
            xml_data = xml_data + Environment.NewLine + "<item>"

            xml_data = xml_data + Environment.NewLine + String.Format("<title>{0}", Parent("title")) + "</title>"
            xml_data = xml_data + Environment.NewLine + "<meta>"
            xml_data = xml_data + Environment.NewLine + String.Format("<imdb>{0}", Parent("imdb")) + "</imdb>"
            xml_data = xml_data + Environment.NewLine + String.Format("<content>{0}", Parent("content")) + "</content>"
            xml_data = xml_data + Environment.NewLine + String.Format("<title>{0}", Parent("originaltitle")) + "</title>"
            xml_data = xml_data + Environment.NewLine + String.Format("<year>{0}", Parent("year")) + "</year>"
            If Parent("content").value <> "movie" Then
                xml_data = xml_data + Environment.NewLine + String.Format("<season>{0}", Parent("season")) + "</season>"
                xml_data = xml_data + Environment.NewLine + String.Format("<episode>{0}", Parent("episode")) + "</episode>"

            End If
            xml_data = xml_data + Environment.NewLine + "</meta>"
            xml_data = xml_data + Environment.NewLine + "<link>"

            If Parent("link").GetType Is GetType(System.String) Then
                xml_data = xml_data + Environment.NewLine + "<sublink>" + Parent("link") + "</sublink>"
            Else
                For Each links In Parent("link")
                    xml_data = xml_data + Environment.NewLine + "<sublink>" + links + "</sublink>"
                Next
            End If

            xml_data = xml_data + Environment.NewLine + "</link>"
            xml_data = xml_data + Environment.NewLine + String.Format("<thumbnail>{0}", Parent("thumbnail")) + "</thumbnail>"
            xml_data = xml_data + Environment.NewLine + String.Format("<fanart>{0}", Parent("fanart")) + "</fanart>"

            xml_data = xml_data + Environment.NewLine + "</item>"
        End If



        Return xml_data
    End Function
    Public Function SerializeObject(Of t)(ByVal arg As t) As String

        Dim sw = New StringWriter()

        Using jsonWriter = New JsonTextWriter(sw)
            jsonWriter.Formatting = Newtonsoft.Json.Formatting.Indented
            jsonWriter.IndentChar = " "c
            jsonWriter.Indentation = 4

            Dim jsonSerializer = Newtonsoft.Json.JsonSerializer.CreateDefault()
            jsonSerializer.Serialize(jsonWriter, arg)
        End Using

        Return sw.ToString()

    End Function

    Private Sub SaveFileToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveFileToolStripMenuItem.Click
        Dim all_prj As String = ""
        Dim overwrite As Boolean = True
        SaveFileDialog1.InitialDirectory = InitialDirectory
        SaveFileDialog1.FileName = saved_project_name

        SaveFileDialog1.Filter = "MiProject Files (*.mprj*)|*.mprj"
        If SaveFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then

            If overwrite Then
                For Each item In file_list
                    If Len(all_prj) = 0 Then
                        all_prj = item
                    Else
                        all_prj = all_prj + Environment.NewLine + item
                    End If


                Next
            End If

        End If
        If overwrite Then
            My.Computer.FileSystem.WriteAllText(SaveFileDialog1.FileName, all_prj, True)
        End If


    End Sub

    Private Sub ItemToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim File_Text As String = ""
        OpenFileDialog1.InitialDirectory = InitialDirectory
        OpenFileDialog1.Filter = "txt files All files (*.*)|*.*|(*.json)|*.json"
        OpenFileDialog1.FileName = ""
        OpenFileDialog1.FilterIndex = 2
        OpenFileDialog1.RestoreDirectory = True
        Dim file_path As String = ""
        ' Call ShowDialog.
        Dim result As DialogResult = OpenFileDialog1.ShowDialog()

        ' Test result.
        If result = Windows.Forms.DialogResult.OK Then
            file_path = OpenFileDialog1.FileName
            ' Get the file name.

            Try
                ' Read in text.
                File_Text = File.ReadAllText(file_path)



            Catch ex As Exception

                ' Report an error.
                Me.Text = "Error"

            End Try
        End If

        MetroPanel2_list_files.Items.Add(Path.GetFileName(file_path))
        file_list.Add(file_path)
        selected_file = file_path
        'sender.SelectedIndex = sender.SelectedIndex + 1
        MetroPanel2_list_files.SelectedItem = Path.GetFileName(file_path)
        File_Text = File.ReadAllText(file_list.Item(MetroPanel2_list_files.SelectedIndex))
        selected_file = file_list.Item(MetroPanel2_list_files.SelectedIndex)


        clear_movie_list()
        MetroPanel1.Enabled = False

        refresh_movie_list(File_Text)
        MetroPanel1.Enabled = True
    End Sub

    Private Sub ToolStripMenuItem8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)


    End Sub

    Private Sub DirectoryToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DirectoryToolStripMenuItem.Click
        Dim name As String
        Dim data_row As New Data_entry
        Dim json_row As String = ""
        Dim parent As New Newtonsoft.Json.Linq.JObject

        name = InputBox("Directory name", "Enter directory name")
        data_row.title = name
        parent("title") = name
        parent("thumbnail") = ""
        parent("fanart") = ""
        parent("imdb") = ""
        parent("season") = "0"
        parent("episode") = "0"
        parent("type") = "dir"
        parent("year") = ""
        parent("originaltitle") = ""
        parent("mediatype") = ""
        parent("genre") = ""
        parent("rating") = ""
        parent("director") = ""
        parent("duration") = ""
        parent("studios") = ""
        parent("writer") = ""
        parent("votes") = ""
        parent("dateadded") = ""
        parent("trailer") = ""
        parent("link") = ""
        parent("content") = ""



        json_row = refresh_data_row(data_row, parent, json_row)
        'add_button(json_row, data_row.title)
        all_data_list.Insert(Selected_index, json_row)

        clean_buttons()
        update_buttons()
    End Sub

    Private Sub CustomItemToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CustomItemToolStripMenuItem.Click
        Dim name As String
        Dim data_row As New Data_entry
        Dim json_row As String = ""
        Dim parent As New Newtonsoft.Json.Linq.JObject

        name = InputBox("Item name", "Enter Item name")
        data_row.title = name
        parent("title") = name
        parent("thumbnail") = ""
        parent("fanart") = ""
        parent("imdb") = ""
        parent("season") = "0"
        parent("episode") = "0"
        parent("type") = "item"
        parent("year") = ""
        parent("originaltitle") = ""
        parent("mediatype") = ""
        parent("genre") = ""
        parent("rating") = ""
        parent("director") = ""
        parent("duration") = ""
        parent("studios") = ""
        parent("writer") = ""
        parent("votes") = ""
        parent("dateadded") = ""
        parent("trailer") = ""
        parent("link") = ""
        parent("content") = ""


        json_row = refresh_data_row(data_row, parent, json_row)
        'add_button(json_row, data_row.title)
        all_data_list.Insert(Selected_index, json_row)

        clean_buttons()
        update_buttons()

    End Sub
    Private Sub Button2_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim json_row As String = ""
        sender.Enabled = False
        json_row = JsonConvert.SerializeObject(build_value_from_fields())
        all_data_list.Item(Selected_index) = json_row
        refresh_data_list()
    End Sub

    Private Sub ToolStripMenuItem7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem7.Click
        Dim File_Text As String

        OpenFileDialog1.InitialDirectory = InitialDirectory
        OpenFileDialog1.Filter = "txt files All files (*.*)|*.*|(*.json)|*.json"
        OpenFileDialog1.FileName = ""
        OpenFileDialog1.FilterIndex = 2
        OpenFileDialog1.RestoreDirectory = True
        Dim file_path As String = ""
        ' Call ShowDialog.
        Dim result As DialogResult = OpenFileDialog1.ShowDialog()

        ' Test result.
        If result = Windows.Forms.DialogResult.OK Then
            file_path = OpenFileDialog1.FileName
            ' Get the file name.
            InitialDirectory = Path.GetDirectoryName(file_path)
            Try
                ' Read in text.
                File_Text = File.ReadAllText(file_path)



            Catch ex As Exception

                ' Report an error.
                Me.Text = "Error"

            End Try
        Else
            Exit Sub

        End If

        MetroPanel2_list_files.Items.Add(Path.GetFileName(file_path))
        file_list.Add(file_path)
        selected_file = file_path
        'sender.SelectedIndex = sender.SelectedIndex + 1
        MetroPanel2_list_files.SelectedItem = Path.GetFileName(file_path)
        File_Text = File.ReadAllText(file_list.Item(MetroPanel2_list_files.SelectedIndex))
        selected_file = file_list.Item(MetroPanel2_list_files.SelectedIndex)


        clear_movie_list()
        MetroPanel1.Enabled = False

        refresh_movie_list(File_Text)
        MetroPanel1.Enabled = True

    End Sub

    Private Sub Timer2_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer2.Tick
        Timer2.Enabled = False
        trd(0) = New Thread(AddressOf Fanart_image_download)
        trd(0).IsBackground = True
        trd(0).Start(public_fanart)

        trd(0) = New Thread(AddressOf Icon_image_download)
        trd(0).IsBackground = True
        trd(0).Start(public_icon)

    End Sub

    Private Sub ToolStripMenuItem13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem13.Click
        Try
            Dim remove_item As Integer = MetroPanel2_list_files.SelectedIndex
            MetroPanel2_list_files.Items.RemoveAt(remove_item)
            file_list.RemoveAt(remove_item)
        Catch ex As Exception

        End Try

    End Sub
    Public Sub clean_buttons()
        Dim i As Integer

        For i = (MetroPanel1.Controls.Count - 1) To 0 Step -1
            MetroPanel1.Controls.RemoveAt(i)
        Next

        y_location = 78

        Selected_index = 0
        ComboBox1.Items.Clear()
    End Sub
    Private Sub ToolStripMenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem2.Click
        If all_data_list.Count > Selected_index Then
            all_data_list.RemoveAt(Selected_index)
            MetroPanel1.Controls.Remove(all_button.Item(Selected_index))
            clean_buttons()
            update_buttons()
        End If

    End Sub

    Private Sub ToolStripMenuItem3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem3.Click
        Dim saved_item
        If Selected_index > 0 Then
            saved_item = all_data_list(Selected_index - 1)
            all_data_list(Selected_index - 1) = all_data_list(Selected_index)
            all_data_list(Selected_index) = saved_item
            clean_buttons()
            update_buttons()
        End If

    End Sub

    Private Sub ToolStripMenuItem4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem4.Click
        Dim saved_item
        If (Selected_index + 1) < all_data_list.Count Then
            saved_item = all_data_list(Selected_index + 1)
            all_data_list(Selected_index + 1) = all_data_list(Selected_index)
            all_data_list(Selected_index) = saved_item
            clean_buttons()
            update_buttons()
        End If

    End Sub

    Private Sub ContextMenuStrip1_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip1.Opening
        Dim parent As New Newtonsoft.Json.Linq.JObject
        Dim name As String
        Dim regex_n2, matches_n2, new_color, matches_n3



        If Selected_index + 1 < all_data_list.Count Then
            name = Newtonsoft.Json.Linq.JObject.Parse(all_data_list(Selected_index))("title").ToString



            name = name.Replace("[/COLOR]", "")
            regex_n2 = New Regex("\[COLOR=(.+?)\]")
            matches_n2 = regex_n2.Matches(name)

            If matches_n2.count > 0 Then
                new_color = (matches_n2(0).Groups(1).Value)
                name = name.Replace(String.Format("[COLOR={0}]", new_color), "")
            Else
                regex_n2 = New Regex("\[COLOR (.+?)\]")
                matches_n3 = regex_n2.Matches(name)
                If matches_n3.count > 0 Then
                    new_color = (matches_n3(0).Groups(1).Value)
                    name = name.Replace(String.Format("[COLOR {0}]", new_color), "")
                End If

            End If


            ContextMenuStrip1.Items(1).Text = "Remove: """ + name + """"
            ContextMenuStrip1.Items(2).Text = "Move Up: """ + name + """"
            ContextMenuStrip1.Items(3).Text = "Move Down: """ + name + """"
        Else


        End If


    End Sub
    Public Function clean_name(ByVal name)
        Dim regex_n2, matches_n2, new_color, matches_n3
        name = name.Replace("[/COLOR]", "")
        regex_n2 = New Regex("\[COLOR=(.+?)\]")
        matches_n2 = regex_n2.Matches(name)

        If matches_n2.count > 0 Then
            new_color = (matches_n2(0).Groups(1).Value)
            name = name.Replace(String.Format("[COLOR={0}]", new_color), "")
        Else
            regex_n2 = New Regex("\[COLOR (.+?)\]")
            matches_n3 = regex_n2.Matches(name)
            If matches_n3.count > 0 Then
                new_color = (matches_n3(0).Groups(1).Value)
                name = name.Replace(String.Format("[COLOR {0}]", new_color), "")
            End If

        End If
        Return name
    End Function
    Private Sub OrderAlphabeticlyToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OrderAlphabeticlyToolStripMenuItem.Click
        Dim new_list As New List(Of Dictionary(Of String, String))
        Dim dict As List(Of KeyValuePair(Of String, String)) = New List(Of KeyValuePair(Of String, String))




        Dim parent As New Newtonsoft.Json.Linq.JObject


        For Each item In all_data_list

            parent = Newtonsoft.Json.Linq.JObject.Parse(item)


            dict.Add((New KeyValuePair(Of String, String)(clean_name(parent("title").ToString), item)))







        Next
        Dim sorted = (From item In dict Order By item.Key Select item).ToList
        all_data_list.Clear()

        For Each pair As KeyValuePair(Of String, String) In sorted

            all_data_list.Add(pair.Value)


        Next



        clean_buttons()
        update_buttons()
    End Sub

    Private Sub OrderByYearToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OrderByYearToolStripMenuItem.Click
        Dim new_list As New List(Of Dictionary(Of String, String))
        Dim dict As List(Of KeyValuePair(Of String, String)) = New List(Of KeyValuePair(Of String, String))




        Dim parent As New Newtonsoft.Json.Linq.JObject


        For Each item In all_data_list

            parent = Newtonsoft.Json.Linq.JObject.Parse(item)


            dict.Add((New KeyValuePair(Of String, String)(clean_name(parent("year").ToString), item)))







        Next
        Dim sorted = (From item In dict Order By item.Key Select item).ToList
        all_data_list.Clear()

        For Each pair As KeyValuePair(Of String, String) In sorted

            all_data_list.Add(pair.Value)


        Next

        all_data_list.Reverse()

        clean_buttons()
        update_buttons()

    End Sub

    Private Sub SaveFileToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)


    End Sub

    Private Sub SaveAsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveAsToolStripMenuItem.Click
        Dim overwrite As Boolean = True
        SaveFileDialog1.InitialDirectory = InitialDirectory
        SaveFileDialog1.FileName = saved_file_name

        SaveFileDialog1.Filter = "Json Files (*.json*)|*.json"
        If SaveFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then

            If overwrite Then
                Dim json_string As String
                Dim parent As New Newtonsoft.Json.Linq.JObject


                all_converted_list.Clear()

                For Each item In all_data_list
                    Dim data_row As New Data_entry
                    parent = Newtonsoft.Json.Linq.JObject.Parse(item)
                    data_row = build_data_row(data_row, parent)
                    all_converted_list.Add(data_row)
                    data_row.Dispose()


                Next
                json_string = "{" + Environment.NewLine + """items"":"
                json_string = json_string + SerializeObject(all_converted_list)
                json_string = json_string + Environment.NewLine + "}"
                File.WriteAllText(SaveFileDialog1.FileName, json_string)
            End If



        End If


    End Sub

    Private Sub SaveFileAsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub OpenFileToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenFileToolStripMenuItem.Click
        OpenFileDialog1.InitialDirectory = InitialDirectory
        OpenFileDialog1.Filter = "txt files All files (*.*)|*.*|(*.mprj)|*.mprj"
        OpenFileDialog1.FileName = ""
        OpenFileDialog1.FilterIndex = 2
        OpenFileDialog1.RestoreDirectory = True
        Dim file_path As String = ""
        ' Call ShowDialog.
        Dim result As DialogResult = OpenFileDialog1.ShowDialog()

        ' Test result.
        file_list.Clear()
        MetroPanel2_list_files.Items.Clear()

        If result = Windows.Forms.DialogResult.OK Then
            file_path = OpenFileDialog1.FileName
            InitialDirectory = Path.GetDirectoryName(file_path)
            saved_project_name = Path.GetFileName(file_path)
            Dim line_text As String
            Dim objReader As New System.IO.StreamReader(file_path)

            Do While objReader.Peek() <> -1

                line_text = objReader.ReadLine()
                If Len(line_text) > 0 Then
                    MetroPanel2_list_files.Items.Add(Path.GetFileName(line_text))
                    file_list.Add(line_text)
                End If

            Loop

            objReader.Close()

        End If

    End Sub

    Private Sub SeasonListToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SeasonListToolStripMenuItem.Click
        Dim result As DialogResult = MessageBox.Show("Add all seasons", "This will clear the list", MessageBoxButtons.YesNoCancel)
        If result = DialogResult.Yes Then
            Dim imdbid As String
            Dim response
            Dim json_row As String
            Dim year As String
            Dim iconimage As String = ""
            Dim fanart As String = ""
            Dim genres_pre, genres, rating
            Dim format As String = "yyyy-MM-d HH:mm:ss"
            Dim time As DateTime = DateTime.Now
            imdbid = InputBox("Enter ImdbId")

            response = Imdb_Data.get_all_season_data(imdbid)

            genres_pre = response("genres")
            For Each i In genres_pre
                If genres = "" Then
                    genres = i("name").value
                Else
                    genres = genres + "/" + i("name").value
                End If

            Next
            If response("backdrop_path").value <> Nothing Then
                fanart = "http://image.tmdb.org/t/p/original/" + response("backdrop_path").value
            End If

            rating = response("vote_average")
            all_data_list.Clear()

            For Each items In response("seasons")
                Dim data_row As New Data_entry
                data_row.title = "Season " + items("season_number").ToString


                If items("poster_path").value <> Nothing Then
                    iconimage = "http://image.tmdb.org/t/p/original/" + items("poster_path").value
                End If


                data_row.fanart = fanart
                data_row.imdb = imdbid
                data_row.thumbnail = iconimage
                Try
                    year = Microsoft.VisualBasic.Left(response("air_date").value, 4)
                Catch ex As Exception
                    year = ""
                End Try



                data_row.season = items("season_number")
                data_row.episode = ""
                data_row.type = "dir"
                data_row.year = year
                data_row.originaltitle = clean_name(items("name"))
                data_row.mediatype = "tv"
                data_row.plot = items("overview")
                data_row.genre = genres
                data_row.rating = rating
                data_row.director = ""
                data_row.duration = ""
                data_row.studios = ""
                data_row.writer = ""
                data_row.votes = response("vote_count")
                data_row.dateadded = time.ToString(format)
                data_row.trailer = ""
                data_row.link.Clear()
                data_row.content = "episode"

                json_row = JsonConvert.SerializeObject(data_row)
                all_data_list.Add(json_row)
                data_row.Dispose()

            Next

            clean_buttons()
            update_buttons()
        End If




    End Sub

    Private Sub EpisodeListToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EpisodeListToolStripMenuItem.Click
        Dim result As DialogResult = MessageBox.Show("Add all Episodes", "This will clear the list", MessageBoxButtons.YesNoCancel)
        If result = DialogResult.Yes Then
            Dim imdbid As String
            Dim response
            Dim season As String
            Dim episode As String
            Dim votes As String
            Dim json_row As String
            Dim year As String
            Dim iconimage As String = ""
            Dim fanart As String = ""
            Dim original_title As String

            Dim genres_pre, genres, rating
            Dim format As String = "yyyy-MM-d HH:mm:ss"
            Dim time As DateTime = DateTime.Now
            imdbid = InputBox("Enter ImdbId")
            season = InputBox("Enter Season")


            response = Imdb_Data.get_all_season_data(imdbid)

            original_title = response("name")
            genres_pre = response("genres")
            For Each i In genres_pre
                If genres = "" Then
                    genres = i("name").value
                Else
                    genres = genres + "/" + i("name").value
                End If

            Next
            For Each items In response("seasons")
                If items("season_number") = season Then
                    If items("poster_path").value <> Nothing Then
                        iconimage = "http://image.tmdb.org/t/p/original/" + items("poster_path").value
                    End If
                End If


            Next

            rating = response("vote_average")
            all_data_list.Clear()


            response = Imdb_Data.get_all_episode_data(imdbid, season)

            For Each items In response("episodes")
                Dim data_row As New Data_entry
                data_row.title = items("name")


                If items("still_path").value <> Nothing Then
                    fanart = "http://image.tmdb.org/t/p/original/" + items("still_path").value
                End If


                data_row.fanart = fanart
                data_row.imdb = imdbid
                data_row.thumbnail = iconimage
                Try
                    year = Microsoft.VisualBasic.Left(response("air_date").value, 4)
                Catch ex As Exception
                    year = ""
                End Try



                data_row.season = season
                data_row.episode = items("episode_number")
                data_row.type = "dir"
                data_row.year = year
                data_row.originaltitle = original_title
                data_row.mediatype = "tv"
                data_row.plot = items("overview")

                data_row.genre = genres
                data_row.rating = items("vote_average")
                data_row.director = ""
                data_row.duration = ""
                data_row.studios = ""
                data_row.writer = ""
                data_row.votes = items("vote_count")
                data_row.dateadded = time.ToString(format)
                data_row.trailer = ""
                data_row.link.Clear()
                data_row.content = "episode"

                json_row = JsonConvert.SerializeObject(data_row)
                all_data_list.Add(json_row)
                data_row.Dispose()

            Next

            clean_buttons()
            update_buttons()

        End If
    End Sub

    Private Sub MenuStrip1_ItemClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles MenuStrip1.ItemClicked

    End Sub

    Private Sub ClearListToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ClearListToolStripMenuItem.Click
        clear_movie_list()
        all_data_list.Clear()

    End Sub

    Private Sub AboutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem.Click
        MessageBox.Show("Made by Fort")

    End Sub

    Private Sub AddXmlFileToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddXmlFileToolStripMenuItem.Click
        Dim File_Text As String
        Dim json_row As String = ""
        Dim parent As New Newtonsoft.Json.Linq.JObject



        parent("title") = ""
        parent("thumbnail") = ""
        parent("fanart") = ""
        parent("imdb") = ""
        parent("season") = "0"
        parent("episode") = "0"
        parent("type") = "dir"
        parent("year") = ""
        parent("originaltitle") = ""
        parent("mediatype") = ""
        parent("genre") = ""
        parent("rating") = ""
        parent("director") = ""
        parent("duration") = ""
        parent("studios") = ""
        parent("writer") = ""
        parent("votes") = ""
        parent("dateadded") = ""
        parent("trailer") = ""
        parent("link") = ""




        OpenFileDialog1.InitialDirectory = InitialDirectory
        OpenFileDialog1.Filter = "txt files All files (*.*)|*.*|(*.xml)|*.xml"
        OpenFileDialog1.FileName = ""
        OpenFileDialog1.FilterIndex = 2
        OpenFileDialog1.RestoreDirectory = True
        Dim file_path As String = ""
        ' Call ShowDialog.
        Dim result As DialogResult = OpenFileDialog1.ShowDialog()

        ' Test result.
        If result = Windows.Forms.DialogResult.OK Then
            file_path = OpenFileDialog1.FileName
            File_Text = File.ReadAllText(file_path)




            clear_movie_list()
            parse_xml(File_Text, file_path)
            MetroPanel2_list_files.Items.Add(Path.GetFileName(file_path))
            file_list.Add(file_path)
            selected_file = file_path
            'sender.SelectedIndex = sender.SelectedIndex + 1
            MetroPanel2_list_files.SelectedItem = Path.GetFileName(file_path)

            selected_file = file_list.Item(MetroPanel2_list_files.SelectedIndex)



            MetroPanel1.Enabled = False


            MetroPanel1.Enabled = True
        End If

    End Sub
    Public Sub parse_xml(ByVal file_text, ByVal file_path)
        Dim regex, m
        Dim temp, json_row
        regex = New Regex("<dir>(.+?)</dir>", RegexOptions.Singleline)
        m = regex.Matches(file_text)
        For Each item In m
            Dim data_row As New Data_entry
            regex = New Regex("<title>(.+?)</title>", RegexOptions.Singleline)
            m = regex.Matches(item.value)
            temp = (m(0).Groups(1).Value)
            data_row.title = temp

            regex = New Regex("<link>(.+?)</link>", RegexOptions.Singleline)
            m = regex.Matches(item.value)
            If m.count > 0 Then
                temp = (m(0).Groups(1).Value)
                data_row.link.Add(temp)

            End If

            regex = New Regex("<thumbnail>(.+?)</thumbnail>", RegexOptions.Singleline)
            m = regex.Matches(item.value)

            If m.count > 0 Then
                temp = (m(0).Groups(1).Value)
                data_row.thumbnail = temp
            End If


            regex = New Regex("<fanart>(.+?)</fanart>", RegexOptions.Singleline)
            m = regex.Matches(item.value)
            If m.count > 0 Then
                temp = (m(0).Groups(1).Value)
                data_row.fanart = temp
            End If

            regex = New Regex("<season>(.+?)</season>", RegexOptions.Singleline)
            m = regex.Matches(item.value)
            If m.count > 0 Then
                temp = (m(0).Groups(1).Value)
                data_row.season = temp
            End If

            regex = New Regex("<content>(.+?)</content>", RegexOptions.Singleline)
            m = regex.Matches(item.value)
            If m.count > 0 Then
                temp = (m(0).Groups(1).Value)
                data_row.content = temp
            End If

            data_row.type = "dir"


            json_row = JsonConvert.SerializeObject(data_row)
            'add_button(json_row, data_row.title)
            all_data_list.Add(json_row)

            data_row.originaltitle = clean_name(data_row.title)
            data_row.Dispose()




        Next
        Dim m2

        regex = New Regex("<item>(.+?)</item>", RegexOptions.Singleline)
        m = regex.Matches(file_text)
        For Each item In m
            Dim data_row As New Data_entry
            regex = New Regex("<title>(.+?)</title>", RegexOptions.Singleline)
            m = regex.Matches(item.value)
            temp = (m(0).Groups(1).Value)
            data_row.title = temp

            regex = New Regex("<link>(.+?)</link>", RegexOptions.Singleline)
            m = regex.Matches(item.value)
            If m.count > 0 Then
                regex = New Regex("<sublink>(.+?)</sublink>", RegexOptions.Singleline)
                m2 = regex.Matches((m(0).Groups(1).Value))
                If m2.count > 0 Then
                    For Each item2 In m2
                        temp = item2.Groups(1).Value
                        data_row.link.Add(temp)
                    Next
                Else
                    data_row.link.Add((m(0).Groups(1).Value))
                End If


            End If

            regex = New Regex("<thumbnail>(.+?)</thumbnail>", RegexOptions.Singleline)
            m = regex.Matches(item.value)

            If m.count > 0 Then
                temp = (m(0).Groups(1).Value)
                data_row.thumbnail = temp
            End If

            regex = New Regex("<content>(.+?)</content>", RegexOptions.Singleline)
            m = regex.Matches(item.value)

            If m.count > 0 Then
                temp = (m(0).Groups(1).Value)
                data_row.content = temp
            End If


            regex = New Regex("<fanart>(.+?)</fanart>", RegexOptions.Singleline)
            m = regex.Matches(item.value)
            If m.count > 0 Then
                temp = (m(0).Groups(1).Value)
                data_row.fanart = temp
            End If



            regex = New Regex("<imdb>(.+?)</imdb>", RegexOptions.Singleline)
            m = regex.Matches(item.value)

            If m.count > 0 Then
                temp = (m(0).Groups(1).Value)
                data_row.imdb = temp
            End If

            regex = New Regex("<year>(.+?)</year>", RegexOptions.Singleline)
            m = regex.Matches(item.value)

            If m.count > 0 Then
                temp = (m(0).Groups(1).Value)
                data_row.year = temp
            End If

            regex = New Regex("<content>(.+?)</content>", RegexOptions.Singleline)
            m = regex.Matches(item.value)

            If m.count > 0 Then
                temp = (m(0).Groups(1).Value)
                data_row.mediatype = temp

            End If

            regex = New Regex("<season>(.+?)</season>", RegexOptions.Singleline)
            m = regex.Matches(item.value)

            If m.count > 0 Then
                temp = (m(0).Groups(1).Value)
                data_row.season = temp
            End If

            regex = New Regex("<episode>(.+?)</episode>", RegexOptions.Singleline)
            m = regex.Matches(item.value)

            If m.count > 0 Then
                temp = (m(0).Groups(1).Value)
                data_row.episode = temp
            End If
            data_row.type = "item"

            data_row.originaltitle = clean_name(data_row.title)


            json_row = JsonConvert.SerializeObject(data_row)
            'add_button(json_row, data_row.title)
            all_data_list.Add(json_row)


            data_row.Dispose()


        Next



        clean_buttons()
        update_buttons()

       
  
    End Sub
    Private Sub SaveXMLFileToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveXMLFileToolStripMenuItem.Click
        Dim overwrite As Boolean = True
        Dim xml_data As String = "<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes"" ?>" + Environment.NewLine + "<?xml-stylesheet href=""hide-it.xsl"" type=""text/xsl""?>"

        SaveFileDialog1.InitialDirectory = InitialDirectory
        SaveFileDialog1.FileName = saved_file_name

        SaveFileDialog1.Filter = "Json Files (*.xml*)|*.xml"
        If SaveFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then

            If overwrite Then
                Dim json_string As String
                Dim parent As New Newtonsoft.Json.Linq.JObject


                all_converted_list.Clear()

                For Each item In all_data_list

                    parent = Newtonsoft.Json.Linq.JObject.Parse(item)
                    xml_data = xml_data + Environment.NewLine + build_xml_data_row(parent)
                  


                Next
                
                File.WriteAllText(SaveFileDialog1.FileName, xml_data)
            End If



        End If

    End Sub

    Private Sub PictureBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox1.Click

    End Sub
End Class
