using System;
using MelonLoader;

[assembly: MelonGame("Alpha Blend Interactive", "ChilloutVR")]
[assembly: MelonInfo(typeof(Koneko.ControllerFreeze), "ControllerFreeze", "1.1.0", "Exterrata")]
[assembly: HarmonyDontPatchAll]

namespace Koneko;
public class ControllerFreeze : MelonMod
{
    public override void OnInitializeMelon()
    {
        Patches.controllers = new Dictionary<int, Patches.Controller>();
        try {
            HarmonyInstance.PatchAll(typeof(Patches));
        } catch(Exception e) { 
            MelonLogger.Error(e);
        }
    }
}