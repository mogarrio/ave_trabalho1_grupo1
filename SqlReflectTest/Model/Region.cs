﻿using SqlReflect.Attributes;

namespace SqlReflectTest.Model
{
    [Table("Region")]
    public struct Region
    {
        [PK]
        [NotIdentity]
        public int RegionID { get; set; }
        public string RegionDescription { get; set; }
    }
}