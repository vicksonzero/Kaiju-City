using System;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class BuildAutoIncrement : IPreprocessBuildWithReport
{
    private IPreprocessBuildWithReport _preprocessBuildWithReportImplementation;
    private int _callbackOrder;

    int IOrderedCallback.callbackOrder => 1;

    void IPreprocessBuildWithReport.OnPreprocessBuild(BuildReport report)
    {
        var so = ScriptableObject.CreateInstance<BuildNumberSO>();

        so.buildNumber = IncrementBuildNumber(PlayerSettings.bundleVersion);
        switch (report.summary.platform)
        {
            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneWindows64:
            case BuildTarget.StandaloneOSX:
                so.buildNumber = IncrementBuildNumber(PlayerSettings.macOS.buildNumber);
                PlayerSettings.macOS.buildNumber = so.buildNumber.ToString();
                break;
            case BuildTarget.iOS:
                so.buildNumber = IncrementBuildNumber(PlayerSettings.iOS.buildNumber);
                PlayerSettings.iOS.buildNumber = so.buildNumber.ToString();
                break;
            case BuildTarget.Android:
                so.buildNumber = PlayerSettings.Android.bundleVersionCode + 1;
                PlayerSettings.Android.bundleVersionCode = so.buildNumber;
                break;
            case BuildTarget.Switch:
                so.buildNumber = IncrementBuildNumber(PlayerSettings.Switch.displayVersion);
                PlayerSettings.Switch.displayVersion = so.buildNumber.ToString();
                break;
            case BuildTarget.WebGL:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        PlayerSettings.bundleVersion = so.buildNumber.ToString();

        AssetDatabase.DeleteAsset(BuildNumberSO.BuildAssetPath);
        AssetDatabase.CreateAsset(so, BuildNumberSO.BuildAssetPath);
        AssetDatabase.SaveAssets();
    }

    int IncrementBuildNumber(string buildNumber)
    {
        int.TryParse(buildNumber, out var output);

        return output + 1;
    }
}