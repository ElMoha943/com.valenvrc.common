using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace valenvrc.Common
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None),Icon("Packages/com.valenvrc.common/Editor/Resources/ValenFace.png"), HelpURL("https://docs.valenvrc.com/valencommons/utility#fallrecover")]
    public class FallRecover : UdonSharpBehaviour
    {
        VRCPlayerApi localPlayer;
        Vector3 lastPosition;
        Quaternion lastRotation;

        void Start(){
            localPlayer = Networking.LocalPlayer;
            SendCustomEventDelayedSeconds(nameof(CustomLoop), 0.1f);
        }

        public void CustomLoop(){
            if (localPlayer.IsPlayerGrounded()){
                lastPosition = localPlayer.GetPosition();
                lastRotation = localPlayer.GetRotation();
            }
            SendCustomEventDelayedSeconds(nameof(CustomLoop), 0.1f);
        }

        public void ReturnToLastPosition(){
            //Teleports the player a bit back to the last position they were grounded, to avoid falling through the ground
            localPlayer.TeleportTo(lastPosition + Vector3.up * 0.1f + Vector3.back * 0.1f, lastRotation);
        }

    }
}