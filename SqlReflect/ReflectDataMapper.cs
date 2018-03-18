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

        const string COLUMNN;//TODO como fazemos isto? o const nao funciona. propriedades normais?
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


            var memberInfo = klass.GetField("State");

            TableAttribute att = klass.GetCustomAttribute(TableAttribute, false);//TODO sintaxe errada? devolve a+enas 1 atributo?
            
            
            
            
            
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
                Type pType= p.PropertyType;
                object setParam = dr[p.Name]; //TODO como se faz o casting?
                object[] paramArray = (object[])Array.CreateInstance(pType, 1);
                paramArray[0] = setParam;
                pSet.Invoke(item, paramArray);//TODO existe uma maneira de fazer isto enviando apenas 1 parametro
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
