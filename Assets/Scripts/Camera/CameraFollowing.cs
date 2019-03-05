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
        Vector3 targetposition = followed.transform.position;
        targetposition.z = transform.position.z;
        transform.position = Vector3.MoveTowards(transform.position, targetposition, 0.1f);
    }
}
