using UnityEngine;

namespace P0.Scripts._1._3
{
    public class ScalarMultiplication : MonoBehaviour
    {
        [Header("Addition")] [SerializeField] private Vector3 _vectorA;
        [SerializeField] private float _factor;

        private void OnDrawGizmos()
        {
            DrawScalarMultiplication();
        }

        private void DrawScalarMultiplication()
        {
            Vector3 position = transform.position;

            VectorDrawer.DrawVector(position, _vectorA * _factor, Color.red);
            VectorDrawer.DrawVector(position, _vectorA, Color.blue);
        }
    }
}