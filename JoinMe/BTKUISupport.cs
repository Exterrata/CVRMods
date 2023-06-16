using BTKUILib;

namespace Koneko;
internal class BTKUISupport
{
    public static void Initialize()
    {
        var misc = QuickMenuAPI.MiscTabPage;
        var mainCatagory = misc.AddCategory("JoinMe");
        mainCatagory.AddToggle("Enabled", "Enable JoinMe", JoinMe.Enabled.Value).OnValueUpdated += b => JoinMe.Enabled.Value = b;
    }
}