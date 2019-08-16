using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Management;
using WindowsFirewallHelper;
using System.Linq;
using System.Text;

namespace FlagDaemon.Controller.Environments
{
    public class Windows : FlagDaemon.Controller.Environments.Base
    {


        // Map 'universal' policy name to WMI short name (left side = same for win/nix, right-size = WMI short name from API.WMI)
        private Dictionary<String,String> PolicyMappings = new Dictionary<String,String> {
            {"password-complexity", "PasswordComplexity"},
            {"password-store-cleartext", "ClearTextPassword"},
            {"password-age-max", "MaximumPasswordAge"},
            {"password-age-min", "MinimumPasswordAge"},
            {"password-age-history", "PasswordHistorySize"},
            {"password-length-min", "MinimumPasswordLength"},
            {"account-lockout-duration", "LockoutDuration"},
            {"account-lockout-reset", "ResetLockoutCount"},
            {"account-lockout-threshold", "LockoutBadCount"},
            {"audit-logon-events", "AuditLogonEvents"},
            {"account-login-notice","REG_LogonNotice"}
        };

        /*
        
         Methods specific to Windows
        
        */

    

        /*
         Implemented methods
        */
        
        public bool HasFile(string FilePath) => File.Exists(FilePath); // Fancy lambda-style definition (via System.IO.File)

        public bool CreateFile(string FilePath, byte[] FileContents)
        {
            try {

                // Open file for writing
                BinaryWriter FileHandle = new BinaryWriter(File.Open(FilePath, FileMode.OpenOrCreate));

                // Write to file
                FileHandle.Write(FileContents);

                // Clean up
                FileHandle.Flush();
                FileHandle.Close();

                // Return success
                return true;

            } catch (Exception e) {

                System.Console.WriteLine(String.Format(
                    "Received error '{0}'",
                    e.ToString()
                ));
                return false;

            }
        }

        public bool CreateFile(string FilePath, string[] FileContents)
        {
            try {

                // Open file for writing
                //Use BinaryWriter for In-Memory representation of contents
                BinaryWriter FileHandle = new BinaryWriter(File.Open(FilePath, FileMode.OpenOrCreate));
                
                // Write each content to file
                foreach(var item in FileContents) {
                    FileHandle.Write(item);
                }

                // Clean up
                FileHandle.Flush();
                FileHandle.Close();

                // Return success
                return true;

            } catch (Exception e) {

                System.Console.WriteLine(String.Format(
                    "Received error '{0}'",
                    e.ToString()
                ));
                return false;

            }
        }

        public bool CreateFile(string FilePath, List<string> FileContents)
        {
            try {

                // Open file for writing
                //Use BinaryWriter for In-Memory representation of contents
                BinaryWriter FileHandle = new BinaryWriter(File.Open(FilePath, FileMode.OpenOrCreate));

                // Write each content to file
                foreach(var item in FileContents) {
                    FileHandle.Write(item);
                }

                // Clean up
                FileHandle.Flush();
                FileHandle.Close();

                // Return success
                return true;

            } catch (Exception e) {

                System.Console.WriteLine(String.Format(
                    "Received error '{0}'",
                    e.ToString()
                ));
                return false;

            }
        }
        public Dictionary<string, string> GetPolicy(string PolicyName) => API.WMI.QueryRSOPPolicyByName(this.PolicyMappings[PolicyName]).Data;

        /*
            Firewall Methods
         */
        public bool FirewallAllowsTcp(ushort PortNumber)
        {
            foreach(var rule in FirewallManager.Instance.Rules) 
            {
                //Check if it is ports being added to the firewall rule and with rule contains the port number
                if(rule.LocalPorts.Length >= 1 && rule.LocalPorts.Contains(PortNumber)) {
                    //Check if everything is the correct credentials
                    if(rule.Protocol.Equals(FirewallProtocol.TCP) && rule.IsEnable && rule.Action.Equals(FirewallAction.Allow))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool FirewallAllowsTcp(ushort PortNumber, List<string> AllowedIP)
        {
           foreach(var rule in FirewallManager.Instance.Rules) 
            {
                //Check if it is ports being added to the firewall rule and with rule contains the port number
                if(rule.LocalPorts.Length >= 1 && rule.LocalPorts.Contains(PortNumber)) {
                    //Check if everything is the correct credentials
                    if(rule.Protocol.Equals(FirewallProtocol.TCP) && rule.IsEnable && rule.Action.Equals(FirewallAction.Allow))
                    {    
                        foreach(var NeededAddress in AllowedIP) 
                        {
                            foreach(var ContainedAddress in rule.RemoteAddresses) 
                            {
                                //Super cheeky way of reverse working through the Remote Addresses
                                if(NeededAddress.Equals(ContainedAddress.ToString()))
                                    goto FirstLoop;
                            }
                            //If the Remote Addresses do not contain the IP needed, it will return false here
                            return false;

                            FirstLoop:
                            continue;
                        }
                        //At this point all needed addresses were inside of the remote addresses and we can move on
                        return true;
                    }
                }
            }
            return false;
        }
        
        public bool FirewallAllowsUdp(ushort PortNumber) {
             foreach(var rule in FirewallManager.Instance.Rules) 
            {
                //Check if it is ports being added to the firewall rule and with rule contains the port number
                if(rule.LocalPorts.Length >= 1 && rule.LocalPorts.Contains(PortNumber)) {
                    //Check if everything is the correct credentials
                    if(rule.Protocol.Equals(FirewallProtocol.UDP) && rule.IsEnable && rule.Action.Equals(FirewallAction.Allow))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool FirewallAllowsUdp(ushort PortNumber, List<string> AllowedIP) {
           foreach(var rule in FirewallManager.Instance.Rules) 
            {
                //Check if it is ports being added to the firewall rule and with rule contains the port number
                if(rule.LocalPorts.Length >= 1 && rule.LocalPorts.Contains(PortNumber)) {
                    //Check if everything is the correct credentials
                    if(rule.Protocol.Equals(FirewallProtocol.UDP) && rule.IsEnable && rule.Action.Equals(FirewallAction.Allow))
                    {    
                        foreach(var NeededAddress in AllowedIP) 
                        {
                            foreach(var ContainedAddress in rule.RemoteAddresses) 
                            {
                                //Super cheeky way of reverse working through the Remote Addresses
                                if(NeededAddress.Equals(ContainedAddress.ToString()))
                                    goto FirstLoop;
                            }
                            //If the Remote Addresses do not contain the IP needed, it will return false here
                            return false;

                            FirstLoop:
                            continue;
                        }
                        //At this point all needed addresses were inside of the remote addresses and we can move on
                        return true;
                    }
                }
            }
            return false;
        }


        // Not implemented yet
        public bool CreateFile(string FilePath, string SourcePath)
        {
            throw new NotImplementedException();
        }

        public bool EnableService(string ServiceName)
        {
            throw new NotImplementedException();
        }

        public bool FileContains(string FilePath, int LineNumber, string FlagSubstring)
        {
            throw new NotImplementedException();
        }

        public bool HasPolicy(string PolicyName)
        {
            throw new NotImplementedException();
        }

        public bool InstallPackage(string ProgramName, Dictionary<string, string> Arguments)
        {
            throw new NotImplementedException();
        }

        public bool PackageInstalled(string ProgramName)
        {
            throw new NotImplementedException();
        }

        public bool PortListening(int PortNumber)
        {
            throw new NotImplementedException();
        }

        public bool ProcessRunning(string ProcessName)
        {
            throw new NotImplementedException();
        }

        public bool ServiceEnabled(string ServiceName)
        {
            throw new NotImplementedException();
        }

        public bool ServiceExists(string ServiceName)
        {
            throw new NotImplementedException();
        }

        public bool ServiceRunning(string ServiceName)
        {
            throw new NotImplementedException();
        }

        public bool SetPolicy(string PolicyName, Dictionary<string, string> PolicySettings)
        {
            throw new NotImplementedException();
        }

        public bool StartService(string ServiceName)
        {
            throw new NotImplementedException();
        }
    }
}
