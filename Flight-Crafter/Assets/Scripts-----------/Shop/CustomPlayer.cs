using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CustomPlayer : MonoBehaviour
{
    [SerializeField] private Transform contentTransform;
    public void SetupAll()
    {
        foreach (Transform child in contentTransform)
        {
            Destroy(child.gameObject);
        }

        Dictionary<PartType, string> currentParts = PlayerData.Instance.GetAllCurrentParts();

        foreach (var kvp in currentParts)
        {
            PartType type = kvp.Key;
            string resourceFileName = kvp.Value;

            PartData loadedPart = Resources.Load<PartData>($"Parts/{resourceFileName}");
            if (loadedPart == null)
            {
                Debug.LogWarning($"Part {resourceFileName} が見つかりませんでした。");
                continue;
            }

            GameObject instance = Instantiate(loadedPart.partPrefab, contentTransform);
            instance.transform.localPosition = Vector3.zero;
        }
    }
}
