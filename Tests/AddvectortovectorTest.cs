using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Vector vec1 = new Vector(25, 30);
            Vector vec2 = new Point(22, 29);
            Vector sum = new Vector();

            vec1 += vec2;

            return vec1;





        }


    }
}

