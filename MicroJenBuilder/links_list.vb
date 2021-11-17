Public Class links_list

    Private Sub ListBox1_SelectedIndexChanged(ByVal sender As ListBox, ByVal e As System.EventArgs) Handles ListBox1.SelectedIndexChanged
        If sender.SelectedIndex <> -1 Then
            Button2.Enabled = True
            Button3.Enabled = True

        End If
    End Sub

    Private Sub links_list_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ListBox1.Items.Clear()

        For Each item In Main.ComboBox1.Items
            ListBox1.Items.Add(item)


        Next
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        ListBox1.Items.Add(InputBox("Add Link"))

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        ListBox1.Items(ListBox1.SelectedIndex) = InputBox("Add Link", "Link", ListBox1.SelectedItem)

    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        ListBox1.Items.RemoveAt(ListBox1.SelectedIndex)
        If ListBox1.Items.Count = 0 Then
            Button2.Enabled = False
            Button3.Enabled = False
        End If
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Main.ComboBox1.Items.Clear()
        Main.ComboBox1.Text = ""
        For Each item In ListBox1.Items
            Main.ComboBox1.Items.Add(item)

        Next
        Try
            Main.ComboBox1.SelectedIndex = 0

        Catch ex As Exception

        End Try
    End Sub
End Class