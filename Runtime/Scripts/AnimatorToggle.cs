using UdonSharp;
using UnityEngine;

namespace valenvrc.Common{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None),Icon("Packages/com.valenvrc.common/Editor/Resources/ValenFace.jpg"), HelpURL("https://discord.gg/nv5ax3wDqc")]
    public class AnimatorToggle : UdonSharpBehaviour
    {
        [SerializeField] Animator anim;
        [SerializeField] string parameterName;

        void OnEnable(){
            if(anim != null) anim.SetBool(parameterName, true);
        }

        void OnDisable(){
            if(anim != null) anim.SetBool(parameterName, false);
        }
    }
}