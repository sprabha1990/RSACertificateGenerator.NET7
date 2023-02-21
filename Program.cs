using System.Diagnostics;
using System.Reflection;

RSAHelper rsaHelperInstance = new RSAHelper();

Console.Write("Enter the subject name of the certificate : ");
var subjectName = $"CN={Console.ReadLine()?.ToUpper()}";

CertificateModel certificate = rsaHelperInstance.CreateRSACertificate(subjectName: subjectName
    , hashAlgorithmName: HashAlgorithmName.SHA256
    , signaturePadding: RSASignaturePadding.Pkcs1);

Console.WriteLine("Certificate is ready !!");
Console.Write("Do you want to save it locally? Press y or n : ");

char choice = Console.ReadKey().KeyChar;

if (choice == 'y')
{
    if (!Directory.Exists("Certificates"))
        Directory.CreateDirectory("Certificates");

    File.WriteAllText("Certificates/certificate.pem", certificate.PublicKeyPEM);
    File.WriteAllText("Certificates/certificate.private.pem", certificate.PrivateKeyPEM);
    File.WriteAllText("Certificates/certificate.pfx", certificate.PublicKeyPFX);

    Console.WriteLine();
    string filesLocation = Path.Combine(Directory.GetParent(Assembly.GetExecutingAssembly().Location)?.FullName ?? "", "Certificates");
    Console.WriteLine($"Files are saved in {filesLocation}");

    Process.Start(fileName: "explorer.exe", arguments: filesLocation);
}
