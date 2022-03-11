using UnityEngine;
using System.Collections;
using UnityEditor;
using SpykeGames.Showcase.Core;

[CustomEditor(typeof(SlotMachine))]
public class SlotMachineEditor : Editor
{
   public override void OnInspectorGUI()
    {
        var myTarget = (SlotMachine)target;
        base.OnInspectorGUI();
        
        if(GUILayout.Button("Roll"))
        {
            myTarget.Roll();
        }
        
    }
}
