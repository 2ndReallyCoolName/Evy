using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Vector vec1 = new Vector(5, 8);
            Point v = new Point(6, 9);
            Point sum = new Point();

            sum = Vector.Add(v, vec1);

            return sum;





        }


    }
}

