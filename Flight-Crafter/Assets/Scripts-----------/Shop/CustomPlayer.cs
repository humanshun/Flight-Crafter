using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CustomPlayer : MonoBehaviour
{
    [SerializeField] private Transform contentTransform;

    void Start()
    {
        SetupAll();
    }
    public void SetupAll()
    {
        foreach (Transform child in contentTransform)
        {
            Destroy(child.gameObject);
        }
        var currentPart = PlayerData.Instance.GetAllCurrentParts();

        //bodyから読み込む（位置情報を取得するため）
        if (!currentPart.TryGetValue(PartType.Body, out var bodyResourceName))
        {
            Debug.LogWarning("Bodyパーツが装備されていません。");
            return;
        }

        BodyData body = Resources.Load<BodyData>($"Parts/{bodyResourceName}");
        if (body == null)
        {
            Debug.LogWarning($"Body{bodyResourceName} が見つかりませんでした。");
            return;
        }

        //Bodyインスタンス
        // Instantiate(body.partPrefab, contentTransform).transform.localPosition = Vector3.zero;
        GameObject bodyInstance = Instantiate(body.partPrefab, contentTransform);
        Vector3 bottomOffset = CalculateBottomoffset(bodyInstance);
        bodyInstance.transform.localPosition = -bottomOffset;

        SetupPart(currentPart, PartType.Rocket, body.rocketPosition);
        SetupPart(currentPart, PartType.Wing, body.wingPosition);
        SetupPart(currentPart, PartType.Tire, body.rightTirePosition);
        SetupPart(currentPart, PartType.Tire, body.leftTirePosition);
    }

    private void SetupPart(Dictionary<PartType, string> currentPart, PartType partType, Vector2 localPos)
    {
        if (!currentPart.TryGetValue(partType, out var partResourceName)) return;

        PartData part = Resources.Load<PartData>($"Parts/{partResourceName}");
        if (part == null)
        {
            Debug.LogWarning($"Part{partResourceName} が見つかりませんでした。");
            return;
        }

        var instance = Instantiate(part.partPrefab, contentTransform);
        instance.transform.localPosition = localPos;
    }

    private Vector3 CalculateBottomoffset(GameObject obj)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
        {
            Debug.LogWarning("Rendererが見つかりません（Body補正できません）");
            return Vector3.zero;
        }

        float minY = float.MaxValue;
        foreach (var r in renderers)
        {
            float localY = obj.transform.InverseTransformPoint(r.bounds.min).y;
            if (localY < minY)
            {
                minY = localY;
            }
        }
        
        return new Vector3(0f, minY, 0f);
    }
}
