using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class ButtonCooldown : UdonSharpBehaviour
{
    void setCooldown(){
        Selectable i = GetComponent<Selectable>();
        i.interactable = false;
        SendCustomEventDelayedSeconds("resetCooldown", 2);
    }

    void resetCooldown(){
        Selectable i = GetComponent<Selectable>();
        i.interactable = true;
    }
}
