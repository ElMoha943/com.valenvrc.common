# Copilot Instructions for valenvrc.Common

## Project Overview
**Unity VRChat UdonSharp Utility Library** - A VPM package providing reusable utility scripts, editor tools, and UI components for VRChat world development. This package serves as a dependency for other valenvrc assets.

- **Version**: 2.3.0
- **Unity Target**: 2022.3.22f1+
- **VRChat SDK**: 3.8.1+
- **Primary Language**: C# (UdonSharp for runtime, standard C# for editor)

## Architecture & Structure

### Directory Organization
```
Editor/               # Unity Editor-only scripts
├── Core/            # Define manager, asset registry
├── CustomEditors/   # Custom inspectors for Udon components
├── Utilities/       # Editor helper utilities, object pool tools
└── Licensing/       # DLL for license validation

Runtime/             # VRChat runtime components
├── Scripts/
│   ├── Animation/   # Animator utilities (AnimatorActivator, AnimatorToggle)
│   ├── Utility/     # Core Udon scripts (Invoke, GestureInvoke, TriggerInvoke)
│   ├── Toggles/     # Toggle behaviors (TriggerToggle)
│   ├── UI/          # UI components (ButtonCooldown, PanelActivator)
│   ├── Pickups/     # VRC pickup utilities
│   └── Enums/       # Gesture and action enums
└── Resources/       # Materials, fonts, images
```

### Key Architectural Patterns

#### 1. **Assembly Definition Structure**
- **Editor assemblies**: `valenvrc.Common.Editor` → references UdonSharpEditor, VRCSDKBase
- **Runtime assemblies**: `valenvrc.Common.Runtime` → references UdonSharp, VRC SDKs
- **Namespace convention**: `valenvrc.Common` (runtime), `valenvrc.Common.Editor.*` (editor)

#### 2. **Define Symbol Management** ([Editor/Core/DefineManager.cs](Editor/Core/DefineManager.cs))
Automatically injects scripting defines based on installed assets:
- Adds `VALENVRC_COMMON` and `VALENVRC_[ASSETNAME]` symbols
- Executes on script reload via `[DidReloadScripts]`
- Manages defines for multi-package dependencies (AdminTablet, SecurityKeypad, etc.)

#### 3. **Invoke Pattern** (Core Runtime Concept)
Common pattern for triggering UdonBehaviour events:
- **Base**: `Invoke.cs` - Interact to call methods on target UdonBehaviours
- **Variants**: `TriggerInvoke`, `GestureInvoke`, `PickupInvoke`
- **Storage**: Parallel arrays `UdonBehaviour[] targets` + `string[] methodNames`
- **Custom Editor**: [Editor/CustomEditors/InvokeEditor.cs](Editor/CustomEditors/InvokeEditor.cs) provides ReorderableList UI

```csharp
// Example usage pattern
[SerializeField] UdonBehaviour[] targets;
[SerializeField] string[] methodNames;
targets[i].SendCustomEvent(methodNames[i]);
```

#### 4. **VRChat-Specific Attributes**
All runtime scripts use:
```csharp
[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
[Icon("Packages/com.valenvrc.common/Editor/Resources/ValenFace.jpg")]
[HelpURL("https://discord.gg/nv5ax3wDqc")]
public class MyScript : UdonSharpBehaviour
```

## Development Conventions

### Naming & Style
- **Namespace**: Always `valenvrc.Common` or subnamespaces
- **Brace style**: Opening braces on same line (K&R style)
- **Serialized fields**: camelCase, no prefix
- **Private fields**: camelCase (no m_ prefix)
- **Icons**: Reference package resources via `Packages/com.valenvrc.common/Editor/Resources/`

### UdonSharp Constraints
- **No properties with logic** - Use public methods instead
- **No LINQ** - Use explicit loops and arrays
- **No async/await** - Use `SendCustomEventDelayedSeconds()`
- **Array initialization**: Always specify size `new GameObject[1]`
- **Udon method naming**: Use PascalCase for public custom events

### Editor Script Patterns

#### ReorderableList Creation ([Editor/Utilities/EditorUtilities.cs](Editor/Utilities/EditorUtilities.cs))
```csharp
ReorderableList list = EditorUtilities.CreateReorderableList(
    serializedObject,
    targetsProperty,      // First property (displayed left)
    methodNameProperty,   // Second property (displayed right)
    "Target:",           // First label
    "Target tooltip",
    "Method:",           // Second label  
    "Method tooltip",
    "Header Text"
);
```
**Critical**: Handles synchronized array deletion for parallel arrays.

#### Custom Dialog
```csharp
EditorUtilities.CustomDialog.ShowDialog("Title", "Message", texture2D);
```

### VRCObjectPool Integration
Use [Editor/Utilities/ObjectPoolUtility.cs](Editor/Utilities/ObjectPoolUtility.cs) for quick pool setup:
- Adds context menu "Fill Object Pool With All Childs"
- Appends all child GameObjects to VRCObjectPool.Pool array

## Testing & Validation
- **VRChat ClientSim**: Test locally before uploading
- **Build validation**: Ensure no Editor-only code in Runtime assemblies
- **Define symbols**: Run "Valenvrc/Reload Defines" menu if symbols missing

## Package Integration
- **VPM package**: Installed via VRChat Creator Companion
- **Dependencies**: Other valenvrc packages reference this via `com.valenvrc.common`
- **Asset registry**: [Editor/Core/ValenAssets.cs](Editor/Core/ValenAssets.cs) tracks known assets for define management

## Common Pitfalls
1. **Don't** use Unity's standard `MonoBehaviour` in Runtime/ - use `UdonSharpBehaviour`
2. **Don't** reference Editor assemblies from Runtime assemblies
3. **Do** include `.meta` files for Unity asset tracking
4. **Do** maintain parallel array indices when using Invoke pattern
5. **Do** check `player.isLocal` before local-only operations in triggers

## Contact & Support
Discord: https://discord.gg/6AcpahXKQu
