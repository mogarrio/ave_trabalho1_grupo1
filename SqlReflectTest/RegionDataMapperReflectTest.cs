using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlReflect;
using SqlReflectTest.Model;

namespace SqlReflectTest {
    [TestClass]
    public class RegionDataMapperReflectTest : AbstractRegionDataMapperTest {
        public RegionDataMapperReflectTest() : base(new ReflectDataMapper(typeof(Region), NORTHWIND)) {
        }

        [TestMethod]
        public void TestRegionGetAllReflect() {
            base.TestRegionGetAll();
        }

        [TestMethod]
        public void TestRegionGetByIdReflect() {
            base.TestRegionGetById();
        }
        [TestMethod]
        public void TestRegionInsertAndDeleteReflect() {
            base.TestRegionInsertAndDelete();
        }

        [TestMethod]
        public void TestRegionUpdateReflect() {
            base.TestRegionUpdate();
        }
    }
}
