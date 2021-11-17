Public Class Movie_tv

   

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Imdb_Data.tv_movie = "tv"
        Imdb_Data.data_series = True
        Imdb_Data.data_movie = False
        Main.Show()
        Me.Close()

    End Sub



    Private Sub Button2_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Imdb_Data.tv_movie = "movie"
        Imdb_Data.data_movie = True
        Imdb_Data.data_series = False
        Main.Show()

        Me.Close()

    End Sub
End Class