using UnityEngine;
using MelonLoader;
using BTKUILib;
using BTKUILib.UIObjects;
using ABI_RC.Core.InteractionSystem;
using ABI_RC.Core.Savior;

[assembly: MelonGame("Alpha Blend Interactive", "ChilloutVR")]
[assembly: MelonInfo(typeof(RotateMod.rotateMod), "Rotate Mod", "1.0.0", "Exterrata")]

namespace RotateMod
{
    public class rotateMod  : MelonMod
    {
        private Transform _localPlayer;
        private Transform _cameraRig;
        private Transform _camera;

        private bool _initialized = false;

        private bool _mode = false;

        private float _speed = 1f;

        private BTKUILib.UIObjects.Components.Button _modeButton;
        private BTKUILib.UIObjects.Components.Button _speedButton;

        public override void OnApplicationStart()
        {
            QuickMenuAPI.OnMenuRegenerate += Setup;
        }

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            if (buildIndex != 3) return;
            _localPlayer = GameObject.Find("_PLAYERLOCAL").transform;
            _cameraRig = _localPlayer.transform.FindChild("[CameraRigVR]");
            _camera = _cameraRig.transform.FindChild("Camera");
        }

        void Setup(CVR_MenuManager unused)
        {
            if (_initialized) return;
            _initialized = true;

            var page = new Page("RotateMod", "Rotate Mod", true);
            page.MenuTitle = "Rotate Mod";
            page.MenuSubtitle = "You Should Rotate Yourself Now";


            var Category = page.AddCategory("Rotate X");

            var button = Category.AddButton("X+", "idk", "X+");
            button.OnPress += RXP;

            button = Category.AddButton("X-", "idk", "X-");
            button.OnPress += RXM;


            Category = page.AddCategory("Rotate Y");

            button = Category.AddButton("Y+", "idk", "Y+");
            button.OnPress += RYP;

            button = Category.AddButton("Y-", "idk", "Y-");
            button.OnPress += RYM;


            Category = page.AddCategory("Rotate Z");

            button = Category.AddButton("Z+", "idk", "Z+");
            button.OnPress += RZP;

            button = Category.AddButton("Z-", "idk", "Z-");
            button.OnPress += RZM;


            Category = page.AddCategory("Settings");

            button = Category.AddButton("Increase Speed", "idk", "Increase Rotation Speed");
            button.OnPress += SpeedUp;

            button = Category.AddButton("Decease Speed", "idk", "decrease Rotation Speed");
            button.OnPress += SpeedDown;

            button = Category.AddButton("Reset Speed (1)", "idk", "Reset Rotation Speed");
            button.OnPress += SpeedReset;
            _speedButton = button;

            button = Category.AddButton("Reset Rotation", "idk", "Reset Rotation");
            button.OnPress += RotationReset;

            button = Category.AddButton("Horizon Center", "idk", "Rotates Tracking So Current Facing Direction Looks At The Horizon. VR Only");
            button.OnPress += HorizonCenter;

            button = Category.AddButton("Toggle Mode (Player)", "idk", "Toggle Between Player Rotation And Tracking Rotation. VR Only");
            button.OnPress += Mode;
            _modeButton = button;
        }
        
        void SpeedUp() { _speed += 0.5f; _speedButton.ButtonText = "Reset Speed (" + _speed + ")"; }
        void SpeedDown() { _speed -= 0.5f; _speedButton.ButtonText = "Reset Speed (" + _speed + ")"; }
        void SpeedReset() { _speed = 1f; _speedButton.ButtonText = "Reset Speed (1)";  }
        void RotationReset() { _localPlayer.transform.rotation = Quaternion.identity; _cameraRig.transform.rotation = Quaternion.identity; }
        void Mode() { _mode = !_mode; if (_mode) _modeButton.ButtonText = "Toggle Mode (Tracking)"; else _modeButton.ButtonText = "Toggle Mode (Player)"; }
        void RXP() { if (MetaPort.Instance.isUsingVr && _mode) _cameraRig.Rotate(new Vector3(_speed, 0, 0)); else _localPlayer.Rotate(new Vector3(_speed, 0, 0)); }
        void RXM() { if (MetaPort.Instance.isUsingVr && _mode) _cameraRig.Rotate(new Vector3(-_speed, 0, 0)); else _localPlayer.Rotate(new Vector3(-_speed, 0, 0)); }
        void RYP() { if (MetaPort.Instance.isUsingVr && _mode) _cameraRig.Rotate(new Vector3(0, _speed, 0)); else _localPlayer.Rotate(new Vector3(0, _speed, 0)); }
        void RYM() { if (MetaPort.Instance.isUsingVr && _mode) _cameraRig.Rotate(new Vector3(0, -_speed, 0)); else _localPlayer.Rotate(new Vector3(0, -_speed, 0)); }
        void RZP() { if (MetaPort.Instance.isUsingVr && _mode) _cameraRig.Rotate(new Vector3(0, 0, _speed)); else _localPlayer.Rotate(new Vector3(0, 0, _speed)); }
        void RZM() { if (MetaPort.Instance.isUsingVr && _mode) _cameraRig.Rotate(new Vector3(0, 0, -_speed)); else _localPlayer.Rotate(new Vector3(0, 0, -_speed)); }
        void HorizonCenter()
        {
            _localPlayer.transform.rotation = Quaternion.identity;
            _cameraRig.transform.rotation = Quaternion.identity;
            _camera.rotation.ToAxisAngle(out Vector3 axis, out float angle);
            _cameraRig.rotation = Quaternion.AxisAngle(axis, -angle);
        }
    }
}
