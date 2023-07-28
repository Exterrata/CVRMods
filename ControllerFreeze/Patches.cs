using HarmonyLib;
using MelonLoader;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

namespace Koneko;
internal class Patches
{
    public static Dictionary<int, Controller> controllers;

    public class Controller
    {
        public Transform transform;
        public Vector3 PrevPos;
        public Quaternion PrevRot;
        public Vector3 FreezePos;
        public Quaternion FreezeRot;
        public bool frozen;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Valve.VR.SteamVR_Behaviour_Pose), "SteamVR_Behaviour_Pose_OnUpdate")]
    public static void FreezeControllers(SteamVR_Behaviour_Pose __instance, int ___deviceIndex)
    {
        if (!controllers.ContainsKey(___deviceIndex)) {
            var controller = new Controller();
            controller.transform = __instance.transform;
            controllers.Add(___deviceIndex, controller);
        }

        if (controllers[___deviceIndex].transform.position != controllers[___deviceIndex].PrevPos)
        {
            controllers[___deviceIndex].FreezePos = controllers[___deviceIndex].PrevPos;
            controllers[___deviceIndex].FreezeRot = controllers[___deviceIndex].PrevRot;
            controllers[___deviceIndex].frozen = false;
        }
        else if (!controllers[___deviceIndex].frozen) controllers[___deviceIndex].frozen = true;

        controllers[___deviceIndex].PrevPos = controllers[___deviceIndex].transform.position;
        controllers[___deviceIndex].PrevRot = controllers[___deviceIndex].transform.rotation;

        if (controllers[___deviceIndex].frozen)
        {
            controllers[___deviceIndex].transform.position = controllers[___deviceIndex].FreezePos;
            controllers[___deviceIndex].transform.rotation = controllers[___deviceIndex].FreezeRot;
        }
    }
}
