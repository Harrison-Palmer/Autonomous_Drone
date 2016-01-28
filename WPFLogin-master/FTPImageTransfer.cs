using System;
using System.Net;
using System.IO;

namespace WpfApp1
{
    class FTPImageTransfer
    {
        // Constructor, assigns address, 
        // login, and password to a value.
        public FTPImageTransfer(string address, string login, string password)
        {
            Address = address;
            Login = login;
            Password = password;
        }

        // Establishes connection to server, the uploads the specified file.
        public void Upload(String path, String name)
        {
            using (WebClient webClient = new WebClient())
            {
                webClient.Credentials = new NetworkCredential(Login, Password);
                byte[] b = webClient.UploadFile(Address + "//" + name, "STOR", path);
            }
        }

        // Getters and Setters.
        public string Address { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}

