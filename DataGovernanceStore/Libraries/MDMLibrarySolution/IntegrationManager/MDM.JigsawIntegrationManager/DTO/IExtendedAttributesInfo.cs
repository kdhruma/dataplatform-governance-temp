using System;

namespace MDM.JigsawIntegrationManager.DTO
{
    public  interface IExtendedAttributesInfo
    {
        /// <summary>
        /// 
        /// </summary>
        ChangeContext JsChangeContext { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        Relationship JsRelationship { get; set; }
    }
}
