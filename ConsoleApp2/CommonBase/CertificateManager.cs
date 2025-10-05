using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace ConsoleApp2.Framework
{
    public class CertificateManager
    {
        public static bool IsExistCert(string thumbprint)
        {
            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            try
            {
                store.Open(OpenFlags.ReadOnly);
                var certs = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);
                return certs != null && certs.Count > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                store.Close();
            }
        }

        public static X509Certificate2 GetCertificate(string subjectName)
        {
            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            try
            {
                store.Open(OpenFlags.ReadOnly);
                var certs = store.Certificates.Find(X509FindType.FindBySubjectName, subjectName.Replace("CN=", string.Empty), false);
                if (certs != null && certs.Count > 0)
                {
                    return certs[0];
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                store.Close();
            }
            return null;
        }

        public static X509Certificate2 InstallCertificate(string certName, string certCode)
        {
            try
            {
                string path = @"C:\Users\v-arulmani\source\repos\ConsoleApp2\ConsoleApp2\TenantsCerts\";
                string cert = path + certName + ".pfx";
                X509Certificate2 certificate = new X509Certificate2(cert, certCode);
                if (IsExistCert(certificate.Thumbprint))
                {
                    RemoveCertificateByIssuer(certificate.Issuer);
                }
                using (X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser))
                {
                    store.Open(OpenFlags.ReadWrite);
                    store.Add(certificate);
                    store.Close();
                }
                if (!IsExistCert(certificate.Thumbprint))
                {
                    throw new Exception($"Could not install the certificate {certificate.Subject}");
                }
                return certificate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void RemoveCertificateByIssuer(string issuerName)
        {
            try
            {
                X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadWrite);
                X509Certificate2Collection certificates = store.Certificates.Find(X509FindType.FindByIssuerName, issuerName.Replace("CN=", string.Empty), false);
                if (certificates.Count > 0)
                {
                    foreach (X509Certificate2 cert in certificates)
                    {
                        store.Remove(cert);
                    }
                }
                store.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
