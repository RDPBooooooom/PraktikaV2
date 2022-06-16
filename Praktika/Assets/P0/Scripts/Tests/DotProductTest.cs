using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace P0.Scripts.Tests
{
    public class DotProductTest
    {
        // A Test behaves as an ordinary method
        [Test]
        [TestCaseSource(nameof(DotProductTestCases))]
        public void DotProductTestNormalizedRange(Vector2 a, Vector2 b)
        {
            float dotProduct = Vector2.Dot(a.normalized, b.normalized);

            Assert.AreEqual(0, dotProduct, 1 + double.Epsilon);
        }

        public static IEnumerable<TestCaseData> DotProductTestCases
        {
            get
            {
                // Erstellt Vektoren im Abstand von 0.05 radianten um den Punkt 0,0  
                for (float i = 0; i < 3.1415f * 2; i += 0.05f)
                {
                    yield return new TestCaseData(new Vector2(Mathf.Cos(i) * 2, Mathf.Sin(i) * 2), new Vector2(0, 1));
                }
            }
        }
    }
}