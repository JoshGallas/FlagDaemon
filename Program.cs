using System;
using System.Text;

namespace FlagDaemon
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Let's do some testing");

            // Initialize Windows interface
            FlagDaemon.Controller.Environments.Windows WindowsInterface = new FlagDaemon.Controller.Environments.Windows();

            /*
            
                Test file methods
            
            */

            /*
                HasFile() method testing
            */

            // Let's see if C:\test.txt exists
            if (WindowsInterface.HasFile("C:\\test.txt"))
                Console.WriteLine("The test file has been created");
            else  
                Console.WriteLine("Test file does not exist yet");

            /*
                CreateFile() method testing
            */

            // We're going to test creating a file, first using a byte array
            String MyTest = "This is a test \n of a multi-line file \n that we will convert to a byte array";
            Byte[] MyTestInBytes = Encoding.ASCII.GetBytes(MyTest);

            if (WindowsInterface.CreateFile("C:\\test.txt", MyTestInBytes))
                Console.WriteLine("We've created test.txt!");
            else
                Console.WriteLine("Unable to create test.txt...");

            //Test to see if TCP port 8543 is open for any connection

            if (WindowsInterface.FirewallAllowsTcp(8543))
                Console.WriteLine("Port 8543 is open for TCP connections!");
            else 
                Console.WriteLine("Port 8543 is not open for TCP connections...");
            


        }
    }
}
