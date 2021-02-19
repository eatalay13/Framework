using System;
using System.Linq;
using System.Threading.Tasks;

namespace MvcWeb.Framework.Themes
{
    public partial class ThemeContext : IThemeContext
    {
        #region Properties

        private string WorkingThemeName { get; set; } = "Default";

        #endregion
        public virtual async Task<string> GetWorkingThemeNameAsync()
        {

            return WorkingThemeName;
        }

        public virtual async Task SetWorkingThemeNameAsync(string workingThemeName)
        {
            WorkingThemeName = workingThemeName;
        }
    }
}