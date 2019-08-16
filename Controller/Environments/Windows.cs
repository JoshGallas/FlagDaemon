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

        // Did a little abstraction
        private IEnumerable<IFirewallRule> FindMatchingFirewallRules(ushort PortNumber, FirewallProtocol RuleProtocol) {

            // System.Linq ftw
            IEnumerable<IFirewallRule> Matched = FirewallManager.Instance.Rules.Where(
                Rule => Rule.LocalPorts.Contains(PortNumber) &&
                        Rule.Protocol.Equals(RuleProtocol)
            );

            return Matched;

        }

        private IEnumerable<IFirewallRule> GetFirewallTCPRuleByAction(ushort PortNumber, FirewallAction RuleAction) {

            return this.FindMatchingFirewallRules(PortNumber, FirewallProtocol.TCP).Where(Rule => Rule.IsEnable && Rule.Action.Equals(RuleAction));

        }
        private IEnumerable<IFirewallRule> GetFirewallUDPRuleByAction(ushort PortNumber, FirewallAction RuleAction) {

            return this.FindMatchingFirewallRules(PortNumber, FirewallProtocol.UDP).Where(Rule => Rule.IsEnable && Rule.Action.Equals(RuleAction));

        }

        public bool FirewallAllowsTcp(ushort PortNumber) {

            IEnumerable<IFirewallRule> MatchedRules = this.GetFirewallTCPRuleByAction(PortNumber, FirewallAction.Allow);
            return (MatchedRules != null) && (MatchedRules.ToArray().Length > 0);

        } 

        public bool FirewallAllowsTcp(ushort PortNumber, List<string> AllowedIP)
        {

            // Hit count for matched IPs
            int i = 0;

            // For each rule allowing inbound TCP on a port
            foreach(IFirewallRule rule in this.GetFirewallTCPRuleByAction(PortNumber, FirewallAction.Allow))
                // Check if IP match
                foreach (IAddress ConfiguredAddress in rule.RemoteAddresses) 
                    // If it allows an IP we didn't request, fail here
                    if (! AllowedIP.Contains(ConfiguredAddress.ToString()))
                        return false;
                    else // Otherwise, note that we found a new match
                        i++;

            // Return true only if there exists a single matching inbound firewall rule which has exactly the same port and remote address list as in parameters
            return i == AllowedIP.Count();

        }
        
        public bool FirewallAllowsUdp(ushort PortNumber) {

            IEnumerable<IFirewallRule> MatchedRules = this.GetFirewallUDPRuleByAction(PortNumber, FirewallAction.Allow);
            return (MatchedRules != null) && (MatchedRules.ToArray().Length > 0);

        } 

        public bool FirewallAllowsUdp(ushort PortNumber, List<string> AllowedIP) {

            // Hit count for matched IPs
            int i = 0;

            // For each rule allowing inbound TCP on a port
            foreach(IFirewallRule rule in this.GetFirewallUDPRuleByAction(PortNumber, FirewallAction.Allow))
                // Check if IP match
                foreach (IAddress ConfiguredAddress in rule.RemoteAddresses)
                    // If it allows an IP we didn't request, fail here
                    if (! AllowedIP.Contains(ConfiguredAddress.ToString()))
                        return false;
                    else // Otherwise, note that we found a new match
                        i++;

            // Return true only if there exists a single matching inbound firewall rule which has exactly the same port and remote address list as in parameters
            return i == AllowedIP.Count();

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
