using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDM.Imports.Interfaces;

namespace MDM.Imports.Processor
{
    /// <summary>
    /// Implements the ICoreAttributeObjects for the core database. 
    /// </summary>
    public class AttributeObjects : ICoreAttributeObjects
    {
        #region public methods
        public AttributeObjects()
        {
        }
        #endregion

        #region ICoreAttributeObjects Methods
        IBulkInsert ICoreAttributeObjects.GetCommonAttribueObject()
        {
            return new CommonAttributes();
        }

        IBulkInsert ICoreAttributeObjects.GetTechnicalAttributeObject()
        {
            return new TechnicalAttributes();
        }
        
        IDNSearch ICoreAttributeObjects.GetDNSearchObject()
        {
            return new DNSearch();
        }

        IBulkInsert ICoreAttributeObjects.GetRelationshipAttribueObject()
        {
            return new RelationshipAttributes();
        }


        #endregion
    }
}
