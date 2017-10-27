Imports System.Data.Linq.Mapping
Imports System.ComponentModel
Imports DataModel

Public Class CardView
    Implements INotifyPropertyChanged
    Private _title As String
    Public Property Title As String
        Get
            Return _title
        End Get
        Set(value As String)
            _title = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Title"))
        End Set
    End Property

    Private _text As String
    Public Property Text As String
        Get
            Return _text
        End Get
        Set(value As String)
            _text = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Text"))
        End Set
    End Property

    Public Sub Load(ByRef card As Card)
        Me.Title = card.Card_Title
        Me.Text = card.Card_Text
    End Sub

    Public Sub Save(ByRef card As Card)
        card.Card_Title = Me.Title
        card.Card_Text = Me.Text
    End Sub

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
End Class
