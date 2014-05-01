using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using McSntt.Models;
using NUnit.Framework;

namespace McSntt.Test.Models
{
    [TestFixture]
    class BoatTest : Boat
    {
        [Test]
        public void ID_OneInstance_FirstIdIsZero()
        {
            var instanceOne = new Boat();
            Assert.AreNotEqual(instanceOne.BoatId, 0);
        }
    }
}
