using System.Threading.Tasks;

namespace MvcWeb.Framework.Themes
{
    public interface IThemeContext
    {
        Task<string> GetWorkingThemeNameAsync();

        Task SetWorkingThemeNameAsync(string workingThemeName);
    }
}
