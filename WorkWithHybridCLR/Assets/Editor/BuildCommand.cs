using HybridCLR.Editor;
using HybridCLR.Editor.Commands;
using Obfuz.Settings;
using Obfuz4HybridCLR;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class BuildCommand
{
    [MenuItem("Build/CompileAndObfuscateAndCopyToStreamingAssets")]
    public static void CompileAndObfuscateAndCopyToStreamingAssets()
    {
        BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
        CompileDllCommand.CompileDll(target);

        string obfuscatedHotUpdateDllPath = PrebuildCommandExt.GetObfuscatedHotUpdateAssemblyOutputPath(target);
        ObfuscateUtil.ObfuscateHotUpdateAssemblies(target, obfuscatedHotUpdateDllPath);

        Directory.CreateDirectory(Application.streamingAssetsPath);

        string hotUpdateDllPath = $"{SettingsUtil.GetHotUpdateDllsOutputDirByTarget(target)}";
        List<string> obfuscationRelativeAssemblyNames = ObfuzSettings.Instance.assemblySettings.GetObfuscationRelativeAssemblyNames();

        if (ObfuzSettings.Instance.polymorphicDllSettings.enable)
        {
            {
                string inputDir = SettingsUtil.GetAssembliesPostIl2CppStripDir(target);
                string oldDll = $"{inputDir}/mscorlib.dll";
                string newDll = $"{Application.streamingAssetsPath}/mscorlib.dll.bytes";
                ObfuscateUtil.GeneratePolymorphicDll(oldDll, newDll);
            }
            {
                string intpuDir = SettingsUtil.GetHotUpdateDllsOutputDirByTarget(target);
                string oldDll = $"{intpuDir}/HotUpdate.dll";
                string newDll = $"{Application.streamingAssetsPath}/HotUpdate.dll.bytes";
                ObfuscateUtil.GeneratePolymorphicDll(oldDll, newDll);
            }
        }
        else
        {
            foreach (string assName in SettingsUtil.HotUpdateAssemblyNamesIncludePreserved)
            {
                string srcDir = obfuscationRelativeAssemblyNames.Contains(assName) ? obfuscatedHotUpdateDllPath : hotUpdateDllPath;
                string srcFile = $"{srcDir}/{assName}.dll";
                string dstFile = $"{Application.streamingAssetsPath}/{assName}.dll.bytes";
                if (File.Exists(srcFile))
                {
                    File.Copy(srcFile, dstFile, true);
                    Debug.Log($"[CompileAndObfuscate] Copy {srcFile} to {dstFile}");
                }
            }

        }
    }

    [MenuItem("Build/TestGenPolymorphicDlls")]
    public static void TestGenPolymorphicDlls()
    {
        BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
        {
            string inputDir = SettingsUtil.GetAssembliesPostIl2CppStripDir(target);
            string oldDll = $"{inputDir}/mscorlib.dll";
            string newDll = $"{inputDir}/mscorlib.dll.bytes";
            ObfuscateUtil.GeneratePolymorphicDll(oldDll, newDll);
        }
        {
            string intpuDir = SettingsUtil.GetHotUpdateDllsOutputDirByTarget(target);
            string oldDll = $"{intpuDir}/HotUpdate.dll";
            string newDll = $"{intpuDir}/HotUpdate.dll.bytes";
            ObfuscateUtil.GeneratePolymorphicDll(oldDll, newDll);
        }
    }
}
