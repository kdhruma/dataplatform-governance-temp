using System;
using System.Collections.Generic;

namespace MDM.Utility
{
    using MDM.Core;

    /// <summary>
    /// Represents a class which acts as a collection of the specified object type based on context parameters.
    /// </summary>
    public class ContextualObjectCollection<T>
    {
        #region Fields

        /// <summary>
        /// Represents the object context key format
        /// </summary>
        private const String OBJECT_CONTEXT_KEY_FORMAT = "OBJECT_CON{0}_ET{1}_CAT{2}";

        /// <summary>
        /// Holds the objects based on the context specified(Container Id, Category Id, Entity Type) 
        /// </summary>
        private IDictionary<String, T> _contextualObjects = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Instantiates the class using the default constructor.
        /// </summary>
        public ContextualObjectCollection()
        {
            _contextualObjects = new Dictionary<String, T>();
        }

        /// <summary>
        /// Instantiates the class using the specified object with a default context.
        /// </summary>
        /// <param name="mdmObject">Represents the object to be added for the default context</param>
        public ContextualObjectCollection(T mdmObject) : this()
        {
            SetObject(mdmObject);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Sets the attribute model collection based on the context parameters.
        /// </summary>
        /// <param name="obj">Specifies the object to be added for the context</param>
        /// <param name="containerId">Specifies the container id which is a part of the context</param>
        /// <param name="entityTypeId">Specifies the entity type id which is a part of the context</param>
        /// <param name="categoryId">Specifies the category id which is a part of the context</param>        
        public void SetObject(T obj, Int32 containerId = 0, Int32 entityTypeId = 0, Int32 categoryId = 0)
        {
            String contextKey = GetContextKey(containerId, entityTypeId, categoryId);

            if (!_contextualObjects.ContainsKey(contextKey))
            {
                _contextualObjects.Add(contextKey, obj);
            }
        }

        /// <summary>
        /// Returns the object based on the context parameters.
        /// </summary>
        /// <param name="containerId">Specifies the container id for fetching the object</param>
        /// <param name="entityTypeId">Specifies the entity type id for fetching the object</param>
        /// <param name="categoryId">Specifies the category id for fetching the object</param>        
        /// <returns>An object based on the context parameters specified</returns>
        public T GetObjectByContext(Int32 containerId = 0, Int32 entityTypeId = 0, Int32 categoryId = 0)
        {
            String contextKey = GetContextKey(containerId, entityTypeId, categoryId);

            T obj = default(T);
            _contextualObjects.TryGetValue(contextKey, out obj);

            obj = (T)Convert.ChangeType(obj, typeof(T));
            return obj;
        }

        #endregion

        #region Private Methods

        private String GetContextKey(Int32 containerId, Int32 entityTypeId, Int32 categoryId)
        {
            return String.Format(OBJECT_CONTEXT_KEY_FORMAT, containerId, entityTypeId, categoryId);
        }

        #endregion
    }
}
