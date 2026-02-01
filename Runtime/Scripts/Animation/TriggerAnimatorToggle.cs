using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace valenvrc.Common{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None),Icon("Packages/com.valenvrc.common/Editor/Resources/ValenFace.png"), HelpURL("https://docs.valenvrc.com/valencommons/animation#triggeranimatortoggle")]
    public class TriggerAnimatorToggle : UdonSharpBehaviour
    {
        [SerializeField] Animator anim;
        [SerializeField] string parameterName;
        [SerializeField] bool workForAllPlayers = false;

        int hash;

        void Start(){
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
            if(player.isLocal || workForAllPlayers){
                anim.SetBool(hash, true);
            }
        }
        
        public override void OnPlayerTriggerExit(VRCPlayerApi player){
            if(player.isLocal || workForAllPlayers){
                anim.SetBool(hash, false);
            }
        }
    }
}