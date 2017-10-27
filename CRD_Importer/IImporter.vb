Imports DataModel
Imports System.IO

Public Interface ICardFileImporter
    Function GetCards(ByRef buff As Byte()) As IList(Of Card)
End Interface
