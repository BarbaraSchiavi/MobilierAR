using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARFoundation;

//[RequireComponent(typeof(ARSessionOrigin))]
public class ObjectPositionOnPlane : MonoBehaviour
{
    //[SerializeField]
    //[Tooltip("Instantiates this prefab on a plane at the touch location.")]
    //GameObject m_PlacedPrefab;


    /// <summary>
    /// The prefab to instantiate on touch.
    /// </summary>
    //public GameObject placedPrefab
    //{
    //    get { return m_PlacedPrefab; }
    //    set { m_PlacedPrefab = value; }
    //}

    /// <summary>
    /// The object instantiated as a result of a successful raycast intersection with a plane.
    /// </summary>
    public GameObject spawnedObject { get; private set; }

    ARSessionOrigin m_SessionOrigin;

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    void Awake()
    {
        m_SessionOrigin = GetComponent<ARSessionOrigin>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (m_SessionOrigin.Raycast(touch.position, s_Hits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = s_Hits[0].pose;

                GameObject furniture;

                if (GameObject.FindGameObjectWithTag("furniture"))
                { 
                    furniture = GameObject.FindGameObjectWithTag("furniture");

                    if (furniture.GetComponent<ObjectBehavior>().GetIsSelected())
                    {
                        furniture.transform.position = hitPose.position;
                    }
                    else
                    {
                        //furniture.GetComponent<Renderer>().enabled = true;
                        SSTools.ShowMessage("Cliquez sur le meuble pour le déplacer", SSTools.Position.bottom, SSTools.Time.twoSecond);
                    }
                }
                else
                {
                    SSTools.ShowMessage("Sélectionnez un meuble dans le menu", SSTools.Position.bottom, SSTools.Time.twoSecond);
                }
                //if (spawnedObject == null)
                //{
                //    spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position, hitPose.rotation);
                //}
                //else
                //{
                //    spawnedObject.transform.position = hitPose.position;
                //}
            }
        }
    }
}
