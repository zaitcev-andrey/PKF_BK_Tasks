using UnityEngine;

public class CellEye : MonoBehaviour
{
    private static InterfaceManager manager;

    private void Start()
    {
        manager = GameObject.FindObjectOfType<InterfaceManager>();
    }

    public static void SwitchLocalEyeOnClick(int index)
    {
        manager.AllObjectsInInterface[index].IsChange = true;
        manager.AllObjectsInInterface[index].IsEyeChange = true;
    }

    public static void SwitchGlobalEyeOnClick()
    {
        manager.IsGlobalEyesChange = true;
    }
}
