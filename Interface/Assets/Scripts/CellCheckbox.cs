using UnityEngine;

public class CellCheckbox : MonoBehaviour
{
    private static InterfaceManager manager;

    private void Start()
    {
        manager = GameObject.FindObjectOfType<InterfaceManager>();
    }

    public static void SwitchLocalCheckboxOnClick(int index)
    {
        manager.AllObjectsInInterface[index].IsChange = true;
        manager.AllObjectsInInterface[index].IsCheckBoxChange = true;
    }

    public static void SwitchGlobalCheckboxOnClick()
    {
        manager.IsGlobalCheckboxesChange = true;
    }
}
