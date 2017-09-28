using System.ServiceModel;

namespace WCF
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        string CreateSite(string applicationName, string applicationPath, int portNumber, string applicationPoolName = null);

        [OperationContract]
        string CreateNewApplicationPool(string applicationPoolName);

        [OperationContract]
        string AddAssemblyToWebsite(string applicationName, string assemblyName, byte[] assemblyByteArray);

        [OperationContract]
        string EditSiteConfig(string applicationName, string xPath, string attribute, string value);
    }
}