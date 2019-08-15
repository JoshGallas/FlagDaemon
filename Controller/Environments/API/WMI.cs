using System;
using System.Collections.Generic;
using System.Management;
using FlagDaemon.Models.Environments.Windows.WMI;

namespace FlagDaemon.Controller.Environments.API
{
    public static class WMI
    {

        // Map RSOP WMI object to setting: Shortname -> ClassName,KeyName,ValueField
        public static Dictionary<String,RSOPObject> WMIMappings = new Dictionary<String,RSOPObject> {
            {"PasswordComplexity", new RSOPObject("RSOP_SecuritySettingBoolean","KeyName","PasswordComplexity",new List<String>(){"Setting"})},
            {"ClearTextPassword", new RSOPObject("RSOP_SecuritySettingBoolean","KeyName","ClearTextPassword",new List<String>(){"Setting"})},
            {"LockoutDuration", new RSOPObject("RSOP_SecuritySettingNumeric","KeyName","LockoutDuration",new List<String>(){"Setting"})},
            {"MaximumPasswordAge", new RSOPObject("RSOP_SecuritySettingNumeric","KeyName","MaximumPasswordAge",new List<String>(){"Setting"})},
            {"MinimumPasswordAge", new RSOPObject("RSOP_SecuritySettingNumeric","KeyName","MinimumPasswordAge",new List<String>(){"Setting"})},
            {"ResetLockoutCount", new RSOPObject("RSOP_SecuritySettingNumeric","KeyName","ResetLockoutCount",new List<String>(){"Setting"})},
            {"LockoutBadCount", new RSOPObject("RSOP_SecuritySettingNumeric","KeyName","LockoutBadCount",new List<String>(){"Setting"})},
            {"PasswordHistorySize", new RSOPObject("RSOP_SecuritySettingNumeric","KeyName","PasswordHistorySize",new List<String>(){"Setting"})},
            {"MinimumPasswordLength", new RSOPObject("RSOP_SecuritySettingNumeric","KeyName","MinimumPasswordLength",new List<String>(){"Setting"})},
            {"AuditLogonEvents", new RSOPObject("RSOP_AuditPolicy","Category","AuditLogonEvents",new List<String>(){"Success","Failure"})},
            {"AuditAccountLogon", new RSOPObject("RSOP_AuditPolicy","Category","AuditAccountLogon",new List<String>(){"Success","Failure"})},
            {"REG_LogonNotice", new RSOPObject("RSOP_RegistryValue","Path","MACHINE\\\\Software\\\\Microsoft\\\\Windows\\\\CurrentVersion\\\\Policies\\\\System\\\\LegalNoticeText",new List<String>(){"Data"})}
        };

        /*
        
            Abstracted GPO querying functions
        
        */

        public static RSOPObject QueryRSOPPolicyByName(String Shortname) {

            // Get base RSOP object to populate
            RSOPObject MatchingPolicy = WMIMappings[Shortname];

            try { 

                // Get matching objects
                MatchingPolicy.Populate(
                    Environments.API.WMI.QueryRSOPByType(
                        MatchingPolicy.ClassName,
                        MatchingPolicy.IDField,
                        MatchingPolicy.ID
                    )
                );

            } catch (Exception e) {

                System.Console.WriteLine(String.Format("Could not query for '{0}': {1}", Shortname, e.ToString()));

            }

            return MatchingPolicy;

        }

        /*
        
            RSOP base functions
        
        */
        public static ManagementObjectCollection QueryRSOPByType(String ClassName, String IDField, String ID) => QueryRSOP(
            String.Format(
                "Select * FROM {0} WHERE {1}='{2}' AND Precedence=1",
                ClassName,
                IDField,
                ID
            )
        );

        // Query RSOP specifically
        public static ManagementObjectCollection QueryRSOP(String QUERY_STRING) {
            
            ObjectQuery query_object = new ObjectQuery(QUERY_STRING);
            ManagementScope scope = new ManagementScope("\\\\localhost\\root\\rsop\\Computer");

            return SearchWMI(scope, query_object);

        }

        // Arbitrary WMI query
        public static ManagementObjectCollection SearchWMI(ManagementScope ROOT_SCOPE, ObjectQuery QUERY_OBJECT) {

            try {

                ManagementObjectSearcher searcher = new ManagementObjectSearcher(ROOT_SCOPE, QUERY_OBJECT);

                ManagementObjectCollection collection = searcher.Get();

                return collection;

            } catch (Exception e) {

                System.Console.WriteLine(String.Format("Couldn't search WMI at '{0}' for '{1}': [{2}]", ROOT_SCOPE.ToString(), QUERY_OBJECT.ToString(), e.ToString()));
                return null;

            }

        }

    }

}