using System;
using System.Net;
using System.IO;

namespace WpfApp1
{
    class FTPImageTransfer
    {

        public FTPImageTransfer(string address, string login, string password)
        {
            Address = address;
            Login = login;
            Password = password;
        }

        public void Upload(String path, String name)
        {
            using (WebClient webClient = new WebClient())
            {
                webClient.Credentials = new NetworkCredential(Login, Password);
                byte[] b = webClient.UploadFile(Address + "//" + name, "STOR", path);
                //Console.WriteLine(System.Text.Encoding.ASCII.GetString(b));
            }
        }

        public string Address { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

    }
}

