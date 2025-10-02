using UnityEngine;
using System.Linq;

namespace Stage
{
    /// <summary>
    ///         複数のランプポイントを管理するクラス
    /// </summary>
    public class RampManager : MonoBehaviour
    {
        [SerializeField] private RampPoint[] _rampPoint;

        /// <summary>
        ///         一番近いRampPointを探して返す処理
        /// </summary>
        /// <param name="playerPos"></param>
        /// <returns></returns>
        public RampPoint GetNearestRampPoint(Vector3 playerPos)
        {
            return _rampPoint
                .OrderBy(ramp => Vector3.Distance(playerPos, ramp.transform.position))
                .FirstOrDefault();
        }
    }
}