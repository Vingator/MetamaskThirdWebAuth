using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;

#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif

public class PostBuildScript
{
    [PostProcessBuild]
    public static void OnPostProcessBuild(BuildTarget target, string pathToBuiltProject) {
        if (target != BuildTarget.iOS) {
            return;
        }
#if UNITY_IOS
        var projPath = pathToBuiltProject + "/Unity-iPhone.xcodeproj/project.pbxproj";
        SetCompilingFlags(projPath);
#endif
    }
#if UNITY_IOS
    private static void SetCompilingFlags(string projPath) {
        var proj = new PBXProject();
        proj.ReadFromString(File.ReadAllText(projPath));
        var targetGUID = proj.GetUnityFrameworkTargetGuid();
        proj.AddBuildProperty(targetGUID, "OTHER_LDFLAGS", "-ld_classic"); 
        File.WriteAllText(projPath, proj.WriteToString());
    }
#endif
}
