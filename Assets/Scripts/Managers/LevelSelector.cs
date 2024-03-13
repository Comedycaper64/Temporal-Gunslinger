using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelector : MonoBehaviour
{
    // private bool selectorActive = false;

    // [SerializeField]
    // private Transform selectorArrow;
    // public static EventHandler<int> OnSelectLevel;

    // private void Start()
    // {
    //     ToggleLevelSelector(true);
    // }

    // private void Update()
    // {
    //     if (selectorActive)
    //     {
    //         Vector3 mousePosition = InputManager.Instance.GetMousePosition();
    //         mousePosition.z = 10f;
    //         Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
    //         mouseWorldPosition.z = 0.5f;
    //         Debug.Log(mouseWorldPosition);
    //         selectorArrow.LookAt(mouseWorldPosition, Vector3.up);
    //         selectorArrow.rotation = Quaternion.Euler(
    //             new Vector3(0, 0, selectorArrow.eulerAngles.z)
    //         );
    //     }
    // }

    // public void ToggleLevelSelector(bool toggle)
    // {
    //     selectorActive = toggle;
    //}
}
