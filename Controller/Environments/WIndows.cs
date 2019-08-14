using System;
using System.Collections.Generic;
using System.IO;

namespace FlagDaemon.Controller.Environments
{
    public class Windows : FlagDaemon.Controller.Environments.Base
    {

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

        public bool FirewallAllows(int PortNumber)
        {
            throw new NotImplementedException();
        }

        public bool FirewallAllows(int PortNumber, List<string> AllowedIP)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, string> GetPolicy(string PolicyName)
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