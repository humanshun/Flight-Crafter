using UnityEngine;

namespace Ricimi
{
    public class PartsShop : MonoBehaviour
    {
        // ショップデータを格納するためのパブリック変数
        public ShopData shopData;
        // ポップアップのプレハブを格納するためのパブリック変数
        public GameObject popupPrefab;

        // キャンバスを格納するための保護された変数
        protected Canvas m_canvas;

        // スクリプトが有効になったときに最初に呼び出されるメソッド
        protected void Start()
        {
            ShopData();
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
        }
        void OnMouseDown()
        {
            Debug.Log("オブジェクトがクリックされました！");
            OpenPopup();
        }
        void ShopData()
        {
            int count = 0;

            foreach (PartData part in shopData.shopItems)
            {
                Debug.Log(part.name + part.partType);
                count++;
            }
            Debug.Log(count);
        }
    }
}
