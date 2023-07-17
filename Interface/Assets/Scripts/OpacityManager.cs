using UnityEngine;

public class OpacityManager : MonoBehaviour
{
    private InterfaceManager manager;

    private void Start()
    {
        manager = GameObject.FindObjectOfType<InterfaceManager>();
    }

    /// <summary>
    /// Метод, настраивающий прозрачность для отмеченных в чекбоксе объектов на сцене.
    /// </summary>
    /// <param name="opacity"></param>
    public void ChangeOpacityOnClick(int opacity)
    {
        foreach (var item in manager.AllObjectsInInterface)
        {
            if(item.IsCheckBoxTurnOn)
            {
                item.NewOpacity = opacity;
            }
        }
    }
}
