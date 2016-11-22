using UnityEngine;
using System.Collections;

namespace AI
{
    public enum ItemType
    {
        // エネルギー（体力）の回復アイテム
        Energy,

        // 攻撃回数アップアイテム
        Power,
    }

    /// <summary>
    /// 手に入れることができるaアイテムオブジェクト
    /// </summary>
    public class Item : MonoBehaviour
    {
        [SerializeField]
        private ItemType _itemType;
        public ItemType ItemType
        {
            get
            {
                return _itemType;
            }
        }
        /// <summary>
        /// 取得可能アイテムを取得
        /// </summary>
        public void PickedUp()
        {
            Destroy(gameObject);
        }
    }
}