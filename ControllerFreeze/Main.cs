using System;
using MelonLoader;
using UnityEngine;
using ABI_RC.Core.Savior;

[assembly: MelonGame("Alpha Blend Interactive", "ChilloutVR")]
[assembly: MelonInfo(typeof(Koneko.ControllerFreeze), "ControllerFreeze", "1.0.0", "Exterrata")]
[assembly: HarmonyDontPatchAll]

namespace Koneko;
public class ControllerFreeze : MelonMod
{
    public static Transform Left;
    public static Vector3 PrevPosLeft;
    public static Quaternion PrevRotLeft;
    public static Vector3 FreezePosLeft;
    public static Quaternion FreezeRotLeft;

    public static Transform Right;
    public static Vector3 PrevPosRight;
    public static Quaternion PrevRotRight;
    public static Vector3 FreezePosRight;
    public static Quaternion FreezeRotRight;

    public static bool frozenLeft;
    public static bool frozenRight;
    public static bool Initialized = false;


    public override void OnInitializeMelon()
    {
        try {
            HarmonyInstance.PatchAll(typeof(Patches));
        } catch(Exception e) { 
            MelonLogger.Error(e);
        }
    }

    public override void OnSceneWasInitialized(int buildIndex, string sceneName)
    {
        if (buildIndex == 3)
        {
            Left = GameObject.Find("Controller (left)").transform;
            Right = GameObject.Find("Controller (right)").transform;
        }
    }

    public override void OnLateUpdate()
    {
        if (!Initialized || !MetaPort.Instance.isUsingVr) return;

        if (Left.position != PrevPosLeft)
        {
            FreezePosLeft = PrevPosLeft;
            FreezeRotLeft = PrevRotLeft;
            frozenLeft = false;
        } 
        else if (!frozenLeft) frozenLeft = true;

        if (Right.position != PrevPosRight) 
        {
            FreezePosRight = PrevPosRight;
            FreezeRotRight = PrevRotRight;
            frozenRight = false;
        } 
        else if (!frozenRight) frozenRight = true;

        PrevPosLeft = Left.position;
        PrevRotLeft = Left.rotation;
        PrevPosRight = Right.position;
        PrevRotRight = Right.rotation;

        if (frozenLeft)
        {
            Left.position = FreezePosLeft;
            Left.rotation = FreezeRotLeft;
        }
        if (frozenRight)
        {
            Right.position = FreezePosRight;
            Right.rotation = FreezeRotRight;
        }
    }
}