using UdonSharp;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

namespace valenvrc.Common{
    [UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync),Icon("Packages/com.valenvrc.common/Runtime/PromotionalImages/ValenFace.jpg"), HelpURL("https://discord.gg/nv5ax3wDqc")]
    public class NightModeSlider : UdonSharpBehaviour
    {
        [SerializeField] PostProcessVolume postProcessVolume;
        Slider slider;

        void Start(){
            slider = GetComponent<Slider>();
        }

        public void OnValueChange(){
            postProcessVolume.weight = slider.value;
        }
    }
}