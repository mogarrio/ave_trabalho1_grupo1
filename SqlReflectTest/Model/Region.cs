using SqlReflect.Attributes;

namespace SqlReflectTest.Model
{
    [Table("Region")]
    public struct Regions
    {
        [PK]
        public int RegionID { get; set; }
        public string RegionDescription { get; set; }
    }
}