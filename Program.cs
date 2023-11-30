using Renci.SshNet;
using Renci.SshNet.Sftp;
using System.Text.Json;

namespace DiskMonitor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Connecting!");
                using var client = new SftpClient(
                    Environment.GetEnvironmentVariable("SSH_HOST"),
                    int.Parse(Environment.GetEnvironmentVariable("SSH_PORT")),
                    Environment.GetEnvironmentVariable("SSH_USER"),
                    "secret");
                client.Connect();
                PrintInfo(client, "/home");
                PrintInfo(client, "/init");
                PrintInfo(client, "/mnt/c");
                PrintInfo(client, "/mnt/d");
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonSerializer.Serialize(ex));
            }
        }

        static void PrintInfo(SftpClient client, string path)
        {
            SftpFileSytemInformation info = client.GetStatus(path);
            Console.WriteLine(path);
            Console.WriteLine($"Sid                 : {info.Sid}");
            Console.WriteLine($"SupportsSetUid      : {info.SupportsSetUid}");
            Console.WriteLine($"AvailableBlocks     : {info.AvailableBlocks}");
            Console.WriteLine($"AvailableBlocks %   : {((decimal)info.AvailableBlocks / (decimal)info.TotalBlocks * 100).ToString("N")}");
            Console.WriteLine($"Usage %             : {(((decimal)info.TotalBlocks - (decimal)info.AvailableBlocks) / (decimal)info.TotalBlocks * 100).ToString("N")}");
            Console.WriteLine($"FreeBlocks          : {info.FreeBlocks}");
            Console.WriteLine($"TotalBlocks         : {info.TotalBlocks}");
            Console.WriteLine($"BlockSize           : {info.BlockSize}");
            Console.WriteLine($"FileSystemBlockSize : {info.FileSystemBlockSize}");
            Console.WriteLine($"AvailableNodes      : {info.AvailableNodes}");
            Console.WriteLine($"FreeNodes           : {info.FreeNodes}");
            Console.WriteLine($"TotalNodes          : {info.TotalNodes}");
            Console.WriteLine($"IsReadOnly          : {info.IsReadOnly}");
            Console.WriteLine($"MaxNameLenght       : {info.MaxNameLenght}");
            Console.WriteLine();
        }
    }
}