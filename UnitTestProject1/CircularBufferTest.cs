using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AnnelidaLauncher.Model;

namespace UnitTestProject1
{
    [TestClass]
    public class CircularBufferTest
    {
        private CircularBuffer<float> buffer;


        [TestMethod]
        public void TestMaxCapacity()
        {
            buffer = new CircularBuffer<float>(4);
            buffer.Enqueue(1);
            buffer.Enqueue(2);
            buffer.Enqueue(3);
            buffer.Enqueue(4);
            int currentCount = buffer.Count;
            buffer.Enqueue(5);
            buffer.Enqueue(6);
            int afterCount = buffer.Count;

            Assert.AreEqual(currentCount, afterCount);
        }

        [TestMethod]
        public void TestPipeLikeBehaviour()
        {
            buffer = new CircularBuffer<float>(2);
            buffer.Enqueue(1);
            buffer.Enqueue(2);
            buffer.Enqueue(3);

            var expected = new float[] { 2.0f, 3.0f };
            var actual = buffer.ToArray();
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
