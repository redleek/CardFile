Imports System.ComponentModel
Imports System.Data.Linq

Public Class CardFileDB
    Inherits DataContext

    Public CardFiles As Table(Of CardFile)
    Public Cards As Table(Of Card)

    Public Sub New(ByRef connection As String)
        MyBase.New(connection)
        If Me.DatabaseExists() = True Then
            CardFiles = Me.GetTable(Of CardFile)
            Cards = Me.GetTable(Of Card)
        End If
    End Sub
End Class
