using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Transactions;
using System.Xml;
using System.Data;
using System.Linq;

using MDM.Core;
using MDM.BusinessObjects;
using MDM.KnowledgeManager.Data;

namespace MDM.KnowledgeManager.Business
{
    public class UomBL : BusinessLogicBase
    {
        #region Fields

        private UomDA _uomDA = new UomDA();

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods
        public string GetAll()
        {
            return _uomDA.GetAll();
        }

        #endregion
    }
}
