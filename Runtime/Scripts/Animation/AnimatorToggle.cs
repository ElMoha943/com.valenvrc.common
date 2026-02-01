using UdonSharp;
using UnityEngine;

namespace valenvrc.Common{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None),Icon("Packages/com.valenvrc.common/Editor/Resources/ValenFace.png"), HelpURL("https://docs.valenvrc.com/valencommons/animation#animatortoggle")]
    public class AnimatorToggle : UdonSharpBehaviour
    {
        [SerializeField] Animator anim;
        [SerializeField] string parameterName;
        int hash;

        void Start(){
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