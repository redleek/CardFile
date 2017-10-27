Imports DataModel
Imports System.IO

Public Class RRGImporter
    Implements ICardFileImporter

    Public Function GetCards(ByRef buff As Byte()) As IList(Of Card) Implements ICardFileImporter.GetCards
        Throw New NotImplementedException()
    End Function
End Class
