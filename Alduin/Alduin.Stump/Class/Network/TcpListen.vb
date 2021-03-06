﻿Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports Alduin.Stump.Alduin.Stump.Class.Commands
Imports Starksoft.Aspen.Proxy
Namespace Alduin.Stump.Class.Network
    Public Class TcpListen
        Private ReadOnly adr As Net.IPAddress = Net.IPAddress.Parse(IPAddress.Loopback.ToString())
        Private client As TcpClient
        Private ReadOnly listener As TcpListener
        Public Reader As StreamReader
        Public Sub New()
            listener = New TcpListener(adr, Config.Variables.ListenerPort)
        End Sub

        Public Sub TcpAsync()
            listener.Start()
            Dim Message As String
            Dim Result
            If listener.Pending = True Then
                client = listener.AcceptTcpClient
                Message = ""
                Dim Reader As New StreamReader(client.GetStream())
                While Reader.Peek > -1
                    Message += Convert.ToChar(Reader.Read())
                End While
                'Call commands and wait result
                Console.WriteLine(Message)
                Result = Command.CommandHandler(Message, client)
                client.SendBufferSize = 8192
                Dim stream As NetworkStream = client.GetStream()
                Dim buffer As Byte() = System.Text.Encoding.UTF8.GetBytes(Result)
                stream.Write(buffer, 0, buffer.Length)
                client.Close()
            End If
        End Sub

    End Class
End Namespace

