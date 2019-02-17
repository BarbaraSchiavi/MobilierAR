/*============================================================================== 
Copyright (c) 2015-2017 PTC Inc. All Rights Reserved.

Copyright (c) 2015 Qualcomm Connected Experiences, Inc. All Rights Reserved. 

Vuforia is a trademark of PTC Inc., registered in the United States and other 
countries.   
==============================================================================*/
using UnityEngine;
using System.Collections;

public class Reticle : MonoBehaviour
{
    #region PRIVATE_METHODS
    private const float mScale = 0.012f; // relative to viewport width
    #endregion

    #region PUBLIC_MEMBER_VARIABLES
    public float activationTime = 1.5f;
    public Material focusedMaterial;
    public Material nonFocusedMaterial;
    public bool Focused { get; set; }
    #endregion // PUBLIC_MEMBER_VARIABLES

    #region MONOBEHAVIOUR_METHODS
    void Update()
    {
        Camera cam = Camera.main;//DigitalEyewearARController.Instance.PrimaryCamera ?? Camera.main;
        if (cam.projectionMatrix.m00 > 0 || cam.projectionMatrix.m00 < 0)
        {

            // if the frustum is not skewed, then we apply a default depth (which works nicely in VR view)
            this.transform.localPosition = Vector3.forward * (cam.nearClipPlane + 0.5f);
            

            // We scale the reticle to be a small % of viewport width
            float localDepth = this.transform.localPosition.z;
            float tanHalfFovX = 1.0f / cam.projectionMatrix[0, 0];
            float tanHalfFovY = 1.0f / cam.projectionMatrix[1, 1];
            float maxTanFov = Mathf.Max(tanHalfFovX, tanHalfFovY);
            float viewWidth = 2 * maxTanFov * localDepth;
            this.transform.localScale = new Vector3(mScale * viewWidth * 2, mScale * viewWidth * 2, 1);
        }

        RaycastHit hit;
        Ray cameraGaze = new Ray(cam.transform.position, cam.transform.forward);
        Physics.Raycast(cameraGaze, out hit, Mathf.Infinity);
        Focused = hit.collider;// && (hit.collider.gameObject == gameObject);

        UpdateMaterials(Focused);
    }
    #endregion // MONOBEHAVIOUR_METHODS

    #region PRIVATE_METHODS
    private void UpdateMaterials(bool focused)
    {
        Renderer meshRenderer = GetComponent<Renderer>();

        meshRenderer.material = focused ? focusedMaterial : nonFocusedMaterial;

        //float t = focused ? Mathf.Clamp01(mFocusedTime / activationTime) : 0;

        //foreach (var rnd in GetComponentsInChildren<Renderer>())
        //{
        //    if (rnd.material.shader.name.Equals("Custom/SurfaceScan"))
        //    {
        //        rnd.material.SetFloat("_ScanRatio", t);
        //    }
        //}
    }

    //private IEnumerator ResetAfter(float seconds)
    //{
    //    Debug.Log("Resetting View trigger after: " + seconds);

    //    yield return new WaitForSeconds(seconds);

    //    Debug.Log("Resetting View trigger: " + name);

    //    // Reset variables
    //    mTriggered = false;
    //    mFocusedTime = 0;
    //    Focused = false;
    //    UpdateMaterials(false);
    //}
    #endregion // PRIVATE_METHODS
}
