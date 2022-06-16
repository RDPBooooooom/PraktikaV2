using UnityEngine;

namespace P0.Scripts._1._5
{
    public class CrossProduct : MonoBehaviour
    {
        [Header("Vectors")] [SerializeField] private Vector3 _vectorA;
        [SerializeField] private Vector3 _vectorB;

        private void OnDrawGizmos()
        {
            DrawCrossProduct();
        }

        private void DrawCrossProduct()
        {
            Vector3 position = transform.position;

            VectorDrawer.DrawVector(position, Vector3.Cross(_vectorA, _vectorB), Color.red);

            VectorDrawer.DrawVector(position, _vectorA, Color.blue);
            VectorDrawer.DrawVector(position, _vectorB, Color.yellow);
        }
    }
}