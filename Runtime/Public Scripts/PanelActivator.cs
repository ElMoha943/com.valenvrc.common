using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync)]
public class PanelActivator : UdonSharpBehaviour
{
    [SerializeField] Selectable[] collection;

    void OnEnable(){
        foreach (Selectable selectable in collection){
            if (selectable != null)
                selectable.interactable = true;
        }
    }

    void OnDisable(){
        foreach (Selectable selectable in collection){
            if (selectable != null)
                selectable.interactable = false;
        }
    }
}