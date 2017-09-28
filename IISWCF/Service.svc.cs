using System;
using System.Linq;
using System.IO;
using Microsoft.Web.Administration;

namespace WCF
{
    public class Service : IService
    {
        #region Constansts

        private const string IIS_CONFIG_LOCAL_PATH = @"C:\Windows\System32\inetsrv\config\applicationHost.config";

        #endregion

        #region Public Functions

        public string CreateSite(string applicationName, string applicationPath, int portNumber, string applicationPoolName = null)
        {

            try
            {
                using (ServerManager serverManager = new ServerManager(IIS_CONFIG_LOCAL_PATH))
                {
                    Site newSite = serverManager.Sites.Add(applicationName, applicationPath, portNumber);
                    newSite.ServerAutoStart = true;

                    if (!string.IsNullOrEmpty(applicationPoolName))
                    {
                        newSite.Applications[0].ApplicationPoolName = applicationPoolName;
                        ApplicationPool applicationPool = serverManager.ApplicationPools[applicationPoolName];
                        serverManager.CommitChanges();
                        applicationPool.Recycle();
                    }
                    else
                    {
                        serverManager.CommitChanges();
                    }
                }

                return "Fuck yeah! It works baby";
            }
            catch (Exception exc)
            {
                return exc.Message;
            }
        }

        public string CreateNewApplicationPool(string applicationPoolName)
        {
            try
            {
                using (ServerManager serverManager = new ServerManager(IIS_CONFIG_LOCAL_PATH))
                {
                    ApplicationPool applicationPool = serverManager.ApplicationPools.Add(applicationPoolName);
                    serverManager.CommitChanges();
                    applicationPool.Recycle();

                }

                return "Fuck yeah! It works baby";
            }
            catch (Exception exc)
            {
                return exc.Message;
            }
        }

        public string AddAssemblyToWebsite(string applicationName, string assemblyName, byte[] assemblyByteArray)
        {
            try
            {

                string applicationAssembliesLocalPath = getApplicationLocalPath(applicationName);

                //using (var fileStream = new FileStream(Path.Combine(applicationAssembliesLocalPath, assemblyName), FileMode.Create, FileAccess.Write))
                //{
                //    fileStream.Write(assemblyByteArray, 0, assemblyByteArray.Length);
                //}

                return "Fuck yeah! It works baby";
            }
            catch (Exception exc)
            {
                return exc.Message;
            }

        }

        public string EditSiteConfig(string applicationName, string xPath, string attribute, string value)
        {
            try
            {
                using (ServerManager serverManager = new ServerManager(IIS_CONFIG_LOCAL_PATH))
                {
                    Configuration webConfiguration = serverManager.GetWebConfiguration(applicationName);
                    ConfigurationSection configurationSection = webConfiguration.GetSection(xPath);
                    ConfigurationAttribute configurationAttribute = configurationSection.GetAttribute(attribute);

                    configurationAttribute.Value = value;
                    serverManager.CommitChanges();
                }

                return "Fuck yeah! It works baby";
            }
            catch (Exception exc)
            {
                return exc.Message;
            }
        }

        #endregion

        #region Private Functions

        private string getApplicationLocalPath(string applicationName)
        {
            string windowsDrive = Path.GetPathRoot(Environment.SystemDirectory);

            using (ServerManager serverManager = new ServerManager(IIS_CONFIG_LOCAL_PATH))
            {
                Site site = serverManager.Sites.Where(s => s.Name.Equals(applicationName)).Single();
                if (site == null)
                {
                    throw new Exception("Application not found");
                }

                Application applicationRoot = site.Applications.Where(a => a.Path == "/").Single();
                VirtualDirectory virtualRoot = applicationRoot.VirtualDirectories.Where(v => v.Path == "/").Single();

                return Path.Combine(virtualRoot.PhysicalPath.Replace("%SystemDrive%", windowsDrive), "bin");
            }
        }

        #endregion

    }
}
