using ABI_RC.Core.Player;
using ABI_RC.Core.Savior;
using ABI_RC.Systems.IK.SubSystems;
using HarmonyLib;

namespace Koneko;
internal class Patches
{
    [HarmonyPostfix]
	[HarmonyPatch(typeof(PlayerSetup), "Update")]
	public static void KnucklesAlwaysTrack()
	{
	    if (MetaPort.Instance.isUsingVr)
	    {
	        BodySystem.TrackingLeftArmEnabled = true;
	        BodySystem.TrackingRightArmEnabled = true;
	    }
	}
}
