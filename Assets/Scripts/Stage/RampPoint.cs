using Unity.Cinemachine;
using UnityEngine;

namespace Stage
{
    /// <summary>
    ///         ランプ1つ分の情報 
    /// </summary>
    public class RampPoint : MonoBehaviour
    {
        [Header("行き用カート")]
        public CinemachineSplineCart forwardCart;

        [Header("帰り用カート")]
        public CinemachineSplineCart returnCart;
    }
}
