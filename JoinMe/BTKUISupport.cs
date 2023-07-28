using BTKUILib;

namespace Koneko;
internal class BTKUISupport
{
    public static void Initialize()
    {
        var misc = QuickMenuAPI.MiscTabPage;
        var mainCatagory = misc.AddCategory("JoinMe");
        mainCatagory.AddToggle("Enabled", "Enable JoinMe", JoinMe.Enabled.Value).OnValueUpdated += b => JoinMe.Enabled.Value = b;
        mainCatagory.AddToggle("Enable Request Whitelist", "Enable invite request whitelist", JoinMe.RequestWhitelistEnabled.Value).OnValueUpdated += b => JoinMe.RequestWhitelistEnabled.Value = b;
        var player = QuickMenuAPI.PlayerSelectPage;
        var playerCatagory = player.AddCategory("JoinMe");
        QuickMenuAPI.OnPlayerSelected += (sender, e) =>
        {
            playerCatagory.ClearChildren();
            playerCatagory.AddToggle("Whitelist Requests", "Whitelist invite requests from user", JoinMe.GetRequestWhitelist(QuickMenuAPI.SelectedPlayerName)).OnValueUpdated += b =>
            {
                JoinMe.ChangeRequestWhitelist(QuickMenuAPI.SelectedPlayerName);
            };
        };
    }
}