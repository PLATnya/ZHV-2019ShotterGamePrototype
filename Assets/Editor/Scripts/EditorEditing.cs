using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
[ExecuteAlways]
public class EditorEditing : MonoBehaviour
{
   static Transform[] trans;

    
    static System.Collections.Generic.Dictionary<int, Transform> dick;

    /*
    [MenuItem("Example/Save Scene while on play mode")]
    static void EditorPlaying()
    {
        if (EditorApplication.isPlaying)
        {
            string sceneName = EditorApplication.currentScene;
            string[] path = sceneName.Split(char.Parse("/"));
            path[path.Length - 1] = "Temp_" + path[path.Length - 1];
            var tempScene = string.Join("/", path);

            

            EditorApplication.SaveCurrentSceneIfUserWantsTo();

            EditorApplication.isPaused = false;
            EditorApplication.isPlaying = false;

            FileUtil.DeleteFileOrDirectory(EditorApplication.currentScene);
            FileUtil.MoveFileOrDirectory(tempScene, sceneName);
            FileUtil.DeleteFileOrDirectory(tempScene);

            EditorApplication.OpenScene(sceneName);
        }
    }
    */
    [MenuItem("Example/Load")]
    static void Load()
    {

        trans = FindObjectsOfType(typeof(Transform)) as Transform[];
        for (int i = 0; i < trans.Length; i++)
        {
            if (trans[i].tag == "Player"||trans[i].tag =="MainCamera"||trans[i].tag =="Ignore") continue;
            trans[i].position = new Vector3(PlayerPrefs.GetFloat("X" + trans[i].GetHashCode()), PlayerPrefs.GetFloat("Y" + trans[i].GetHashCode()), PlayerPrefs.GetFloat("Z" + trans[i].GetHashCode()));
            if (trans[i].gameObject.tag == "Scaleble")
            {
                trans[i].localScale = new Vector3(PlayerPrefs.GetFloat("Xx" + trans[i].GetHashCode()), PlayerPrefs.GetFloat("Yy" + trans[i].GetHashCode()), PlayerPrefs.GetFloat("Zz" + trans[i].GetHashCode()));
            }
        }
        
        

       // FindObjectsOfType<Transform>()[0].position= new Vector3(PlayerPrefs.GetFloat("X"), PlayerPrefs.GetFloat("Y"), PlayerPrefs.GetFloat("Z"));
        print("Fuck"); 
        
    }
    [MenuItem("Example/Save")]
    static void Save()
    {

        //GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
        trans = FindObjectsOfType(typeof(Transform)) as Transform[];
        
        for (int i = 0;i<trans.Length; i++)
        {
            if (trans[i].tag == "Player" || trans[i].tag == "MainCamera" || trans[i].tag == "Ignore") continue;
            PlayerPrefs.SetFloat("X"+trans[i].GetHashCode(), trans[i].position.x);
            PlayerPrefs.SetFloat("Y"+ trans[i].GetHashCode(), trans[i].position.y);
            PlayerPrefs.SetFloat("Z"+ trans[i].GetHashCode(), trans[i].position.z);

            if (trans[i].gameObject.tag == "Scaleble")
            {
                PlayerPrefs.SetFloat("Xx" + trans[i].GetHashCode(), trans[i].localScale.x);
                PlayerPrefs.SetFloat("Yy" + trans[i].GetHashCode(), trans[i].localScale.y);
                PlayerPrefs.SetFloat("Zz" + trans[i].GetHashCode(), trans[i].localScale.z);
            }
        }
        
        PlayerPrefs.Save();
    }

    
    
}
