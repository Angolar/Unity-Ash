﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSubstitute;
using NUnit.Framework;

namespace Ash.Core
{
    [TestFixture]
    public class EngineTests
    {
        private Engine _engine;

        [SetUp]
        public void Setup()
        {
            _engine = new Engine();
        }

        [Test]
        public void AddingASystem_InformsTheSystem()
        {
            var system = Substitute.For<ISystem>();
            _engine.AddSystem(system, 12);
            system.Received().AddedToEngine(_engine);
        }

        [Test]
        public void RemovingASystem_InformsTheSystem()
        {
            var system = Substitute.For<ISystem>();
            _engine.RemoveSystem(system);
            system.Received().RemovedFromEngine(_engine);
        }

        [Test]
        public void SystemsAreUpdatedInPriorityOrder()
        {
            var system1 = Substitute.For<ISystem>();
            var system2 = Substitute.For<ISystem>();
            var system3 = Substitute.For<ISystem>();

            _engine.AddSystem(system1, 100);
            _engine.AddSystem(system2, -20);
            _engine.AddSystem(system3, 30);
            _engine.Update(23.45f);

            Received.InOrder(() =>
            {
                system2.Update(23.45f);
                system3.Update(23.45f);
                system1.Update(23.45f);
            });
        }

        [Test]
        public void RemovedSystemsAreNoLongerUpdated()
        {
            var system1 = Substitute.For<ISystem>();
            var system2 = Substitute.For<ISystem>();
            var system3 = Substitute.For<ISystem>();

            _engine.AddSystem(system1, 100);
            _engine.AddSystem(system2, -20);
            _engine.AddSystem(system3, 30);
            _engine.RemoveSystem(system2);
            _engine.Update(23.45f);

            system1.Received().Update(23.45f);
            system2.DidNotReceive().Update(23.45f);
            system3.Received().Update(23.45f);
        }

        [Test]
        public void GetsNodesForAGivenType()
        {
            var nodes1 = _engine.GetNodes<MockNodeA>();
            Assert.IsNotNull(nodes1);
        }

        [Test]
        public void IfTwoNodeTypesAreReturned_DifferentFamiliesAreReturned()
        {
            var nodes1 = _engine.GetNodes<MockNodeA>();
            var nodes2 = _engine.GetNodes<MockNodeB>();
            Assert.IsTrue(nodes1 != nodes2);
        }

        [Test]
        public void IfSameNodeIsRequestedTwice_SameFamilyIsReturned()
        {
            var nodes1 = _engine.GetNodes<MockNodeA>();
            var nodes2 = _engine.GetNodes<MockNodeA>();
            Assert.IsTrue(nodes1 == nodes2);
        }

        [Test]
        public void IfNodesAreReleased_DifferentFamilyIsReturned()
        {
            var nodes1 = _engine.GetNodes<MockNodeA>();
            _engine.ReleaseNodes<MockNodeA>();
            var nodes2 = _engine.GetNodes<MockNodeA>();
            Assert.IsTrue(nodes1 != nodes2);
        }
    }
}
