using ABI_RC.Core.Player;
using MelonLoader;
using UnityEngine;

[assembly: MelonGame("Alpha Blend Interactive", "ChilloutVR")]
[assembly: MelonInfo(typeof(Koneko.ControllerFreeze), "ControllerFreeze", "1.2.0", "Exterrata")]
[assembly: HarmonyDontPatchAll]

namespace Koneko;
public class ControllerFreeze : MelonMod
{
    public override void OnSceneWasInitialized(int buildIndex, string sceneName)
    {
        if (buildIndex != 3) return;
        PlayerSetup.Instance.vrLeftHandTracker.AddComponent(typeof(ControllerFreezer));
        PlayerSetup.Instance.vrRightHandTracker.AddComponent(typeof(ControllerFreezer));
    }
}