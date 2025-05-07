using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace valenvrc.Common{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None),Icon("Packages/com.valenvrc.common/Editor/Resources/ValenFace.jpg"), HelpURL("https://discord.gg/nv5ax3wDqc")]
    public class TriggerAnimatorToggle : UdonSharpBehaviour
    {
        [SerializeField] Animator anim;
        [SerializeField] string parameterName;
        [SerializeField] bool toggleOnEnter = true;

        public override void OnPlayerTriggerEnter(VRCPlayerApi player){
            if(player.isLocal){
                anim.SetBool(parameterName, toggleOnEnter);
            }
        }
        
        public override void OnPlayerTriggerExit(VRCPlayerApi player){
            if(player.isLocal){
                anim.SetBool(parameterName, !toggleOnEnter);
            }
        }
    }
}