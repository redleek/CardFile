Imports System.IO
Imports Microsoft.Win32
Imports System.Configuration

Class MainWindow
    Dim fileImporter As New CRD_Importer.CRD_Importer
    Dim loadedCardFile As DataModel.CardFile = Nothing
    Dim openFileDialog As OpenFileDialog = New OpenFileDialog With {
        .DefaultExt = ".crd"
    }
    Dim ActiveDB As DataModel.CardFileDB = Nothing

    '#If DEBUG Then
    Dim dbConnectionString As String =
        ConfigurationManager.ConnectionStrings("CardFileDB").ConnectionString.ToString()
    '#Else
    'Dim dbConnectionString As String = String.Empty
    '#End If

    Private Sub OpenButton_Click(sender As Object, e As RoutedEventArgs)
        Dim cardFilePath As String
        If openFileDialog.ShowDialog() Then
            cardFilePath = openFileDialog.FileName
            Try
                loadedCardFile = fileImporter.ParseFile(cardFilePath)
            Catch ex As FileFormatException
                MessageBox.Show(String.Format("File {0} not a valid Card File", cardFilePath))
            Catch ex As NotSupportedException
                MessageBox.Show(
                        String.Format(
                            "File format in file {0} not supported. Needs to be of .crd format",
                            cardFilePath)
                            )
            End Try

            If loadedCardFile IsNot Nothing Then
                Card_ListBox.ItemsSource = loadedCardFile.Cards
                Me.Title = "CardFile: " + loadedCardFile.CardFile_Name
            End If
        End If
    End Sub

    Private Sub ExportButton_Click(sender As Object, e As RoutedEventArgs)
        If loadedCardFile IsNot Nothing Then
            '#If Not DEBUG Then
            '            Dim connectionWindow As New DBConnectionWindow
            '            If connectionWindow.ShowDialog() Then
            '                dbConnectionString = connectionWindow.ConnectionString
            '            End If
            '#End If
            If dbConnectionString IsNot String.Empty Then
                ActiveDB = New DataModel.CardFileDB(dbConnectionString)
                If ActiveDB.DatabaseExists() = False Then
                    MessageBox.Show("Cannot connect to database")
                Else
                    ' Put files into database
                    FileInDB()
                End If
            End If
        Else
            MessageBox.Show("No CardFile loaded to export. Use 'Open' to open a CardFile")
        End If
    End Sub

    Private Sub ScrollViewer_PreviewMouseWheel(sender As Object, e As MouseWheelEventArgs)
        Dim scv As ScrollViewer = sender
        scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta / 2)
    End Sub

    Private Sub CardList_SelectionChanged(sender As Object, e As RoutedEventArgs)
        Dim view As New CardView
        Dim selectedCard As DataModel.Card = TryCast(Card_ListBox.SelectedItem, DataModel.Card)
        If selectedCard IsNot Nothing Then
            view.Load(selectedCard)
            Me.DataContext = view
        End If
    End Sub

    Private Sub FileInDB()
        ActiveDB.CardFiles.InsertOnSubmit(loadedCardFile)
        ActiveDB.SubmitChanges()

        Dim cardFileID As Integer = loadedCardFile.CardFile_ID

        For Each _card In loadedCardFile.Cards
            _card.CardFile_ID = cardFileID
            ActiveDB.Cards.InsertOnSubmit(_card)
        Next
        ActiveDB.SubmitChanges()
    End Sub

    Private Sub Search_TextBox_TextChanged(sender As Object, e As TextChangedEventArgs) Handles Search_TextBox.TextChanged
        Dim searchResults As New List(Of DataModel.Card)
        Dim searchTerm As String = Search_TextBox.Text.Trim().ToLower()
        If searchTerm IsNot String.Empty Then
            For Each _card In loadedCardFile.Cards
                If _card.Card_Title.ToLower().Contains(searchTerm) Or
                    _card.Card_Text.ToLower().Contains(searchTerm) Then
                    searchResults.Add(_card)
                End If
            Next
            Card_ListBox.ItemsSource = searchResults
        Else
            Card_ListBox.ItemsSource = loadedCardFile.Cards
        End If

    End Sub
End Class
