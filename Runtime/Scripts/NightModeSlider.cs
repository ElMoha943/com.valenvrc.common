using UdonSharp;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

namespace valenvrc.Common{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None),Icon("Packages/com.valenvrc.common/Editor/Resources/ValenFace.jpg"), HelpURL("https://discord.gg/nv5ax3wDqc")]
    public class NightModeSlider : UdonSharpBehaviour
    {
        [SerializeField] PostProcessVolume postProcessVolume;
        [SerializeField] Slider slider;

        void Start(){
            if (!slider) slider = GetComponent<Slider>();
            if (!slider || !postProcessVolume){
                Debug.LogError("NightModeSlider: Missing slider or postProcessVolume reference.");
                gameObject.SetActive(false);
            }
            #if UNITY_ANDROID
            Debug.Log("NightModeSlider: Quest users dont see post processing, disabling slider.");
            slider.enabled = false;
            #endif
        }

        #if UNITY_STANDALONE_WIN
        public void OnValueChange(){
            if(postProcessVolume)postProcessVolume.weight = slider.value;
        }
        #endif
    }
}