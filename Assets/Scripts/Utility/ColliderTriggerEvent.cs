using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ColliderEvent : UnityEvent<Collider>
{ }

[RequireComponent(typeof(Collider))]
public class ColliderTriggerEvent : MonoBehaviour
{
    public ColliderEvent onTriggerEnterEvent;
    public ColliderEvent onTriggerStayEvent;
    public ColliderEvent onTriggerExitEvent;

    private void OnTriggerEnter(Collider other)
    {
        onTriggerEnterEvent?.Invoke(other);
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    onTriggerStayEvent?.Invoke(other);
    //}

    private void OnTriggerExit(Collider other)
    {
        onTriggerExitEvent?.Invoke(other);
    }
}