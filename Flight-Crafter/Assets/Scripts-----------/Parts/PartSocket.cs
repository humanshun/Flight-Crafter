using UnityEngine;

public class PartSocket : MonoBehaviour
{
    public PartType socketType; // ← enumに合わせて型を変更するのがおすすめ！
    public Transform attachPoint;
    private GameObject currentPart;

    public bool TryAttach(GameObject partPrefab)
    {
        Part part = partPrefab.GetComponent<Part>();
        if (part != null && part.partData != null && part.partData.partType == socketType)
        {
            if (currentPart != null) Destroy(currentPart);
            currentPart = Instantiate(partPrefab, attachPoint.position, attachPoint.rotation, attachPoint);
            currentPart.transform.SetParent(attachPoint); // 念のため
            return true;
        }
        return false;
    }
}
