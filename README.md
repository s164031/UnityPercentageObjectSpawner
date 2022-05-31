# UnityPercentageObjectSpawner

Hey!

Please see PercentageTest.cs for an example on how to apply this to your own game!

From any script you like you can now create a list of any type (gameobject, string, int, float etc.) and then access it from the inspector.

In your monobehavior script simply add something like:
```
[SerializeField] PercentageSpawner<GameObject> gameObjectPercentage = new PercentageSpawner<GameObject>();

private void OnValidate()
{
    gameObjectPercentage.ValidateFields();
}
```
