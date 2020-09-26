#region License
/*
MIT License

Copyright(c) 2020 Petteri Kautonen

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
#endregion

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VPKSoft.DropOutStack;

namespace AudioVisualization.Test
{
    [TestClass]
    public class DropOutStack_UnitTest
    {
        private const string DigitsAlphabetsUpper = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        [TestMethod]
        public void ArrayTest()
        {
            var stackTest = Initialize(40);
            var digitsAlphabetsReverse = string.Concat(Reverse(DigitsAlphabetsUpper).ToCharArray());

            Assert.AreEqual(string.Concat(digitsAlphabetsReverse.ToCharArray()),
                string.Concat(stackTest.ToArray()));
        }

        [TestMethod]
        public void PopTest()
        {
            var stackTest = Initialize(40);
            var digitsAlphabetsReverse = string.Concat(Reverse(DigitsAlphabetsUpper).ToCharArray());

            for (int i = 0; i < digitsAlphabetsReverse.Length; i++)
            {
                Assert.AreEqual(stackTest.Pop(), digitsAlphabetsReverse[i]);
            }
        }

        #region HelperMethods
        private string Reverse(string value)
        {
            var reverse = value.ToCharArray();
            Array.Reverse(reverse);
            return string.Concat(reverse);
        }

        private DropOutStack<char> Initialize(int capacity)
        {
            var stackTest = new DropOutStack<char>(capacity);
            foreach (var ch in DigitsAlphabetsUpper)
            {
                stackTest.Push(ch);
            }

            return stackTest;
        }
        #endregion

        [TestMethod]
        public void ForIntTest()
        {
            var stackTest = Initialize(40);

            var reverse = Reverse(DigitsAlphabetsUpper).ToCharArray();
            for (int i = 0; i < stackTest.Count; i++)
            {
                Console.WriteLine($@"Character from stack: {stackTest[i]} / should be: {reverse[i]}.");
                Assert.AreEqual(stackTest[i], reverse[i]);
            }
        }

    }
}
