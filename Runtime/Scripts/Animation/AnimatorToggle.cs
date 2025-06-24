using UdonSharp;
using UnityEngine;

namespace valenvrc.Common{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None),Icon("Packages/com.valenvrc.common/Editor/Resources/ValenFace.jpg"), HelpURL("https://discord.gg/nv5ax3wDqc")]
    public class AnimatorToggle : UdonSharpBehaviour
    {
        [SerializeField] Animator anim;
        [SerializeField] string parameterName;
        int hash;

        void Awake(){
            if (anim == null){
                Debug.LogError("AnimatorToggle: Animator component not found on " + gameObject.name);
                return;
            }
            if (string.IsNullOrEmpty(parameterName)){
                Debug.LogError("AnimatorToggle: Parameter name is not set on " + gameObject.name);
                return;
            }
            hash = Animator.StringToHash(parameterName);
        }
        
        public void _Toggle(){
            anim.SetBool(hash, !anim.GetBool(hash));
        }

        public override void Interact(){
            _Toggle();
        }
    }
}