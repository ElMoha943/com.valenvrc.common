using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace valenvrc.Common{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None),Icon("Packages/com.valenvrc.common/Editor/Resources/ValenFace.png"), HelpURL("https://docs.valenvrc.com/valencommons/toggles#triggertoggle")]
    public class TriggerToggle : UdonSharpBehaviour
    {
        [SerializeField] GameObject[] toggleableObjects;

        public override void OnPlayerTriggerEnter(VRCPlayerApi player){
            if (player.isLocal){
                foreach (GameObject obj in toggleableObjects){
                    obj.SetActive(!obj.activeSelf);
                }
            }
        }

        public override void OnPlayerTriggerExit(VRCPlayerApi player){
            if (player.isLocal){
                foreach (GameObject obj in toggleableObjects){
                    obj.SetActive(!obj.activeSelf);
                }
            }
        }
    }
}