using NAudio.Wave;
using System;
using System.Net;
using System.Net.Sockets;

public static class MicrophoneThread 
{
    public static UdpClient UdpSocket = new UdpClient();
    static WaveOut waveout;
    static BufferedWaveProvider provider;
    public static void Start()
    {
        UdpSocket.Client.Bind(ServerFunctions.MainPoint);
        waveout = new WaveOut();
        waveout.DeviceNumber = 0;
        provider = new BufferedWaveProvider(new WaveFormat(44100, 2));
        waveout.Init(provider);
        waveout.Play();
        while (ServerFunctions.Running)
        {
            IPEndPoint point = new IPEndPoint(IPAddress.Any, 0);
            byte[] buffer = UdpSocket.Receive(ref point);
            provider.AddSamples(buffer, 0, buffer.Length);
        }
    }
}