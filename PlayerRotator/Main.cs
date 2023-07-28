using UnityEngine;
using MelonLoader;
using BTKUILib;
using BTKUILib.UIObjects;
using ABI_RC.Core.Savior;
using System.Reflection;

[assembly: MelonGame("Alpha Blend Interactive", "ChilloutVR")]
[assembly: MelonInfo(typeof(Koneko.PlayerRotator), "PlayerRotator", "1.1.0", "Exterrata")]
[assembly: MelonAdditionalDependencies("BTKUILib")]

namespace Koneko;
public class PlayerRotator : MelonMod
{
    static Transform LocalPlayer;
    static Transform CameraRig;
    static Transform Camera;
    static Vector3 RotationSpeed;
    static bool Mode = false;
    static bool Initialized = false;

    static BTKUILib.UIObjects.Components.Button ModeButton;
    static BTKUILib.UIObjects.Components.SliderFloat SliderX;
    static BTKUILib.UIObjects.Components.SliderFloat SliderY;
    static BTKUILib.UIObjects.Components.SliderFloat SliderZ;

    public override void OnSceneWasInitialized(int buildIndex, string sceneName)
    {
        if (buildIndex != 3) return;
        LocalPlayer = GameObject.Find("_PLAYERLOCAL").transform;
        CameraRig = LocalPlayer.transform.FindChild("[CameraRigVR]");
        Camera = CameraRig.transform.FindChild("Camera");

        QuickMenuAPI.PrepareIcon("PlayerRotator", "Tracking", Assembly.GetExecutingAssembly().GetManifestResourceStream("PlayerRotator.Icons.Tracking.png"));
        QuickMenuAPI.PrepareIcon("PlayerRotator", "Horizon", Assembly.GetExecutingAssembly().GetManifestResourceStream("PlayerRotator.Icons.Horizon.png"));
        QuickMenuAPI.PrepareIcon("PlayerRotator", "Cords", Assembly.GetExecutingAssembly().GetManifestResourceStream("PlayerRotator.Icons.Cords.png"));
        QuickMenuAPI.PrepareIcon("PlayerRotator", "ResetRotation", Assembly.GetExecutingAssembly().GetManifestResourceStream("PlayerRotator.Icons.ResetRotation.png"));

        var page = new Page("PlayerRotator", "PlayerRotator", true, "Cords");
        page.MenuTitle = "Player Rotator";
        page.MenuSubtitle = "You Should Rotate Yourself Now";

        SliderX = page.AddSlider("X Rotation: 0", "X Rotation", 0, -1, 1);
        SliderX.OnValueUpdated += b => RX(b);
        SliderY = page.AddSlider("Y Rotation: 0", "Y Rotation", 0, -1, 1);
        SliderY.OnValueUpdated += b => RY(b);
        SliderZ = page.AddSlider("Z Rotation: 0", "Z Rotation", 0, -1, 1);
        SliderZ.OnValueUpdated += b => RZ(b);
        SliderX.AllowDefaultReset = true;
        SliderY.AllowDefaultReset = true;
        SliderZ.AllowDefaultReset = true;

        var Category = page.AddCategory("Settings");

        Category.AddButton("Reset Rotation", "ResetRotation", "Reset Rotation").OnPress += RotationReset;
        Category.AddButton("Horizon Center", "Horizon", "Rotates Tracking So Current Facing Direction Looks At The Horizon. VR Only").OnPress += HorizonCenter;
        ModeButton = Category.AddButton("Current Mode: Player", "Tracking", "Toggle Between Player Rotation And Tracking Rotation. VR Only");
        ModeButton.OnPress += ModeToggle;
        Initialized = true;
    }

    public override void OnUpdate()
    {
        if (!Initialized) return;
        if (MetaPort.Instance.isUsingVr && Mode) CameraRig.Rotate(RotationSpeed * Time.deltaTime * 100); 
        else LocalPlayer.Rotate(RotationSpeed * Time.deltaTime * 100);
        RotationSpeed = Vector3.zero;
    }

    void RX(float value) { RotationSpeed.x = value; SliderX.SliderName = "X Rotation: " + (Mode ? CameraRig.eulerAngles.x : LocalPlayer.eulerAngles.x); }
    void RY(float value) { RotationSpeed.y = value; SliderY.SliderName = "Y Rotation: " + (Mode ? CameraRig.eulerAngles.y : LocalPlayer.eulerAngles.y); }
    void RZ(float value) { RotationSpeed.z = value; SliderZ.SliderName = "Z Rotation: " + (Mode ? CameraRig.eulerAngles.z : LocalPlayer.eulerAngles.z); }

    public void RotationReset()
    {
        LocalPlayer.transform.rotation = Quaternion.identity;
        CameraRig.transform.rotation = Quaternion.identity;
        SliderX.SliderName = "X Rotation: 0";
        SliderY.SliderName = "Y Rotation: 0";
        SliderZ.SliderName = "Z Rotation: 0";
    }

    public void ModeToggle()
    {
        Mode = !Mode;
        if (Mode) ModeButton.ButtonText = "Current Mode: Tracking";
        else ModeButton.ButtonText = "Current Mode: Player";
        RotationReset();
    }

    public void HorizonCenter()
    {
        LocalPlayer.transform.rotation = Quaternion.identity;
        CameraRig.transform.rotation = Quaternion.identity;
        Camera.rotation.ToAxisAngle(out Vector3 axis, out float angle);
        CameraRig.rotation = Quaternion.AxisAngle(axis, -angle);
    }
}