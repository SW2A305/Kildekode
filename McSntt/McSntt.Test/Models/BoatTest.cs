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
        public void ID_TwoInstances_diffirentIds()
        {
            var instanceOne = new Boat();
            var instanceTwo = new Boat();

            Assert.AreNotEqual(instanceOne.Id, instanceTwo.Id);
        }

        [Test]
        public void ID_OneInstance_FirstIdIsZero()
        {
            var instanceOne = new Boat();
            Assert.AreNotEqual(instanceOne.Id, 0);
        }
    }
}
