using System;
using Core.Common.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.Common.Tests
{
    [TestClass]
    public class ObjectBaseTest: ObjectBase
    {
        private string _a;

        public string A
        {
            get
            {
                return _a;
                
            }

            set
            {
                if (_a == value) return;

                _a = value;
                OnPropertyChanged(() => A);
            }
        }

        [TestMethod]
        public void TestMethod1()
        {
            bool b = false;
            this.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName.Equals("A"))
                {
                    b = true;
                }
            };

            A= "b";

            Assert.AreEqual(true,b);
        }
    }
}
