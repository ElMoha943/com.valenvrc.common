using UdonSharp;
using UnityEngine;

namespace valenvrc.Common{
    [UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync),Icon("Packages/com.valenvrc.common/Runtime/PromotionalImages/ValenFace.jpg"), HelpURL("https://discord.gg/nv5ax3wDqc")]
    public class AnimatorToggle : UdonSharpBehaviour
    {
        [SerializeField] Animator anim;
        [SerializeField] string parameterName;

        void OnEnable(){
            anim.SetBool(parameterName, true);
        }

        void OnDisable(){
            anim.SetBool(parameterName, false);
        }
    }
}