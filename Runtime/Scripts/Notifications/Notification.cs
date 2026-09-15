using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

namespace valenvrc.Common
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync),Icon("Packages/com.valenvrc.common/Editor/Resources/ValenFace.png")]
    public class Notification : UdonSharpBehaviour{
        [Header("Internal References")]
        [SerializeField] Animator animator;
        [SerializeField] Image icon;
        [SerializeField] TMP_Text text;

        const float TIME_TO_CLOSE = 0.5f;

        public void _Open(string message,float displayDuration,Sprite newIcon){
            if (text == null || animator == null) return;

            if (icon != null) icon.sprite = newIcon;
            text.text = message;
            animator.SetTrigger("Open");

            //Networking.LocalPlayer.AttachTransformToBone(transform, HumanBodyBones.Head);

            SendCustomEventDelayedSeconds(nameof(_Close), displayDuration);
        }

        public void _Close(){
            if (animator == null) return;

            //Networking.LocalPlayer.DetachTransformFromBone(transform);
            animator.SetTrigger("Close");
            SendCustomEventDelayedSeconds(nameof(_Delete), TIME_TO_CLOSE);
        }

        public void _Delete(){
            Destroy(gameObject);
        }
    }
}