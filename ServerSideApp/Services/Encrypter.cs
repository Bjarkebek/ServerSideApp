using System.Security.Cryptography;

namespace ServerSideApp.Services;

public class Encrypter
{
    public static string Encrypt(string txtToEncrypt, string publicKey)
    {
        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
        {
            rsa.FromXmlString(publicKey);

            byte[] dataToEncrypt = rsa.Encrypt(System.Text.Encoding.UTF8.GetBytes(txtToEncrypt), true);
            return Convert.ToBase64String(dataToEncrypt);
        }
    }
}