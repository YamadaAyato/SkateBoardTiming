using UnityEngine;

namespace Stage
{
    /// <summary>
    ///         複数のランプポイントを管理するクラス
    /// </summary>
    public class RampManager : MonoBehaviour
    {
        [SerializeField] private RampPoint[] _rampPoint;

        /// <summary>
        ///         RampPoint を探して返す処理
        /// </summary>
        /// <param name="playerPos"></param>
        /// <returns></returns>
        public RampPoint GetNearestRampPoint(Vector3 playerPos)
        {
            RampPoint nearest = null;
            float minDistance = Mathf.Infinity;

            foreach (var ramp in _rampPoint)
            {
                float distance = Vector3.Distance(playerPos, ramp.start.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearest = ramp;
                }
            }
            return nearest;
        }
    }
}