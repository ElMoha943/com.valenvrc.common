using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDKBase;

namespace valenvrc.Common{
    [UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync),Icon("Packages/com.valenvrc.common/Editor/Resources/ValenFace.png"), HelpURL("https://docs.valenvrc.com/valencommons/pickups#respawnpickups")]
    public class RespawnPickups : UdonSharpBehaviour
    {
        [SerializeField] VRCObjectSync[] objects;
        [SerializeField] bool respawnHeldItems = false;

        public void _RespawnPickups(){
            foreach(VRCObjectSync obj in objects){
                VRCPickup pickup = obj.GetComponent<VRCPickup>();
                if(pickup != null && pickup.IsHeld){
                    if(respawnHeldItems) pickup.Drop();
                    else continue;
                }
                if(!Networking.IsOwner(obj.gameObject)){
                    Networking.SetOwner(Networking.LocalPlayer, obj.gameObject);
                }
                obj.Respawn();
            }
        }

        public override void Interact(){
            _RespawnPickups();
        }
    }
}