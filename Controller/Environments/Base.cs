using System;
using System.Collections.Generic;

namespace FlagDaemon.Controller.Environments
{
    interface Base
    {
        /*

         Policy methods (e.g., password policies)
        
        */

        // Return true if policy is set
        Boolean HasPolicy(String PolicyName);

        /* 
            Returns value of policy ,e.g.,
                GetPolicy(PasswordComplexity) 
                 returns {value: true}
                
                GetPolicy(AuditAccountManagement)
                 returns {auditsuccess: true, auditfailure: false}) 
        */
        Dictionary<String,String> GetPolicy(String PolicyName);

        // Configure policy (basically the opposite of GetPolicy())
        Boolean SetPolicy(String PolicyName, Dictionary<String,String> PolicySettings);


        /*

         Package methods

        */

        // Return true if program/package is installed
        Boolean PackageInstalled(String ProgramName);

        /*
            Return true if successful in installing package 
            Arguments is arbitrary, could be useful:

                Linux
                    Arguments = {type = repo; repo = universe; name = apache2}
                    Arguments = {type = deb; url = http://download.com/file.deb; name = "kafka"}
                Windows
                    Arguments = {type = msi; url = http://download.com/file.msi; cli_args = '/i /s /q'}
                    Arguments = {type = role; role = ".NET 3.5"}
        */
        Boolean InstallPackage(String ProgramName, Dictionary<String,String> Arguments);

        /*
        
            Service methods
        
        */

        // Return true if the service (e.g., Web Publishing) is running
        Boolean ServiceRunning(String ServiceName);

        // Return true if the service (e.g., Web Publishing) is enabled
        Boolean ServiceEnabled(String ServiceName);

        // Return true if service exists at all
        Boolean ServiceExists(String ServiceName);

        // Return true if successful at enabling service
        Boolean EnableService(String ServiceName);

        // Return true if successful at starting service
        Boolean StartService(String ServiceName);


        /*
        
            Process methods
        
        */

        // Return true if process of that name is running
        Boolean ProcessRunning(String ProcessName);

        /*
        
            Network Methods

        */

        // Return true if port is listening (e.g., web server running on correct port)
        Boolean PortListening(int PortNumber);

        // Return true if firewall allows inbound TCP connections to port from any IP
        Boolean FirewallAllowsTcp(ushort PortNumber);

        // Return true if firewall allows inbound TCP connections to port from specific IPs
        Boolean FirewallAllowsTcp(ushort PortNumber, List<String> AllowedIP);

        // Return true if firewall allows inbound UDP connections to port from any IP
        Boolean FirewallAllowsUdp(ushort PortNumber);

        // Return true if firewall allows inbound Udp connections to port from specific IPs
        Boolean FirewallAllowsUdp(ushort PortNumber, List<String> AllowedIP);


        /*

         File methods 

        */

        // Return true if file exists
        Boolean HasFile(String FilePath);

        // Return true if line in file contains a substring
        Boolean FileContains(String FilePath, int LineNumber, String FlagSubstring);

        // Create a file by downloading it from somewhere, return true if successful
        Boolean CreateFile(String FilePath, String SourcePath);

        // Create a file with contents from a byte array, return true if successful
        Boolean CreateFile(String FilePath, Byte[] FileContents); //overload method

        // Create a file with contents from a String array, return true if successful
        Boolean CreateFile(String FilePath, String[] FileContents); //overload method

        // Create a file with contents from a String list, return true if successful
        Boolean CreateFile(String FilePath, List<String> FileContents); //overload method
    }
}
