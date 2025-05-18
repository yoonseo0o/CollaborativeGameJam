using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
public class CutScene:MonoBehaviour
{
    [Header("컷신 담을 오브젝트")]
    [SerializeField] private Image sceneImg;
    private Button sceneButton;
    [Header("컷신 리소스")]
    [SerializeField] private Sprite[] startSceneSource;
    [SerializeField] private Sprite[] clearSceneSource;
    private int currentIndex;
    private void Awake()
    {
        sceneButton = sceneImg.transform.GetComponent<Button>();
        ResetIndex();
    }
    public void ActiveImg(bool IsActive)
    {
        sceneImg.enabled = IsActive;
        sceneButton.enabled = IsActive;
    }
    public void ResetIndex()
    {
        currentIndex = 0;
    }
    public void StartCutSceneNextPage()
    {
        if (currentIndex >= startSceneSource.Length-1)
        {
            ActiveImg(false); ResetIndex();
            GameManager.Instance.GameStart();
            return; 
        }
        sceneImg.sprite = startSceneSource[++currentIndex];
    }
    public void ClearCutSceneNextPage()
    {
        if (currentIndex >= clearSceneSource.Length-1)
            return;
        sceneImg.sprite = clearSceneSource[++currentIndex];
    }
}