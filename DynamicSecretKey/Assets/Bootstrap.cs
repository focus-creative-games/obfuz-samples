using Obfuz;
using Obfuz.EncryptionVM;
using UnityEngine;


public class Bootstrap : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
    private static void SetUpStaticSecretKey()
    {
        Debug.Log("SetUpStaticSecret begin");
        EncryptionService<DefaultStaticEncryptionScope>.Encryptor = new GeneratedEncryptionVirtualMachine(Resources.Load<TextAsset>("Obfuz/defaultStaticSecretKey").bytes);
        Debug.Log("SetUpStaticSecret end");
    }

    private static void SetUpDynamicSecret()
    {
        Debug.Log("SetUpDynamicSecret begin");
        EncryptionService<DefaultDynamicEncryptionScope>.Encryptor = new GeneratedEncryptionVirtualMachine(Resources.Load<TextAsset>("Obfuz/defaultDynamicSecretKey").bytes);
        Debug.Log("SetUpDynamicSecret end");
    }


    void Start()
    {
        // �ӳټ��أ���ʹ��HotUpdate���򼯴���ǰ�ż��ض�̬��Կ��
        // �����Ŀ�õ����ȸ��£�һ����˵���ȸ�����ɺ󣬼���
        // �ȸ��´���ǰ�ż��ض�̬��Կ��
        SetUpDynamicSecret();
        this.gameObject.AddComponent<Entry>();
    }
}
