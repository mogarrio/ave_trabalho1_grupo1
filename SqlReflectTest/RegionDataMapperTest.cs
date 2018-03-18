using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlReflectTest.DataMappers;

namespace SqlReflectTest
{
    [TestClass]
    public class RegionDataMapperTest : AbstractRegionDataMapperTest
    {
        public RegionDataMapperTest() : base(new RegionDataMapper(NORTHWIND))
        {
        }

        [TestMethod]
        public new void TestRegionGetAll() {
            base.TestRegionGetAll();
        }

        [TestMethod]
        public new void TestRegionGetById() {
            base.TestRegionGetById();
        }


        [TestMethod]
        public new void TestRegionInsertAndDelete()
        {
            base.TestRegionInsertAndDelete();
        }

        [TestMethod]
        public new void TestRegionUpdate() {
            base.TestRegionUpdate();
        }
    }
}
