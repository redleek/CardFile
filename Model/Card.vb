Imports System.Data.Linq.Mapping
Imports System.ComponentModel

<Table(Name:="Cards")>
Public Class Card
    Implements INotifyPropertyChanged, INotifyPropertyChanging

    Private _card_ID As Integer = 0
    <Column(IsPrimaryKey:=True, IsDbGenerated:=True)>
    Public Property Card_ID As Integer
        Get
            Return _card_ID
        End Get
        Set(value As Integer)
            _card_ID = value
        End Set
    End Property

    Private _cardFile_ID As Integer = 0
    <Column>
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

    Private _card_Title As String = String.Empty
    <Column>
    Public Property Card_Title As String
        Get
            Return _card_Title
        End Get
        Set(value As String)
            RaiseEvent PropertyChanging(Me, New PropertyChangingEventArgs("Card_Title"))
            _card_Title = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Card_Title"))
        End Set
    End Property

    Private _card_Text As String = String.Empty
    <Column>
    Public Property Card_Text As String
        Get
            Return _card_Text
        End Get
        Set(value As String)
            RaiseEvent PropertyChanging(Me, New PropertyChangingEventArgs("Card_Text"))
            _card_Text = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Card_Text"))
        End Set
    End Property

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    Public Event PropertyChanging As PropertyChangingEventHandler Implements INotifyPropertyChanging.PropertyChanging
End Class
