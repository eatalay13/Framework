using System.Collections.Generic;
using System.Threading.Tasks;
using Services.Plugins;

namespace Services.Cms
{
    /// <summary>
    /// Represents a widget plugin manager
    /// </summary>
    public partial interface IWidgetPluginManager : IPluginManager<IWidgetPlugin>
    {
        /// <summary>
        /// Load active widgets
        /// </summary>
        /// <param name="customer">Filter by customer; pass null to load all plugins</param>
        /// <param name="storeId">Filter by store; pass 0 to load all plugins</param>
        /// <param name="widgetZone">Widget zone; pass null to load all plugins</param>
        /// <returns>List of active widget</returns>
        Task<IList<IWidgetPlugin>> LoadActivePluginsAsync(string widgetZone = null);

        /// <summary>
        /// Check whether the passed widget is active
        /// </summary>
        /// <param name="widget">Widget to check</param>
        /// <returns>Result</returns>
        bool IsPluginActive(IWidgetPlugin widget);

        /// <summary>
        /// Check whether the widget with the passed system name is active
        /// </summary>
        /// <param name="systemName">System name of widget to check</param>
        /// <param name="customer">Filter by customer; pass null to load all plugins</param>
        /// <param name="storeId">Filter by store; pass 0 to load all plugins</param>
        /// <returns>Result</returns>
        Task<bool> IsPluginActiveAsync(string systemName);
    }
}