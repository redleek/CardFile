Imports System.Data.Linq
Imports System.Data.Linq.Mapping
Imports System.ComponentModel
Imports System.Collections.Generic

<Table(Name:="CardFiles")>
Public Class CardFile
    Implements INotifyPropertyChanged, INotifyPropertyChanging

    Public Cards As New List(Of Card)

    Private _cardFile_ID As Integer = 0
    <Column(IsPrimaryKey:=True, IsDbGenerated:=True)>
    Public Property CardFile_ID As Integer
        Get
            Return _cardFile_ID
        End Get
        Set(value As Integer)
            RaiseEvent PropertyChanging(Me, New PropertyChangingEventArgs("CardFile_ID"))
            _cardFile_ID = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("CardFile_ID"))
        End Set
    End Property

    Private _cardFile_Name As String = String.Empty
    <Column>
    Public Property CardFile_Name As String
        Get
            Return _cardFile_Name
        End Get
        Set(value As String)
            RaiseEvent PropertyChanging(Me, New PropertyChangingEventArgs("CardFile_Name"))
            _cardFile_Name = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("CardFile_Name"))
        End Set
    End Property

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    Public Event PropertyChanging As PropertyChangingEventHandler Implements INotifyPropertyChanging.PropertyChanging
End Class
