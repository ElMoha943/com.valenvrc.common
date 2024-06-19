using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync)]
public class PickupInvoke : UdonSharpBehaviour
{
    [SerializeField] string methodName;
    [SerializeField] UdonBehaviour target;
    
    public override void OnPickupUseDown(){
        target.SendCustomEvent(methodName);
    }
}
