using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDKBase;

[UdonBehaviourSyncMode(BehaviourSyncMode.None),Icon("Packages/com.valenvrc.common/Runtime/PromotionalImages/ValenFace.jpg"), HelpURL("https://discord.gg/nv5ax3wDqc")]
public class PickUpResetZone : UdonSharpBehaviour
{
    [SerializeField,Tooltip("If not empty, only the objects in this list will be respawned upon leaving the trigger area.")] VRCObjectSync[] objects;

    public override void OnPlayerTriggerExit(VRCPlayerApi player){
        if (player.isLocal){
            //Get the pickup on the players right hand
            VRC_Pickup pickup = Networking.LocalPlayer.GetPickupInHand(VRC_Pickup.PickupHand.Right);
            if (pickup != null){
                //check if the pickup is in the list of objects, if the list is not empty
                if(CheckInList(pickup)){
                    TryDropAndRespawn(pickup);
                }  
            }
            //Get the pickup on the players left hand
            pickup = Networking.LocalPlayer.GetPickupInHand(VRC_Pickup.PickupHand.Left);
            if (pickup != null){
                //check if the pickup is in the list of objects, if the list is not empty
                if(CheckInList(pickup)){
                    TryDropAndRespawn(pickup);
                }
            }
        }
    }

    bool CheckInList(VRC_Pickup pickup){
        if(objects.Length > 0){
            foreach(VRCObjectSync obj in objects)
                return true;
            return false;
        }
        return true;
    }

    void TryDropAndRespawn(VRC_Pickup pickup){
        pickup.Drop();
        VRCObjectSync objSync = pickup.gameObject.GetComponent<VRCObjectSync>();
        if(objSync != null){
            objSync.Respawn();
        }
    }
}
