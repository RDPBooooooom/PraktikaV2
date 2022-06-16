using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace P0.Scripts.Tests
{
    public class CrossProductTest
    {
        [Test]
        [TestCaseSource(nameof(CrossProductTestCases))]
        public void CrossProductTestSurface(Vector3 vectorA, Vector3 vectorB)
        {
            Vector3 crossProduct = Vector3.Cross(vectorA, vectorB);

            Assert.AreEqual(vectorA.magnitude * vectorB.magnitude, crossProduct.magnitude, double.Epsilon);
        }

        public static IEnumerable<TestCaseData> CrossProductTestCases
        {
            get
            {
                yield return new TestCaseData(new Vector3(0, 0, 2), new Vector3(1, 0, 0));
                yield return new TestCaseData(new Vector3(0, 0, 1), new Vector3(2, 0, 0));
                yield return new TestCaseData(new Vector3(0, 0, 5), new Vector3(4, 0, 0));
            }
        }
    }
}