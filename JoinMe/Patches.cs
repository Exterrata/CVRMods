using System.Collections.Generic;
using HarmonyLib;
using MelonLoader;
using ABI_RC.Core.Networking.API;
using ABI_RC.Core.InteractionSystem;

namespace Koneko;
public class Patches
{
    public static bool acceptedRequest = false;
    [HarmonyPrefix]
    [HarmonyPatch(typeof(ViewManager), "ShowInviteRequest")]
    public static void AcceptRequest(List<ABI_RC.Core.Networking.API.Responses.RequestInvite> requestInvites)
    {
        if (!JoinMe.Enabled.Value) return;
        foreach (ABI_RC.Core.Networking.API.Responses.RequestInvite requestInvite in requestInvites)
        {
            if (JoinMe.RequestWhitelistEnabled.Value)
            {
                bool whitelisted = false;
                foreach (string name in JoinMe.RequestWhitelistList)
                {
                    if (name == requestInvite.Sender.Name) whitelisted = true;
                }
                if (whitelisted)
                {
                    acceptedRequest = true;
                    ApiConnection.SendWebSocketRequest(ABI_RC.Core.Networking.API.UserWebsocket.RequestType.RequestInviteAccept, new { id = requestInvite.Id });
                    ViewManager.Instance.BufferHudMessage("Accepted request from " + requestInvite.Sender.Name);
                    MelonLogger.Msg("Accepted request from " + requestInvite.Sender.Name);
                    requestInvites.Remove(requestInvite);
                }
            }
            else
            {
                acceptedRequest = true;
                ApiConnection.SendWebSocketRequest(ABI_RC.Core.Networking.API.UserWebsocket.RequestType.RequestInviteAccept, new { id = requestInvite.Id });
                ViewManager.Instance.BufferHudMessage("Accepted request from " + requestInvite.Sender.Name);
                MelonLogger.Msg("Accepted request from " + requestInvite.Sender.Name);
                requestInvites.Remove(requestInvite);
            }
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(ViewManager), "BufferMenuPopup")]
    public static bool CancelPopup(string message)
    {
        if (JoinMe.Enabled.Value && message == "Invite sent." && acceptedRequest)
        {
            acceptedRequest = false;
            return false;
        }
        return true;
    }
}