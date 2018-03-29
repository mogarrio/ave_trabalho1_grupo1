using SqlReflect;
using SqlReflectTest.Model;
using System;
using System.Data.SqlClient;

namespace SqlReflectTest.DataMappers
{
    class RegionDataMapper : AbstractDataMapper
    {
        const string COLUMNS = "RegionDescription, RegionID";
        const string SQL_GET_ALL = @"SELECT RegionID, " + COLUMNS + " FROM Region";
        const string SQL_GET_BY_ID = SQL_GET_ALL + " WHERE RegionID=";
        const string SQL_INSERT = "INSERT INTO Region (" + COLUMNS + ") OUTPUT INSERTED.RegionID VALUES ";
        const string SQL_DELETE = "DELETE FROM Region WHERE RegionID = ";
        const string SQL_UPDATE = "UPDATE Region SET RegionDescription={1} WHERE RegionID = {0}";
        

        public RegionDataMapper(string connStr) : base(connStr) ///TODO duvida, porque é contrutor não tem nada implementado?
        {
        }

        protected override string SqlGetAll()
        {
            return SQL_GET_ALL;
        }
        protected override string SqlGetById(object id)
        {
            return SQL_GET_BY_ID + id;
        }

        protected override object Load(SqlDataReader dr)
        {
            Region c = new Region();
            c.RegionID = (int)dr["RegionID"]; /// TODO porque não se usa get()?, se sim, porque temos get e set?
            c.RegionDescription = (string)dr["RegionDescription"];
            return c;
        }

        protected override string SqlInsert(object target)
        {
            Region c = (Region)target;
            string values = "'" + c.RegionDescription + "'" + ", " + c.RegionID;
            return SQL_INSERT + "(" + values + ")";
        }

        protected override string SqlUpdate(object target)
        {
            Region c = (Region)target;
            return String.Format(SQL_UPDATE,
                c.RegionID,
                "'" + c.RegionDescription + "'"
            );
        }

        protected override string SqlDelete(object target)
        {
            Region c = (Region)target;
            return SQL_DELETE + c.RegionID;
        }
    }
}
