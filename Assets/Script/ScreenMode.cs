using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenMode : MonoBehaviour
{
    public TMP_Dropdown screenModeDropdown; // TMP_Dropdown 쓰면 형식 바꿔줘야 함

    void Start()
    {
        // 현재 화면 모드에 따라 드롭다운 초기화
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
            case 1: // 전체화면
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
            case 0: // 창모드
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 2: // 최대화 창모드
                Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
                break;
        }
    }
}