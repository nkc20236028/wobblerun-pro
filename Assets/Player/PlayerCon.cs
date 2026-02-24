using UnityEngine;

public class DemoPlayerPhysics : MonoBehaviour
{
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
}