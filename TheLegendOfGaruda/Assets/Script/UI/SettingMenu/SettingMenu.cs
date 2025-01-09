using UnityEngine;

public class SettingMenu : MonoBehaviour
{
    public void OpenSetting()
    {
        gameObject.SetActive(true);
    }

    public void CloseSetting()
    {
        gameObject.SetActive(false);
    }
}
