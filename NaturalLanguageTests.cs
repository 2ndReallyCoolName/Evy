using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vspace = NaturalLanguage.vector.VectorSpace;

namespace Tests
{
    [TestClass]
    class NaturalLanguageTests
    {
        [TestMethod]
        public void TestMethod2()
        {
            float[] v1 = { 1.0f, 2.0f, 3.0f };
            float[] v2 = { 1.0f, 2.0f, 3.0f };

            float[] v3 = NaturalLanguage.vector.VectorSpace.Add(v1, v2);

            CollectionAssert.AreEqual(v3, Vspace.Scale(2, v1));
            CollectionAssert.AreEqual(Vspace.Negate(v1), Vspace.Scale(-1, v1));
            Assert.AreEqual(Vspace.DotProduct(v1, v2), 14);
            CollectionAssert.AreEqual(Vspace.Normalize(v1), new float[]{1 / 6f, 2 / 6f, 3 / 6f });

        }
    }
}
