using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private Camera camera;
    [SerializeField]
    private Transform pointForMoveDistance;
    [SerializeField]
    private Transform target;

    private float cameraToTargetMaxDistance;

    private bool moveRight;

    void Start()
    {
        cameraToTargetMaxDistance = camera.transform.position.x - pointForMoveDistance.position.x;
    }

    void Update()
    {
        Vector2 cameraToPlayer = new Vector2(camera.transform.position.x, 0) - new Vector2(target.transform.position.x, 0);
        if (cameraToPlayer.x > 0) moveRight = true;
        else if (cameraToPlayer.x < 0) moveRight = false;

        float cameraToPlayerDistance = cameraToPlayer.magnitude;

        if (cameraToPlayerDistance > cameraToTargetMaxDistance)
        {
            float speed = cameraToPlayerDistance - cameraToTargetMaxDistance;
            if (!moveRight)
            {
                camera.transform.position = new Vector3(camera.transform.position.x + speed, camera.transform.position.y, camera.transform.position.z);
            }
            else
            {
                camera.transform.position = new Vector3(camera.transform.position.x - speed, camera.transform.position.y, camera.transform.position.z);
            }
        }

    }
}
