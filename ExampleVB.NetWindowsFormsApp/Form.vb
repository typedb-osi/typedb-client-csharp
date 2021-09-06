Public Class Form

    Private Sub Form_Load(sender As Object, e As EventArgs) Handles Me.Load

        'Test Grpc            
        Dim lrTypeDBCallInvoker As New TypeDBGrpcCallInvoker()
        Dim lrConnection As New GrpcServer.TypeDB.TypeDBClient(lrTypeDBCallInvoker)

    End Sub

End Class
