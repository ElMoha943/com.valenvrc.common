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

        int hash;

        void Awake(){
            if (anim == null){
                Debug.LogError("AnimatorToggle: Animator component not found on " + gameObject.name);
                return;
            }
            if (string.IsNullOrEmpty(parameterName)) {
                Debug.LogError("AnimatorToggle: Parameter name is not set on " + gameObject.name);
                return;
            }
            hash = Animator.StringToHash(parameterName);
        }

        public override void OnPlayerTriggerEnter(VRCPlayerApi player){
            if(player.isLocal){
                anim.SetBool(hash, toggleOnEnter);
            }
        }
        
        public override void OnPlayerTriggerExit(VRCPlayerApi player){
            if(player.isLocal){
                anim.SetBool(hash, !toggleOnEnter);
            }
        }
    }
}