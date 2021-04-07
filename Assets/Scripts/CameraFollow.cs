using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private TankMove target;

    public void SetTarget(TankMove _tank)
    {
        target = _tank;
        gameObject.SetActive(true);
    }

    private void LateUpdate()
    {
        this.transform.position = target.transform.position;
        this.transform.eulerAngles = new Vector3(0, target.transform.eulerAngles.y, 0);
    }
}