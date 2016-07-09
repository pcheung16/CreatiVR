using UnityEngine;
using System.Collections;

public class RightControllerInteraction : MonoBehaviour {

    public GameObject rightController;
    public Vector3 rightControllerPosition;
    public Vector3 sphereScale;
    public Rigidbody rb;
    public Rigidbody rbSphere;
    public Material SphereMaterial;
    private bool ignore = false;

    public bool drawTypeSphere = true;    //this will be where it is a sphere or cylinder

    void Start()
    {
        GetComponent<SteamVR_TrackedController>().TriggerClicked += new ClickedEventHandler(DoClick);   // gets the trigger click and 
        sphereScale = new Vector3(.1f, .1f, .1f);       //sets the sphere scale
    }

    void Update()
    {
        GameObject ballPosition = GameObject.Find("PlayerPoint");  //the ball position is going to be the point where we spawn new ones
        Debug.Log(ballPosition.transform.position);
        // ballPosition.transform.position = playerPointPosition;

        //cool but not being used now 
        rightControllerPosition = transform.position;

        if (GetComponent<SteamVR_TrackedController>().triggerPressed == true && drawTypeSphere == true)
        {
            Debug.Log("trigger right is pressed");
            GameObject spherePoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);      //creates sphere
            spherePoint.AddComponent<Rigidbody>();          // adds rigig body to sphere
           // spherePoint.transform.position = rightControllerPosition;      //sets position as.... the position of the controller
            spherePoint.transform.position = ballPosition.transform.position;      //sets position as.... the posiiton of PlayerPoint
            spherePoint.transform.localScale = sphereScale;
            Destroy(spherePoint.GetComponent<Rigidbody>());
            (spherePoint.GetComponent<MeshRenderer>()).material = SphereMaterial;
        }

        //GetComponent<SteamVR_TrackedController>().TriggerPressed;

        //if (GetComponent<SteamVR_TrackedController>().OnTriggerClicked()) ;
        // gameObject.GetComponent.position = rightControllerPosition;
    }

    void DoClick(object sender, ClickedEventArgs e)
    {

    }
}