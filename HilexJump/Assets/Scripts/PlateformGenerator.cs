using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlateformGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject plateformPrefab;

    [SerializeField]
    List<GameObject> plateforms;

    [SerializeField]
    private int numberOfPlateforms = 10;

    [SerializeField]
    private float distanceBetweenPlateforms = 2f;


    private void OnValidate()
    {
        if (numberOfPlateforms < 1)
        {
            numberOfPlateforms = 1;
        }

        if (plateforms == null)
        {
            plateforms = new List<GameObject>();
        }

#if UNITY_EDITOR
        // Avoid doing destructive changes during OnValidate; defer to editor loop
        EditorApplication.delayCall += () =>
        {
            // the object may have been destroyed in the meantime
            if (this == null) return;
            SyncPlateforms();
        };
#else
        SyncPlateforms();
#endif
    }

    private void SyncPlateforms()
    {
        if (plateforms == null)
        {
            plateforms = new List<GameObject>();
        }

        if (plateforms.Count > numberOfPlateforms)
        {
            for (int i = plateforms.Count - 1; i >= numberOfPlateforms; i--)
            {
                GameObject toRemove = plateforms[i];
                plateforms.RemoveAt(i);
                if (toRemove != null)
                {
                    if (Application.isPlaying)
                    {
                        Destroy(toRemove);
                    }
                    else
                    {
                        // called from EditorApplication.delayCall -> safe to use DestroyImmediate
                        DestroyImmediate(toRemove);
                    }
                }
            }
        }
        else if (plateforms.Count < numberOfPlateforms)
        {
            for (int i = plateforms.Count; i < numberOfPlateforms; i++)
            {
                Vector3 spawnPosition = transform.position + new Vector3(0f, i * -distanceBetweenPlateforms, 0f);
                float randomRotation = UnityEngine.Random.Range(0f, 360f);
                Quaternion spawnRotation = Quaternion.Euler(0f, randomRotation, 0f);

                GameObject newPlateform = Instantiate(plateformPrefab, spawnPosition, spawnRotation, this.transform);
                newPlateform.name = "Plateform" + i;

                plateforms.Add(newPlateform);
            }
        }
    }
}
