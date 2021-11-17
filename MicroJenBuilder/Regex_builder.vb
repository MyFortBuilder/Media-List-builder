Imports System.IO
Imports System.Net
Imports System.Text.RegularExpressions
Imports System.Runtime.InteropServices
Imports System.Text
Imports Newtonsoft.Json

Imports System.Web.Script.Serialization


Public Class Regex_builder
    Public UserPath As String = Directory.GetCurrentDirectory
    Public tv_mode As String = "movie"
    Public new_name As String
    Public new_icon, new_plot, new_image As String
    Public imdb_name, imdb_icon, imdb_plot, imdb_image, imdb_year
    Public load_stated
    Public parant_node
    Public f_imdbid As String
    Public all_imdb As List(Of String) = New List(Of String)
    Public all_links As List(Of String) = New List(Of String)
    Public final_father As String

    Public season_value, episode_value As String

    Public exists_items As New List(Of String)
    Public regex_type, _FileContent As String
    Dim Regex
    Public upper_season As String = " "

    Public once_run
    Dim web1 As New WebBrowser()
    Dim web2 As New WebBrowser()
    Dim pageready As Boolean = False
    Dim next_page_index As Integer = 1


    Dim matches
    Public all_data As New Dictionary(Of String, String)








    Private Sub coloringRTB(ByVal rtb As RichTextBox, ByVal index As Integer, ByVal length As Integer, ByVal color As Color)
        Dim selectionStartSave As Integer = rtb.SelectionStart 'to return this back to its original position
        rtb.SelectionStart = index
        rtb.SelectionLength = length
        rtb.SelectionColor = color
        rtb.SelectionLength = 0
        rtb.SelectionStart = selectionStartSave
        rtb.SelectionColor = rtb.ForeColor 'return back to the original color
    End Sub






    
    Public Function modify_father(ByVal tnode As TreeNode, ByVal father As String)


        'Iterate through the child nodes of the node passed into this subroutine
        final_father = ""
        Dim node
        Dim array_a(0) As String
        array_a(0) = ""
        node = tnode
        While (node.Parent IsNot Nothing)
            If node.Parent.Name = "Node0" Or node.Parent.Name = "שורש" Then
                Exit While
            End If
            array_a.Add(node.Parent.Name)


            node = node.Parent
        End While

        Array.Reverse(array_a)


        final_father = String.Join("", array_a)

        Return final_father
    End Function
    Public Sub get_imdb_data(ByVal imdbid As String, ByVal season As String, ByVal episode As String)


        season_value = season
        episode_value = episode
        all_data = Imdb_Data.get_all_movie_data(imdbid, season, episode)

        Try

            TextBox3.Text = all_data("tvshowtitle")
            If Len(all_data("heb title")) > 0 Then
                TextBox3.Text = all_data("heb title")
                If season_value = " " And upper_season = " " Then
                    season = InputBox("הכנס מספר עונה", "הכנסת נתונים")
                Else
                    If season_value <> " " Then
                        season = season_value
                    Else
                        season = upper_season
                    End If
                End If
                If episode_value = " " Then
                    episode = InputBox("הכנס מספר פרק", "הכנסת נתונים")
                Else
                    episode = episode_value
                End If

                all_data("Season") = season
                all_data("Episode") = episode

            Else
                TextBox3.Text = all_data("tvshowtitle")
                season = InputBox("הכנס מספר עונה", "הכנסת נתונים")
                episode = InputBox("הכנס מספר פרק", "הכנסת נתונים")
                all_data("Season") = season
                all_data("Episode") = episode
            End If

        Catch ex As Exception
            If all_data.Count = 0 Then
                Exit Sub

            End If
            Label23.Text = all_data("originaltitle")

            If Len(all_data("heb title")) > 0 Then
                TextBox3.Text = all_data("heb title")
            Else
                TextBox3.Text = all_data("title")
            End If
        End Try




        TextBox4.Text = all_data("icon")
        TextBox5.Text = all_data("poster")
        TextBox6.Text = all_data("plot")
        TextBox7.Text = all_data("imdbnumber")
        TextBox9.Text = all_data("year")

        imdb_name = TextBox3.Text
        imdb_icon = TextBox4.Text
        imdb_plot = TextBox6.Text
        imdb_image = TextBox5.Text
        imdb_year = TextBox9.Text

        TextBox8.Text = JsonConvert.SerializeObject(all_data, Formatting.Indented).Replace(vbCr, "").Replace(vbLf, "")
        Imdb_Data.Close()
        Regex = New Regex("""Season"": ""(.+?)"".+?""Episode"": ""(.+?)""")
        matches = Regex.Matches(TextBox8.Text)
        If matches.Count > 0 Then
            TextBox16.Text = (matches(0).Groups(1).Value)
            TextBox17.Text = (matches(0).Groups(2).Value)
        End If
    End Sub
    
    
    Private Declare Function InternetSetCookie Lib "wininet.dll" Alias "InternetSetCookieA" (ByVal lpszUrlName As String, ByVal lpszCookieName As String, ByVal lpszCookieData As String) As Boolean

    Public Function GetIndexOfKey(ByVal dict As Dictionary(Of String, String), _
                           ByVal key As String) As Integer
        Dim keys(dict.Count - 1) As String
        dict.Keys.CopyTo(keys, 0)
        Return Array.IndexOf(keys, key)
    End Function

   

    Private Sub Button2_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If Len(RichTextBox1.SelectedText) > 0 Then

            Try
                RichTextBox1.Text = RichTextBox1.Text.Replace(RichTextBox1.SelectedText, String.Format("(@@@@{0}@@@@.+?)", regex_type))
            Catch ex As Exception

            End Try
        End If



    End Sub

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If Len(RichTextBox1.SelectedText) > 0 Then
            RichTextBox1.Text = RichTextBox1.Text.Replace(RichTextBox1.SelectedText, ".+?")
        End If

    End Sub

    Private Sub Button3_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim Nodes(10) As TreeNode
        If RichTextBox1.Text.Contains("season") And RichTextBox1.Text.Contains("episode") Then
            tv_mode = "tv"
        ElseIf RichTextBox1.Text.Contains("episode") Then
            tv_mode = "tv"
            upper_season = InputBox("מספר עונה", "הכנס מספר עונה")

      

        End If
        DataGridView1.Rows.Clear()
        'link_name, link_link, link_icon, link_image, link_plot, link_data, paranet_name.Replace("Node0", "").Replace("שורש", ""), Type

        Dim col As New DataGridViewTextBoxColumn
        col.DataPropertyName = "Name"
        col.HeaderText = "Name"
        col.Name = "Name"
        DataGridView1.Columns.Add(col)


        Dim col2 As New DataGridViewTextBoxColumn
        col2.DataPropertyName = "link"
        col2.HeaderText = "link"
        col2.Name = "link"
        DataGridView1.Columns.Add(col2)
        Dim col3 As New DataGridViewTextBoxColumn
        col3.DataPropertyName = "icon"
        col3.HeaderText = "icon"
        col3.Name = "icon"
        DataGridView1.Columns.Add(col3)
        Dim col4 As New DataGridViewTextBoxColumn
        col4.DataPropertyName = "image"
        col4.HeaderText = "image"
        col4.Name = "image"
        DataGridView1.Columns.Add(col4)
        Dim col5 As New DataGridViewTextBoxColumn
        col5.DataPropertyName = "plot"
        col5.HeaderText = "plot"
        col5.Name = "plot"
        DataGridView1.Columns.Add(col5)
        Dim col6 As New DataGridViewTextBoxColumn
        col6.DataPropertyName = "data"
        col6.HeaderText = "data"
        col6.Name = "data"
        DataGridView1.Columns.Add(col6)
        Dim col7 As New DataGridViewTextBoxColumn
        col7.DataPropertyName = "paranet_name"
        col7.HeaderText = "paranet_name"
        col7.Name = "paranet_name"
        DataGridView1.Columns.Add(col7)

        Dim col8 As New DataGridViewTextBoxColumn

        col8.DataPropertyName = "type"
        col8.HeaderText = "type"
        col8.Name = "type"
        DataGridView1.Columns.Add(col8)

        TreeView1.Nodes.Clear()
        Nodes(0) = New TreeNode("Root")
        Nodes(0).Name = "Root"
        Nodes(0).Tag = " "
        Nodes(0).ForeColor = Color.Blue
        TreeView1.Nodes.Add(Nodes(0))
        TextBox3.Text = " "
        TextBox2.Text = " "
        TextBox4.Text = " "
        TextBox5.Text = " "
        TextBox6.Text = " "
        TextBox7.Text = " "
        TextBox9.Text = " "
        TextBox17.Text = " "
        TextBox16.Text = " "
        TextBox8.Text = " "

        web1 = New WebBrowser

        AddHandler web1.DocumentCompleted, AddressOf DocumentComple
        If IsNumeric(TextBox12.Text) Then
            next_page_index = TextBox12.Text * TextBox14.Text
        Else
            next_page_index = 0
        End If


        ' Do stuff


        'Dim cf As DictionaryEntry = CloudFlare.Bypass(_FileContent, "bombshare.altervista.org")
        web1.ScriptErrorsSuppressed = True
        Label10.Text = "טוען דף"
        Button3.Enabled = False
        Button2.Enabled = False
        Button1.Enabled = False

        web1.Navigate(TextBox1.Text)
        once_run = 1

    End Sub
    Private Sub Progresschange(ByVal sender As Object, ByVal e As WebBrowserProgressChangedEventArgs)


        Console.WriteLine("CurrentProgress: " + e.CurrentProgress.ToString)
        If e.CurrentProgress = e.MaximumProgress Then
            'The maximun progres is reached

        End If
        'The page is confirmed downloaded after the pregres return to 0
        If e.CurrentProgress = 0 Then
            If load_stated Then
                'the page is ready to print or download...
                getweb_data()
                load_stated = False
            End If
        End If
    End Sub

    Private Sub PageWaiter(ByVal sender As Object, ByVal e As WebBrowserDocumentCompletedEventArgs)
        If web2.ReadyState = WebBrowserReadyState.Complete Then
            pageready = True
            RemoveHandler web2.DocumentCompleted, New WebBrowserDocumentCompletedEventHandler(AddressOf PageWaiter)
        End If
    End Sub
    Public Function get_direct_link(ByVal link)
        web2 = New WebBrowser
        Dim String_for_regex, _FileContent2
        Dim link_val As String = " "
        Dim matchess
        Dim Regex22
        Dim matchess2

        AddHandler web2.DocumentCompleted, New WebBrowserDocumentCompletedEventHandler(AddressOf PageWaiter)

        web2.ScriptErrorsSuppressed = True
        web2.Navigate(link)


        While Not pageready
            Application.DoEvents()
        End While
        pageready = False
        _FileContent2 = web2.DocumentText
        String_for_regex = RichTextBox3.Text.Replace("(@@@@name@@@@.", "(.").Replace("(@@@@link@@@@.", "(.").Replace("(@@@@icon@@@@.", "(.").Replace("(@@@@plot@@@@.", "(.").Replace("(@@@@fanart@@@@.", "(.").Replace("(@@@@imdb@@@@.", "(.").Replace("(@@@@year@@@@.", "(.").Replace("(@@@@season@@@@.", "(.").Replace("(@@@@episode@@@@.", "(.")
        Regex22 = New Regex(String_for_regex, RegexOptions.Singleline)

        matchess = Regex22.Matches(_FileContent2)
        For Each Match_1 In matchess


            matchess2 = Regex22.Matches(Match_1.Value)
            If link_val = " " Then
                link_val = matchess2(0).Groups(1).Value()
            Else
                link_val = link_val + "$$$" + matchess2(0).Groups(1).Value
            End If



        Next
        web2.Dispose()

        Return link_val
    End Function
    Private Sub DocumentComple(ByVal sender As Object, ByVal e As System.Windows.Forms.WebBrowserDocumentCompletedEventArgs)

        If (web1.ReadyState <> WebBrowserReadyState.Complete) Then
            Return
        Else
            getweb_data()
        End If


    End Sub
    Public Sub getweb_data()

        Dim String_for_regex
        Dim Name_val = " ", link_val = " ", icon_val = " ", fanart_val = " ", plot_val As String = " "
        Dim matches2, matches3
        Dim Regex2, regex3
        Dim x, Max, matches_fimdb
        Dim all_data2 As New Dictionary(Of String, String)
        Dim new_imdbid As String

        Dim imdb_Index, year_Index, season_Index, episode_Index
        Dim imdb_val = " ", year_val = " ", season_val = " ", episode_val As String = " "
        Dim name_Index, link_Index, icon_Index, fanart_Index, plot_Index As Integer
    

        _FileContent = web1.DocumentText
        If once_run = 1 Then
            once_run = 0
            Label10.Text = " מעבד מידע"

            regex3 = New Regex("@@@@(.+?)@@@@", RegexOptions.Singleline)
            matches3 = regex3.Matches(RichTextBox1.Text)

            For Each Values In matches3
                all_data2(Values.Value.replace("@@@@", "")) = "1"



            Next


            String_for_regex = RichTextBox1.Text.Replace("(@@@@name@@@@.", "(.").Replace("(@@@@link@@@@.", "(.").Replace("(@@@@icon@@@@.", "(.").Replace("(@@@@plot@@@@.", "(.").Replace("(@@@@fanart@@@@.", "(.").Replace("(@@@@imdb@@@@.", "(.").Replace("(@@@@year@@@@.", "(.").Replace("(@@@@season@@@@.", "(.").Replace("(@@@@episode@@@@.", "(.")
            Regex = New Regex(String_for_regex, RegexOptions.Singleline)


            matches = Regex.Matches(_FileContent)
            If matches.count = 0 Then
                MsgBox("אין תוצאות נסה שוב")
                Button3.Enabled = True
                Button2.Enabled = True
                Button1.Enabled = True
            Else
                ProgressBar1.Maximum = matches.count - 1

                new_imdbid = ""

                x = 0
                Max = (matches.count - 1)
                If tv_mode = "tv" Then
                    Regex = New System.Text.RegularExpressions.Regex("www.imdb.com/title/(.+?)/", RegexOptions.Singleline)


                    matches_fimdb = Regex.Matches(TextBox1.Text)



                    If matches_fimdb.count > 0 Then

                        f_imdbid = matches_fimdb(0).Groups(1).Value
                    Else
                        f_imdbid = InputBox("IMDB", "הכנס IMDB של סדרה")
                    End If
                End If


                For Each Match In matches

                    Application.DoEvents()
                    ProgressBar1.Value = x
                    Label26.Text = x.ToString + "/" + Max.ToString

                    x = x + 1

                    Name_val = " "
                    link_val = " "
                    icon_val = " "
                    fanart_val = " "
                    plot_val = " "
                    imdb_val = " "
                    year_val = " "
                    season_val = " "
                    episode_val = " "
                    Regex2 = New Regex(String_for_regex, RegexOptions.Singleline)

                    matches2 = Regex2.Matches(Match.Value)
                    name_Index = GetIndexOfKey(all_data2, "name") + 1
                    link_Index = GetIndexOfKey(all_data2, "link") + 1
                    icon_Index = GetIndexOfKey(all_data2, "icon") + 1
                    fanart_Index = GetIndexOfKey(all_data2, "fanart") + 1
                    plot_Index = GetIndexOfKey(all_data2, "plot") + 1

                    imdb_Index = GetIndexOfKey(all_data2, "imdb") + 1
                    year_Index = GetIndexOfKey(all_data2, "year") + 1
                    season_Index = GetIndexOfKey(all_data2, "season") + 1
                    episode_Index = GetIndexOfKey(all_data2, "episode") + 1


                    If name_Index > 0 Then
                        Name_val = System.Web.HttpUtility.HtmlDecode(matches2(0).Groups(name_Index).Value)
                    End If
                    If link_Index > 0 Then
                        link_val = matches2(0).Groups(link_Index).Value
                    End If
                    If icon_Index > 0 Then
                        icon_val = matches2(0).Groups(icon_Index).Value
                    End If
                    If fanart_Index > 0 Then
                        fanart_val = matches2(0).Groups(fanart_Index).Value
                    Else
                        fanart_val = icon_val
                    End If
                    If plot_Index > 0 Then
                        plot_val = System.Web.HttpUtility.HtmlDecode(matches2(0).Groups(plot_Index).Value)
                    End If

                    If imdb_Index > 0 Then
                        imdb_val = matches2(0).Groups(imdb_Index).Value
                    End If


                    If year_Index > 0 Then
                        year_val = matches2(0).Groups(year_Index).Value
                    End If

                    If season_Index > 0 Then
                        season_val = matches2(0).Groups(season_Index).Value
                    Else
                        season_val = upper_season
                    End If

                    If episode_Index > 0 Then
                        episode_val = matches2(0).Groups(episode_Index).Value
                    End If
                    If icon_val = " " Then
                        icon_val = fanart_val

                    End If
                    If fanart_val = " " Then
                        fanart_val = icon_val

                    End If


                    Label11.Text = "מוסיף את " + Name_val

                    'If Len(RichTextBox3.Text) > 10 And Len(link_val) > 3 Then
                    '    link_val = get_direct_link(link_val)

                    'End If
                    season_value = season_val
                    episode_value = episode_val
                    new_imdbid = imdb_val
                    'If tv_mode = "tv" And new_imdbid = "" Then

                    '    regex_imdb = New Regex("www.imdb.com/title/(.+?)/")
                    '    matches_imdb = regex_imdb.Matches(TextBox1.Text)
                    '    If matches_imdb.count > 0 Then
                    '        imdb_val2 = matches_imdb(0).Groups(1).Value
                    '        new_imdbid = imdb_val2
                    '    Else
                    '        new_imdbid = imdb_val
                    '    End If

                    'Else
                    '    If Not tv_mode = "tv" Then
                    '        new_imdbid = imdb_val
                    '    End If

                    'End If

                    


                Next
                RemoveHandler web1.DocumentCompleted, AddressOf DocumentComple
                web1.Dispose()
                Label11.Text = "הסתיים"
                next_page_index = Int(next_page_index)
                If IsNumeric(TextBox10.Text) And next_page_index > 0 Then


                    If next_page_index < (TextBox10.Text * TextBox14.Text) Then
                        web1 = New WebBrowser

                        AddHandler web1.DocumentCompleted, AddressOf DocumentComple

                        ' Do stuff


                        'Dim cf As DictionaryEntry = CloudFlare.Bypass(_FileContent, "bombshare.altervista.org")
                        web1.ScriptErrorsSuppressed = True
                        Label10.Text = "טוען עמוד " + (next_page_index / TextBox14.Text).ToString + "מתוך" + TextBox10.Text

                        Button3.Enabled = False
                        Button2.Enabled = False
                        Button1.Enabled = False

                        web1.Navigate(TextBox11.Text.Replace("PAGE_INDEX", next_page_index.ToString))
                        MsgBox(TextBox11.Text.Replace("PAGE_INDEX", next_page_index.ToString))
                        once_run = 1

                        next_page_index = next_page_index + TextBox14.Text
                    Else
                        next_page_index = 0
                        Label10.Text = "הסתיימו כל העמודים"
                        Button3.Enabled = True
                        Button2.Enabled = True
                        Button1.Enabled = True
                    End If
                Else
                    next_page_index = 0
                    Label10.Text = "  הסתיים :-)"
                    Button3.Enabled = True
                    Button2.Enabled = True
                    Button1.Enabled = True
                End If
            End If



        End If
    End Sub

    Private Sub TreeView1_AfterSelect_1(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TreeView1.AfterSelect
        Dim string_s As String
        Dim Parent As Linq.JObject
        If Not TreeView1.SelectedNode Is TreeView1.Nodes(0) Then



            string_s = ""
            If TreeView1.SelectedNode.Tag.ToString.Contains("<category") Then
                string_s = TreeView1.SelectedNode.Text
            ElseIf TreeView1.SelectedNode.Tag.ToString.Contains("<item") Then
                string_s = TreeView1.SelectedNode.Parent.Text




            End If


        End If

        If Len(TextBox8.Text) > 1 And TextBox8.Text.Contains("imdbnumber") Then
            Try


                Parent = Linq.JObject.Parse(TextBox8.Text)

                TextBox4.Text = Parent("icon")

                TextBox5.Text = Parent("poster")
                TextBox6.Text = Parent("plot")
                TextBox7.Text = Parent("imdbnumber")
                TextBox9.Text = Parent("year")
                TextBox16.Text = Parent("Season")
                TextBox17.Text = Parent("Episode")

                Label23.Text = Parent("heb title")
                Label21.Text = Parent("rating")
                Label19.Text = Parent("genre")
                Label20.Text = Parent("title")
                Label22.Text = Parent("year")

            Catch ex As Exception

            End Try
            '  Label17.Text = "https://www.youtube.com/watch?v=" + parent("trailer")

        End If

    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click

        Try
            RichTextBox3.Text = RichTextBox3.Text.Replace(RichTextBox3.SelectedText, String.Format("(@@@@{0}@@@@.+?)", regex_type))
        Catch ex As Exception

        End Try

    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        RichTextBox3.Text = RichTextBox3.Text.Replace(RichTextBox3.SelectedText, ".+?")
    End Sub
    Dim KeyWordsColors As List(Of Color) = New List(Of Color)(New Color() {Color.Blue, Color.Red, Color.Green, Color.Cyan, Color.Indigo, Color.Khaki, Color.Lavender})
    Dim KeyWords As List(Of String) = New List(Of String)(New String() {"(@@@@name@@@@.+?)", "(@@@@link@@@@.+?)", "(@@@@icon@@@@.+?)", "(@@@@fanart@@@@.+?)", "(@@@@plot@@@@.+?)", "(@@@@imdb@@@@.+?)", "(@@@@year@@@@.+?)"})
    Private Sub RichTextBox1_TextChanged1(ByVal sender As Object, ByVal e As System.EventArgs) Handles RichTextBox1.TextChanged
        Dim words As IEnumerable(Of String) = RichTextBox1.Text.Split(New Char() {" "c, ",", "!", """", ">", "<", "/"})
        Dim index As Integer = 0
        Dim rtb As RichTextBox = sender 'to give normal color according to the base fore color
        For Each word As String In words
            'If the list contains the word, then color it specially. Else, color it normally
            'Edit: Trim() is added such that it may guarantee the empty space after word does not cause error
            coloringRTB(sender, index, word.Length, If(KeyWords.Contains(word.ToLower().Trim()), KeyWordsColors(KeyWords.IndexOf(word.ToLower().Trim())), rtb.ForeColor))
            index = index + word.Length + 1 '1 is for the whitespace, though Trimmed, original word.Length is still used to advance
        Next
    End Sub

    Private Sub RichTextBox3_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RichTextBox3.TextChanged
        Dim words As IEnumerable(Of String) = RichTextBox3.Text.Split(New Char() {" "c, ",", "!", """", ">", "<", "/"})
        Dim index As Integer = 0
        Dim rtb As RichTextBox = sender 'to give normal color according to the base fore color
        For Each word As String In words
            'If the list contains the word, then color it specially. Else, color it normally
            'Edit: Trim() is added such that it may guarantee the empty space after word does not cause error
            coloringRTB(sender, index, word.Length, If(KeyWords.Contains(word.ToLower().Trim()), KeyWordsColors(KeyWords.IndexOf(word.ToLower().Trim())), rtb.ForeColor))
            index = index + word.Length + 1 '1 is for the whitespace, though Trimmed, original word.Length is still used to advance
        Next
    End Sub


    Private Sub IterateTreeNodes(ByVal originalNode As TreeNode, ByVal rootNode As TreeNode)
        Dim childNode As TreeNode

        For Each childNode In originalNode.Nodes

            Dim NewNode As TreeNode = New TreeNode(childNode.Text)
            NewNode.Tag = childNode.Tag
            NewNode.ForeColor = NewNode.ForeColor
           
            IterateTreeNodes(childNode, NewNode)
        Next
    End Sub
  

    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click
        Dim Nodes(10) As TreeNode
        TreeView1.Nodes.Clear()
        Nodes(0) = New TreeNode("Root")
        Nodes(0).Name = "Root"
        Nodes(0).Tag = " "
        Nodes(0).ForeColor = Color.Blue
        TreeView1.Nodes.Add(Nodes(0))
        TextBox3.Text = " "
        TextBox2.Text = " "
        TextBox4.Text = " "
        TextBox5.Text = " "
        TextBox6.Text = " "
        TextBox7.Text = " "
        TextBox9.Text = " "
        TextBox17.Text = " "
        TextBox16.Text = " "
        TextBox8.Text = " "
        Button3.Enabled = True
        Button2.Enabled = True
        Button1.Enabled = True



        tv_mode = "movie"
        imdb_name = ""
        imdb_icon = ""
        imdb_plot = ""
        imdb_image = ""
        imdb_year = ""
        load_stated = ""
        parant_node = ""

        season_value = ""
        episode_value = ""

        exists_items.Clear()

        regex_type = ""
        _FileContent = ""

        upper_season = " "

        once_run = 0


        pageready = (False)
        next_page_index = 0



    End Sub

    Private Sub TextBox4_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox4.TextChanged
        Try
            PictureBox3.Image = Image.FromStream(New WebClient().OpenRead(TextBox4.Text))
        Catch ex As Exception

        End Try
    End Sub

    Private Sub TextBox5_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox5.TextChanged
        Try
            PictureBox1.Image = Image.FromStream(New WebClient().OpenRead(TextBox5.Text))
        Catch ex As Exception

        End Try
    End Sub

    Private Sub TextBox6_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox6.TextChanged
        RichTextBox2.Text = TextBox6.Text
    End Sub

    Private Sub TextBox13_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox13.TextChanged

    End Sub

    Private Sub Button9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button9.Click
        RichTextBox1.Text = "itemprop=""episodeNumber"" content=""(@@@@episode@@@@.+?)""/>"
    End Sub

    Private Sub Button10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button10.Click
        RichTextBox1.Text = "<td class=""titleColumn"">.+?<a href=""/title/(@@@@imdb@@@@.+?)/"
    End Sub
   

   
    Private Sub Button12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button12.Click
      

    End Sub

    Private Sub Button13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button13.Click

       

    End Sub

    Private Sub TextBox14_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox14.KeyPress
        If Asc(e.KeyChar) <> 8 Then
            If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub TextBox14_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox14.TextChanged

    End Sub
End Class
'Friend Class CloudFlare

'    Public Shared Function Bypass(ByVal html As String, ByVal host As String) As DictionaryEntry
'        Dim js As New MSScriptControl.ScriptControl() With {.AllowUI = False, .Language = "JScript"}
'        Dim result As DictionaryEntry = Nothing

'        Try

'            Dim m As MatchCollection = Regex.Matches(html, "var t,r,a,f, (?<value>.*?|[\s\S]*?)a.value")
'            If m.Count.Equals(0) Then Throw New Exception("Could not parse script values.")

'            Dim value As String = m(0).Groups("value").Value
'            Dim val1, val2 As String
'            val1 = value.Split("="c)(0)
'            val2 = value.Split(""""c)(1)
'            Dim key As String
'            key = val1 + "." + val2
'            Dim lines As New List(Of String)
'            For Each line As String In From item In value.Split(";"c) Select item = item.Trim Where Not String.IsNullOrEmpty(item) Where item.StartsWith(key.Split("."c)(0))
'                If line.StartsWith(key.Split("."c)(0) & "=") Then
'                    lines.Add("var " & line & ";")
'                Else
'                    lines.Add(line & ";")
'                End If
'            Next

'            Dim code As New StringBuilder("function bypass(host)")
'            With code
'                .AppendLine("{")
'                For Each line As String In lines
'                    .AppendLine(line)
'                Next
'                .AppendLine(String.Format("    var res = parseInt({0}, 10) + host.length;", key))
'                .AppendLine("    return res;")
'                .AppendLine("}")

'                js.AddCode(.ToString)
'            End With

'            Dim params As Object() = New Object() {host}

'            result = New DictionaryEntry(Regex.Match(html, """([a-fA-F\d]{32})""").Groups(1).Value, js.Run("bypass", params).ToString)

'        Catch ex As Exception
'        Finally
'            js.Reset()
'            Marshal.FinalReleaseComObject(js)
'            js = Nothing
'        End Try

'        Return result
'    End Function

'End Class

