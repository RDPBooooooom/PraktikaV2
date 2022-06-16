using UnityEngine;

namespace P0.Scripts._1._2
{
    public class AdditionAndSubtraction : MonoBehaviour
    {
        [Header("Options")] [SerializeField] private bool _drawAddition;
        [SerializeField] private bool _drawSubtraction;

        [Header("Addition")] [SerializeField] private Vector3 _additionA;
        [SerializeField] private Vector3 _additionB;

        [Header("Subtraction")] [SerializeField]
        private Vector3 _subtractionA;

        [SerializeField] private Vector3 _subtractionB;

        private void OnDrawGizmos()
        {
            if (_drawAddition) DrawAddition();
            if (_drawSubtraction) DrawSubtraction();
        }

        private void DrawAddition()
        {
            Vector3 position = transform.position;

            VectorDrawer.DrawVector(position, _additionA + _additionB, Color.yellow);

            VectorDrawer.DrawVector(position, _additionA, Color.blue);
            VectorDrawer.DrawVector(_additionA + position, _additionB, Color.green);
        }

        private void DrawSubtraction()
        {
            Vector3 position = transform.position + Vector3.right * 5;

            VectorDrawer.DrawVector(position, _subtractionA - _subtractionB, Color.yellow);

            VectorDrawer.DrawVector(position, _subtractionA, Color.blue);
            VectorDrawer.DrawVector(_subtractionA + position, _subtractionB * -1, Color.green);
        }
    }
}