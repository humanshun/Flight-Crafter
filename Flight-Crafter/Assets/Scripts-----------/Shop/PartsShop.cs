using UnityEngine;

namespace Ricimi
{
    public class PartsShop : MonoBehaviour
    {
        // ショップデータを格納するためのパブリック変数
        public ShopData shopData;
        // ポップアップのプレハブを格納するためのパブリック変数
        public GameObject popupPrefab;
        public GameObject itemPrefab;

        // キャンバスを格納するための保護された変数
        protected Canvas m_canvas;

        // スクリプトが有効になったときに最初に呼び出されるメソッド
        protected void Start()
        {
            // 親オブジェクトからCanvasコンポーネントを取得し、m_canvasに格納
            m_canvas = GetComponentInParent<Canvas>();
        }

        // ポップアップを生成して表示するためのメソッド
        public virtual void OpenPopup()
        {
            // popupPrefabをインスタンス化し、新しいゲームオブジェクトとしてpopupに格納
            var popup = Instantiate(popupPrefab) as GameObject;
            // ポップアップをアクティブにする
            popup.SetActive(true);
            // ポップアップのスケールをゼロに設定（後でアニメーションで拡大するため）
            popup.transform.localScale = Vector3.zero;
            // ポップアップをキャンバスの子オブジェクトとして設定
            popup.transform.SetParent(m_canvas.transform, false);
            // ポップアップのPopupコンポーネントのOpenメソッドを呼び出して、ポップアップを開く
            popup.GetComponent<Popup>().Open();

            // Contentオブジェクトを取得
            Transform contentTransform = FindChildByName(m_canvas.transform, "Content");
            if (contentTransform == null)
            {
                Debug.LogError("Contentオブジェクトが見つかりません。");
                return;
            }

            foreach (PartData part in shopData.shopItems)
            {
                // Debug.Log(part.name + part.partType);
                // var item = Instantiate(itemPrefab) as GameObject;

                // item.transform.SetParent(contentTransform.transform, false);
                
                // パーツのタイプが "Body" の場合のみ処理を実行
                if (part.partType == PartType.Body)
                {
                    Debug.Log(part.name + " (" + part.partType + ")");
                    var item = Instantiate(itemPrefab) as GameObject;

                    // Contentオブジェクトの子として設定
                    item.transform.SetParent(contentTransform.transform, false);
                }
            }
        }
        // 再帰的に子オブジェクトを検索するメソッド
        private Transform FindChildByName(Transform parent, string name)
        {
            foreach (Transform child in parent)
            {
                if (child.name == name)
                {
                    return child;
                }
                Transform result = FindChildByName(child, name);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }
    }
}
