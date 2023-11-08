using UnityEngine;

namespace DicksonMd.Utils
{
    public class AutoDie : MonoBehaviour
    {
        [Tooltip("In unity seconds")]
        public float lifeTime;

        // Start is called before the first frame update
        void Start()
        {
            Destroy(gameObject, lifeTime);
        }
    }
}