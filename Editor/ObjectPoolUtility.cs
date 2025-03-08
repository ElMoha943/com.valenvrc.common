using UnityEngine;
using UnityEditor;
using VRC.SDK3.Components;

[CustomEditor(typeof(VRCObjectPool), true)]
public class ObjectPoolUtility : Editor
{
    public override void OnInspectorGUI(){
        DrawDefaultInspector();
        if (GUILayout.Button("Fill Object Pool With All Childs")){
            FillObjectPoolWithAllChilds();
        }
    }

    [ContextMenu("Fill Object Pool With All Childs")]
    void FillObjectPoolWithAllChilds(){

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
