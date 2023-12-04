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

                var pk = new PrivateKeyFile("path_to_key_file");
                var keyFiles = new[] { pk };
                var methods = new List<AuthenticationMethod>
                {
                    new PrivateKeyAuthenticationMethod(Environment.GetEnvironmentVariable("SSH_USER"), keyFiles)
                };
                var con = new ConnectionInfo(
                    Environment.GetEnvironmentVariable("SSH_HOST"),
                    int.Parse(Environment.GetEnvironmentVariable("SSH_PORT")),
                    Environment.GetEnvironmentVariable("SSH_USER"),
                    methods.ToArray());
                using var client = new SftpClient(con);
                client.Connect();
                PrintInfo(client, "/home");
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

            var availableSpaceGb = (info.AvailableBlocks * info.BlockSize) / 1024 / 1024 / 1024;
            Console.WriteLine($"availableSpaceGb: {availableSpaceGb}");
            Console.WriteLine();
        }
    }
}