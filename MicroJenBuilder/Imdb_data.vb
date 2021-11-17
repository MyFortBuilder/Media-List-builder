Imports System.IO
Imports System.Net
Imports System.Web
Imports System.Text.RegularExpressions
Imports System.Text


Imports System.ServiceModel.Web
Imports Newtonsoft.Json
Imports System.Runtime.CompilerServices


Imports System.Xml

'=======================================================
'Service provided by Telerik (www.telerik.com)
'Conversion powered by NRefactory.
'Twitter: @telerik
'Facebook: facebook.com/telerik
'=======================================================

Public Module MyExtensions
    <Extension()> _
    Public Sub Add(Of T)(ByRef arr As T(), ByVal item As T)
        Array.Resize(arr, arr.Length + 1)
        arr(arr.Length - 1) = item
    End Sub
End Module

Public Class Imdb_Data

    Public _SiteTitle As String = "https://www.imdb.com/title/"
    Public _IMDBNo As String
    Public _FileContent As String
    Public tmdbid As String
    Public _duration As String = "0"

    Public _rating As String
    Public _votes As String
    Public myPageSource As String
    Public posterImageUrl As Match
    Public imdbid As String
    Public response_ep
    Public poster
    Public tv_movie As String
    Public iconimage, _icon
    Public data_movie, data_series As Boolean

    Dim _genre As String() = {" "}
    Public _title, _year, _plot As String
    Public video_info As New Dictionary(Of String, String)
    Public video_info_final As New Dictionary(Of String, String)
    Dim result As New Dictionary(Of String, String)




    Function deserialize_to_linq_to_json_jobject(ByVal json As String, ByVal data_type As String)

        Dim test2
        Dim test3




        Dim parent As Newtonsoft.Json.Linq.JObject = Newtonsoft.Json.Linq.JObject.Parse(json)
        If data_type = "imdb_id" Then

            data_movie = False
            data_series = False

            Try
                test3 = parent.Value(Of Newtonsoft.Json.Linq.JArray)("movie_results")(0)
                data_movie = True
                tv_movie = "movie"

            Catch ex As Exception

            End Try
            Try
                test3 = parent.Value(Of Newtonsoft.Json.Linq.JArray)("tv_results")(0)
                data_series = True
                tv_movie = "tv"

            Catch ex As Exception
                Try
                    test3 = parent.Value(Of Newtonsoft.Json.Linq.JArray)("tv_episode_results")(0)


                    data_series = True
                    tv_movie = "tv"
                Catch ex2 As Exception

                End Try
            End Try


            'If data_series = True And data_movie = True Or (data_series = False And data_movie = False) Then
            '    Try
            '        If Regex_builder.upper_season <> " " Or Regex_builder.season_value <> " " Or Regex_builder.episode_value <> " " Then

            '        Else
            '            Movie_tv.ShowDialog()
            '        End If
            '    Catch ex As Exception
            '        Movie_tv.ShowDialog()
            '    End Try



            'End If
            If data_movie Then

                test2 = parent.Value(Of Newtonsoft.Json.Linq.JArray)("movie_results")
            ElseIf data_series Then
                test2 = parent.Value(Of Newtonsoft.Json.Linq.JArray)("tv_results")
                If test2.Count = 0 Then
                    test2 = parent.Value(Of Newtonsoft.Json.Linq.JArray)("tv_episode_results")
                End If

            Else

                test2 = parent
            End If

        Else
            test2 = parent

        End If


        Return test2
    End Function
    Public Function add_tv(ByVal IMDBNo)


        _IMDBNo = Trim(IMDBNo)


        'download the whole page, to be able to search it by regex
        Dim URL As String = _SiteTitle + _IMDBNo + "/episodes?ref_=tt_eps_sm"
        Try
            Dim sr As New StreamReader(New WebClient().OpenRead(URL))
            _FileContent = sr.ReadToEnd()

        Catch ex As Exception
            _FileContent = ""

        End Try


        Return _FileContent
    End Function
    '=======================================================
    'Service provided by Telerik (www.telerik.com)
    'Conversion powered by NRefactory.
    'Twitter: @telerik
    'Facebook: facebook.com/telerik
    '=======================================================

    Public Function add_movie(ByVal IMDBNo)


        _IMDBNo = Trim(IMDBNo)


        'download the whole page, to be able to search it by regex
        Dim URL As String = _SiteTitle + _IMDBNo + "/"
        Try
            Dim sr As New StreamReader(New WebClient().OpenRead(URL))
            _FileContent = sr.ReadToEnd()

        Catch ex As Exception
            _FileContent = ""

        End Try


        Return _FileContent
    End Function
    Public Function tmdb_tv_episode_info(ByVal tmdbid, ByVal imdbid, ByVal season)
        Dim postdata As New Dictionary(Of String, String)
        Dim url As String
        Dim data, response

        postdata.Add("api_key", "34142515d9d23817496eeb4ff1d223d0")
        postdata.Add("append_to_response", "account_states,alternative_titles,credits,images,keywords,releases,videos,translations,similar,reviews,lists,rating")
        postdata.Add("language", "en")
        postdata.Add("include_image_language", "en")



        url = "https://api.themoviedb.org/3/" + String.Format(tv_movie + "/{0}/season/{1}", tmdbid, season)


        data = urldecode(postdata)
        response = post_url(url, data, "tv_results")
        Return response
    End Function

    Public Function tmdb_tv_season_info(ByVal tmdbid, ByVal imdbid)
        Dim postdata As New Dictionary(Of String, String)
        Dim url As String
        Dim data, response

        postdata.Add("api_key", "34142515d9d23817496eeb4ff1d223d0")
        postdata.Add("append_to_response", "account_states,alternative_titles,credits,images,keywords,releases,videos,translations,similar,reviews,lists,rating")
        postdata.Add("language", "en")
        postdata.Add("include_image_language", "en")



        url = "https://api.themoviedb.org/3/" + String.Format(tv_movie + "/{0}", tmdbid)


        data = urldecode(postdata)
        response = post_url(url, data, "tv_results")
        Return response
    End Function
    Public Function GetseasonInfo(ByVal imdbid)
        Dim season_data
        tmdbid = imdb_id_to_tmdb(imdbid)

        season_data = tmdb_tv_season_info(tmdbid, imdbid)
        Return season_data
    End Function
    Function get_all_episode_data(ByVal imdbid, ByVal season)
        Dim episode_data
        tmdbid = imdb_id_to_tmdb(imdbid)

        episode_data = tmdb_tv_episode_info(tmdbid, imdbid, season)
        Return episode_data
    End Function
    Public Sub GetInfo(ByVal imdbid, ByVal season, ByVal episode)

        'scrape duration
        Dim matches
        Dim Regex
        Dim matches2
        Regex = New Regex("<meta property='og:title' content=""(.+?)""")

        matches = Regex.Matches(_FileContent)
        Try
            _title = HttpUtility.HtmlDecode(matches(0).Groups(1).Value)

        Catch ex As Exception
            Regex = New Regex("""name"":""(.+?)""")

            matches = Regex.Matches(_FileContent)
            _title = HttpUtility.HtmlDecode(matches(0).Groups(1).Value)

        End Try

        Regex = New Regex("""duration""\:""PT(.+?)M""")
        matches = Regex.Matches(_FileContent)
        If matches.count > 0 Then
            _duration = matches(0).Groups(1).Value
            Try
                _duration = _duration.Split("H")(0) * 60 + _duration.Split("H")(1)
            Catch ex As Exception
                _duration = "0"
            End Try


        Else
            _duration = "0"
        End If


        Regex = New Regex("""ratingValue""\:(.+?)\},")
        matches = Regex.Matches(_FileContent)
        If matches.count > 0 Then
            _rating = Math.Round(Convert.ToDouble(matches(0).Groups(1).Value.replace("}", "")))
            _rating = _rating.ToString

        Else
            _rating = "0"


        End If

        Regex = New Regex("""ratingCount""\:(.+?),")
        matches = Regex.Matches(_FileContent)
        If matches.count > 0 Then
            _votes = matches(0).Groups(1).Value.replace("}", "")
        Else
            _votes = "0"
        End If

        Regex = New Regex("<span id=""titleYear"".+?href=""/year/(.+?)/")
        matches = Regex.Matches(_FileContent)
        If matches.count > 0 Then
            _year = matches(0).Groups(1).Value
        Else
            _year = "0"
        End If
        Regex = New Regex("span class=""itemprop"".+?itemprop=""genre"">(.+?)<")
        matches = Regex.Matches(_FileContent)
        If matches.count > 0 Then
            For Each gend In matches
                Regex = New Regex("span class=""itemprop"".+?itemprop=""genre"">(.+?)<")
                matches2 = Regex.Matches(gend.Value)

                _genre.Add(matches2(0).Groups(1).Value)
            Next

        Else
            _genre.Add(" ")

        End If
        Regex = New Regex("itemprop=""description"">(.+?)<", RegexOptions.Singleline)
        matches = Regex.Matches(_FileContent)


        If matches.count > 0 Then



            _plot = HttpUtility.HtmlDecode(Trim(matches(0).Groups(1).Value).Replace(vbCr, "").Replace(vbLf, ""))


        Else
            _plot = (" ")

        End If

        If _FileContent.Contains("Tv Series") Or _FileContent.Contains("TV Episode") Then
            tv_movie = "tv"
        Else
            tv_movie = "movie"
        End If
        Try
            tv_movie = Regex_builder.tv_mode

        Catch ex As Exception

        End Try


        Regex = New Regex("<div class=""poster"">.+?src=""(.+?)""", RegexOptions.Singleline)
        matches = Regex.Matches(_FileContent)

        If matches.count > 0 Then
            For Each gend In matches
                Regex = New Regex("<div class=""poster"">.+?src=""(.+?)""", RegexOptions.Singleline)
                matches2 = Regex.Matches(gend.Value)

                _icon = (matches2(0).Groups(1).Value)
            Next

        Else
            Regex = New Regex("class=""ipc-image"" loading=""lazy"" src=""(.+?)""", RegexOptions.Singleline)
            matches2 = Regex.Matches(_FileContent)
            If matches2.count > 0 Then
                _icon = (matches2(0).Groups(1).Value)
            Else

                _icon = (" ")

            End If
        End If

        result.Add("duration", _duration)
        result.Add("rating", _rating)
        result.Add("votes", _votes)
        tmdbid = imdb_id_to_tmdb(imdbid)

        video_info_final = tmdb_movie_info(tmdbid, imdbid, season, episode)


    End Sub
    Public Function urldecode(ByVal dict)
        Dim result As New StringBuilder()
        For Each kvp In dict
            If result.Length > 0 Then result.Append("&"c)
            result.Append(kvp.Key).Append("="c).Append(kvp.Value)
        Next
        Return result.ToString()

    End Function
    Function tmdb_get_trailer(ByVal tmdb_movie_id)
        Dim url As String
        Dim Data As String
        Dim response
        Dim results

        Dim postdata As New Dictionary(Of String, String)
        postdata.Add("api_key", "34142515d9d23817496eeb4ff1d223d0")
        postdata.Add("language", "en,he")
        Data = urldecode(postdata)
        url = "https://api.themoviedb.org/3/" + String.Format(tv_movie + "/{0}/videos", tmdb_movie_id)
        response = post_url(url, Data, "")
        results = response("results")
        Return results
    End Function
    Function tmdb_movie_info(ByVal tmdbid, ByVal imdbid, ByVal season, ByVal episode)
        Dim postdata As New Dictionary(Of String, String)
        Dim url, Data As String
        Dim response
        Dim title, url_ep

        Dim actorslist As New List(Of Dictionary(Of String, String))()
        Dim trailer_pre
        Dim trailer, plot
        Dim genres, writers, directors, studios
        Dim actors As New Dictionary(Of String, String)
        Dim genres_pre, writers_pre, directors_pre, studios_pre, actors_pre
        If imdbid = "" Then
            Movie_tv.ShowDialog()

        End If



        genres = ""
        directors = ""
        writers = ""
        title = _title
        trailer = " "
        plot = _plot
        studios = ""
        iconimage = _icon
        poster = _icon

        For Each i In _genre
            If i <> " " Then


                If genres = "" Then
                    genres = i
                Else
                    genres = genres + "/" + i
                End If
            End If
        Next
        Try


            If Not tmdbid Is Nothing Then
                postdata.Add("api_key", "34142515d9d23817496eeb4ff1d223d0")
                postdata.Add("append_to_response", "account_states,alternative_titles,credits,images,keywords,releases,videos,translations,similar,reviews,lists,rating")
                postdata.Add("language", "en")
                postdata.Add("include_image_language", "en")

                If tv_movie = "tv" And season = "0" And episode <> "0" Then
                    'season = InputBox("הכנס מספר עונה", "הכנסת נתונים")
                    'episode = InputBox("הכנס מספר פרק", "הכנסת נתונים")
                    season = "0"
                    episode = "0"

                End If

                url = "https://api.themoviedb.org/3/" + String.Format(tv_movie + "/{0}", tmdbid)
                If episode <> "0" Then
                    url_ep = "https://api.themoviedb.org/3/" + String.Format(tv_movie + "/{0}", tmdbid) + "/season/" + season
                Else
                    url_ep = "https://api.themoviedb.org/3/" + String.Format(tv_movie + "/{0}", tmdbid)
                End If

                Data = urldecode(postdata)
                response = post_url(url, Data, "movie_results")
                If response Is Nothing Then
                    Return video_info
                End If
                If tv_movie = "tv" And episode <> "0" Then
                    response_ep = post_url(url_ep, Data, "tv_results")
                    If response_ep Is Nothing Then
                        Return video_info
                    End If
                End If

                Try
                    If tv_movie = "tv" And episode <> "0" Then
                        title = response_ep("episodes")(Convert.ToInt16(episode) - 1)("name").value
                    Else
                        title = response("title").value
                    End If


                Catch ex As Exception
                    title = response("name").value
                End Try

                genres_pre = response("genres")
                writers_pre = response("credits")("crew")
                directors_pre = response("credits")("crew")
                studios_pre = response("production_companies")
                actors_pre = response("credits")("cast")
                If _votes = "0" Then
                    _votes = response("vote_average")

                End If
                If tv_movie = "tv" And episode <> "0" Then
                    If response_ep("episodes")(Convert.ToInt16(episode) - 1)("still_path").value <> Nothing Then
                        poster = "http://image.tmdb.org/t/p/original/" + response_ep("episodes")(Convert.ToInt16(episode) - 1)("still_path").value
                    End If

                Else
                    If response("backdrop_path").value <> Nothing Then
                        poster = "http://image.tmdb.org/t/p/original/" + response("backdrop_path").value
                    End If


                End If
                If response("poster_path").value <> Nothing Then
                    iconimage = "http://image.tmdb.org/t/p/original/" + response("poster_path").value
                End If


                Try
                    _year = Microsoft.VisualBasic.Left(response("release_date").value, 4)
                Catch ex As Exception
                    _year = Microsoft.VisualBasic.Left(response("first_air_date").value, 4)
                End Try
                For Each i In genres_pre
                    If genres = "" Then
                        genres = i("name").value
                    Else
                        genres = genres + "/" + i("name").value
                    End If

                Next

                For Each i In writers_pre
                    If i("department").value = "Writing" Then
                        If writers = "" Then
                            writers = i("name").value
                        Else
                            writers = writers + "/" + i("name").value
                        End If

                    End If
                    If i("department").value = "Directing" Then
                        If directors = "" Then
                            directors = i("name").value
                        Else
                            directors = directors + "/" + i("name").value
                        End If


                    End If

                Next

                For Each i In actors_pre
                    actors.Add("name", i("name").value)
                    actors.Add("role", i("character").value)
                    actors.Add("thumbnail", i("name").value)
                    actors.Add("order", i("order").value)
                    actorslist.Add(actors)
                    actors.Clear()



                Next

                For Each i In studios_pre
                    If studios = "" Then
                        studios = i("name").value.Replace("""", "")
                    Else
                        studios = studios + "/" + i("name").value.Replace("""", "")
                    End If


                Next
                trailer_pre = tmdb_get_trailer(tmdbid)
                trailer = ""

                For Each v In trailer_pre
                    If v("site") = "YouTube" And v("type") = "Trailer" Then

                        trailer = v("key").value
                    End If


                Next
                If tv_movie = "tv" And episode <> "0" Then
                    If response_ep("episodes")(Convert.ToInt16(episode) - 1)("overview").value <> Nothing Then
                        plot = response_ep("episodes")(Convert.ToInt16(episode) - 1)("overview").value
                    End If
                Else

                    plot = response("overview").value
                End If

            End If

            If plot = "" Then
                plot = _plot
            End If
        Catch ex As Exception

        End Try
        Dim time As DateTime = DateTime.Now
        Dim format As String = "yyyy-MM-d HH:mm:ss"
        Dim now_time
        now_time = (time.ToString(format))

        If _title = Nothing Then
            _title = title
        End If
        If tv_movie = "movie" Then
            video_info.Add("originaltitle", _title.Replace("""", "'").Replace("(", "[").Replace(")", "]"))
            video_info.Add("title", title.replace("""", "'").replace("(", "[").replace(")", "]"))
            video_info.Add("mediatype", "movie")
            video_info.Add("season", "")
            video_info.Add("episode", "")
            video_info.Add("type", "item")
            video_info.Add("content", "movie")
        Else
            video_info.Add("tvshowtitle", _title.Replace("""", "'").Replace("(", "[").Replace(")", "]"))
            video_info.Add("originaltitle", _title.Replace("""", "'").Replace("(", "[").Replace(")", "]"))
            video_info.Add("title", title.replace("""", "'").replace("(", "[").replace(")", "]"))
            video_info.Add("season", season)
            video_info.Add("episode", episode)
            video_info.Add("mediatype", "tv")
            video_info.Add("content", "episode")
            If episode <> "0" Then
                video_info.Add("type", "item")
            Else
                video_info.Add("type", "dir")
            End If
        End If

        video_info.Add("heb title", title.replace("""", "'").replace("(", "[").replace(")", "]"))
        video_info.Add("genre", genres)

        video_info.Add("rating", _rating)
        video_info.Add("director", directors)
        video_info.Add("plot", plot.replace("""", "'").replace("(", "[").replace(")", "]"))
        video_info.Add("duration", _duration)
        video_info.Add("studios", studios)
        video_info.Add("writer", writers.replace("""", "'"))
        video_info.Add("code", imdbid)
        video_info.Add("imdbnumber", imdbid)
        video_info.Add("imdb", imdbid)

        video_info.Add("tmdb", tmdbid)
        video_info.Add("votes", _votes)
        video_info.Add("dateadded", time.ToString(format))
        video_info.Add("trailer", "https://www.youtube.com/watch?v=" + trailer)

        video_info.Add("poster", poster)
        video_info.Add("fanart", poster)
        video_info.Add("icon", iconimage)
        video_info.Add("thumbnail", iconimage)

        video_info.Add("year", _year)

        video_info.Add("link", "https://www.youtube.com/watch?v=" + trailer)

        Return video_info
    End Function
    Public Function imdb_id_to_tmdb(ByVal imdb_movie_id)
        Dim url As String
        Dim data As String
        Dim temp
        Dim tmdbid As String = ""

        Dim postdata As New Dictionary(Of String, String)
        postdata.Add("api_key", "34142515d9d23817496eeb4ff1d223d0")
        postdata.Add("external_source", "imdb_id")

        url = "https://api.themoviedb.org/3/" + String.Format("find/{0}", imdb_movie_id)
        data = urldecode(postdata)
        temp = post_url(url, data, "imdb_id")
        Try
            tmdbid = temp(0)("id").value
        Catch ex As Exception

        End Try


        Return tmdbid
    End Function

    Public Sub GetPhoto()
        Dim MovieName As String '= _MovieTitle.Split(" "c)(0)

        'find the img tag containing the poster in the page
        Dim RegExPattern As String = (Convert.ToString("<img [^\>]* ") & MovieName) + ". [^\>]* src \s* = \s* [\""\']? ( [^\""\'\s>]* )"

        Dim R1 As New Regex(RegExPattern, RegexOptions.IgnoreCase Or RegexOptions.IgnorePatternWhitespace)

        Dim matches As MatchCollection = R1.Matches(_FileContent)

        'find the link in the img tag and download the image and save it in the images folder
        Dim R2 As New Regex("http.{0,}", RegexOptions.IgnoreCase Or RegexOptions.IgnorePatternWhitespace)

        If matches.Count > 0 Then
            posterImageUrl = R2.Match(matches(0).Value)
            Dim posterImage As Image = Image.FromStream(New WebClient().OpenRead(posterImageUrl.Value))

            '_thumbnail = posterImage
        End If
    End Sub

    Public Function get_all_season_data(ByVal imdbid)
        Dim return_value

        add_tv(imdbid)


        result.Clear()

        imdbid = imdbid
        return_value = GetseasonInfo(imdbid)
        Return return_value

    End Function

    Public Function get_all_movie_data(ByVal imdbid, ByVal season, ByVal episode)
        video_info.Clear()

        add_movie(imdbid)
        result.Clear()

        imdbid = imdbid
        GetInfo(imdbid, season, episode)
        Return video_info_final

    End Function
    Public Function get_all_tmdb_data(ByVal imdbid)

        GetInfo(imdbid, "0", "0")
        Return video_info_final

    End Function
    Public Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        add_movie(TextBox1.Text)
        imdbid = TextBox1.Text
        GetInfo(imdbid, "0", "0")
        'GetPhoto()


        RichTextBox1.Text = _duration + Environment.NewLine + _rating + Environment.NewLine + _votes + Environment.NewLine
        RichTextBox2.Text = _FileContent
    End Sub

    Dim logincookie As CookieContainer

    Function post_url(ByVal url, ByVal data, ByVal data_type)

        Try


            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'User-agent': USER_AGENT})
            Dim results
            Dim myHttpWebRequest = CType(WebRequest.Create(url + "?" + data), HttpWebRequest)
            myHttpWebRequest.Accept = "application/json"
            myHttpWebRequest.ContentType = "application/json"
            myHttpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:52.0) Gecko/20100101 Firefox/52.0"

            Dim myHttpWebResponse = CType(myHttpWebRequest.GetResponse(), HttpWebResponse)
            Dim myWebSource As New StreamReader(myHttpWebResponse.GetResponseStream())
            myPageSource = (myWebSource.ReadToEnd())


            'Dim obj = JsonConvert.DeserializeObject(Of Dictionary(Of String, String))(myPageSource)
            results = deserialize_to_linq_to_json_jobject(myPageSource, data_type)
            Return results
        Catch ex As Exception


        End Try
        Return ""

    End Function

    Public Sub Imdb_Data_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class