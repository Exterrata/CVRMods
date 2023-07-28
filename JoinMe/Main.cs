using System;
using MelonLoader;
using HarmonyLib;
using System.Linq;
using System.Collections;
using ABI_RC.Core.InteractionSystem;
using ABI_RC.Core.Networking.API.Responses;
using ABI_RC.Core.Networking.API;
using UnityEngine;
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
    }
}