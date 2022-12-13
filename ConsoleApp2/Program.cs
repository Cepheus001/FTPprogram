using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.IO;
using System.Security;
using System.Security.Permissions;

namespace ConsoleApp2
{
    internal class Program
    {
        class argumentdata
        {
            public static string ConfirmIP = string.Empty;
            public static string ConfirmDir = string.Empty;
            public static string destinationDir1 = string.Empty;
            public static string CredentialsRequest = string.Empty;
            public static string storeUri = string.Empty;
            public static string ReqDir = string.Empty;
            public static string pureDir = string.Empty;
            public static string targetdir1 = string.Empty;
        }

        public static void MainMenu()
        {
            Console.WriteLine("Do you want to start connection to FTP? Select an option");
            Console.WriteLine("1 - Connect with FTP");
            Console.WriteLine("2 - Exit Program");
            var menuselect = Console.ReadLine();
            if (menuselect == "1")
            {
                AskFirstQuestion();
            }
            else if (menuselect == "2")
            {
                ExitingCommand();
            }
            else if (menuselect.ToLower() == "exit")
            {
                ExitingCommand();
            }
            else
            {
                Console.WriteLine("Error. Enter your choice again.");
                MainMenu();
            }
        }
        public static void AskFirstQuestion()
        {
            Console.WriteLine("Enter the IP address you wish to connect to.");
            var IPinput = Console.ReadLine();
            argumentdata.ConfirmIP = IPinput;
            Console.WriteLine("Is this the correct IP? (Y/N)");
            var correctIPyn = Console.ReadLine();

            if (correctIPyn.ToLower() == "n")
            {
                AskFirstQuestion();
            }
            else if (correctIPyn.ToLower() == "y")
            {
                useDefaultDir();
            }
            else if (correctIPyn.ToLower() == "exit")
            {
                ExitingCommand();
            }
            else if (correctIPyn.ToLower() == "dir")
            {
                if (argumentdata.ConfirmIP != null)
                {
                    Console.Clear();
                    RequestDirectory();
                }
                else
                {
                    Console.WriteLine("There is no pre-existing IP. Please enter one.");
                    AskFirstQuestion();
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("There was an error in your input. Please start again.");
                AskFirstQuestion();
            }
        }
        public static void ExitingCommand()
        {
            Console.WriteLine("This program will exit now. Are you sure you want to continue?(Y/N)");
            var SureYouWanttoExit = Console.ReadLine();
            if (SureYouWanttoExit == "n")
            {
                Console.WriteLine("Return to menu?");
                var returntomenuconfirm = Console.ReadLine();
                if (returntomenuconfirm.ToLower() == "n")
                {
                    ExitingCommand();
                }
                else if (returntomenuconfirm.ToLower() == "y")
                {
                    MainMenu();
                }
                else
                {
                    Console.WriteLine("Error in input");
                    ExitingCommand();
                }
            }
            else if (SureYouWanttoExit == "y")
            { 
            Console.WriteLine("Quiting in....");
            Console.WriteLine("1");
            Console.WriteLine("2");
            Console.WriteLine("3");
            Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Error in input.");
                ExitingCommand();
            }
        }

        public static void useDefaultDir()
        {
            Console.WriteLine("If you don't know the directory you want, would you like to try the default directory?");
            var choosedefaultftpdir = Console.ReadLine();
            if( choosedefaultftpdir.ToLower() == "n")
            {
                AskSecondQuestion();
            }
            else if ( choosedefaultftpdir.ToLower() == "y")
            {
                var defDir = "/./";
                argumentdata.ConfirmDir = defDir;
                RequestDirectory();
            }
            else
            {
                Console.WriteLine("Error. Please try re-entering the required information");
                useDefaultDir();
            }
        }
        public static void AskSecondQuestion()
        {
            Console.WriteLine("Now, enter the directory of the FTP connection.");
            var DirInput = Console.ReadLine();
            argumentdata.ConfirmDir = DirInput;
            Console.WriteLine("Is this the correct directory? (Y/N)");
            var correctDir = Console.ReadLine();

            if (correctDir.ToLower() == "n")
            {
                AskSecondQuestion();
            }
            else if (correctDir.ToLower() == "y")
            {
                RequestDirectory();
            }
            else
            {
                Console.Clear();
                Console.WriteLine("There was an error in your input. Please start again.");
                AskFirstQuestion();
            }
        }

        public static void RequestDirectory() //string IPinput, string DirInput
        {
            
                // Get the object used to communicate with the server.
                //var IpLocal = "http://192.168.2.28/";
                UriBuilder builder = new UriBuilder("ftp://" + argumentdata.ConfirmIP + argumentdata.ConfirmDir);
                Uri uri = builder.Uri;
                // Console.WriteLine(uri.ToString());
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

                // This example assumes the FTP site uses anonymous logon.
                request.Credentials = new NetworkCredential("anonymous", "anonymous@" + argumentdata.ConfirmIP);
            try
            {
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                Console.WriteLine(reader.ReadToEnd());

                Console.WriteLine($"Directory List Complete, status {response.StatusDescription}");

                Console.WriteLine("This is the connection you have made: " + uri.ToString());
                reader.Close();
                response.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Do you want to go back to the beginning?(Y/N)");
                var backtomenu = Console.ReadLine();
                if (backtomenu.ToLower() == "y")
                {
                    Console.Clear();
                    AskFirstQuestion();
                }
                else if (backtomenu.ToLower() == "n")
                {
                    Console.Clear();
                    RequestDirectory();
                }
                else if (backtomenu.ToLower() == "dir")
                {
                    Console.Clear();
                    RequestDirectory();
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("There was an error in your input. Try again.");
                    MainMenu();
                }
            }
            Console.WriteLine("Do you want to choose another directory?(Y/N)");
            var chooseOtherDir = Console.ReadLine();
            if (chooseOtherDir.ToLower() == "n")
            {
                Console.WriteLine("Do you want to download from this directory?(Y/N)");
                var chooseFile = Console.ReadLine();
                if (chooseFile.ToLower() == "n")
                {
                    RequestDirectory();
                }
                else if (chooseFile.ToLower() == "y")
                {
                    JumpTodownload();
                }
                else if (chooseFile.ToLower() =="dir")
                {
                    Console.Clear();
                    RequestDirectory();
                }
                else
                {
                    Console.WriteLine("Invalid Input, Try again");
                    Console.Clear();
                    AskFirstQuestion();
                }

            }
            else if (chooseOtherDir.ToLower() == "y")
            {
                AskSecondQuestion();
            }
            else if (chooseOtherDir.ToLower() == "dir")
            {
                Console.Clear();
                RequestDirectory();
            }
            else
            {
                Console.WriteLine("Invalid Input, Try again");
                Console.Clear();
                AskFirstQuestion();
            }
        }

        public static void JumpTodownload()
        {
            Console.WriteLine("Now, enter the destination directory.");
            var destinationDir = Console.ReadLine();
            argumentdata.pureDir = destinationDir;
            argumentdata.destinationDir1 = "ftp://" + "anonymous@" + "192.168.2.85" + destinationDir;
            Console.WriteLine("ftp://" + argumentdata.ConfirmIP + argumentdata.ConfirmDir);
            Console.WriteLine("This is the current directory above. ^");
            if (argumentdata.pureDir.ToLower() == "dir")
            {
                Console.Clear();
                RequestDirectory();
            }
            else
            {
                ContinueToDownload();
            }

        }
        public static void ContinueToDownload()
        { 
            Console.WriteLine(argumentdata.destinationDir1);
            Console.WriteLine("This is the current destination directory above. ^");
            Console.WriteLine("Specify the requesting directory? ^");
            var specifiedDir = Console.ReadLine();
            argumentdata.ReqDir = specifiedDir;
            Console.WriteLine("Now, write the target for the directory");
            var dirtarget = Console.ReadLine();
            argumentdata.targetdir1 = dirtarget;
            Console.WriteLine("Awesome! This is all the data we need!");
            Console.WriteLine("Do you want to continue with these current settings? (Y/N)");
            var finaldownloadconfirm = Console.ReadLine();
            if (finaldownloadconfirm == "n")
            {
                Console.Clear();
                RequestDirectory();
            }
            else if (finaldownloadconfirm.ToLower() == "y")
            {
                DownloadFile();
            }
            else if (finaldownloadconfirm.ToLower() == "Exit")
            {
                ExitingCommand();
            }
            else if (finaldownloadconfirm.ToLower() == "menu")
            {
                Console.Clear();
                MainMenu();
            }
            else if (finaldownloadconfirm.ToLower() == "dir")
            {
                Console.Clear();
                RequestDirectory();
            }
            else
            {
                Console.WriteLine("Input error. Try again.");
                JumpTodownload();
            }
        }

        public static string DownloadFile()
        {
            FtpWebRequest req;
            bool usePassive = true;
            bool BinaryIsTrue = true;
            bool keepingalive = true;
            int port = 21;
            UriBuilder newbuilder = new UriBuilder("ftp", "anonymous@" + argumentdata.ConfirmIP, port); //argumentdata.ReqDir
            Uri uriTwo = newbuilder.Uri;
            Console.WriteLine(uriTwo.ToString());
            string ResponseDescription = "";
            string PureFileName = new FileInfo(argumentdata.targetdir1).Name;  // argumentdata.ReqDir
            string DownloadedFilePath = PureFileName;
            Console.WriteLine(PureFileName);
            string downloadUrl = String.Format("{0}/{1}", uriTwo, PureFileName); //"{0}/{1}"
            req = (FtpWebRequest)FtpWebRequest.Create(new Uri(downloadUrl));
            req.Method = WebRequestMethods.Ftp.DownloadFile;
            req.Credentials = new NetworkCredential("anonymous", "anonymous@" + argumentdata.ConfirmIP);
            req.KeepAlive = keepingalive;
            req.UseBinary = BinaryIsTrue;
            req.Proxy = null;
            req.UsePassive = usePassive;
            try
            {
                //fs2.Demand();
                FtpWebResponse response = (FtpWebResponse)req.GetResponse();
                Stream respstream = response.GetResponseStream();
                //StreamReader reader = new StreamReader(respstream, Encoding.UTF8);
                byte[] buffer = new byte[2048];
                FileStream fs = new FileStream(DownloadedFilePath, FileMode.Create);
                int ReadCount = respstream.Read(buffer, 0, buffer.Length);
                while (ReadCount > 0)
                {
                    fs.Write(buffer, 0, ReadCount);
                    ReadCount = respstream.Read(buffer, 0, buffer.Length);
                }
                ResponseDescription = response.StatusDescription;
                Console.WriteLine(ResponseDescription);
                fs.Close();
                respstream.Close();
            }
            catch (WebException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("First Path (PureFileName) --> " + PureFileName.ToString());
                Console.WriteLine("Second Path (downloadUrl) --> " + downloadUrl);
                //Console.WriteLine(ResponseDescription);
                Console.WriteLine(((FtpWebResponse)e.Response).StatusDescription);

                Console.WriteLine("Do you want to go back to the beginning?");
                var backtomenu = Console.ReadLine();
                if (backtomenu.ToLower() == "y")
                {
                    Console.Clear();
                    AskFirstQuestion();
                }
                else if (backtomenu.ToLower() == "n")
                {
                    Console.Clear();
                    RequestDirectory();
                }
                else if (backtomenu.ToLower() == "Exit")
                {
                    Console.Clear();
                    ExitingCommand();
                }
                else
                {
                    Console.WriteLine("Error in input");
                    RequestDirectory();
                }
            }
            return ResponseDescription;

        }

        public static void Main(string[] args)
        {
            MainMenu();
        }
    }
}
