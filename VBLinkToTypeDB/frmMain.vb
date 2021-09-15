Imports GrpcServer
Imports TypeDBCustom
Imports System.Reflection

Public Class frmMain

    'this is the client we build in C# 
    Private client As CoreClient = Nothing

    Private Sub AddValuesToTextBox(msg As String)
        If Me.InvokeRequired Then
            Me.Invoke(Sub() AddValuesToTextBox(msg))
            Return
        End If

        RichTextBox1.AppendText(msg)
        RichTextBox1.AppendText(vbCrLf)

    End Sub

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        'make connection to server
        client = New CoreClient(TextBox1.Text, CInt(TextBox2.Text))
        RichTextBox1.Text = $"Successfully connected to server on {TextBox1.Text}:{TextBox2.Text}"
        RichTextBox1.AppendText(vbCrLf)

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        RichTextBox1.AppendText(vbCrLf)
        'get all databases and add it to combobox
        ComboBox1.Items.AddRange(client.GetAllDatabases())
        If ComboBox1.Items.Count >= 1 Then
            ComboBox1.SelectedIndex = 0
        End If
        For Each dbName In ComboBox1.Items
            RichTextBox1.AppendText($"found databse with name ""{dbName}""")
            RichTextBox1.AppendText(vbCrLf)
        Next

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        'connect to selected database from combo box and create session + pulse automatically.
        Call client.OpenDatabase(ComboBox1.SelectedItem.ToString())
        RichTextBox1.AppendText($"session successfully opened for database ""{ComboBox1.SelectedItem}""")
        RichTextBox1.AppendText(vbCrLf)
        RichTextBox1.AppendText(vbCrLf)

    End Sub

    Private Async Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

        Try


            'this is how we setup a match query, for different query type you have to use different property of query object
            Dim query As New QueryManager.Types.Req() With {
            .MatchReq = New QueryManager.Types.Match.Types.Req() With {.Query = TextBox3.Text},
            .Options = New Options() With {.Parallel = True}
        }

            'clear the existing transactions if there are any.
            client.transactionClient.Reqs.Clear()
            'you can add multiple transaction queries at once
            client.transactionClient.Reqs.Add(New Transaction.Types.Req() With {.QueryManagerReq = query, .ReqId = client.SessionID})
            'write the transaction to bi-directional stream
            Await client.Transactions.RequestStream.WriteAsync(client.transactionClient)

            Dim ServerResp As Transaction.Types.Server = Nothing
            'this is like an enumrator, you have to call MoveNext for every chunk of data you will receive
            Do While Await client.Transactions.ResponseStream.MoveNext(Threading.CancellationToken.None)
                ServerResp = client.Transactions.ResponseStream.Current 'set the current enumrator object to local so can access it shortly

                'this is check if the stream have done sending the data and we exit the loop,
                'if this miss you will stuck on MoveNext 
                If ServerResp.ResPart.ResCase = Transaction.Types.ResPart.ResOneofCase.StreamResPart AndAlso
                    ServerResp.ResPart.StreamResPart.State = Transaction.Types.Stream.Types.State.Done Then
                    Exit Do
                End If

                'this will be different according to your scenario. you need to use breakpoint to see data
                'to implement better logic here. "Answers" below is the array of ConceptMap
                For Each itm In ServerResp.ResPart.QueryManagerResPart.MatchResPart.Answers.ToArray()
                    Dim cncpt As Concept = Nothing 'this will be used to get the concept from conceptMap so you can access values.
                    itm.Map.TryGetValue("fn", cncpt) 'you can get the maping key from your query
                    AddValuesToTextBox($"[{DateTime.Now}] received response: {cncpt.Thing.Value.String}")
                Next

            Loop

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            MsgBox(lsMessage)
        End Try

    End Sub
End Class
