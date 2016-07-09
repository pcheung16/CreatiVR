using UnityEngine;

public class CreateLine : MonoBehaviour
{
    public GameObject lineDraw;     //this is the object to be extruded
    public Transform start;
    public Transform target;
    // Use this for initialization
    void Start()
    {
        float distancex = (start.position.x - target.position.x);
        float distancey = (start.position.y - target.position.y);
        float distancez = (start.position.z - target.position.z);
        float distance = Mathf.Sqrt(distancex * distancex + distancey * distancey + distancez * distancez);
        Vector3 size;
        size.x = .1f;
        size.y = distance/2;
        size.z = .1f;
        Vector3 midPoint;
        midPoint.x = (start.position.x + target.position.x) * .5f;
        midPoint.y = (start.position.y + target.position.y) * .5f;
        midPoint.z = (start.position.z + target.position.z) * .5f;
        lineDraw.transform.position = midPoint;
//      lineDraw.transform.LookAt(target);
        lineDraw.transform.localScale = size;
        Vector3 lineDirection;       //direction that the line will pont (start to end)
        lineDirection.x = distancex;
        lineDirection.y = distancey;
        lineDirection.z = distancez;
        lineDraw.transform.up = lineDirection;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
