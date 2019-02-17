using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton3D : MonoBehaviour
{

    //public enum ButtonType
    //{
    //    ICON,
    //    MENU,
    //    ACTION
    //}

    enum ButtonState
    {
        NOT_SELECTED,
        SELECTED
        //HIGHLIGHTED
    }

    #region PUBLIC_MEMBER_VARIABLES
    //public ButtonType buttonType = ButtonType.ICON;
    public float activationTime = 1.5f;
    public Material focusedMaterial;
    public Material nonFocusedMaterial;

    public bool Focused { get; set; }
    public static int index = 1;
    #endregion // PUBLIC_MEMBER_VARIABLES

    #region PRIVATE_MEMBER_VARIABLES
    ButtonState buttonState = ButtonState.NOT_SELECTED;
    float mFocusedTime;
    bool mTriggered;
    Renderer meshRenderer;
    //TransitionManagerCustom mTransitionManager;
    //Transform cameraTransform;
    #endregion // PRIVATE_MEMBER_VARIABLES

    #region MONOBEHAVIOUR_METHODS
    // Use this for initialization
    void Start()
    {
        meshRenderer = GetComponent<Renderer>();
        meshRenderer.material = nonFocusedMaterial;
    }

    void Update()
    {
        Camera cam = Camera.main;//DigitalEyewearARController.Instance.PrimaryCamera ?? Camera.main;

        RaycastHit hit;
        Ray cameraGaze = new Ray(cam.transform.position, cam.transform.forward);
        Physics.Raycast(cameraGaze, out hit, Mathf.Infinity);
        Focused = hit.collider && (hit.collider.gameObject == this.gameObject);

        //if (mTriggered)
        //    return;


        if (Focused)
        {
            // Update the "focused state" time
            mFocusedTime += Time.deltaTime;
            if (mFocusedTime > 2.0f/*activationTime*/)
            {
                //mTriggered = true;
                mFocusedTime = 0;

                if (buttonState != ButtonState.SELECTED)
                {
                    meshRenderer.material = focusedMaterial;
                    buttonState = ButtonState.SELECTED;
                    ButtonSelected();

                }
                else if (buttonState == ButtonState.SELECTED)
                {
                    meshRenderer.material = nonFocusedMaterial;
                    buttonState = ButtonState.NOT_SELECTED;
                    ButtonDiselected();
                }

                //index++;

                // Activate transition from AR to VR or vice versa
                //bool goingBackToAR = (triggerType == TriggerType.AR_TRIGGER);
                //bool getVRState = (triggerType == TriggerType.VR_TRIGGER_HOUSE);
                //mTransitionManager.Play(goingBackToAR, getVRState);
                //StartCoroutine(ResetAfter(0.3f * mTransitionManager.transitionDuration));

            }
        }
        else
        {
            // Reset the "focused state" time
            mFocusedTime = 0;
        }


        //else if (index > 1) //set other icon selected to not_selected
        //{
        //    meshRenderer.material = nonFocusedMaterial;
        //    buttonState = ButtonState.NOT_SELECTED;
        //    ButtonDiselected();
        //    index = 1;
        //}
    }
    #endregion // MONOBEHAVIOUR_METHODS

    #region PUBLIC_METHODS
    public virtual void ButtonSelected() { }
    public virtual void ButtonDiselected() { }
    #endregion // PUBLIC_METHODS

    #region PRIVATE_METHODS
    private void UpdateMaterials(bool focused)
    {
        
        meshRenderer.material = focused ? focusedMaterial : nonFocusedMaterial;
        //buttonState = focused ? ButtonState.HIGHLIGHTED : ButtonState.NOT_SELECTED;
        
    }


    #endregion // PRIVATE_METHODS
}
