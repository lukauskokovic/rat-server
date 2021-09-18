using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public static class ShareScreen 
{
    public static Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    
    public static void Start()
    {
        client.Bind(new IPEndPoint(IPAddress.Parse("192.168.0.17"), 1421));
        client.Listen(1);
        while (ServerFunctions.Running)
        {
            Console.WriteLine("Waiting on share screen connection");
            Socket socket = client.Accept();
            Console.WriteLine("Connected share screen");
            while (true)
            {
                MemoryStream stream = new MemoryStream();
                byte[] buffer = new byte[5000];
                int read;
                while ((read = socket.Receive(buffer)) > 0)
                {
                    stream.Write(buffer, 0, read);
                    if (read < 5000) break;
                }
                if (read == 0)
                {
                    Console.WriteLine("Share screen disconnect");
                    break;
                }
                Bitmap bitmap = new Bitmap(stream);
                stream.Dispose();
                ServerFunctions.Parent.shareScreenBox.BackgroundImage = (Bitmap)bitmap.Clone();
            }
        }
    }
}