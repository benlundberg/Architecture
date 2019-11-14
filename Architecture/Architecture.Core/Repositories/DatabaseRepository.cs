using SQLite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Architecture.Core
{
    public class DatabaseRepository
    {
        private static readonly Lazy<DatabaseRepository> implementation = new Lazy<DatabaseRepository>(() => CreateStore(), isThreadSafe: true);

        public static DatabaseRepository Current
        {
            get
            {
                var ret = implementation.Value;

                if (ret == null)
                {
                    throw new NotImplementedException();
                }

                return ret;
            }
        }

        private static DatabaseRepository CreateStore()
        {
            return new DatabaseRepository();
        }

        public void Init(string databasePath)
        {
            connection = new SQLiteAsyncConnection(databasePath);
        }

        public async Task<bool> ClearAsync<T>() where T : new()
        {
            await connection.DropTableAsync<T>();
            await connection.CreateTableAsync<T>();

            return true;
        }

        public async Task<bool> DeleteAsync<T>(List<T> list)
        {
            foreach (var item in list)
            {
                await connection.DeleteAsync(item);
            }

            return true;
        }

        public async Task<bool> DeleteAsync<T>(T entity)
        {
            try
            {
                int rows = await connection.DeleteAsync(entity);

                return rows != 0;
            }
            catch (Exception ex)
            {
                ex.Print();
            }

            return false;
        }

        public async Task<IEnumerable<T>> LoadAllAsync<T>() where T : new()
        {
            try
            {
                var items = await connection.Table<T>().ToListAsync();

                return items;
            }
            catch (Exception ex)
            {
                ex.Print();
            }

            return default;
        }

        public async Task<IEnumerable<T>> LoadAllAsync<T>(string query) where T : new()
        {
            try
            {
                var items = await connection.QueryAsync<T>(query);

                return items;
            }
            catch (Exception ex)
            {
                ex.Print();
            }

            return default;
        }

        public async Task<IEnumerable<T>> LoadAllAsync<T>(Expression<Func<T, bool>> predExpr) where T : new()
        {
            try
            {
                var items = await connection.Table<T>().Where(predExpr)?.ToListAsync();

                return items;
            }
            catch (Exception ex)
            {
                ex.Print();
            }

            return default;
        }

        public async Task<T> LoadAsync<T>(object id) where T : new()
        {
            try
            {
                var item = await connection.GetAsync<T>(id);

                return item;
            }
            catch (Exception ex)
            {
                ex.Print();
            }

            return default;
        }

        public async Task<T> LoadAsync<T>(string query) where T : new()
        {
            try
            {
                var item = await connection.FindAsync<T>(query);

                return item;
            }
            catch (Exception ex)
            {
                ex.Print();
            }

            return default;
        }

        public async Task<T> LoadWithChildrenAsync<T>(object id) where T : new()
        {
            var item = await this.LoadAsync<T>(id);

            if (item == null)
            {
                return item;
            }

            return await LoadChildrenAsync(item, id);
        }

        private async Task<T> LoadChildrenAsync<T>(T item, object pk) where T : new()
        {
            // Get child property-objects with attribute indicating they need to be stored in db
            IEnumerable<PropertyInfo> propertyInfos = GetPropertyInfos(item.GetType(), typeof(ListAttribute), typeof(SingleAttribute));

            // Return item if no properties is stored
            if (propertyInfos?.Any() != true)
            {
                return item;
            }

            foreach (var propertyInfo in propertyInfos)
            {
                // The type the property is
                Type correctType;

                bool isList = false;

                if (HasAttribute(propertyInfo, typeof(ListAttribute)))
                {
                    // If the property is a list
                    correctType = propertyInfo.PropertyType.GenericTypeArguments[0];
                    isList = true;
                }
                else
                {
                    // If the property is a single object
                    correctType = propertyInfo.PropertyType;
                }

                // The child property that holds the foreign key
                PropertyInfo foreignKey = GetForeignKeyPropertyInfo(correctType, typeof(T));

                // Return if childs fk is missing
                if (foreignKey == null)
                {
                    continue;
                }

                // Get table mapping from the type
                TableMapping tableMapping = await connection.GetMappingAsync(correctType);

                // Get name of table and column
                string tableName = tableMapping.TableName;
                string colName = tableMapping.FindColumnWithPropertyName(foreignKey.Name)?.Name;

                // Query to get the rows
                var vals = await connection.QueryAsync(tableMapping, $"select * from {tableName} where {colName} = '{pk}'");

                if (vals?.Any() != true)
                {
                    continue;
                }

                if (isList)
                {
                    // If the child property has childs of it's own we need to load them as well
                    if (correctType.GetProperties().Any(prop => HasAttribute(prop, typeof(ListAttribute), typeof(SingleAttribute))))
                    {
                        var primaryKeyProperty = correctType.GetProperty(tableMapping.PK.PropertyName);

                        var list = new List<object>();

                        foreach (var val in vals)
                        {
                            var child = await LoadChildrenAsync(val, primaryKeyProperty.GetValue(val));

                            if (child != null)
                            {
                                list.Add(child);
                            }
                        }

                        propertyInfo.SetValue(item, ConvertTo(list, correctType));
                    }
                    else
                    {
                        propertyInfo.SetValue(item, ConvertTo(vals, correctType));
                    }
                }
                else
                {
                    // If the child property has childs of it's own we need to load them as well
                    if (correctType.GetProperties().Any(prop => HasAttribute(prop, typeof(ListAttribute), typeof(SingleAttribute))))
                    {
                        var primaryKeyProperty = correctType.GetProperty(tableMapping.PK.PropertyName);

                        var val = vals.FirstOrDefault();

                        var child = await LoadChildrenAsync(val, primaryKeyProperty.GetValue(val));

                        propertyInfo.SetValue(item, child);
                    }
                    else
                    {
                        propertyInfo.SetValue(item, vals.FirstOrDefault());
                    }
                }
            }

            return item;
        }

        public async Task<bool> InsertAsync<T>(T entity)
        {
            int rows = await connection.InsertAsync(entity);

            return rows != 0;
        }

        public async Task<bool> InsertWithChildrenAsync<T>(T entity)
        {
            // Get properties that will be stored in there own tables
            IEnumerable<PropertyInfo> properties = GetPropertyInfos(entity.GetType(), typeof(ListAttribute), typeof(SingleAttribute));

            if (properties?.Any() == true)
            {
                // Get the primary value of parent
                object primaryKey = GetPropertyInfo(entity.GetType(), typeof(PrimaryKeyAttribute))?.GetValue(entity);

                foreach (var property in properties)
                {
                    // Get value from the child property
                    var val = property.GetValue(entity);

                    if (val is IList list)
                    {
                        // If the value is a list we need to store them each of their own
                        foreach (var item in list)
                        {
                            // Set childs foreign key to the parents primary key
                            GetForeignKeyPropertyInfo(item.GetType(), typeof(T))?.SetValue(item, primaryKey);

                            // Insert child
                            await InsertWithChildrenAsync(item);
                        }
                    }
                    else
                    {
                        // Set childs foreign key to the parents primary key
                        GetForeignKeyPropertyInfo(val.GetType(), typeof(T))?.SetValue(val, primaryKey);

                        // Insert child
                        await InsertWithChildrenAsync(val);
                    }
                }
            }

            int rows = await connection.InsertAsync(entity);

            return rows != 0;
        }

        public async Task<bool> InsertOrReplaceAsync<T>(T entity)
        {
            int row = await connection.InsertOrReplaceAsync(entity);

            return row != 0;
        }

        public async Task<bool> UpdateAsync<T>(T entity)
        {
            int rows = await connection.UpdateAsync(entity);

            return rows != 0;
        }

        public async Task<bool> CreateTablesAsync(List<Type> entities)
        {
            foreach (var item in entities)
            {
                await connection.CreateTablesAsync(CreateFlags.None, item);
            }

            return true;
        }

        public async Task<bool> InsertAsync<T>(List<T> entities)
        {
            int rows = await connection.InsertAllAsync(entities);

            return rows != 0;
        }

        public async Task ExecuteQueryAsync(string query)
        {
            await connection.ExecuteAsync(query);
        }

        private SQLiteAsyncConnection connection;

        #region Helper methods

        private IList ConvertTo(IList source, Type itemType)
        {
            var listType = typeof(List<>);

            Type[] typeArgs = { itemType };

            var genericListType = listType.MakeGenericType(typeArgs);

            var typedList = (IList)Activator.CreateInstance(genericListType);

            foreach (var item in source)
            {
                typedList.Add(item);
            }

            return typedList;
        }

        private bool HasAttribute(PropertyInfo propertyInfo, params Type[] types)
        {
            foreach (var type in types)
            {
                if (propertyInfo.IsDefined(type, inherit: false))
                {
                    return true;
                }
            }

            return false;
        }

        private IEnumerable<PropertyInfo> GetPropertyInfos(Type type, params Type[] types)
        {
            return type.GetProperties().Where(prop => HasAttribute(prop, types));
        }

        private PropertyInfo GetPropertyInfo(Type type, params Type[] types)
        {
            return type.GetProperties().FirstOrDefault(prop => HasAttribute(prop, types));
        }

        private PropertyInfo GetForeignKeyPropertyInfo(Type type, Type typeOf)
        {
            return type.GetProperties().FirstOrDefault(prop => prop.IsDefined(typeof(ForeignKeyAttribute), false) && prop.GetCustomAttribute<ForeignKeyAttribute>()?.ForeignTypes?.Contains(typeOf) == true);
        }

        #endregion
    }

    public class ListAttribute : Attribute
    {
    }

    public class SingleAttribute : Attribute
    {
    }

    public class ForeignKeyAttribute : Attribute
    {
        public ForeignKeyAttribute(params Type[] types)
        {
            ForeignTypes = types?.Any() == true ? new List<Type>(types) : new List<Type>();
        }

        public IList<Type> ForeignTypes { get; set; }
    }
}
