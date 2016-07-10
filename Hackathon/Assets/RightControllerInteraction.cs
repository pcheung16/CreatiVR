using UnityEngine;
using System.Collections;
using System;

public class RightControllerInteraction : MonoBehaviour {

    public GameObject rightController;
    public GameObject SpherePoint;
    public Vector3 rightControllerPosition;
    public Vector3 sphereScale;
    public Rigidbody rb;
    public Rigidbody rbSphere;
    
    private bool ignore = false;
    //public int i;

    public bool drawTypeSphere = true;    //this will be where it is a sphere or cylinder
    public Material sphereMaterial { get; private set; }        //this is the materaial that the balls all are  
    public UnityEngine.Object[] materials;            // the array of materials from the folder

    void Start()
    {
            try
            {
                Debug.Log("Loading with Method #1...");
                materials = Resources.LoadAll("SphereMaterials", typeof(Material));
            }
            catch (Exception e)
            {
                Debug.Log("Method #1 failed with the following exception: ");
                Debug.Log(e);
            }

            SetSphereMaterial();
            GetComponent<SteamVR_TrackedController>().TriggerClicked += new ClickedEventHandler(DoClick);   // gets the trigger click and 
            sphereScale = new Vector3(.1f, .1f, .1f);       //sets the sphere scale
    }

    void Update()
    {
        GetComponent<SteamVR_TrackedController>().PadClicked += new ClickedEventHandler(DoClick);

        GameObject ballPosition = GameObject.Find("PlayerPoint");  //the ball position is going to be the point where we spawn new ones
        //Debug.Log(ballPosition.transform.position);
        // ballPosition.transform.position = playerPointPosition;

        //cool but not being used now 
        rightControllerPosition = transform.position;

        if (GetComponent<SteamVR_TrackedController>().triggerPressed == true && drawTypeSphere == true)
        {
            Debug.Log("trigger right is pressed");
            GameObject spherePoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);      //creates sphere called SpherePoint
            spherePoint.AddComponent<Rigidbody>();          // adds rigig body to sphere
           // spherePoint.transform.position = rightControllerPosition;      //sets position as.... the position of the controller
            spherePoint.transform.position = ballPosition.transform.position;      //sets position as.... the posiiton of PlayerPoint
            spherePoint.transform.localScale = sphereScale;
            Destroy(spherePoint.GetComponent<Rigidbody>());
            (spherePoint.GetComponent<MeshRenderer>()).material = sphereMaterial;
        }

        GetComponent<SteamVR_TrackedController>().PadClicked += new ClickedEventHandler(DoClick);

        if (GetComponent<SteamVR_TrackedController>().triggerPressed == true && drawTypeSphere == false)
        {
            Debug.Log("trigger right is pressed and drawsphere is false");
            GameObject spherePoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);      //creates sphere
            spherePoint.AddComponent<Rigidbody>();          // adds rigig body to sphere
                                                            // spherePoint.transform.position = rightControllerPosition;      //sets position as.... the position of the controller
            spherePoint.transform.position = ballPosition.transform.position;      //sets position as.... the posiiton of PlayerPoint
            spherePoint.transform.localScale = sphereScale;
            Destroy(spherePoint.GetComponent<Rigidbody>());
            (spherePoint.GetComponent<MeshRenderer>()).material = sphereMaterial;
        }

        if (GetComponent<SteamVR_TrackedController>().padPressed == true)
        {
        Debug.Log("padclicked");
        SetSphereMaterial();
        }
        //GetComponent<SteamVR_TrackedController>().TriggerPressed;

        //if (GetComponent<SteamVR_TrackedController>().OnTriggerClicked()) ;
        // gameObject.GetComponent.position = rightControllerPosition;
    }

    void DoClick(object sender, ClickedEventArgs e)
    {

    }

    void SetSphereMaterial()
    {
        sphereMaterial = (Material)materials[UnityEngine.Random.Range(0, 115)];      //set sphere material to a material in materials fro mthe range 0 to 115
        Debug.Log(sphereMaterial.name);
//for (int i=0; i <= 115;  i++)
  //      {
    // SphereMaterial= new Material()
      //  }     
    }
}