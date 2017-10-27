Imports System.ComponentModel

Public Class DBConnectionWindow
    Implements INotifyPropertyChanged

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        DataContext = Me
    End Sub

    Private _connectionString As String = String.Empty

    Private WindowTitleText As String = "DB Connection"

    Public Property ConnectionString As String
        Get
            Return _connectionString
        End Get
        Set(value As String)
            _connectionString = value.Trim()
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ConnectionString"))
        End Set
    End Property

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Private Sub ConfirmButton_Click(sender As Object, e As RoutedEventArgs)
        If ConnectionString Is String.Empty Then
            MessageBox.Show(Me, "Connection string cannot be empty", WindowTitleText)
        Else
            Me.DialogResult = True
        End If
    End Sub
End Class
