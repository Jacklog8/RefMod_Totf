using UnityEngine;

namespace RefMod
{
    /// <summary>
    /// Moves a gameobjects transform to another existing transform every update
    /// </summary>
    public class TransformToTarget : MonoBehaviour
    {
        /// <summary>
        /// Make sure to set this field for the mono to work
        /// </summary>
        public Transform target = null;
        void Update()
        {
            if (target != null)
            {
                transform.position = target.position;
                transform.rotation = target.rotation;
                transform.localScale = target.localScale;
            }
        }
    }
}
