using HybridCLR;
using Obfuz;
using Obfuz.EncryptionVM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;


public class Bootstrap : MonoBehaviour
{
    // ��ʼ��EncryptionService�󱻻����Ĵ�������������У�
    // ��˾����ܵ���س�ʼ������
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
    private static void SetUpStaticSecretKey()
    {
        Debug.Log("SetUpStaticSecret begin");
        EncryptionService<DefaultStaticEncryptionScope>.Encryptor = new GeneratedEncryptionVirtualMachine(Resources.Load<TextAsset>("Obfuz/defaultStaticSecretKey").bytes);
        Debug.Log("SetUpStaticSecret end");
    }


    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
        Assembly ass = AppDomain.CurrentDomain.GetAssemblies().First(ass => ass.GetName().Name == "HotUpdate");
#else
        LoadMetadataForAOTAssemblies();
        Assembly ass = Assembly.Load(File.ReadAllBytes($"{Application.streamingAssetsPath}/HotUpdate.dll.bytes"));
#endif
        Type entry = ass.GetType("Entry");
        this.gameObject.AddComponent(entry);
    }

    private static void LoadMetadataForAOTAssemblies()
    {
        HomologousImageMode mode = HomologousImageMode.SuperSet;
        var aotDlls = new string[] { "mscorlib" };
        foreach (var aotDllName in aotDlls)
        {
            byte[] dllBytes = File.ReadAllBytes($"{Application.streamingAssetsPath}/{aotDllName}.dll.bytes");
            LoadImageErrorCode err = RuntimeApi.LoadMetadataForAOTAssembly(dllBytes, mode);
            Debug.Log($"LoadMetadataForAOTAssembly:{aotDllName}. mode:{mode} ret:{err}");
        }
    }
}
