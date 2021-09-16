namespace Apex.GettingStarted
{
    using UnityEditor;
    using UnityEngine;

    [InitializeOnLoad]
    public class LayersCreator
    {
        static LayersCreator()
        {
            EnsureLayers(
                new Layer(8, "Enemies"),
                new Layer(9, "StaticObstacles"),
                new Layer(10, "Terrain"),
                new Layer(11, "Units"));
        }

        private static void EnsureLayers(params Layer[] layers)
        {
            var arr = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset");
            if (arr == null || arr.Length == 0)
            {
                Debug.LogWarning("Unable to set up default layers.");
                return;
            }

            var tagManagerAsset = new SerializedObject(arr[0]);
#if UNITY_5
            SerializedProperty layersProp = tagManagerAsset.FindProperty("layers");
#endif
            for (int i = 0; i < layers.Length; i++)
            {
#if UNITY_5
                var sp = layersProp.GetArrayElementAtIndex(layers[i].index);
#else
                var layerPropName = "User Layer " + layers[i].index;
                var sp = tagManagerAsset.FindProperty(layerPropName);
#endif
                if (sp == null)
                {
                    continue;
                }

                sp.stringValue = layers[i].name;
            }

            tagManagerAsset.ApplyModifiedProperties();
        }

        private struct Layer
        {
            public Layer(int index, string name)
            {
                this.index = index;
                this.name = name;
            }

            public int index;
            public string name;
        }
    }
}
