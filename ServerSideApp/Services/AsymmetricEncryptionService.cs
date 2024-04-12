using System.Security.Cryptography;

namespace ServerSideApp.Services;

public class AsymmetricEncryptionService
{
    private string _privateKey;
    private string _publicKey;


    public AsymmetricEncryptionService()
    {
        // Tjekker om key-fil eksisterer
        if (!File.Exists("privKey.pem"))
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                // sætter keys
                _privateKey = rsa.ToXmlString(true);
                _publicKey = rsa.ToXmlString(false);


                // opretter key-filer
                File.WriteAllText("privKey.pem", _privateKey);
                File.WriteAllText("pubKey.pem", _publicKey);
            }
        }
        else
        {
            // henter key-filer og sætter keys
            _privateKey = File.ReadAllText("privKey.pem");
            _publicKey = File.ReadAllText("pubKey.pem");
        }
    }

    public string EncryptData(string txtToEncrypt)
        => Encrypter.Encrypt(txtToEncrypt, _publicKey);
    

    public string DecryptData(string txtToDecrypt)
    {
        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
        {            
            rsa.FromXmlString(_privateKey);

            byte[] dataToDecrypt = rsa.Decrypt(Convert.FromBase64String(txtToDecrypt), true);
            return System.Text.Encoding.UTF8.GetString(dataToDecrypt);
        }
    }
}
