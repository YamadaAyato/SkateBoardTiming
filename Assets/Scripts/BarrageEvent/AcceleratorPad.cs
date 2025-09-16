using UnityEngine;

public class AcceleratorPad : MonoBehaviour
{
    [SerializeField] private BarrageGameFlow _gameFlow;

    private void OnTriggerEnter(Collider other)
    {
        // Playerタグのオブジェクトに当たったときだけ反応
        if (other.CompareTag("Player"))
        {
            Debug.Log("加速板に乗った！ジャンプ開始");

            _gameFlow.StartFinalJump();
            Destroy(this.gameObject);
        }
    }
}
