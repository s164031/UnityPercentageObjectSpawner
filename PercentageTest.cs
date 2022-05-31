using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PercentageTest : MonoBehaviour
{
    [SerializeField]
    PercentageSpawner<GameObject> gameObjectPercentage = new PercentageSpawner<GameObject>();

    [SerializeField]
    PercentageSpawner<int> intPercentage = new PercentageSpawner<int>();

    [SerializeField]
    PercentageSpawner<string> stringPercentage = new PercentageSpawner<string>();

    void Start()
    {
        print(intPercentage.GetRandomValue());
        print(stringPercentage.GetRandomValue());
    }

    private void OnValidate()
    {
        gameObjectPercentage.ValidateFields();
        intPercentage.ValidateFields();
        stringPercentage.ValidateFields();
    }
}
