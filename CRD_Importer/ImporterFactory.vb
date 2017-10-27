''' <summary>
''' Generate ICardFileImporter objects
''' </summary>
Public Class ImporterFactory
    ''' <summary>
    ''' Get an importer from a CRD_Format
    ''' </summary>
    ''' <param name="format">Format of the card file</param>
    ''' <returns>Importer needed for importing file</returns>
    ''' <exception cref="ArgumentException">Thrown when format is not supported</exception>
    Public Function GetImporter(ByVal format As CRD_Format) As ICardFileImporter
        Select Case format
            Case CRD_Format.MGC
                Return New MGCImporter
            Case CRD_Format.RRG
                Return New RRGImporter
            Case Else
                Throw New ArgumentException("Case Not Supported")
        End Select
    End Function
End Class
