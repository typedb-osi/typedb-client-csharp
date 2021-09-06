Public Class TypeDBGrpcCallInvoker

    Inherits Grpc.Core.CallInvoker

    Public Overrides Function BlockingUnaryCall(Of TRequest As Class, TResponse As Class)(method As Grpc.Core.Method(Of TRequest, TResponse), host As String, options As Grpc.Core.CallOptions, request As TRequest) As TResponse
        Throw New NotImplementedException()
    End Function

    Public Overrides Function AsyncUnaryCall(Of TRequest As Class, TResponse As Class)(method As Grpc.Core.Method(Of TRequest, TResponse), host As String, options As Grpc.Core.CallOptions, request As TRequest) As Grpc.Core.AsyncUnaryCall(Of TResponse)
        Throw New NotImplementedException()
    End Function

    Public Overrides Function AsyncServerStreamingCall(Of TRequest As Class, TResponse As Class)(method As Grpc.Core.Method(Of TRequest, TResponse), host As String, options As Grpc.Core.CallOptions, request As TRequest) As Grpc.Core.AsyncServerStreamingCall(Of TResponse)
        Throw New NotImplementedException()
    End Function

    Public Overrides Function AsyncClientStreamingCall(Of TRequest As Class, TResponse As Class)(method As Grpc.Core.Method(Of TRequest, TResponse), host As String, options As Grpc.Core.CallOptions) As Grpc.Core.AsyncClientStreamingCall(Of TRequest, TResponse)
        Throw New NotImplementedException()
    End Function

    Public Overrides Function AsyncDuplexStreamingCall(Of TRequest As Class, TResponse As Class)(method As Grpc.Core.Method(Of TRequest, TResponse), host As String, options As Grpc.Core.CallOptions) As Grpc.Core.AsyncDuplexStreamingCall(Of TRequest, TResponse)
        Throw New NotImplementedException()
    End Function

End Class
