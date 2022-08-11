using UnityEditor;
using UnityEditor.Experimental.AssetImporters;
using UnityEngine;
using System.Reflection;
using System;
using System.Collections;
using System.Linq;
using Object = UnityEngine.Object;
using System.Collections.Generic;
using UnityEditor.AssetImporters;

// Found here: https://gist.github.com/DataGreed/c4ac8fa738304893f060d04b3b683e13
// Reference for trying to get Prefabs to display correctly. Not sure how to use it fully, but it's helping

/*
public class CustomPrefabImporterEditor : AssetImporterEditor
{
    #region Injection
    
     // * See Editor/Mono/CustomEditorAttributes.cs for original logic of getting an editor by type. The UnityEditor.CustomEditorAttributes class handles this
     // * 
     // * We sneak ourselfs in by
     // * 1. Call FindCustomEditorTypeByType to initialize state
     // * 2. Modify the kSCustomEditors and kSCustomMultiEditors lists with our class for the type 'UnityEditor.PrefabImporter'
     // * 3. ??
     // * 4. Profit!
     

    //We have to find the types in this assembly
    private static readonly Assembly editorAssembly = Assembly.GetAssembly(typeof(AssetImporterEditor));
    private static readonly string customEditorAttributes = "UnityEditor.CustomEditorAttributes",
        findCustomEditorTypeByType = "FindCustomEditorTypeByType",
        kSCustomEditors = "kSCustomEditors",
        kSCustomMultiEditors = "kSCustomMultiEditors",
        prefabImporter = "PrefabImporter",
        m_InspectorType = "m_InspectorType";

    [InitializeOnLoadMethod]
    private static void inject()
    {
        callPrivateStaticMethod(customEditorAttributes, findCustomEditorTypeByType, null, false); //null as type to get does an early out. 

        var editorTypeDictionary = getPrivateStaticField<ICollection>(customEditorAttributes, kSCustomEditors);

        //editorTypeKey is of type KeyValuePair<Type, List<UnityEditor.CustomEditorAttributes.MonoEditorType>>, but we cant cast it. Reflection all the way!
        foreach (var editorTypeDictEntry in editorTypeDictionary)
        {
            if (getPublicProperty<Type>(editorTypeDictEntry, "Key").Name != prefabImporter) continue; //Only our importer

            //editor is of type UnityEditor.CustomEditorAttribute.MonoEditorType
            foreach (var editor in getPublicProperty<IList>(editorTypeDictEntry, "Value"))
                setPublicField(editor, m_InspectorType, typeof(CustomPrefabImporterEditor));
        }
    }

    private static object callPrivateStaticMethod(string typeName, string methodName, params object[] parameters)
    {
        var type = editorAssembly.GetType(typeName, true);
        var method = type.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static);

        return method.Invoke(null, parameters);
    }

    private static T getPrivateStaticField<T>(string typeName, string fieldName)
    {
        var type = editorAssembly.GetType(typeName, true);
        var field = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static);
        return (T)field.GetValue(null);
    }

    private static T getPublicProperty<T>(object target, string fieldName)
    {
        return (T)target.GetType().GetProperty(fieldName, BindingFlags.Public | BindingFlags.Instance).GetValue(target);
    }

    public static void setPublicField(object target, string fieldName, object value)
    {
        target.GetType().GetField(fieldName, BindingFlags.Public | BindingFlags.Instance).SetValue(target, value);
    }
    #endregion

    #region Inspector GUI
    private Editor[] targetEditors;

    public override void OnEnable()
    {
        base.OnEnable();

        if (assetTargets == null) return;

        if (assetTargets.Length <= 1)
        {
            var targets = (assetTarget as GameObject).GetComponents<Component>();
            targetEditors = new Editor[targets.Length + 1];
            assetTarget.hideFlags = HideFlags.None;
            targetEditors[0] = CreateEditor(assetTarget);
            for (int i = 0; i < targets.Length; i++)
            {
                targets[i].hideFlags = HideFlags.None; //This is the magic that makes them editable, by default they are set to NonEditable.
                targetEditors[i + 1] = CreateEditor(targets[i]);
            }
        }
        else //multiple
        {
            //Get all components that are shared on all objects
            var commonComponents = assetTargets.SelectMany(a => (a as GameObject).GetComponents<Component>()).GroupBy(c => c.GetType())
                .Where(g => g.Count() > 1);

            foreach (var t in assetTargets) t.hideFlags = HideFlags.None;

            targetEditors = new Editor[commonComponents.Count() + 1];
            targetEditors[0] = CreateEditor(assetTargets);

            int i = 1;
            foreach (var group in commonComponents)
            {
                foreach (var c in group)
                    c.hideFlags = HideFlags.None;
                targetEditors[i++] = CreateEditor(group.ToArray());
            }
        }
    }

    public override void OnInspectorGUI()
    {
        for (int i = 0; i < targetEditors.Length; i++)
        {
            EditorGUIUtility.labelWidth = 0;
            targetEditors[i].DrawHeader();

            EditorGUI.BeginChangeCheck();
            targetEditors[i].OnInspectorGUI();

            //When the object is changed it is reimported, and our editors point to incorrect objects. Restart to create new editors!
            if (EditorGUI.EndChangeCheck())
            {
                OnEnable();
                return;
            }
        }

        using (new EditorGUI.DisabledScope(assetTargets.Length > 1))
        {
            if (GUILayout.Button("Open Prefab"))
            {
                AssetDatabase.OpenAsset(assetTarget);

                GUIUtility.ExitGUI();
            }
        }
    }

    protected override void OnHeaderGUI() { }
    public override bool showImportedObject { get { return false; } }
    #endregion
}
*/