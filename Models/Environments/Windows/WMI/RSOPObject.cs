using System.Collections.Generic;
using System;
using System.Management;

namespace FlagDaemon.Models.Environments.Windows.WMI
{
    public class RSOPObject
    {
        public String ClassName;
        public String Path;
        // Can be alias for KeyName, Category, etc
        public string ID;
        public string IDField;
        // Can include Data, Success, Failure properties, etc
        public List<String> DataFields = new List<String>();
        public Dictionary<String,String> Data = new Dictionary<String,String>();

        public RSOPObject(String ClassName, String IDField, String ID, List<String> ValueFields) {

            // Save for later
            this.ClassName = ClassName;
            this.ID = ID;
            this.IDField = IDField;
            this.DataFields = ValueFields;

            // Initialize value database
            foreach (String ValueField in ValueFields)
                this.Data[ValueField] = null;

        }

        public void Populate(ManagementObjectCollection SearchResults) {

            // Assume only one match (?)
            ManagementObject MatchedObject = null;

            foreach (ManagementObject LoopMatch in SearchResults) {
                MatchedObject = LoopMatch;
                break; // Assume only one match...
            }

            // If there was at least one match
            if (MatchedObject != null) {

                this.Path = MatchedObject.Path.ToString();
                foreach (String DataField in this.DataFields)
                    this.Data[DataField] = (String)(MatchedObject.GetPropertyValue(DataField).ToString());
                
            }

        }

        public override String ToString() {

            List<String> Buffer = new List<String>();

            foreach (KeyValuePair<String,String> Item in this.Data)
                Buffer.Add(String.Format("{0}:'{1}'", Item.Key, (String)(Item.Value)));
            
            return String.Join(",", Buffer);

        }

    }

}