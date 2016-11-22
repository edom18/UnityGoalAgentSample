using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Emit things that Enemy, Energy, Power.
/// </summary>
public class Emitter : MonoBehaviour
{
    [SerializeField]
    private Transform _enemyPoint;
    private List<Vector3> _enemyPointList = new List<Vector3>();

    [SerializeField]
    private Transform _powerPoint;
    private List<Vector3> _powerPointList = new List<Vector3>();

    [SerializeField]
    private Transform _energyPoint;
    private List<Vector3> _energyPointList = new List<Vector3>();

    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private GameObject _powerPrefab;

    [SerializeField]
    private GameObject _energyPrefab;

    [SerializeField]
    private float _interval = 5f;

	void Start()
    {
        foreach (Transform child in _enemyPoint)
        {
            _enemyPointList.Add(child.position);
        }

        foreach (Transform child in _powerPoint)
        {
            _powerPointList.Add(child.position);
        }

        foreach (Transform child in _energyPoint)
        {
            _energyPointList.Add(child.position);
        }

        StartCoroutine(EmitStart());
	}

    /// <summary>
    /// Start emit
    /// </summary>
    IEnumerator EmitStart()
    {
        float time = 0f;

        while (true)
        {
            if (time >= _interval)
            {
                time = 0f;
                Emit();
            }

            time += Time.deltaTime;

            yield return null;
        }
    }

    /// <summary>
    /// Emit any item.
    /// </summary>
    void Emit()
    {
        Debug.Log("EMIT!");

        int type = Random.Range(0, 3);
        List<Vector3> list = new List<Vector3>();
        GameObject prefab = _enemyPrefab;
        switch (type)
        {
            case 0:
                prefab = _enemyPrefab;
                list = _enemyPointList;
                break;
            case 1:
                prefab = _powerPrefab;
                list = _powerPointList;
                break;
            case 2:
                prefab = _energyPrefab;
                list = _energyPointList;
                break;
        }

        int index = Random.Range(0, list.Count);

        var point = list[index];

        var newObj = Instantiate(prefab);
        newObj.transform.position = point;
    }


}
