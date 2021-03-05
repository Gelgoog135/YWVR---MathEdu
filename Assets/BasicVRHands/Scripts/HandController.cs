using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using System;

public class HandController : MonoBehaviour {

    //private Animator animator;

    //private ActionBasedController controller;
    //// Start is called before the first frame update

    //void Start () {
    //    animator = GetComponent<Animator>();
    //    controller = GetComponent<ActionBasedController>();

    //    controller.selectAction.action.performed += SelectAction_Performed;
    //    controller.selectAction.action.canceled += SelectAction_Canceled;
    //    animator.updateMode = AnimatorUpdateMode.AnimatePhysics;
    //    animator.updateMode = AnimatorUpdateMode.UnscaledTime;
    //    animator.updateMode = AnimatorUpdateMode.Normal;
    //}

    //private void SelectAction_Canceled(InputAction.CallbackContext obj)
    //{
    //    Debug.Log("cancelled");
    //    animator.SetBool("isGrabbing", false);
    //}

    //private void SelectAction_Performed(InputAction.CallbackContext obj)
    //{
    //    animator = GetComponent<Animator>();
    //    Debug.Log("performed");
    //    animator.SetBool("isGrabbing", true);
    //    //animator.SetTrigger("Grab");
    //}

    //void Update ()
    //{
    //    //animator = GetComponent<Animator>();
    //    //controller = GetComponent<ActionBasedController>();
    //    ////animator.SetBool("isGrabbing", Input.GetKey(KeyCode.F));
    //    //try
    //    //{
    //    //    bool isGrabbing = controller.selectAction.action.ReadValue<float>() == 1;
    //    //    Debug.Log(isGrabbing);
    //    //    animator.SetBool("isGrabbing", isGrabbing);
    //    //}
    //    //catch (System.Exception)
    //    //{

    //    //}
    //}
}
