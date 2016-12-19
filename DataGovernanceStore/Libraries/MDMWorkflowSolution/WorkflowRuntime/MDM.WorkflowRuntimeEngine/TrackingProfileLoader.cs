using System;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Activities.Tracking;
using System.ServiceModel.Activities.Tracking.Configuration;

namespace MDM.WorkflowRuntimeEngine
{
    /// <summary>
    /// Helps in loading the Tracking Profile from the configuration file
    /// </summary>
    public class TrackingProfileLoader
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public TrackingProfileLoader()
        {
  
        }

        #endregion

        #region Methods

        /// <summary>
        /// Loads the tracking profile from web configuration file
        /// </summary>
        /// <param name="profileName">The Name of the profile to be loaded</param>
        public static TrackingProfile LoadFromWebConfig(String profileName)
        {
            TrackingProfile profile = null;
            TrackingSection ts = (TrackingSection)System.Configuration.ConfigurationManager.GetSection("system.serviceModel/tracking");

            if (ts != null && ts.TrackingProfiles != null)
            {
                profile = (from tp in ts.TrackingProfiles
                                           where tp.Name == profileName
                                           select tp).SingleOrDefault();

                if (profile == null)
                {
                    throw new ArgumentException(String.Format("Tracking Profile {0} not found in app.config", profileName));
                }
            }

            return profile;
        }

        /// <summary>
        /// Loads the tracking profile from passed XML
        /// </summary>
        /// <param name="trackingProfileXML">The tracking profile XML</param>
        public static TrackingProfile LoadFromInputXML(String trackingProfileXML)
        {
            if (String.IsNullOrEmpty(trackingProfileXML))
            {
                return LoadFromWebConfig("MDMTrackingProfile");
            }

            TrackingProfile profile = new TrackingProfile();

            XmlDocument xmlDocument = new XmlDocument();

            //Load the tracking profile XML
            xmlDocument.LoadXml(trackingProfileXML);

            XmlNode rootNode = xmlDocument.SelectSingleNode("trackingProfile");

            //Get the name of the tracking profile from root node
            if (rootNode != null && rootNode.Attributes != null)
            {
                XmlAttribute nameAttribute = rootNode.Attributes["name"];

                if (nameAttribute != null && nameAttribute.Value != null)
                    profile.Name = nameAttribute.Value.ToString();
            }

            //Extract WorkflowInstanceQueries and add to the tracking profile
            foreach (XmlNode workflowInstanceQueryNode in xmlDocument.SelectNodes("trackingProfile/workflow/workflowInstanceQueries/workflowInstanceQuery"))
            {
                WorkflowInstanceQuery workflowInstanceQuery = new WorkflowInstanceQuery();

                //Get the workflow states
                foreach (XmlNode stateNode in workflowInstanceQueryNode.SelectNodes("states/state"))
                {
                    XmlAttribute stateNameAttr = stateNode.Attributes["name"];

                    if (stateNameAttr != null && stateNameAttr.Value != null)
                    {
                        workflowInstanceQuery.States.Add(stateNameAttr.Value.ToString());

                        if (stateNameAttr.Value.ToString() == "*")
                            break;
                    }
                }

                profile.Queries.Add(workflowInstanceQuery);
            }

            //Extract ActivityStateQueries and add to the tracking profile
            foreach (XmlNode activityStateQueryNode in xmlDocument.SelectNodes("trackingProfile/workflow/activityStateQueries/activityStateQuery"))
            {
                ActivityStateQuery activityStateQuery = new ActivityStateQuery();

                //Get the ActivityName for which query is defined
                XmlAttribute activityName = activityStateQueryNode.Attributes["activityName"];

                if (activityName != null && activityName.Value != null)
                    activityStateQuery.ActivityName = activityName.Value.ToString();
                else
                    activityStateQuery.ActivityName = "*";

                //Get the activity states
                foreach (XmlNode stateNode in activityStateQueryNode.SelectNodes("states/state"))
                {
                    XmlAttribute stateNameAttr = stateNode.Attributes["name"];

                    if (stateNameAttr != null && stateNameAttr.Value != null)
                    {
                        activityStateQuery.States.Add(stateNameAttr.Value.ToString());

                        if (stateNameAttr.Value.ToString() == "*")
                            break;
                    }
                }

                //Get the activity arguments
                foreach (XmlNode argumentNode in activityStateQueryNode.SelectNodes("arguments/argument"))
                {
                    XmlAttribute argumentNameAttr = argumentNode.Attributes["name"];

                    if (argumentNameAttr != null && argumentNameAttr.Value != null)
                    {
                        activityStateQuery.Arguments.Add(argumentNameAttr.Value.ToString());

                        if (argumentNameAttr.Value.ToString() == "*")
                            break;
                    }
                }

                //Get the activity variables
                foreach (XmlNode variableNode in activityStateQueryNode.SelectNodes("variables/variable"))
                {
                    XmlAttribute variableNameAttr = variableNode.Attributes["name"];

                    if (variableNameAttr != null && variableNameAttr.Value != null)
                    {
                        activityStateQuery.Variables.Add(variableNameAttr.Value.ToString());

                        if (variableNameAttr.Value.ToString() == "*")
                            break;
                    }
                }

                profile.Queries.Add(activityStateQuery);
            }

            //Extract CustomTrackingQueries and add to the tarcking profile
            foreach (XmlNode customTrackingQueryNode in xmlDocument.SelectNodes("trackingProfile/workflow/customTrackingQueries/customTrackingQuery"))
            {
                CustomTrackingQuery customTrackingQuery = new CustomTrackingQuery();

                XmlAttribute nameAttr = customTrackingQueryNode.Attributes["name"];

                if (nameAttr != null && nameAttr.Value != null)
                    customTrackingQuery.Name = nameAttr.Value.ToString();

                XmlAttribute activityNameAttr = customTrackingQueryNode.Attributes["activityName"];

                if (activityNameAttr != null && activityNameAttr.Value != null)
                    customTrackingQuery.ActivityName = activityNameAttr.Value.ToString();

                profile.Queries.Add(customTrackingQuery);
            }

            //Extract FaultPrpagationQueries and add to the profile
            foreach (XmlNode faultPropagationQueryNode in xmlDocument.SelectNodes("trackingProfile/workflow/faultPropagationQueries/faultPropagationQuery"))
            {
                FaultPropagationQuery faultPropagationQuery = new FaultPropagationQuery();

                XmlAttribute faultSourceActivityNameAttr = faultPropagationQueryNode.Attributes["faultSourceActivityName"];

                if (faultSourceActivityNameAttr != null && faultSourceActivityNameAttr.Value != null)
                    faultPropagationQuery.FaultSourceActivityName = faultSourceActivityNameAttr.Value.ToString();

                XmlAttribute faultHandlerActivityNameAttr = faultPropagationQueryNode.Attributes["faultHandlerActivityName"];

                if (faultHandlerActivityNameAttr != null && faultHandlerActivityNameAttr.Value != null)
                    faultPropagationQuery.FaultHandlerActivityName = faultHandlerActivityNameAttr.Value.ToString();

                profile.Queries.Add(faultPropagationQuery);
            }

            return profile;
        }

        #endregion
    }
}
