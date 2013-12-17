﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hl7.Fhir.Rest;

namespace Hl7.Fhir.Test
{
    [TestClass]
    public class TestResourceIdentifier
    {
        [TestMethod]
        public void TestResourceIdentity()
        {
            ResourceIdentity id = new ResourceIdentity("http://localhost/fhir/patient/3");
            Assert.AreEqual("http://localhost/fhir/patient/3", id.ToString());
            Assert.AreEqual("patient", id.Collection);
        }

        [TestMethod]
        public void TestCollection()
        {
            ResourceIdentity identity;

            identity = new ResourceIdentity("http://localhost/fhir/patient/3");
            Assert.AreEqual("patient", identity.Collection);

            identity = new ResourceIdentity("http://localhost/fhir/organization/3/_history/98");
            Assert.AreEqual("organization", identity.Collection);

            identity = new ResourceIdentity("http://localhost/some/sub/path/fhir/patient/3/");
            Assert.AreEqual("patient", identity.Collection);

            identity = new ResourceIdentity("http://localhost/some/sub/path/fhir/organization/3/_history/98");
            Assert.AreEqual("organization", identity.Collection);

            identity = new ResourceIdentity("http://localhost/fhir");
            Assert.AreEqual(null, identity.Collection);

        }

        [TestMethod]
        public void TestId()
        {
            ResourceIdentity identity;

            identity = new ResourceIdentity("http://localhost/fhir/patient/3");
            Assert.AreEqual("3", identity.Id);

            identity = new ResourceIdentity("http://localhost/fhir/organization/508x/_history/98");
            Assert.AreEqual("508x", identity.Id);

            identity = new ResourceIdentity("http://localhost/some/sub/path/fhir/patient/B256/");
            Assert.AreEqual("B256", identity.Id);

            identity = new ResourceIdentity("http://localhost/some/sub/path/fhir/organization/3/_history/98");
            Assert.AreEqual("3", identity.Id);

            identity = new ResourceIdentity("http://localhost/fhir");
            Assert.AreEqual(null, identity.Id);

        }

        [TestMethod]
        public void TestVersionId()
        {
            ResourceIdentity identity;

            identity = new ResourceIdentity("http://localhost/fhir/patient/3");
            Assert.AreEqual(null, identity.VersionId);

            identity = new ResourceIdentity("http://localhost/fhir/organization/508x/_history/98");
            Assert.AreEqual("98", identity.VersionId);

            identity = new ResourceIdentity("http://localhost/some/sub/path/fhir/patient/B256/");
            Assert.AreEqual(null, identity.VersionId);

            identity = new ResourceIdentity("http://localhost/some/sub/path/fhir/organization/3/_history/X98");
            Assert.AreEqual("X98", identity.VersionId);

            identity = new ResourceIdentity("http://localhost/some/sub/path/fhir/organization/3/_history/X98/pipo/clown");
            Assert.AreEqual("X98", identity.VersionId);

            identity = new ResourceIdentity("http://localhost/fhir");
            Assert.AreEqual(null, identity.VersionId);

        }
    }
}
