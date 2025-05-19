using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenMode : MonoBehaviour
{
    public TMP_Dropdown screenModeDropdown; // TMP_Dropdown ���� ���� �ٲ���� ��

    void Start()
    {
        // ���� ȭ�� ��忡 ���� ��Ӵٿ� �ʱ�ȭ
        switch (Screen.fullScreenMode)
        {
            case FullScreenMode.FullScreenWindow:
                screenModeDropdown.value = 0;
                break;
            case FullScreenMode.Windowed:
                screenModeDropdown.value = 1;
                break;
            case FullScreenMode.MaximizedWindow:
                screenModeDropdown.value = 2;
                break;
        }

        screenModeDropdown.onValueChanged.AddListener(ChangeScreenMode);
    }
    void ChangeScreenMode(int index)
    {
        switch (index)
        {
            case 1: // ��üȭ��
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
            case 0: // â���
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 2: // �ִ�ȭ â���
                Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
                break;
        }
    }
}