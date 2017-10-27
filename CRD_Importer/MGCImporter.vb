Imports DataModel
Imports System.Text
Imports System.Text.RegularExpressions

''' <summary>
''' Imports CardFiles with the MGC signature
''' </summary>
Public Class MGCImporter
    Implements ICardFileImporter

    Private Const IndexEntrySize As Integer = 52
    Private Const IndexStartPos As Integer = &H5

    Private noCards As Integer = 0
    Private cardFileBuff() As Byte

    ''' <summary>
    ''' Gets a collection of cards from a card file
    ''' </summary>
    ''' <param name="buff">Buffer of bytes containing card file</param>
    ''' <returns></returns>
    Public Function GetCards(ByRef buff As Byte()) As IList(Of Card) Implements ICardFileImporter.GetCards
        cardFileBuff = buff
        Dim cards As New List(Of Card)

#Region "Header"
        noCards = GetCardCount()
#End Region

#Region "Index & Data"
        Dim indexTable As Byte() = GetIndexTable()
        For Each indexEntry As Byte() In GetNextIndexEntry(indexTable)
            cards.Add(IndexEntryToCard(indexEntry))
        Next
#End Region

        Return cards
    End Function

    ''' <summary>
    ''' Get number of cards in card file
    ''' </summary>
    Private Function GetCardCount() As Integer
        Const countStart = &H3, countEnd = &H4
        Dim count As Integer = 0
        Dim a As Integer = cardFileBuff(countEnd)

        count = BitConverter.ToUInt16(
            New Byte() {cardFileBuff(countStart), cardFileBuff(countEnd)},
            0)
        Return count
    End Function

    ''' <summary>
    ''' Retrieves card from an index entry
    ''' </summary>
    ''' <param name="indexEntry">Index entry to get card info from</param>
    ''' <returns>Returns a card</returns>
    Private Function IndexEntryToCard(ByRef indexEntry As Byte()) As Card
        Dim card As New Card
        Dim dataAddr As Integer = &H0
        Dim dataAddrBytes As New List(Of Byte)
        Dim byteTitle As Byte()
        Dim byteEntry As Byte()
        Dim titleReadPos As Integer = 0

        ' Get Data address
        Dim isPrevNonZero As Boolean = False
        For Each _byte In indexEntry
            If isPrevNonZero And _byte = &H0 Then
                Exit For
            ElseIf _byte > &H0 Then
                dataAddrBytes.Add(_byte)
                isPrevNonZero = True
            End If
            titleReadPos += 1
        Next

        ' Get Index string
        byteTitle = indexEntry.Skip(titleReadPos).ToArray()
        byteTitle = byteTitle.SkipWhile(Function(x) x = 0).ToArray()
        byteTitle = byteTitle.TakeWhile(Function(x) x <> &H0).ToArray()
        card.Card_Title = Encoding.ASCII.GetString(byteTitle).Replace(vbNullChar, Nothing)

        ' Get data entry address in file
        dataAddr = If(dataAddrBytes.Count < 2,
            dataAddrBytes.First(),
            BitConverter.ToUInt16(dataAddrBytes.ToArray(), 0))
        byteEntry = cardFileBuff.Skip(2).Skip(dataAddr).ToArray()
        Dim dataLength = BitConverter.ToUInt16(New Byte() {byteEntry(0), byteEntry(1)}, 0)

        ' Skip data length and take length of data
        byteEntry = byteEntry.Skip(2).Take(dataLength).ToArray()

        card.Card_Text = CleanInput(Encoding.ASCII.GetString(byteEntry))
        Return card
    End Function

    ''' <summary>
    ''' Removes bad characters from string
    ''' </summary>
    ''' <param name="str"></param>
    ''' <returns></returns>
    Private Function CleanInput(ByRef str As String) As String
        Dim result As String = String.Empty
        Dim specialChars As String = "_+-.,!@#$%^&*();\/|<>""'"
        Try
            result = Regex.Replace(
                str,
                String.Format("[^A-Za-z\d{0}\s]", specialChars),
                "",
                options:=RegexOptions.None)
        Catch ex As Exception
            Throw ex
        End Try
        Return result
    End Function

    ''' <summary>
    ''' Get bytes of index table from buffer
    ''' </summary>
    ''' <returns>Byte array of index table</returns>
    Private Function GetIndexTable() As Byte()
        Dim IndexByteLength As Integer = IndexEntrySize * noCards

        ' Trim bytes to get index table
        Dim indexTable As Byte() = cardFileBuff.Skip(IndexStartPos).ToArray()
        indexTable = indexTable.Take(IndexByteLength).ToArray()

        Return indexTable
    End Function

    ''' <summary>
    ''' Retrieves the next index entry from a given index table
    ''' </summary>
    ''' <param name="indexTable">Byte table to iterate through</param>
    ''' <returns>Byte array of the next index entry</returns>
    Private Iterator Function GetNextIndexEntry(ByVal indexTable() As Byte) As IEnumerable(Of Byte())
        Dim indexBuffer(
            IndexEntrySize - 1
            ) As Byte
        Dim skip As Integer = 0
        Dim take = IndexEntrySize

        For i = 1 To noCards
            indexBuffer =
                indexTable.Skip(skip).Take(take).ToArray()
            skip += IndexEntrySize
            Yield indexBuffer
        Next
    End Function
End Class
