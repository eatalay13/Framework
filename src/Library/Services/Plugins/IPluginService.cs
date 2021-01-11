using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Plugins
{
    public partial interface IPluginService
    {
        Task<IList<PluginDescriptor>> GetPluginDescriptorsAsync<TPlugin>(LoadPluginsMode loadMode = LoadPluginsMode.InstalledOnly,
       string group = null, string dependsOnSystemName = "", string friendlyName = null, string author = null) where TPlugin : class, IPlugin;

        Task<PluginDescriptor> GetPluginDescriptorBySystemNameAsync<TPlugin>(string systemName,
            LoadPluginsMode loadMode = LoadPluginsMode.InstalledOnly,
            string group = null) where TPlugin : class, IPlugin;

        Task<IList<TPlugin>> GetPluginsAsync<TPlugin>(LoadPluginsMode loadMode = LoadPluginsMode.InstalledOnly,
            string group = null) where TPlugin : class, IPlugin;

        IPlugin FindPluginByTypeInAssembly(Type typeInAssembly);

        Task<string> GetPluginLogoUrlAsync(PluginDescriptor pluginDescriptor);

        Task PreparePluginToInstallAsync(string systemName, bool checkDependencies = true);

        Task PreparePluginToUninstallAsync(string systemName);

        Task PreparePluginToDeleteAsync(string systemName);

        void ResetChanges();

        void ClearInstalledPluginsList();
        Task InstallPluginsAsync();

        Task UninstallPluginsAsync();

        Task DeletePluginsAsync();

        Task UpdatePluginsAsync();

        bool IsRestartRequired();

        IList<string> GetIncompatiblePlugins();

        IList<PluginLoadedAssemblyInfo> GetAssemblyCollisions();
    }
}