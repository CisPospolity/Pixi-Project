using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraChangeTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject m_Camera;
    [SerializeField]
    private GameObject triggerGamera;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            triggerGamera.gameObject.SetActive(true);
            m_Camera.gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            triggerGamera.gameObject.SetActive(false);
            m_Camera.gameObject.SetActive(true);
        }
    }
}
