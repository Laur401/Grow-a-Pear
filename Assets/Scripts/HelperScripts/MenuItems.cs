using UnityEditor;
using UnityEngine;

namespace HelperScripts
{
    public class MenuItems
    {
        private static string audioPrefabPath = "Assets/SimpleAudioManager/Prefabs/SimpleAudioManager.prefab";
        [MenuItem("GameObject/Custom/Audio Manager")]
        static void AudioManager()
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(audioPrefabPath);
            if (prefab)
            {
                GameObject newObject = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                newObject.transform.position = Vector3.zero;
                Selection.activeGameObject = newObject;
            }
        }
    }
}