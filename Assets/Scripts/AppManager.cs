using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using System.Collections;

public class AppManager : MonoBehaviour
{
    //private static readonly AppManager instance = new AppManager();

    #region PRIVATE_MEMBER_VARIABLES
    [SerializeField]
    GameObject canvas;
    [SerializeField]
    Sprite buttonImageOn;
    [SerializeField]
    Sprite buttonImageOff;
    [SerializeField]
    GameObject menuPanel;
    [SerializeField]
    Image menuButton;
    [SerializeField]
    GameObject[] furnitures;
    GameObject objectSelected;

    //[SerializeField]
    //GameObject[] buttons;

    bool boolTrackingOnOff = true;
    GameObject sessionOriginTmp;
    GameObject m_PlaneMeshVisualizer;
    [SerializeField]
    Material featheredPlaneMaterial;
    [SerializeField]
    Material transparentPlaneMaterial;
    #endregion //PRIVATE_MEMBER_VARIABLES

    #region PUBLIC_MEMBER_VARIABLES
    public GameObject m_SessionOrigin;
    public bool menuOpened { get; set; }
    public bool objectOnScene { get; set; }
    #endregion //PUBLIC_MEMBER_VARIABLES

    //// Explicit static constructor to tell C# compiler
    //// not to mark type as beforefieldinit
    //static AppManager()
    //{
    //}

    //private AppManager()
    //{
    //}

    //public static AppManager Instance
    //{
    //    get
    //    {
    //        return instance;
    //    }
    //}

    #region MONOBEHAVIOUR_METHODS
    void Awake()
    {
        //sessionOriginTmp = Instantiate(m_SessionOrigin, Vector3.zero, Quaternion.identity);

    }
    // Use this for initialization
    void Start()
    {
        //if (GameObject.FindGameObjectWithTag("arplane"))
        //{
        //    m_PlaneMeshVisualizer = GameObject.FindGameObjectWithTag("arplane");
        //    m_PlaneMeshVisualizer.GetComponent<ARFeatheredPlaneMeshVisualizer>().SetPlaneMaterial(featheredPlaneMaterial);
        //}

        objectOnScene = false;
        menuOpened = false;
        menuPanel.SetActive(menuOpened);
        menuButton.sprite = buttonImageOff;
    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion //MONOBEHAVIOUR_METHODS

    #region PUBLIC_METHODS

    //button diselect any furniture
    public void ClearFurniture()
    {
        SSTools.ShowMessage("ClearFurniture", SSTools.Position.bottom, SSTools.Time.twoSecond);
        //objectSelected = null;
        //clear any furniture previously spawned by button selected
        if (GameObject.FindGameObjectsWithTag("furniture") != null)
        {
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("furniture"))
            {
               Destroy(go.gameObject);
            }
        }
    }

    //button choose furniture regarding the index i of furnitures
    public void ChooseFurniture(int i)
    {

        SSTools.ShowMessage("ChooseFurniture", SSTools.Position.bottom, SSTools.Time.twoSecond);
        //clear all outlined buttons before to outline the clicked one
        //foreach (GameObject b in buttons)
        //{
        //    b.GetComponent<Outline>().enabled = false;
        //}
        //buttons[i].GetComponent<Outline>().enabled = true;

        objectSelected = furnitures[i];
    }

    //show or hide AR mesh of floor
    public void DisplayTracking()
    { 
        boolTrackingOnOff = !boolTrackingOnOff;
        Material mat = boolTrackingOnOff ? featheredPlaneMaterial : transparentPlaneMaterial;
        SSTools.ShowMessage("DisplayTracking " + boolTrackingOnOff.ToString(), SSTools.Position.bottom, SSTools.Time.twoSecond);
        //if (GameObject.FindGameObjectWithTag("arplane"))
        //{
        //    m_PlaneMeshVisualizer = GameObject.FindGameObjectWithTag("arplane");
        //    m_PlaneMeshVisualizer.GetComponent<ARFeatheredPlaneMeshVisualizer>().SetPlaneMaterial(mat);
        //}

    }

    //open / close menu button
    public void MenuButton()
    {
        menuOpened = !menuOpened;
        menuPanel.SetActive(menuOpened);
        menuButton.sprite = menuOpened ? buttonImageOn : buttonImageOff;

        //instantiate object if selected in menu
        if (objectSelected != null)
        {
            SSTools.ShowMessage("MenuButton", SSTools.Position.bottom, SSTools.Time.twoSecond);
            GameObject go = Instantiate(objectSelected, Vector3.zero, Quaternion.Euler(-90, 0, 0));
            objectSelected = null;
        }
    }

    //clear objects on scene and AR session
    public void ClearAll()
    {
        SSTools.ShowMessage("ClearAll", SSTools.Position.bottom, SSTools.Time.twoSecond);
        ClearFurniture();
        //reset the AR tracking
        //Destroy(sessionOriginTmp.gameObject);
        //sessionOriginTmp = null;
        //sessionOriginTmp = Instantiate(m_SessionOrigin, Vector3.zero, Quaternion.identity);
    }

    public void TakeAPhoto()
    {
        //hide canvas
        canvas.SetActive(false);
        //TakeAPhoto screenshot
        StartCoroutine("CaptureIt");
    }

    IEnumerator CaptureIt()
    {
        string timeStamp = System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss");
        string fileName = "Screenshot" + timeStamp + ".png";
        string pathToSave = fileName;
        //string path = SaveImageToGallery(tex, "Name", "Description");
        ScreenCapture.CaptureScreenshot(pathToSave);
        yield return new WaitForEndOfFrame();
        //Instantiate(blink, new Vector3(0f, 0f, 0f), Quaternion.identity);
        SSTools.ShowMessage(fileName, SSTools.Position.bottom, SSTools.Time.twoSecond);
        //show canvas
        canvas.SetActive(true);
    }
    #endregion //PUBLIC_METHODS
}