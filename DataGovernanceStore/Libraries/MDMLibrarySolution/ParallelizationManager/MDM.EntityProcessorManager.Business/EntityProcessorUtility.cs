using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDM.BusinessObjects;
using System.Collections.ObjectModel;
using MDM.Core;
using MDM.Utility;
using MDM.MessageManager.Business;

namespace MDM.EntityProcessorManager.Business
{
    public sealed class EntityProcessorUtility
    {
        private LocaleMessageBL _localeMessageBL = new LocaleMessageBL();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityOperationResult"></param>
        /// <param name="callerContext"></param>
        public void EvaluateOperationResult(EntityOperationResult entityOperationResult, CallerContext callerContext)
        {

            // TODO:: to be moved to common place
            if (entityOperationResult != null)
            {
                // Remove params from Information for Entity
                this.EvaluateInformation(entityOperationResult.Informations, callerContext);

                // Remove params from Warning for Entity
                this.EvaluateWarning(entityOperationResult.Warnings, callerContext);

                // Remove params from Error for Entity
                this.EvaluateError(entityOperationResult.Errors, callerContext);

                if (entityOperationResult.AttributeOperationResultCollection != null && entityOperationResult.AttributeOperationResultCollection.Count > 0)
                {
                    foreach (AttributeOperationResult attributeOperationResult in entityOperationResult.AttributeOperationResultCollection)
                    {
                        // Remove params from Information for attribute
                        this.EvaluateInformation(attributeOperationResult.Informations, callerContext);

                        // Remove params from Warning for attribute
                        this.EvaluateWarning(attributeOperationResult.Warnings, callerContext);

                        // Remove params from Error for attribute
                        this.EvaluateError(attributeOperationResult.Errors, callerContext);
                    }
                }

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="informationCollection"></param>
        private void EvaluateInformation(InformationCollection informationCollection, CallerContext callerContext)
        {
            if (informationCollection != null && informationCollection.Count > 0)
            {
                foreach (Information information in informationCollection)
                {
                    if (!String.IsNullOrEmpty(information.InformationCode))
                    {
                        LocaleMessage localeMessage = GetActualMessage(information.InformationCode, information.Params, callerContext);
                        if (localeMessage != null && !String.IsNullOrWhiteSpace(localeMessage.Message))
                            information.InformationMessage = localeMessage.Message;
                    }
                    information.Params = null;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="warningCollection"></param>
        private void EvaluateWarning(WarningCollection warningCollection, CallerContext callerContext)
        {
            if (warningCollection != null && warningCollection.Count > 0)
            {
                foreach (Warning warning in warningCollection)
                {
                    if (!String.IsNullOrEmpty(warning.WarningCode))
                    {
                        LocaleMessage localeMessage = GetActualMessage(warning.WarningCode, warning.Params, callerContext);
                        if (localeMessage != null && !String.IsNullOrWhiteSpace(localeMessage.Message))
                            warning.WarningMessage = localeMessage.Message;
                    }
                    warning.Params = null;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorCollection"></param>
        private void EvaluateError(ErrorCollection errorCollection, CallerContext callerContext)
        {
            if (errorCollection != null && errorCollection.Count > 0)
            {
                foreach (Error error in errorCollection)
                {
                    if (!String.IsNullOrEmpty(error.ErrorCode))
                    {
                        LocaleMessage localeMessage = GetActualMessage(error.ErrorCode, error.Params, callerContext);
                        if (localeMessage != null && !String.IsNullOrWhiteSpace(localeMessage.Message))
                            error.ErrorMessage = localeMessage.Message;
                    }
                    error.Params = null;
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageCode"></param>
        /// <param name="parameters"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private LocaleMessage GetActualMessage(String messageCode, Collection<Object> parameters, CallerContext callerContext)
        {
            // TODO:: to be moved to common place
            LocaleMessage localeMessage = null;
            Object[] actualMessage = null;

            if (parameters != null)
            {
                actualMessage = ValueTypeHelper.ConvertObjectCollectionToArray(parameters);
            }

            localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), messageCode, actualMessage, false, callerContext);
            return localeMessage;
        }
    }
}
