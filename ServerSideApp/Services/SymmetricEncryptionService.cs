using Microsoft.AspNetCore.DataProtection;

namespace ServerSideApp.Services;

public class SymmetricEncryptionService
{
    private readonly IDataProtector _protector;
    public SymmetricEncryptionService(IDataProtectionProvider protector)
    {
        _protector = protector.CreateProtector("KEY_KEY_KEY_KEY");
    }

    public string EncryptData(string txtToEncrypt)
        => _protector.Protect(txtToEncrypt);

    public string DecryptData(string txtToDecrypt)
        => _protector.Unprotect(txtToDecrypt);

}
