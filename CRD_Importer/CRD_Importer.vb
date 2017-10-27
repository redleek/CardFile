Imports DataModel
Imports System.Collections.Generic
Imports System.IO
Imports System.Text

''' <summary>
''' Available formats of CardFiles
''' </summary>
Public Enum CRD_Format
    MGC
    RRG
End Enum

''' <summary>
''' Imports CardFiles
''' </summary>
Public Class CRD_Importer
    Private cardFile As CardFile
    Private cardList As List(Of Card)

    Private Const _stringMGC As String = "MGC"
    Private Const _stringRRG As String = "RRG"

    ''' <summary>
    ''' Parses the card file and extracts a CardFile object
    ''' </summary>
    ''' <param name="filePath">Path to the CardFile</param>
    ''' <returns>Returns an extracted CardFile</returns>
    ''' <exception cref="IOException">Thrown when accessing file</exception>
    ''' <exception cref="FileNotFoundException">Thrown when accessing file</exception>
    Public Function ParseFile(ByRef filePath As String) As CardFile
        cardFile = New CardFile
        Dim crdFileBuffer() As Byte = Nothing

        Using fileStream As New FileStream(filePath, FileMode.Open)
            Using memStream As New MemoryStream
                fileStream.CopyTo(memStream)
                crdFileBuffer = memStream.GetBuffer()
            End Using
        End Using

        Dim fileFormat As CRD_Format = GetFormat(crdFileBuffer)

        Dim _importerFactory As New ImporterFactory
        Dim _importer As ICardFileImporter
        _importer = _importerFactory.GetImporter(format:=fileFormat)

        cardFile.CardFile_Name = GetFileName(filePath)
        Try
            cardFile.Cards = _importer.GetCards(crdFileBuffer)
        Catch exception As Exception
            Throw New ArgumentException("File is not valid")
        End Try

        Return cardFile
    End Function

    Private Function GetFileName(ByRef filePath As String) As String
        Dim fileName As String = String.Empty
        Const delim As Char = "\"
        Dim dirs() = filePath.Split(delim)
        fileName = dirs.Last().Split(".").First()
        Return fileName
    End Function

    ''' <summary>
    ''' Gets format of the CardFile
    ''' </summary>
    ''' <param name="fileBuff">Array where the CardFile is stored</param>
    ''' <returns>Format of the CardFile</returns>
    Private Function GetFormat(ByRef fileBuff As Byte()) As CRD_Format
        Dim format As CRD_Format
        Dim formatBytes(2) As Byte

        For i = 0 To 2
            formatBytes(i) = fileBuff(i)
        Next

        Dim formatString As String = Encoding.ASCII.GetString(formatBytes)
        Select Case formatString.ToUpper()
            Case _stringMGC
                format = CRD_Format.MGC
            Case _stringRRG
                format = CRD_Format.RRG
            Case Else
                Throw New NotSupportedException("Format Not Supported")
        End Select

        Return format
    End Function
End Class
