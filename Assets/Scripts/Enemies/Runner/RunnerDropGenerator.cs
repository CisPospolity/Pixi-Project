using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(RunnerEnemy))]
public class RunnerDropGenerator : MonoBehaviour
{
    private RunnerEnemy enemy;
    [SerializeField]
    private GameObject generator;
    [SerializeField]
    private GameObject heldGenerator;
    private void Awake()
    {
        enemy = GetComponent<RunnerEnemy>();
        enemy.onDeath += SpawnGenerator;
    }

    private void SpawnGenerator()
    {
        heldGenerator.SetActive(false);
        generator.transform.position = transform.position + Vector3.up * 0.5f;
        generator.SetActive(true);
    }
}
