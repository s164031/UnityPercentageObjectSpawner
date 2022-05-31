using System.Collections.Generic;
using UnityEngine;
using System;

public class PercentageSpawner : MonoBehaviour
{
    [Serializable]
    public class SpawnObject
    {
        public GameObject spawnObject;
        [Range(0.00f, 1.00f)]
        public float chanceToSpawn;
        internal float prevChance;
        public bool locked;
        internal bool prevLocked;
    }

    public SpawnObject[] spawnObjects;
    private int lengthObjects = 0;
    private List<int> lockedList = new List<int>();

    private void OnValidate()
    {
        //Check if the list is empty/not initialized
        if (spawnObjects == null)
            return;
        //Check if list entry is added
        if (lengthObjects != spawnObjects.Length)
        {
            lengthObjects = spawnObjects.Length;
            lockedList = new List<int>();
            for (int i = 0; i < spawnObjects.Length; i++)
            {
                spawnObjects[i].chanceToSpawn = 1f / lengthObjects;
                spawnObjects[i].prevChance = spawnObjects[i].chanceToSpawn;
                if (spawnObjects[i].locked)
                    lockedList.Add(i);
            }
        }
        else
        {
            List<int> indexList = new List<int>(lockedList);
            int index = 0;
            bool change = false;
            float changeAmount = 0.000f;
            float changeSum;
            foreach (SpawnObject spawnObject in spawnObjects)
            {
                //Check if it's a change in probability (the slider)
                if (spawnObject.prevChance != spawnObject.chanceToSpawn)
                {
                    if (spawnObject.locked)
                    {
                        spawnObject.locked = false;
                        spawnObject.prevLocked = false;
                        lockedList.Remove(index);
                    }
                    else
                    {
                        indexList.Add(index);
                    }
                    //Check if it is the only object which is unlocked
                    if(spawnObjects.Length > 1 && lockedList.Count >= spawnObjects.Length - 1)
                    {
                        spawnObject.chanceToSpawn = spawnObject.prevChance;
                        break;
                    }

                    change = true;
                    changeAmount = (spawnObject.chanceToSpawn - spawnObject.prevChance) / (spawnObjects.Length - indexList.Count);
                    spawnObject.prevChance = spawnObject.chanceToSpawn;
                    break;
                }
                //Check if a lock has changed
                if (spawnObject.prevLocked != spawnObject.locked)
                {
                    if (spawnObject.locked)
                        lockedList.Add(index);
                    else
                        lockedList.Remove(index);
                    spawnObject.prevLocked = spawnObject.locked;
                    break;
                }
                index++;
            }
            //Adjust all elements with the changed probability
            while (change)
            {
                change = false;
                float sum = 0.000f;
                for (int i = 0; i < spawnObjects.Length; i++)
                {
                    if (indexList.Contains(i))
                    {
                        sum += spawnObjects[i].chanceToSpawn;
                        continue;
                    }
                    spawnObjects[i].chanceToSpawn -= changeAmount;
                    if (spawnObjects[i].chanceToSpawn < 0.000f)
                    {
                        spawnObjects[i].chanceToSpawn = 0.000f;
                        change = true;
                        indexList.Add(i);
                    }
                    else if (spawnObjects[i].chanceToSpawn > 1.000f)
                    {
                        spawnObjects[i].chanceToSpawn = 1.000f;
                        change = true;
                        indexList.Add(i);
                    }
                    sum += spawnObjects[i].chanceToSpawn;
                    spawnObjects[i].prevChance = spawnObjects[i].chanceToSpawn;
                }

                changeSum = sum - 1.000f;
                if (change && spawnObjects.Length - indexList.Count > 0)
                {
                    changeAmount = changeSum / (spawnObjects.Length - indexList.Count);
                }
                if (indexList.Count == spawnObjects.Length && spawnObjects.Length > 1)
                {
                    spawnObjects[index].chanceToSpawn -= changeSum;
                    spawnObjects[index].prevChance = spawnObjects[index].chanceToSpawn;
                    break;
                }
            }
        }
    }

    private GameObject GetRandomObject()
    {
        float rand = UnityEngine.Random.Range(0.000f, 1.000f);
        float chanceSum = 0.00f;
        foreach (SpawnObject obj in spawnObjects)
        {
            if (obj.chanceToSpawn + chanceSum > rand)
            {
                return obj.spawnObject;
            }
            chanceSum += obj.chanceToSpawn;
        }
        return null;
    }
}
