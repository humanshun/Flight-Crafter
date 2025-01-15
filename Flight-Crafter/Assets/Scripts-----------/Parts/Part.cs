using UnityEngine;

public class Part : MonoBehaviour
{
    public PartData partData; // このパーツのデータ

    public void AttachToAirplane(PlayerController playerController)
    {
        // パーツの種類に応じて処理を振り分ける
        if (partData is BodyData body)
        {
            playerController.body = body;
        }
        else if (partData is WingData wing)
        {
            playerController.wing = wing;
        }
        else if (partData is TireData tire)
        {
            playerController.tire = tire;
        }
        else if (partData is RocketData rocket)
        {
            playerController.rocket = rocket;
        }

        // データ更新
        playerController.UpdateStats();

        // 見た目の取り付け
        transform.parent = playerController.transform;
        transform.localPosition = Vector3.zero;
    }
}
