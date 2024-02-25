using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(Collider))]
public class CutsceneActivator : MonoBehaviour
{
    [SerializeField]
    private PlayableDirector cutscene;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        cutscene.Play();
        this.gameObject.SetActive(false);
    }
}
