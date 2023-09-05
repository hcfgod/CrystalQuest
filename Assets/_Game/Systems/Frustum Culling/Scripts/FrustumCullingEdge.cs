using UnityEngine;

namespace FrustumCullingSpace
{
    public class FrustumCullingEdge : MonoBehaviour
    {
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, 0.1f);
        }
    }
}
