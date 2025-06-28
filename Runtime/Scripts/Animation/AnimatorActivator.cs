using UdonSharp;
using UnityEngine;

namespace valenvrc.Common{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None),Icon("Packages/com.valenvrc.common/Editor/Resources/ValenFace.jpg"), HelpURL("https://discord.gg/nv5ax3wDqc")]
    public class AnimatorActivator : UdonSharpBehaviour
    {
        [SerializeField] Animator anim;
        [SerializeField] string parameterName;
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

        void OnEnable(){
            if(anim != null) anim.SetBool(hash, true);
        }

        void OnDisable(){
            if(anim != null) anim.SetBool(hash, false);
        }
    }
}