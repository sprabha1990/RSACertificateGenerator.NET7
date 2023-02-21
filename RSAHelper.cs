namespace SelfCertificateGenerator;

public record CertificateModel(X509Certificate2 Certificate, string PublicKeyPEM, string PrivateKeyPEM, string PublicKeyPFX = "");

internal class RSAHelper
{
    /// <summary>
    ///     Create a random RSA certificate for asymentric encryption
    /// </summary>
    /// <param name="subjectName">Subject name for the certificate. Ex, "cn=test"</param>
    /// <param name="hashAlgorithmName">Algorithm mode for creating hash keys</param>
    /// <param name="signaturePadding">Padding mode for the certificate signature</param>
    /// <returns>Certificate model</returns>
    public CertificateModel CreateRSACertificate(string subjectName
        , HashAlgorithmName hashAlgorithmName
        , RSASignaturePadding signaturePadding)
    {
        using var rsa = RSA.Create();
        var certRequest = new CertificateRequest(subjectName, rsa, hashAlgorithmName, signaturePadding);
        var certificate = certRequest.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddDays(1));

        var privateKey = Convert.ToBase64String(rsa.ExportRSAPrivateKey(), Base64FormattingOptions.InsertLineBreaks);
        var publicKey = certificate.ExportCertificatePem();
        var publicKeyPfx = Convert.ToBase64String(certificate.Export(X509ContentType.Pfx), Base64FormattingOptions.InsertLineBreaks);

        return new CertificateModel(certificate, publicKey, privateKey, publicKeyPfx);
    }
}