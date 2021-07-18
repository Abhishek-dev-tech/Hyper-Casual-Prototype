using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject target;

    public float offset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x, target.transform.position.y + offset, transform.position.z);
    }
}
