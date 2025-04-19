using System;
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon.Common;

namespace valenvrc.Common{
    [UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync),Icon("Packages/com.valenvrc.common/Runtime/PromotionalImages/ValenFace.jpg"), HelpURL("https://discord.gg/nv5ax3wDqc")]
    public class GestureInvoke : UdonSharpBehaviour
    {
        [SerializeField] bool RequireTriggerPressed = true;

        [SerializeField] GestureType gestureType = GestureType.KeyPress;
        [SerializeField] GestureActions gestureAction = GestureActions.TeleportObject;
        [SerializeField] TargetPositions targetPosition = TargetPositions.Front;
        [SerializeField] KeyCode RespawnKey = KeyCode.Q;

        [SerializeField] float times;
        [SerializeField] float gracePeriod = 0.5f;

        [SerializeField] GameObject targetObject;
        [SerializeField] GameObject tpoint;

        VRCPlayerApi localPlayer;
        float avatarHeight = 1.6f;
        bool vrMode = false;

        bool triggerPrimed = false;
        bool primed = false;
        float time = 0;
        
        bool antiRebote = false;
        float nextReset = 0.0f;
    
        void Start(){
            localPlayer = Networking.LocalPlayer;
            vrMode = localPlayer.IsUserInVR();
        }

        void Update(){
            switch(gestureType){
                case GestureType.KeyPress:
                    if(Input.GetKeyDown(RespawnKey)){
                        _ExecuteActions();
                    }
                    break;
                case GestureType.RightJoyStickUpSeconds:
                case GestureType.RightJoyStickDownSeconds:
                    if(primed){
                        time += Time.deltaTime;
                        if(time >= times){
                            _ExecuteActions();
                            time = 0;
                        }
                    }
                    break;
                case GestureType.RightJoyStickUpTimes:
                case GestureType.RightJoyStickDownTimes:
                    if(Time.time >= nextReset){
                        time = 0;
                    }
                    break;
            }
        }

        public override void InputLookVertical(float value, UdonInputEventArgs args){
            if(!vrMode) return;
            //check for input
            if(value > 0.5){
                if(RequireTriggerPressed && !triggerPrimed) return;
                if(gestureType == GestureType.RightJoyStickUpSeconds){
                    primed = true;
                }
                else if(gestureType == GestureType.RightJoyStickUpTimes && !antiRebote){
                    time++;
                    antiRebote = true;
                    nextReset = Time.time + gracePeriod;
                    if(time > times){
                        _ExecuteActions();
                        time = 0;
                    }
                }
                else{
                    primed = false;
                }
            }
            else if(value < -0.5){
                if(RequireTriggerPressed && !triggerPrimed) return;
                if(gestureType == GestureType.RightJoyStickDownSeconds){
                    primed = true;
                }
                else if(gestureType == GestureType.RightJoyStickDownTimes && !antiRebote){
                    time++;
                    antiRebote = true;
                    nextReset = Time.time + gracePeriod;
                    if(time > times){
                        _ExecuteActions();
                        time = 0;
                    }
                }
                else{
                    primed = false;
                }
            }
            else{
                if(gestureType == GestureType.RightJoyStickUpSeconds || gestureType == GestureType.RightJoyStickDownSeconds){
                    primed = false;
                    time = 0;
                }
                else{
                    antiRebote = false;
                }
            }
        }

        public override void InputUse(bool value, UdonInputEventArgs args){
            triggerPrimed = value;
        }

        void _ExecuteActions(){
            switch(gestureAction){
                case GestureActions.Toggle:
                    _Toggle();
                    break;
                case GestureActions.TeleportObject:
                    _MoveObject();
                    break;
                case GestureActions.TelportSelf:
                    _TeleportSelf();
                    break;
            }
        }

        void _Toggle(){
            targetObject.SetActive(!targetObject.activeSelf);
        }

        void _MoveObject(){
            Vector3 targetPos;
            if(targetPosition == TargetPositions.Front){
                VRCPlayerApi.TrackingData td = localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head);
                targetPos = td.position + td.rotation * Vector3.forward * (avatarHeight / 2);
            }
            else if(targetPosition == TargetPositions.RightHand){
                targetPos = localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.RightHand).position;
            }
            else if(targetPosition == TargetPositions.LeftHand){
                targetPos = localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.LeftHand).position;
            }
            else{ // targetPosition == TargetPositions.Custom
                targetPos = tpoint.transform.position;
            }
            targetObject.transform.position = targetPos;
            targetObject.transform.LookAt(localPlayer.GetBonePosition(HumanBodyBones.Head));
        }

        void _TeleportSelf(){
            localPlayer.TeleportTo(tpoint.transform.position, tpoint.transform.rotation);
        }
    }
}