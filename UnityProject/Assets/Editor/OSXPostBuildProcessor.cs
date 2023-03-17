using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.iOS.Xcode;
using UnityEngine;

public class OSXPostBuildProcessor : IPostprocessBuildWithReport
{
    // Set it to a higer number to make sure it gets called after other postprocess scripts.
    public int callbackOrder { get { return 100; } }

    public void OnPostprocessBuild(BuildReport report)
    {
        if (report.summary.platform != BuildTarget.StandaloneOSX)
            return;

        PatchPlist(report.summary.outputPath);
    }

    private static void PatchPlist(string buildOutputPath)
    {
        var plistPath = buildOutputPath + "/Contents/Info.plist";
        if(!File.Exists(plistPath))
        {
            Debug.Log("The plist file doesn't exist.");
            return;
        }

        var plist = new PlistDocument();
        plist.ReadFromString(File.ReadAllText(plistPath));

        var rootDict = plist.root;
        var urlTypesArray = rootDict.CreateArray("CFBundleURLTypes");
        var dict = urlTypesArray.AddDict();
        dict.SetString("CFBundleURLName", "Vincent's handler");
        var urlSchemesArray = dict.CreateArray("CFBundleURLSchemes");
        urlSchemesArray.AddString("vincenttest1");

        File.WriteAllText(plistPath, plist.WriteToString());
    }
}
