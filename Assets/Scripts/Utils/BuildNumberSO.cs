using System;
using UnityEngine;

public class BuildNumberSO : ScriptableObject
{
    public const string BuildAssetPath = "Assets/Scripts/Utils/Resources/Build.asset";
    private const string LoadAssetPath = "Build";
    public int buildNumber;

    public static void GetAsset(Action<BuildNumberSO> callback)
    {
        // var request = Resources.LoadAsync(BuildAssetPath, typeof(BuildNumberSO));
        //
        // request.completed += (obj) =>
        // {
        //     var so = ((ResourceRequest)obj).asset as BuildNumberSO;
        //     callback(so);
        // };
        
        
        var so = Resources.Load<BuildNumberSO>(LoadAssetPath);
        callback(so);
    }
}