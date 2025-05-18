using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
public class CutScene
{
    [Header("컷신 담을 오브젝트")]
    [SerializeField] private Image sceneImg; 
    [Header("컷신 리소스")]
    [SerializeField] private Sprite[] startSceneSource;
    [SerializeField] private Sprite[] clearSceneSource;

}