using System.Collections.Generic;
using HarmonyLib;
using MelonLoader;
using ABI_RC.Core.Networking.API;
using ABI_RC.Core.InteractionSystem;

namespace Koneko;
public class Patches
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(ViewManager), "ShowInviteRequest")]
    public static bool AcceptJoin(List<ABI_RC.Core.Networking.API.Responses.RequestInvite> requestInvites)
    {
        if (JoinMe.Enabled.Value)
        {
            foreach (ABI_RC.Core.Networking.API.Responses.RequestInvite requestInvite in requestInvites)
            {
                ApiConnection.SendWebSocketRequest(ABI_RC.Core.Networking.API.UserWebsocket.RequestType.RequestInviteAccept, new { id = requestInvite.Id });
                ViewManager.Instance.BufferHudMessage("Accepted invite from " + requestInvite.Sender.Name);
                MelonLogger.Msg("Accepted request from " + requestInvite.Sender.Name);
            }
            return false;
        }
        return true;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(ViewManager), "BufferMenuPopup")]
    public static bool CancelPopup(string message)
    {
        if (JoinMe.Enabled.Value && message == "Invite sent.") return false;
        return true;
    }
}