using System;
using System.Net;

class FTPImageTransfer
{
	//Assigns a value to the Address, Login, and Password variables
    public FTPImageTransfer(string address, string login, string password)
    {
        Address = address;
        Login = login;
        Password = password;
    }

	//Establishes a connection to the server and uploads the specified file
    public void Upload(String filePath, String name)
    {
        try {
            using (WebClient webClient = new WebClient())
            {
                webClient.Credentials = new NetworkCredential(Login, Password);
                byte[] b = webClient.UploadFile(Address + "//" + name, "STOR", filePath);
            }
        }
        catch(Exception e)
        {
            
        }
    }

	//Establishes a connection to the server and downloads the requested file
    public void Download(string fileName, String saveName)
    {
        using (WebClient webClient = new WebClient())
        {
            webClient.Credentials = new NetworkCredential(Login, Password);
            webClient.DownloadFile(Address + "/" + fileName, saveName);
        }
    }

    public string Address { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
}

