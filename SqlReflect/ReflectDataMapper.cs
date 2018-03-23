using SqlReflect.Attributes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace SqlReflect
{
    public class ReflectDataMapper : AbstractDataMapper
    {
        private Type klass;
        private string tableName;

        private string ID;
        private string COLUMNN;
        const string C_SQL_GET_ALL = "SELECT {0}, {1} FROM {2} ";
        const string C_SQL_GET_BY_ID = C_SQL_GET_ALL + " WHERE {0}={1} ";
        const string C_SQL_INSERT = "INSERT INTO {0} ({1}) OUTPUT INSERTED.{2} VALUES {3}";
        const string C_SQL_DELETE = "DELETE FROM {0} WHERE {1} = {2}";
        const string C_SQL_UPDATE = "UPDATE {0} SET {1} WHERE {2} = {3}";


        /*
        const string COLUMNS = "CategoryName, Description";
        const string SQL_GET_ALL = @"SELECT CategoryID, " + COLUMNS + " FROM Categories";
        const string SQL_GET_BY_ID = SQL_GET_ALL + " WHERE CategoryID=";
        const string SQL_INSERT = "INSERT INTO Categories (" + COLUMNS + ") OUTPUT INSERTED.CategoryID VALUES ";
        const string SQL_DELETE = "DELETE FROM Categories WHERE CategoryID = ";
        const string SQL_UPDATE = "UPDATE Categories SET CategoryName={1}, Description={2} WHERE CategoryID = {0}";*/


        public ReflectDataMapper(Type klass, string connStr) : base(connStr)
        {
            this.klass = klass;


            //var memberInfo = klass.GetField("State");

            /*SqlReflect.Attributes.TableAttribute tableAttribute = new TableAttribute("Table");
            Type type = tableAttribute.GetType();*/

            TableAttribute att = (TableAttribute)klass.GetCustomAttribute(typeof(TableAttribute), false);
            tableName = att.Name;
            List<string> propertyList = new List<string>();

            foreach (var p in klass.GetProperties())
            {

               // MethodInfo pSet = p.GetSetMethod();
                PKAttribute pk = (PKAttribute)p.GetCustomAttribute(typeof(PKAttribute));
                ID = pk != null ? p.Name : ID;
                
                propertyList.Add(p.Name);
                /*object setParam = dr[p.Name];

                pSet.Invoke(item, new object[1] { setParam });*/
            }
            COLUMNN = string.Join(",", propertyList);
            string SQL_GET_ALL = String.Format(C_SQL_GET_ALL, ID, COLUMNN, tableName);
            string SQL_GET_BY_ID = String.Format(C_SQL_GET_BY_ID, ID,new int());
            string SQL_INSERT = "";
            //array.ToString();


            /*
            object[] atts = klass.GetCustomAttributes(false);
            foreach (var att in atts)
            {
            Type aType = att.GetType();
            /*string attName = aType.Name;
            if(attName == "TableAttribute")
            {

            }
            }*/
        }

        protected override object Load(SqlDataReader dr)
        {
            object item = Activator.CreateInstance(klass);

            foreach (var p in klass.GetProperties())
            {
                MethodInfo pSet = p.GetSetMethod();

                object setParam = dr[p.Name];

                pSet.Invoke(item, new object[1] { setParam });
            }
            return item;
        }

        protected override string SqlGetAll()
        {

            throw new NotImplementedException();
        }

        protected override string SqlGetById(object id)
        {
            throw new NotImplementedException();
        }

        protected override string SqlInsert(object target)
        {
            throw new NotImplementedException();
        }

        protected override string SqlDelete(object target)
        {
            throw new NotImplementedException();
        }

        protected override string SqlUpdate(object target)
        {
            throw new NotImplementedException();
        }
    }
}
