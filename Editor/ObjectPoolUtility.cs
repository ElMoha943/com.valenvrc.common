using UnityEngine;
using UnityEditor;
using VRC.SDK3.Components;

//Context Menu for Object Pool
//When right clicking on the Object Pool in the hierarchy, the followin options appear
//1. Fill Object pool with all the childs of the gameobject
[CustomEditor(typeof(VRCObjectPool), true)]
public class ObjectPoolUtility : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw default inspector UI
        DrawDefaultInspector();
        // Add a button to the inspector
        if (GUILayout.Button("Fill Object Pool With All Childs"))
        {
            FillObjectPoolWithAllChilds();
        }
    }

    [ContextMenu("Fill Object Pool With All Childs")]
    void FillObjectPoolWithAllChilds(){

        // Obtén el GameObject en el cual está ligado este script
        VRCObjectPool myComponent = (VRCObjectPool)target;
        GameObject go = myComponent.gameObject;

        VRCObjectPool objectPool = go.GetComponent<VRCObjectPool>();
        if (objectPool == null){
            Debug.LogError("No ObjectPool component found on the selected gameobject");
            return;
        }

        GameObject[] newPool = new GameObject[objectPool.Pool.Length+go.transform.childCount];
        for (int i = objectPool.Pool.Length; i < newPool.Length; i++){
            GameObject obj = go.transform.GetChild(i-objectPool.Pool.Length).gameObject;
            if(obj == null){
                return;
            }
            newPool[i] = obj;
        }
        objectPool.Pool = newPool;
    }
}
