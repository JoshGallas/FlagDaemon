using System;
using System.Collections.Generic;
using System.Management;

namespace FlagDaemon.Controller.Environments.API
{
    public static class WMI
    {

        // Map RSOP WMI object to setting: Shortname -> ClassName,KeyName,ValueField
        public static Dictionary<String,String[]> WMIMappings = new Dictionary<String,String[]> {
            {"PasswordComplexity", new String[] {"RSOP_SecuritySettingBoolean","PasswordComplexity","Setting"}},
            {"ClearTextPassword", new String[] {"RSOP_SecuritySettingBoolean","ClearTextPassword","Setting"}},
            {"LockoutDuration", new String[] {"RSOP_SecuritySettingNumeric","LockoutDuration","Setting"}},
            {"MaximumPasswordAge", new String[] {"RSOP_SecuritySettingNumeric","MaximumPasswordAge","Setting"}},
            {"MinimumPasswordAge", new String[] {"RSOP_SecuritySettingNumeric","MinimumPasswordAge","Setting"}},
            {"ResetLockoutCount", new String[] {"RSOP_SecuritySettingNumeric","ResetLockoutCount","Setting"}},
            {"LockoutBadCount", new String[] {"RSOP_SecuritySettingNumeric","LockoutBadCount","Setting"}},
            {"PasswordHistorySize", new String[] {"RSOP_SecuritySettingNumeric","PasswordHistorySize","Setting"}},
            {"MinimumPasswordLength", new String[] {"RSOP_SecuritySettingNumeric","MinimumPasswordLength","Setting"}}
        };

        /*
        
            Abstracted GPO querying functions
        
        */

        public static String QueryRSOPPolicyByName(String Shortname) {

            try { 

                // Get matching objects
                ManagementObjectCollection results = Environments.API.WMI.QueryRSOPByType(
                    WMIMappings[Shortname][0],
                    WMIMappings[Shortname][1]
                );

                // Create (and then concatenate) value of objects (might not be necessary?)
                List<String> SettingList = new List<String>();

                foreach (ManagementObject m in results) {
                    SettingList.Add(
                        m.GetPropertyValue(
                            WMIMappings[Shortname][2]
                        ).ToString()
                    );
                }

                return String.Join(",", SettingList);

            } catch (Exception e) {

                System.Console.WriteLine(String.Format("Could not query for '{0}': {1}", Shortname, e.ToString()));
                return null;

            }

        }

        /*
        
            RSOP base functions
        
        */
        public static ManagementObjectCollection QueryRSOPByType(String Type, String Name) => QueryRSOP(
            String.Format(
                "Select * FROM {0} WHERE KeyName='{1}' AND Precedence=1",
                Type,
                Name
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