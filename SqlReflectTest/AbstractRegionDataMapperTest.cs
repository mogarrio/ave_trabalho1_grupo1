using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlReflectTest.Model;
using SqlReflect;
using System.Collections;
using System.Text.RegularExpressions;

namespace SqlReflectTest
{
    public abstract class AbstractRegionDataMapperTest
    {
        protected static readonly string NORTHWIND = @"
                    Server=(LocalDB)\MSSQLLocalDB;
                    Integrated Security=true;
                    AttachDbFileName=" +
                        Environment.CurrentDirectory +
                        "\\data\\NORTHWND.MDF";

        readonly IDataMapper regions;

        public AbstractRegionDataMapperTest(IDataMapper regions)
        {
            this.regions = regions;
        }

        public void TestRegionGetAll()
        {
            IEnumerable res = regions.GetAll();
            int count = 0;
            foreach (object p in res)
            {
                Console.WriteLine(p);
                count++;
            }
            Assert.AreEqual(4, count);
        }
        public void TestRegionGetById()
        {
            Region c = (Region)regions.GetById(3);
            string trimmedResult = c.RegionDescription.Trim(); ///TODO porque é que tem espaços vazios, a info vinda da query?
            Assert.AreEqual("Northern", trimmedResult);
        }

        public void TestRegionInsertAndDelete()
        {
            //
            // Create and Insert new Category
            // 
            Region c = new Region()
            {
                RegionDescription = "Central",
            };
            Console.WriteLine("ORIGINAL1: " + c.RegionDescription + "-----" + c.RegionID);
            object id = regions.Insert(c);
            //
            // Get the new category object from database
            //
            Region actual = (Region)regions.GetById(id);
            string trimmedResult = actual.RegionDescription.Trim();
            trimmedResult = Regex.Replace(trimmedResult, @"\t|\n|\r", "");
            Assert.AreEqual(c.RegionDescription, trimmedResult);
            //
            // Delete the created category from database
            //
            regions.Delete(actual);
            object res = regions.GetById(id);
            actual = res != null ? (Region)res : default(Region);
            trimmedResult = actual.RegionDescription.Trim();
            trimmedResult = Regex.Replace(trimmedResult, @"\t|\n|\r", "");
            Assert.IsNull(trimmedResult);
        }

        public void TestRegionUpdate()
        {
            Region original = (Region)regions.GetById(3);
            Region modified = new Region()
            {
                RegionID = original.RegionID,
                RegionDescription = "Mushrooms",
            };
            regions.Update(modified);
            Region actual = (Region)regions.GetById(3);
            string trimmedResult = actual.RegionDescription.Trim();
            trimmedResult = Regex.Replace(trimmedResult, @"\t|\n|\r", "");
            
            Assert.AreEqual(modified.RegionDescription, trimmedResult);
            regions.Update(original);
            actual = (Region)regions.GetById(3);
            trimmedResult = actual.RegionDescription.Trim();
            trimmedResult = Regex.Replace(trimmedResult, @"\t|\n|\r", "");
            
            Assert.AreEqual("Northern", trimmedResult);
        }
    }
}
