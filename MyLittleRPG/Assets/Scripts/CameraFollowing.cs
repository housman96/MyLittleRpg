using UnityEngine;

public class CameraFollowing : MonoBehaviour
{

    public GameObject followed;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 v = followed.transform.position;
        v.z = -2;
        transform.position = v;

    }
}
