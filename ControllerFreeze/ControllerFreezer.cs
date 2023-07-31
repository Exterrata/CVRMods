using ABI_RC.Core.Savior;
using UnityEngine;

namespace Koneko;
public class ControllerFreezer : MonoBehaviour
{
    public Vector3 PrevPos;
    public Quaternion PrevRot;
    public Vector3 FreezePos;
    public Quaternion FreezeRot;
    public bool frozen;

    public void LateUpdate()
    {
        if (!MetaPort.Instance.isUsingVr || CheckVR.Instance.forceOpenXr || !gameObject.activeInHierarchy) return;

        if (transform.position != PrevPos)
        {
            FreezePos = PrevPos;
            FreezeRot = PrevRot;
            frozen = false;
        }
        else if (!frozen)
        {
            frozen = true;
        }

        PrevPos = transform.position;
        PrevRot = transform.rotation;

        if (frozen)
        {
            transform.position = FreezePos;
            transform.rotation = FreezeRot;
        }
    }
}