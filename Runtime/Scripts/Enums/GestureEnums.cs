using UnityEngine;
using VRC.SDKBase;
using VRC.Udon.Common;

namespace valenvrc.Common{
    public enum GestureType{
        KeyPress,
        RightJoyStickUpSeconds,
        RightJoyStickDownSeconds,
        RightJoyStickUpTimes,
        RightJoyStickDownTimes
    }

    public enum GestureActions{
        Toggle,
        TeleportObject,
        TelportSelf
    }

    public enum TargetPositions{
        Front,
        RightHand,
        LeftHand,
        Custom,
    }
}