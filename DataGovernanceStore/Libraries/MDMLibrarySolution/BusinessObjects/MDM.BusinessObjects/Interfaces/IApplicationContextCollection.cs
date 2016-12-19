using System;
using System.Collections.Generic;
namespace MDM.Interfaces
{
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get the application context collection.
    /// </summary>
    public interface IApplicationContextCollection : ICollection<ApplicationContext>
    {
        /// <summary>
        /// Add new instance into the IApplicationContextCollection
        /// </summary>
        /// <param name="context">IApplication Context Object need to be added in to the collection</param>
        void Add(IApplicationContext context);
        
        /// <summary>
        /// Clear IApplicationContextCollection
        /// </summary>
        new void Clear();
        
        /// <summary>
        /// Check whather the given item is present into the Collection of not.
        /// </summary>
        /// <param name="context">Represents IApplicationContext to be ensured in to the IApplicationContextCollection</param>
        /// <returns>
        ///     <c>true</c> if item is found in the IApplicationContextCollection; otherwise, <c>false</c>.
        /// </returns>
        Boolean Contains(IApplicationContext context);
        
        /// <summary>
        /// Check whather item having the given id is present into the Collection of not.
        /// </summary>
        /// <param name="Id">Represents id of IApplicationContext to be ensured in to the IApplicationContextCollection</param>
        /// <returns>
        ///     <c>true</c> if item is found in the IApplicationContextCollection; otherwise, <c>false</c>.
        /// </returns>
        Boolean Contains(Int32 Id);
        
        /// <summary>
        /// Get the count of no of IApplicationContext present into the IApplicationContextCollection
        /// </summary>
        new Int32 Count { get; }
        
        /// <summary>
        /// Remove item from the IApplicationContextCollection
        /// </summary>
        /// <param name="context">Represent item to be removed from the IApplicationContextCollection</param>
        /// <returns>
        ///     <c>true</c> if item is removed from the IApplicationContextCollection; otherwise, <c>false</c>.
        /// </returns>
        Boolean Remove(IApplicationContext context);

        /// <summary>
        /// Remove item of given id from the IApplicationContextCollection
        /// </summary>
        /// <param name="contextId">Represent id of item to be removed from the IApplicationContextCollection</param>
        /// <returns>
        ///     <c>true</c> if item is removed from the IApplicationContextCollection; otherwise, <c>false</c>.
        /// </returns>
        Boolean Remove(Int32 contextId);
        
        /// <summary>
        /// Get Xml representation of IApplicationContextCollection
        /// </summary>
        /// <returns>Get Xml representation of IApplicationContextCollection</returns>
        string ToXml();
        
        /// <summary>
        /// Get Xml representation of IApplicationContextCollection
        /// </summary>
        /// <param name="serialization">Type of ObjectSerialization like UIProcess, External...</param>
        /// <returns>Get Xml representation of IApplicationContextCollection</returns>
        string ToXml(MDM.Core.ObjectSerialization serialization);
        
        /// <summary>
        /// Get ApplicationContext from the IApplicationContextCollection by given Id
        /// </summary>
        /// <param name="Id">Id of the IApplicationContext</param>
        /// <returns>IApplicationContext</returns>
        IApplicationContext GetApplicationContext(Int32 Id);
    }
}
