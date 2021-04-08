using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private float rotateSpeed = 0.1f;
    private TankMove target;

    public void SetTarget(TankMove _tank)
    {
        target = _tank;
        gameObject.SetActive(true);
    }

    private void LateUpdate()
    {
        this.transform.position = target.transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, target.transform.rotation, rotateSpeed);
    }
}