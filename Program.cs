using System;
using System.Text;
using System.Management;
using System.Collections.Generic;
using System.Linq;
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
            List<string> AllowedIP = new List<string> {"174.184.146.248", "86.178.106.18"};
            if (WindowsInterface.FirewallAllowsTcp(80, AllowedIP))
                Console.WriteLine("Port 80 is open for TCP connections!");
            else 
                Console.WriteLine("Port 80 is not open for TCP connections...");

            if (WindowsInterface.FirewallAllowsTcp(3389))
                Console.WriteLine("Port 3389 is open for TCP connections!");
            else 
                Console.WriteLine("Port 3389 is not open for TCP connections...");

            if (WindowsInterface.FirewallAllowsTcp(445))
                Console.WriteLine("Port 445 is open for TCP connections!");
            else 
                Console.WriteLine("Port 445 is not open for TCP connections...");

            if (WindowsInterface.FirewallAllowsTcp(445, new List<string>()))
                Console.WriteLine("Port 445 is open for TCP connections from ANY address!!");
            else 
                Console.WriteLine("Port 445 is not open for TCP connections from ANY address!!...");

            if (WindowsInterface.FirewallAllowsTcp(6667))
                Console.WriteLine("Port 6667 is open for TCP connections!");
            else 
                Console.WriteLine("Port 6667 is not open for TCP connections...");

            if (WindowsInterface.FirewallAllowsTcp(6667, new List<String> {"8.8.8.8"}))
                Console.WriteLine("Port 6667 is open for TCP connections from 8.8.8.8!");
            else 
                Console.WriteLine("Port 6667 is not open for TCP connections from 8.8.8.8...");

            if (WindowsInterface.FirewallAllowsTcp(6667, new List<String> {"127.0.0.1"}))
                Console.WriteLine("Port 6667 is open for TCP connections from 127.0.0.1!");
            else 
                Console.WriteLine("Port 6667 is not open for TCP connections from 127.0.0.1...");


            /*
            
                Test RSOP queries
            
            */

            // This only works as administrator (i.e., run a cmd prompt as admin and then execute FlagDaemon.exe from bin\Debug\netcoreapp3.0)

            try {

                System.Console.WriteLine(
                    String.Format(
                        "Cleartext password storage: '{0}'",
                        WindowsInterface.GetPolicy("password-store-cleartext")["Setting"]
                    )
                );
                System.Console.WriteLine(
                    String.Format(
                        "Password Complexity: '{0}', with minimum length '{1}'",
                        WindowsInterface.GetPolicy("password-complexity")["Setting"],
                        WindowsInterface.GetPolicy("password-length-min")["Setting"]
                    )
                );
                System.Console.WriteLine(
                    String.Format(
                        "Lockout duration: '{0}' after '{1}' bad attempts, reset attempts after '{2}'",
                        WindowsInterface.GetPolicy("account-lockout-duration")["Setting"],
                        WindowsInterface.GetPolicy("account-lockout-threshold")["Setting"],
                        WindowsInterface.GetPolicy("account-lockout-reset")["Setting"]
                    )
                );
                System.Console.WriteLine(
                    String.Format(
                        "Password Age Range: '{0}-{1}', history of '{2}'",
                        WindowsInterface.GetPolicy("password-age-min")["Setting"],
                        WindowsInterface.GetPolicy("password-age-max")["Setting"],
                        WindowsInterface.GetPolicy("password-age-history")["Setting"]
                    )
                );
                System.Console.WriteLine(
                    String.Format(
                        "Login notice: '{0}'",
                        WindowsInterface.GetPolicy("account-login-notice")["Data"]
                    )
                );
                System.Console.WriteLine(
                    String.Format(
                        "Audit logon: success='{0}', failure='{1}'",
                        WindowsInterface.GetPolicy("audit-logon-events")["Success"],
                        WindowsInterface.GetPolicy("audit-logon-events")["Failure"]
                    )
                );

            } catch (Exception e) {
                System.Console.WriteLine(String.Format("Policy testing only works when running as admin: '{0}'", e.ToString()));
            }

            // Some old testing
            /*try {

                System.Console.WriteLine("Get list of RSOP GPO");
                ManagementObjectCollection RSOPResults_GPO = Controller.Environments.API.WMI.QueryRSOPPolicySetting();

                System.Console.WriteLine("Parse list of RSOP GPO");
                foreach (ManagementObject m in RSOPResults_GPO) {

                    System.Console.WriteLine(String.Format("Found GPO '{0}', scope: {1}, path: {2}", m.ToString(), m.Scope, m.Path));

                    PropertyDataCollection GPO_Properties = m.Properties;
                    List<String> propertylist = new List<String>();
                    foreach (PropertyData property in GPO_Properties) {
                        propertylist.Add(String.Format("'{0}' = '{1}'", property.Name, property.Value));
                    }

                    //System.Console.WriteLine(String.Format("GPO = [{0}]", String.Join(";",propertylist)));

                    
                }
                System.Console.WriteLine("End parse list of RSOP GPO");

            } catch (Exception e) {

                System.Console.WriteLine(String.Format("RSOP testing can only be done as administrator: '{0}'",e.ToString()));

            }*/

        }
    }
}
