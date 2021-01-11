using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domain.Cms;
using Services.Plugins;

namespace Services.Cms
{
    /// <summary>
    /// Represents a widget plugin manager implementation
    /// </summary>
    public partial class WidgetPluginManager : PluginManager<IWidgetPlugin>, IWidgetPluginManager
    {
        #region Fields

        private readonly WidgetSettings _widgetSettings;

        #endregion

        #region Ctor

        public WidgetPluginManager(IPluginService pluginService,
            WidgetSettings widgetSettings) : base(pluginService)
        {
            _widgetSettings = widgetSettings;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Load active widgets
        /// </summary>
        /// <param name="customer">Filter by customer; pass null to load all plugins</param>
        /// <param name="storeId">Filter by store; pass 0 to load all plugins</param>
        /// <param name="widgetZone">Widget zone; pass null to load all plugins</param>
        /// <returns>List of active widget</returns>
        public virtual async Task<IList<IWidgetPlugin>> LoadActivePluginsAsync(string widgetZone = null)
        {
            var widgets = await LoadActivePluginsAsync(_widgetSettings.ActiveWidgetSystemNames);

            //filter by widget zone
            if (!string.IsNullOrEmpty(widgetZone))
                widgets = await widgets.WhereAwait(async widget =>
                    (await widget.GetWidgetZonesAsync()).Contains(widgetZone, StringComparer.InvariantCultureIgnoreCase)).ToListAsync();

            return widgets;
        }

        /// <summary>
        /// Check whether the passed widget is active
        /// </summary>
        /// <param name="widget">Widget to check</param>
        /// <returns>Result</returns>
        public virtual bool IsPluginActive(IWidgetPlugin widget)
        {
            return IsPluginActive(widget, _widgetSettings.ActiveWidgetSystemNames);
        }

        /// <summary>
        /// Check whether the widget with the passed system name is active
        /// </summary>
        /// <param name="systemName">System name of widget to check</param>
        /// <param name="customer">Filter by customer; pass null to load all plugins</param>
        /// <param name="storeId">Filter by store; pass 0 to load all plugins</param>
        /// <returns>Result</returns>
        public virtual async Task<bool> IsPluginActiveAsync(string systemName)
        {
            var widget = await LoadPluginBySystemNameAsync(systemName);

            return IsPluginActive(widget);
        }

        #endregion
    }
}