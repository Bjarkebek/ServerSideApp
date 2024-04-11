using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;

namespace ServerSideApp.Services;

public enum returnType
{
    None,
    String,
    ByteArrray
}

public class HashingService
{

    
    public dynamic MD5Hashing_DO_NOT_USE(string txtToHash, returnType outpputType)
    {
        MD5 md5 = MD5.Create();

        // converting to and hashing bytearray
        byte[] inputBytes = Encoding.ASCII.GetBytes(txtToHash);
        byte[] hashedValue = md5.ComputeHash(inputBytes);

        if (outpputType == returnType.String)
        {
            return Convert.ToBase64String(hashedValue);
        }
        else if (outpputType == returnType.ByteArrray)
        {
            return hashedValue;
        }
        else
            return null;
    }



    public dynamic SHAHashing(string txtToHash)
    {
        SHA256 sha = SHA256.Create();

        // converting to and hashing bytearray
        byte[] inputBytes = Encoding.ASCII.GetBytes(txtToHash);
        byte[] hashedValue = sha.ComputeHash(inputBytes);

        return Convert.ToBase64String(hashedValue);



    }

    public string HMACHashing(string txtToHash)
    {
        // opretter nøgle
        byte[] key = Encoding.ASCII.GetBytes("NØGLE_NØGLE_NØGLE");
        
        // laver ny instans af hmac og tilføjer key
        HMACSHA256 hmac = new HMACSHA256();
        hmac.Key = key;
        
        // converting to and hashing bytearray
        byte[] inputBytes = Encoding.ASCII.GetBytes(txtToHash);
        byte[] hashedValue = hmac.ComputeHash(inputBytes);

        return Convert.ToBase64String(hashedValue);
    }

    public string PBKDF2Hashing(string txtToHash)
    {
        // opretter salt, algorytme m.m.
        byte[] salt = Encoding.ASCII.GetBytes("SALT_SALT_SALT");
        var hashAlgorythm = new HashAlgorithmName("SHA256");
        int itiration = 10;
        int outputLength = 32;


        // converting to and hashing bytearray
        byte[] inputBytes = Encoding.ASCII.GetBytes(txtToHash);
        byte[] hashedValue = Rfc2898DeriveBytes.Pbkdf2(inputBytes, salt, itiration, hashAlgorythm, outputLength);

        return Convert.ToBase64String(hashedValue);
    }

    public dynamic BcryptHashing(string txtToHash, returnType outpputType)
    {
        // Bruges til at gemme hash i db
        if (outpputType == returnType.String)
        {
            return BCrypt.Net.BCrypt.HashPassword(txtToHash);
        }
        else if (outpputType == returnType.ByteArrray)
        {
            byte[] inputBytes = Encoding.ASCII.GetBytes(txtToHash);
            return inputBytes;
        }
        else
            return null;
        
    }

    public bool BcryptVerify(string txtToHash, string hashedValue)
    {
        // bruges til at verificere nyt hashed brugerinput mod gemt hash i db
        return BCrypt.Net.BCrypt.Verify(txtToHash, hashedValue);
    }
}
