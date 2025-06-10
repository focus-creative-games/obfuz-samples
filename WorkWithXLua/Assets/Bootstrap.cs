using Obfuz;
using Obfuz.EncryptionVM;
using System.Collections;
using System.Collections.Generic;
using Tutorial;
using UnityEngine;


public class Bootstrap : MonoBehaviour
{
    // ��ʼ��EncryptionService�󱻻����Ĵ�������������У�
    // ��˾����ܵ���س�ʼ������
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
    private static void SetUpStaticSecretKey()
    {
        //Debug.Log("SetUpStaticSecret begin");
        EncryptionService<DefaultStaticEncryptionScope>.Encryptor = new GeneratedEncryptionVirtualMachine(Resources.Load<TextAsset>("Obfuz/defaultStaticSecretKey").bytes);
        //Debug.Log("SetUpStaticSecret end");
    }

    private void Awake()
    {
        ObfuscationInstincts.RegisterReflectionType<BaseClass>();
        ObfuscationInstincts.RegisterReflectionType<DerivedClass>();
        ObfuscationInstincts.RegisterReflectionType<Tutorial.ICalc>();
        ObfuscationInstincts.RegisterReflectionType<DerivedClass.InnerCalc>();
        ObfuscationInstincts.RegisterReflectionType<DerivedClass.TestEnumInner>();
        ObfuscationInstincts.RegisterReflectionType<Tutorial.TestEnum>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Hello, Obfuz");
        //int a = Add(10, 20);
        //Debug.Log($"a = {a}");
    }
}
