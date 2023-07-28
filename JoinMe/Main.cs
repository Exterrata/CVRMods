using System;
using MelonLoader;
using System.Linq;
using System.Collections.Generic;

[assembly: MelonGame("Alpha Blend Interactive", "ChilloutVR")]
[assembly: MelonInfo(typeof(Koneko.JoinMe), "JoinMe", "1.0.0", "Exterrata")]
[assembly: MelonOptionalDependencies("BTKUILib")]
[assembly: HarmonyDontPatchAll]

namespace Koneko;
public class JoinMe : MelonMod
{
    public static readonly MelonPreferences_Category Category = MelonPreferences.CreateCategory("JoinMe");
    public static readonly MelonPreferences_Entry<bool> Enabled = Category.CreateEntry<bool>("Enabled", true);
    public static readonly MelonPreferences_Entry<bool> RequestWhitelistEnabled = Category.CreateEntry<bool>("Whitelist Request Enabled", false);
    public static readonly MelonPreferences_Entry<string> RequestWhitelist = Category.CreateEntry<string>("Whitelisted Users Requests", "");

    public static List<string> RequestWhitelistList;

    public override void OnInitializeMelon()
    {
        try {
            HarmonyInstance.PatchAll(typeof(Patches));
        } catch(Exception e) { 
            MelonLogger.Error(e);
        }
        if (RegisteredMelons.Any(it => it.Info.Name == "BTKUILib"))
        {
            BTKUISupport.Initialize();
        }
        RequestWhitelistList = RequestWhitelist.Value.Split(',').ToList();
    }

    public static void ChangeRequestWhitelist(string name)
    {
        for (int i = 0; i < RequestWhitelistList.Count; i++)
        {
            if (RequestWhitelistList[i] == name)
            {
                RequestWhitelistList.RemoveAt(i);
                RequestWhitelist.Value = string.Join(",", RequestWhitelistList);
                return;
            }
        }
        RequestWhitelistList.Add(name);

        RequestWhitelist.Value = string.Join(",", RequestWhitelistList);
    }

    public static bool GetRequestWhitelist(string name)
    {
        
        for (int i = 0; i < RequestWhitelistList.Count; i++)
        {
            if (RequestWhitelistList[i] == name)
            {
                return true;
            }
        }
        return false;
    }
}