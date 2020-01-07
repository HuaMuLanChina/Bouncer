using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(blocks))]
public class InspecWalls : Editor
{
    blocks blk;
    private void OnEnable()
    {
        blk = (blocks)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if(GUILayout.Button("fire"))
        {
            blk.setwall();
        }
    }
}
