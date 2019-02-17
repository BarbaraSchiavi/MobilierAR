using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine;

public class ObjectBehavior : MonoBehaviour
{
    #region PUBLIC_MEMBER_VARIABLES
    public enum ObjectState
    {
        NOT_SELECTED,
        SELECTED,
        JUST_SPAWNED
    }


    public ObjectState objectSate = ObjectState.JUST_SPAWNED;
    public bool Focused { get; set; }
    public bool onTouched { get; set; }
    //public Material focusedMaterial;
    //public Material nonFocusedMaterial;
    #endregion // PUBLIC_MEMBER_VARIABLES


    #region PRIVATE_MEMBER_VARIABLES
    //Shader shaderOutline;
    //Shader shaderBump;
    //Renderer meshRenderer;
    ARSessionOrigin m_SessionOrigin;
    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
    #endregion // PRIVATE_MEMBER_VARIABLES

    #region MONOBEHAVIOUR_METHODS
    // Use this for initialization
    void Start()
    {
        SSTools.ShowMessage("object spawned", SSTools.Position.bottom, SSTools.Time.twoSecond);
        //m_SessionOrigin = FindObjectOfType<ARSessionOrigin>().GetComponent<ARSessionOrigin>();
        m_SessionOrigin = GameObject.FindGameObjectWithTag("ARSO").GetComponent<ARSessionOrigin>();
        //meshRenderer = GetComponent<Renderer>();
        //shaderOutline = Shader.Find("Custom/Outline");
        //shaderBump = Shader.Find("Mobile/Bumped Diffuse");
        //meshRenderer.material.shader = shaderBump;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.touchCount > 1)
        {
            if (objectSate == ObjectState.SELECTED)
            {
                Touch touch0 = Input.GetTouch(0);
                Touch touch1 = Input.GetTouch(1);

                var prevPos1 = touch0.position - touch0.deltaPosition;  // Generate previous frame's finger positions
                var prevPos2 = touch1.position - touch1.deltaPosition;

                var prevDir = prevPos2 - prevPos1;
                var currDir = touch1.position - touch0.position;

                var signedAngle = Vector2.SignedAngle(prevDir, currDir);
                //var angle = Vector2.Angle(prevDir, currDir);
                this.transform.Rotate(0, -signedAngle, 0);  // Rotate by the deltaAngle between the two vectors
            }

        }
        else if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            //check if user try to select furniture by touching screen
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(touch.position);

            if (Physics.Raycast(ray, out hit) && (hit.collider.gameObject == this.gameObject))
            {
                //Transform objectHit = hit.transform;
                onTouched = true;
                //Do something with the object that was hit by the raycast.
            }
            else
            {
                onTouched = false;
            }

            if (m_SessionOrigin.Raycast(touch.position, s_Hits, TrackableType.Planes))
            {

                Pose hitPose = s_Hits[0].pose;

                if (objectSate == ObjectState.JUST_SPAWNED)
                {
                    this.transform.position = hitPose.position;
                    objectSate = ObjectState.NOT_SELECTED;
                    SSTools.ShowMessage("ObjectState.JUST_SPAWNED", SSTools.Position.bottom, SSTools.Time.twoSecond);
                }

                if (objectSate == ObjectState.SELECTED && !onTouched)
                {
                    this.transform.position = hitPose.position;
                    SSTools.ShowMessage("ObjectState.SELECTED && !onTouched", SSTools.Position.bottom, SSTools.Time.twoSecond);
                }

            }

        }
        else
        {
            if (onTouched)
            {
                if (objectSate == ObjectState.NOT_SELECTED)
                {
                    objectSate = ObjectState.SELECTED;
                    UpdateMaterials(true);
                }
                else
                {
                    objectSate = ObjectState.NOT_SELECTED;
                    UpdateMaterials(false);
                }
                onTouched = false;
            }
        }
    }
    #endregion //MONOBEHAVIOUR_METHODS

    #region PRIVATE_METHODS
    private void UpdateMaterials(bool focused)
    {
        //meshRenderer.material.shader = focused ? shaderOutline : shaderBump;
        GetComponent<Outline>().enabled = focused;
    }
    #endregion // PRIVATE_METHODS


    #region PUBLIC_METHODS
    public bool GetIsSelected()
    {
        return objectSate == ObjectState.SELECTED;
    }

    #endregion // PUBLIC_METHODS
}
