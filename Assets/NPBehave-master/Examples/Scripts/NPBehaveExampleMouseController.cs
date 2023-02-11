using UnityEngine;

public class NPBehaveExampleMouseController : MonoBehaviour
{
    private Camera theCamera;

    public Vector3 origin;
    public Vector3 direction;
    void Start()
    {
        theCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    void Update()
    {
        Ray ray = theCamera.ScreenPointToRay(Input.mousePosition);
        origin = ray.origin;
        direction = ray.direction;
        Vector3 pos = RayCast(ray);
        this.transform.position = pos;
    }

    private Vector3 RayCast(Ray r)
    {
        float z = 0f;
        float delta = (z - r.origin.z) / r.direction.z;
        if (delta > 0.1f && delta < 10000f)
        {
            return r.origin + r.direction * delta;
        }
        else
        {
            return Vector3.zero;
        }
    }

}