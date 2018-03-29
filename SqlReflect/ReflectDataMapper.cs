using SqlReflect.Attributes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace SqlReflect {
    public class ReflectDataMapper : AbstractDataMapper {
        private Type klass;
        private string tableName;

       /// Dictionary<string, Propriedades> discovery;
       /// <summary>
       ///          classe             NomePropriedade, Cenas a guardar
       /// Propriedades
           /// InfoProperty    id;
           ///                Nome;
           ///                Get;
           ///                Set;
       ///
       /// 
       /// </summary>
       /// 


        private string idField;
        private string COLUMNN;
        const string C_SQL_GET_ALL = "SELECT {0}, {1} FROM {2} ";
        const string C_SQL_GET_BY_ID = C_SQL_GET_ALL + " WHERE {3}={4} ";
        const string C_SQL_INSERT = "INSERT INTO {0} ({1}) OUTPUT INSERTED.{2} VALUES ({3})";
        const string C_SQL_DELETE = "DELETE FROM {0} WHERE {1} = {2}";
        const string C_SQL_UPDATE = "UPDATE {0} SET {1} WHERE {2} = {3}";


        public ReflectDataMapper(Type klass, string connStr) : base(connStr) {
            this.klass = klass;

            TableAttribute att = (TableAttribute)klass.GetCustomAttribute(typeof(TableAttribute), false);
            tableName = att.Name;
            List<string> propertyList = new List<string>();

            foreach (var p in klass.GetProperties()) {
                string propertyName = p.Name;

                PKAttribute pk = (PKAttribute)p.GetCustomAttribute(typeof(PKAttribute));

                if (pk == null) {
                    Type propertyType = p.PropertyType;
                    foreach (var property in propertyType.GetProperties()) {
                        PKAttribute propertyPk = (PKAttribute)property.GetCustomAttribute(typeof(PKAttribute));
                        if (propertyPk != null) {
                            propertyName = property.Name;
                        }
                    }
                    propertyList.Add(propertyName);
                } else {
            
                    idField = p.Name;
                }
            }
            COLUMNN = string.Join(",", propertyList);

        }

        protected override object Load(SqlDataReader dr) {
            object item = Activator.CreateInstance(klass);

            foreach (var p in klass.GetProperties()) {
                Type propertyType = p.PropertyType;
                object setParam;

                if (!propertyType.IsPrimitive && propertyType != typeof(string)) {
                    string connStr = this.connStr;
                    ReflectDataMapper reflectDataMapper = new ReflectDataMapper(propertyType, connStr);
                    setParam = reflectDataMapper.GetById(dr[reflectDataMapper.idField].ToString());
                } else {
                    setParam = dr[p.Name];
                }
                if (setParam.GetType() == typeof(DBNull)) {
                    if (propertyType.IsPrimitive) {
                        setParam = 0;
                    } else {
                        setParam = null;
                    }
                }
                p.SetValue(item, setParam);
            }
            return item;
        }

        protected override string SqlGetAll() {
            string SQL_GET_ALL = String.Format(C_SQL_GET_ALL, idField, COLUMNN, tableName);
            return SQL_GET_ALL;

        }

        protected override string SqlGetById(object id) {
            string SQL_GET_BY_ID = String.Format(C_SQL_GET_BY_ID, idField, COLUMNN, tableName, idField, id);
            return SQL_GET_BY_ID;
        }

        protected override string SqlInsert(object target) {
            List<string> values = new List<string>();
            string columnsToInsert = COLUMNN;
            foreach (var p in klass.GetProperties()) {
                MethodInfo pGet = p.GetGetMethod();
                string valueString = "";

                if (p.Name != idField) {
                    object propertyValue = pGet.Invoke(target, null);
                    Type propertyType = p.PropertyType;

                    if (propertyType.IsPrimitive) {

                        valueString = propertyValue.ToString();

                    } else if (propertyType == typeof(string)) {

                        valueString = "'" + (string)propertyValue + "'";

                    } else {
  
                        foreach (var property in propertyType.GetProperties()) {

                            PKAttribute propertyPk = (PKAttribute)property.GetCustomAttribute(typeof(PKAttribute));

                            if (propertyPk != null) {
                                MethodInfo pGetProperty = property.GetGetMethod();
                                object objectPropertyValue = pGetProperty.Invoke(propertyValue, null);
                                valueString = objectPropertyValue.ToString();
                            }
                        }
                    }
                    values.Add(valueString);
                } else {
                    NotIdentity pNotIdentity = (NotIdentity)p.GetCustomAttribute(typeof(NotIdentity));
                    if (pNotIdentity != null) {
                        MethodInfo pGetProperty = p.GetGetMethod();
                        object objectPropertyValue = pGetProperty.Invoke(target, null);
                        valueString = objectPropertyValue.ToString();
                        values.Add(valueString);
                        columnsToInsert = idField + ", " + columnsToInsert;
                    }
                }
            }
            string sqlValues = string.Join(",", values);
            string SQL_INSERT = String.Format(C_SQL_INSERT, tableName, columnsToInsert, idField, sqlValues);
            return SQL_INSERT;
        }

        protected override string SqlDelete(object target) {
            string idValue = "";
            foreach (var p in klass.GetProperties()) {
                MethodInfo pGet = p.GetGetMethod();

                if (p.Name == idField) {
                    idValue = (pGet.Invoke(target, null).ToString());
                }
            }

            string SQL_DELETE = String.Format(C_SQL_DELETE, tableName, idField, idValue);
            return SQL_DELETE;
        }

        protected override string SqlUpdate(object target) {
            List<string> valuesToInsert = new List<string>();
            string idValue = "";
            foreach (var p in klass.GetProperties()) {
                MethodInfo pGet = p.GetGetMethod();

                if (p.Name == idField) {
                    idValue = pGet.Invoke(target, null).ToString();
                } else {
                    object propertyValue = pGet.Invoke(target, null);
                    string sqlValueQuery = p.Name + " = '" + (string)propertyValue + "'";

                    valuesToInsert.Add(sqlValueQuery);
                }
            }
            string sqlValuesToInsert = string.Join(",", valuesToInsert);

            string SQL_UPDATE = String.Format(C_SQL_UPDATE, tableName, sqlValuesToInsert, idField, idValue);
            Console.WriteLine(SQL_UPDATE);
            return SQL_UPDATE;
        }
    }
}
