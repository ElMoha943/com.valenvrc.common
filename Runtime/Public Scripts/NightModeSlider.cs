using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

[UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync)]
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
