using System;
using System.IO;
using System.Xml;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Input;
using System.Activities;
using System.Activities.Validation;
using System.Activities.Presentation;
using System.Activities.XamlIntegration;
using System.Activities.Presentation.Toolbox;
using System.Activities.Presentation.Metadata;
using System.Activities.Statements;
using System.Activities.Core.Presentation;
using Microsoft.Win32;
using System.Activities.Presentation.Services;

namespace MDM.Workflow.Designer
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Workflow;
    using MDM.Workflow.Utility;
    using MDM.Core;
    using MDM.Workflow.Activities.Core;
    /// <summary>
    /// Interaction logic for DesignerMainPage.xaml
    /// </summary>
    public partial class DesignerMainPage : Page
    {
        #region Fields

        /// <summary>
        /// 
        /// </summary>
        private TextBox _tbXAML = new TextBox();

        /// <summary>
        /// 
        /// </summary>
        private WorkflowDesigner _workflowDesigner = new WorkflowDesigner();

        /// <summary>
        /// 
        /// </summary>
        private DocumentContentPanel _docWindow = new DocumentContentPanel();

        /// <summary>
        /// 
        /// </summary>
        private DockableContentPanel _propertyWindow = new DockableContentPanel();

        /// <summary>
        /// 
        /// </summary>
        private DockableContentPanel _toolBoxWindow = new DockableContentPanel();

        /// <summary>
        /// 
        /// </summary>
        private DockableContentPanel _xamlWindow = new DockableContentPanel();

        /// <summary>
        /// 
        /// </summary>
        private WorkflowVersion _workflowVersion = null;

        /// <summary>
        /// 
        /// </summary>
        private Assembly _systemActivityAssembly = null;


        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public DesignerMainPage()
        {
            try
            {
                MDMNativeActivitiyBase.IsRuntimeExecution = false;

                InitializeComponent();
                RegisterMetadata();
                AddDesigner();

                //Set XAML editor to read only.
                //User has to modify definition by Drag and Drop and property window.
                _tbXAML.IsReadOnly = true;

                lblDescription.Content = "Workflow Designer for: <Workflow Name> - <Workflow Version Name - Version Number (Version Type)>";
            }
            catch (Exception ex)
            {
                String errorMessage = String.Format("The following error occurred while loading the designer.\nError: {0}", ex.Message);
                MessageBox.Show(errorMessage, "MDM.Workflow.Designer", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region Methods

        private void RegisterMetadata()
        {
            DesignerMetadata metaData = new DesignerMetadata();
            metaData.Register();
        }

        private void AddDesigner()
        {
            //Create an instance of WorkflowDesigner class
            this._workflowDesigner = new WorkflowDesigner();

            // Show the XAML when the model changes
            this._workflowDesigner.ModelChanged += ShowWorkflowXAML;

            //Add the WorkflowDesigner to the grid
            this._docWindow.DockManager = dockManager;
            this._docWindow.Content = this._workflowDesigner.View;
            this._docWindow.Show();

            // Add the Property Inspector (property window)
            this._propertyWindow.DockManager = dockManager;
            this._propertyWindow.Content = _workflowDesigner.PropertyInspectorView;
            this._propertyWindow.Show(Dock.Right);
            this._propertyWindow.Title = "Properties";
            //Uri uri = new Uri(@"\Icons\props.ico", UriKind.Relative);
            //this._propertyWindow.Icon = new BitmapImage(uri);

            //Load the System.Activities assembly which is a default for all the built in activities
            Type assignType = typeof(Assign);
            if (assignType != null)
                _systemActivityAssembly = assignType.Assembly;

            // Add the toolbox
            ToolboxControl tc = CreateToolboxControl();
            this._toolBoxWindow.DockManager = dockManager;
            this._toolBoxWindow.Content = tc;
            this._toolBoxWindow.Show(Dock.Left);
            this._toolBoxWindow.Title = "Activities";

            //Add the XAML editor
            this._xamlWindow.DockManager = dockManager;
            this._xamlWindow.Content = _tbXAML;
            this._xamlWindow.Show(Dock.Bottom);
            this._xamlWindow.Title = "XAML";
        }

        private void UpdateDesigner(String workflowXAMLTemplate, String workflowFileName)
        {
            //Create an instance of WorkflowDesigner class
            this._workflowDesigner = new WorkflowDesigner();

            // Show the XAML when the model changes
            this._workflowDesigner.ModelChanged += ShowWorkflowXAML;

            //Add the WorkflowDesigner to the grid
            this._docWindow.DockManager = dockManager;
            this._docWindow.Content = this._workflowDesigner.View;
            this._docWindow.Show();

            if (!String.IsNullOrEmpty(workflowXAMLTemplate))
            {
                this._workflowDesigner.Text = workflowXAMLTemplate;
                this._workflowDesigner.Load();

                // Show the XAML
                ShowWorkflowXAML(null, null);
            }
            else if (!String.IsNullOrEmpty(workflowFileName))
            {
                this._workflowDesigner.Load(workflowFileName);

                // Show the XAML
                ShowWorkflowXAML(null, null);
            }

            this._propertyWindow.Content = _workflowDesigner.PropertyInspectorView;

            //Solution to the known issue--
            //Issue:The in-built pop-up windows were not appearing on loading of a Workflow Definition
            //Solution:Hide and Show dock panels
            //Since it has to be done after loading, to accomplish this adding an event handler on dock manager mouse enter
            this.dockManager.MouseEnter += new MouseEventHandler(dockManager_MouseEnter);

            //Set the GenerateNewGUIDForActivity flag to 'true' whenever a workflow has been loaded
            //so that a new GUID has been generated whenever an activity has been dragged and dropped onto the designer surface
            //or copied and pasted the existing activity.
            //See bug no. : 12637 for further details
            WorkflowHelper.GenerateNewGUIDForActivity = true;
        }

        private void ShowWorkflowXAML(object sender, EventArgs e)
        {
            _workflowDesigner.Flush();
            _tbXAML.Text = _workflowDesigner.Text;
        }

        private ToolboxControl CreateToolboxControl()
        {
            String activitiesConfig = String.Empty;
            ToolboxControl toolBox = new ToolboxControl();

            ////Get the activities configuration XML
            OperationResult operationResult = new OperationResult();
            WorkflowWCFUtilities workflowService = new WorkflowWCFUtilities();
            activitiesConfig = workflowService.GetAppConfigValue("Workflow.Designer.Activity.Config", ref operationResult);

            if (operationResult.HasError)
                throw new Exception(operationResult.Errors[0].ErrorMessage);

            if (String.IsNullOrEmpty(activitiesConfig))
                throw new Exception("AppConfig 'Workflow.Designer.Activity.Config' value is not available.");

            //Load the activities configuration XML
            XmlDocument activitiesConfigXML = new XmlDocument();
            activitiesConfigXML.LoadXml(activitiesConfig);

            //Define the General Category 
            //This category consists the activities for which CategoryName is not defined
            ToolboxCategory generalActivityCategory = new ToolboxCategory("General");

            //Get the Attribute Builder and Resource reader to extract Activity designer
            AttributeTableBuilder builder = new AttributeTableBuilder();
            String dllPath = String.Format("{0}Microsoft.VisualStudio.Activities.dll", System.AppDomain.CurrentDomain.BaseDirectory);
            Assembly visualStudioActivitiesAssembly = Assembly.LoadFile(dllPath);
            System.Resources.ResourceReader resourceReader = new System.Resources.ResourceReader(visualStudioActivitiesAssembly.GetManifestResourceStream("Microsoft.VisualStudio.Activities.Resources.resources"));
            
            //Extract activity category nodes
            foreach (XmlNode activityCategoryNode in activitiesConfigXML.SelectNodes("CustomActivities/ActivityCategory"))
            {
                //Get the attributes
                XmlAttribute categoryNameAttr = activityCategoryNode.Attributes["CategoryName"];
                XmlAttribute categoryVisibility = activityCategoryNode.Attributes["Visible"];

                //Check whether category is set as visible..
                //If visible then only add.. else ignore the category and the activities inside that category
                if (categoryVisibility != null && categoryVisibility.Value != null && categoryVisibility.Value.ToLower() == "true")
                {
                    if (categoryNameAttr != null && !String.IsNullOrEmpty(categoryNameAttr.Value) && categoryNameAttr.Value.ToLower() != "general")
                    {
                        ToolboxCategory activityCategory = new ToolboxCategory(categoryNameAttr.Value);
                        CreateToolBoxCategory(activityCategory, activityCategoryNode, builder, resourceReader);
                        toolBox.Categories.Add(activityCategory);
                    }
                    else
                    {
                        CreateToolBoxCategory(generalActivityCategory, activityCategoryNode, builder, resourceReader);
                    }
                }
            }

            //Add General Category to Toolbox Control
            toolBox.Categories.Add(generalActivityCategory);

            return toolBox;
        }

        private void CreateToolBoxCategory(ToolboxCategory activityCategory, XmlNode activityCategoryNode, AttributeTableBuilder builder, System.Resources.ResourceReader resourceReader)
        {
            Type activityType = null;

            //Extract activities for this category node
            foreach (XmlNode activityNode in activityCategoryNode.SelectNodes("Activity"))
            {
                //Get the attributes
                XmlAttribute assemblyNameAttr = activityNode.Attributes["AssemblyName"];
                XmlAttribute typeNameAttr = activityNode.Attributes["TypeName"];
                XmlAttribute displayNameAttr = activityNode.Attributes["DisplayName"];
                XmlAttribute activityVisibility = activityNode.Attributes["Visible"];

                if (activityVisibility != null && activityVisibility.Value != null && activityVisibility.Value.ToLower() == "true" && typeNameAttr != null && !String.IsNullOrEmpty(typeNameAttr.Value))
                {
                    if (assemblyNameAttr != null && !String.IsNullOrEmpty(assemblyNameAttr.Value))
                    {
                        if (assemblyNameAttr.Value.ToLower() == "system.activities.dll" && _systemActivityAssembly != null)
                        {
                            activityType = _systemActivityAssembly.GetType(typeNameAttr.Value);
                        }
                        else
                        {
                            String assemblyFile = String.Format("{0}{1}", System.AppDomain.CurrentDomain.BaseDirectory, assemblyNameAttr.Value);
                            Assembly activityAssembly = Assembly.LoadFrom(assemblyFile);
                            activityType = activityAssembly.GetType(typeNameAttr.Value);
                        }
                    }
                    else
                        activityType = Type.GetType(typeNameAttr.Value); //must be assembly qualified

                    if (activityType != null)
                    {
                        CreateToolboxBitmapAttributeForActivity(builder, resourceReader, activityType);
                        MetadataStore.AddAttributeTable(builder.CreateTable());

                        String displayName = activityType.Name;
                        if (displayNameAttr != null && !String.IsNullOrEmpty(displayNameAttr.Value))
                            displayName = displayNameAttr.Value;

                        activityCategory.Add(new ToolboxItemWrapper(activityType, displayName));
                    }
                }
            }
        }

        private static void CreateToolboxBitmapAttributeForActivity(AttributeTableBuilder builder, System.Resources.ResourceReader resourceReader, Type builtInActivityType)
        {
            System.Drawing.Bitmap bitmap = ExtractBitmapResource(resourceReader, builtInActivityType.IsGenericType ? builtInActivityType.Name.Split('`')[0] : builtInActivityType.Name);

            if (bitmap != null)
            {
                Type tbaType = typeof(System.Drawing.ToolboxBitmapAttribute);
                Type imageType = typeof(System.Drawing.Image);
                ConstructorInfo constructor = tbaType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { imageType, imageType }, null);
                System.Drawing.ToolboxBitmapAttribute tba = constructor.Invoke(new object[] { bitmap, bitmap }) as System.Drawing.ToolboxBitmapAttribute;
                builder.AddCustomAttributes(builtInActivityType, tba);
            }
        }

        private static System.Drawing.Bitmap ExtractBitmapResource(System.Resources.ResourceReader resourceReader, string bitmapName)
        {
            System.Collections.IDictionaryEnumerator dictEnum = resourceReader.GetEnumerator();

            System.Drawing.Bitmap bitmap = null;

            while (dictEnum.MoveNext())
            {
                if (String.Equals(dictEnum.Key, bitmapName))
                {
                    bitmap = dictEnum.Value as System.Drawing.Bitmap;
                    System.Drawing.Color pixel = bitmap.GetPixel(bitmap.Width - 1, 0);
                    bitmap.MakeTransparent(pixel);
                    break;
                }
            }

            return bitmap;
        }

        private void ClearWorkflowDesigner()
        {
            //TO DO::Check for any changes and alert if any unsaved changes are there..

            if (this._workflowDesigner != null)
            {
                this._docWindow.Remove();

                LayoutGrid.Children.Remove(_workflowDesigner.View);
                LayoutGrid.Children.Remove(_workflowDesigner.PropertyInspectorView);

                this._workflowDesigner = null;
                this._workflowVersion = null;
            }
        }

        private Boolean IsValid(String workflowDefinitionXAML)
        {
            Boolean isValid = true;

            XmlTextReader xmlReader = new XmlTextReader(new StringReader(workflowDefinitionXAML));
            Activity rootActivity = ActivityXamlServices.Load(xmlReader);
            ValidationResults validationResults = ActivityValidationServices.Validate(rootActivity);

            if (validationResults != null && (validationResults.Errors.Count > 0 || validationResults.Warnings.Count > 0))
                isValid = false;

            xmlReader.Close();

            return isValid;
        }

        #endregion

        #region Events

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //clear the designer, no matter what
                ClearWorkflowDesigner();

				lblDescription.Content = "Workflow Designer for: <Workflow Name> - <Workflow Version Name - Version Number (Version Type)>";

				//Template of the new workflow
                //TODO:: Provide configuration to define templates. Fetch the active template and assign here..
                String newWorkflowXAML = @"<Activity mc:Ignorable=""sap"" x:Class=""{x:Null}"" xmlns=""http://schemas.microsoft.com/netfx/2009/xaml/activities"" xmlns:av=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" xmlns:mbw=""clr-namespace:MDM.BusinessObjects.Workflow;assembly=RS.MDM.BusinessObjects"" xmlns:mc=""http://schemas.openxmlformats.org/markup-compatibility/2006"" xmlns:mv=""clr-namespace:Microsoft.VisualBasic;assembly=System"" xmlns:mva=""clr-namespace:Microsoft.VisualBasic.Activities;assembly=System.Activities"" xmlns:s=""clr-namespace:System;assembly=mscorlib"" xmlns:s1=""clr-namespace:System;assembly=System"" xmlns:s2=""clr-namespace:System;assembly=System.Xml"" xmlns:s3=""clr-namespace:System;assembly=System.Core"" xmlns:s4=""clr-namespace:System;assembly=System.ServiceModel"" xmlns:s5=""clr-namespace:System;assembly=System.Configuration"" xmlns:sad=""clr-namespace:System.Activities.Debugger;assembly=System.Activities"" xmlns:sap=""http://schemas.microsoft.com/netfx/2009/xaml/activities/presentation"" xmlns:scg=""clr-namespace:System.Collections.Generic;assembly=System"" xmlns:scg1=""clr-namespace:System.Collections.Generic;assembly=System.ServiceModel"" xmlns:scg2=""clr-namespace:System.Collections.Generic;assembly=System.Core"" xmlns:scg3=""clr-namespace:System.Collections.Generic;assembly=mscorlib"" xmlns:sd=""clr-namespace:System.Data;assembly=System.Data"" xmlns:sl=""clr-namespace:System.Linq;assembly=System.Core"" xmlns:st=""clr-namespace:System.Text;assembly=mscorlib"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
                                                <x:Members>
                                                    <x:Property Name=""MDMDataContext"" Type=""InArgument(mbw:WorkflowDataContext)"" />
                                                </x:Members>
                                                <sap:VirtualizedContainerService.HintSize>654,676</sap:VirtualizedContainerService.HintSize>
                                                    <mva:VisualBasic.Settings>Assembly references and imported namespaces for internal implementation</mva:VisualBasic.Settings>
                                                    <Flowchart sap:VirtualizedContainerService.HintSize=""614,636"">
                                                        <Flowchart.Variables>
                                                            <Variable x:TypeArguments=""mbw:WorkflowActionContext"" Name=""MDMActionContext"" />
                                                        </Flowchart.Variables>
                                                        <sap:WorkflowViewStateService.ViewState>
                                                            <scg3:Dictionary x:TypeArguments=""x:String, x:Object"">
                                                                <x:Boolean x:Key=""IsExpanded"">False</x:Boolean>
                                                                <av:Point x:Key=""ShapeLocation"">270,2.5</av:Point>
                                                                <av:Size x:Key=""ShapeSize"">60,75</av:Size>
                                                            </scg3:Dictionary>
                                                        </sap:WorkflowViewStateService.ViewState>
                                                        <Flowchart.StartNode>
                                                            <x:Null />
                                                        </Flowchart.StartNode>
                                                    </Flowchart>
                                            </Activity>";

                //Update the designer with new XAML
                UpdateDesigner(newWorkflowXAML, null);
            }
            catch (Exception ex)
            {
                String errorMessage = String.Format("The following error occurred while loading.\nError: {0}", ex.Message);
                MessageBox.Show(errorMessage, "MDM.Workflow.Designer", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;

            SelectWorkflow selectWorkflowDialog = new SelectWorkflow();
            selectWorkflowDialog.Owner = Application.Current.MainWindow;

            this.Cursor = Cursors.Arrow;

            if (selectWorkflowDialog.ShowDialog() == true)
            {
                try
                {
                    //clear the designer, no matter what
                    ClearWorkflowDesigner();

                    Int32 requestedWorkflowVersionID = 0;
                    Int32.TryParse(selectWorkflowDialog.cbWorkflowVersion.SelectedValue.ToString(), out requestedWorkflowVersionID);

                    WorkflowWCFUtilities workflowService = new WorkflowWCFUtilities();
                    
                    OperationResult operationResult = new OperationResult();
                    _workflowVersion = new WorkflowVersion();
                    Collection<TrackedActivityInfo> trackedActivityCollection = new Collection<TrackedActivityInfo>();
                    CallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.MDMAdvanceWorkflow);

                    //Get the Workflow Definition from DB for the requested version id
                    workflowService.GetWorkflowViewDetails(requestedWorkflowVersionID, "", ref _workflowVersion, ref trackedActivityCollection, ref operationResult,callerContext);

                    if (operationResult != null && operationResult.HasError)
                        throw new Exception(operationResult.Errors[0].ErrorMessage);

                    if (_workflowVersion != null && !String.IsNullOrEmpty(_workflowVersion.WorkflowDefinition))
                    {
                        //Update Workflow Breadcrumb
                        String descriptionString = String.Format("Workflow Designer for: {0} - {1} - {2} ({3})", _workflowVersion.WorkflowLongName, _workflowVersion.VersionName, _workflowVersion.VersionNumber, _workflowVersion.VersionType);
                        lblDescription.Content = descriptionString;

                        //Update designer with this workflow definition
                        UpdateDesigner(_workflowVersion.WorkflowDefinition, null);
                    }
                    else
                    {
                        throw new Exception("Workflow Definition is not available for this workflow.");
                    }
                }
                catch (Exception ex)
                {
                    this.Cursor = Cursors.Arrow;
                    String errorMessage = String.Format("The following error occurred while opening the workflow.\nError: {0}", ex.Message);
                    MessageBox.Show(errorMessage, "MDM.Workflow.Designer", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Xaml files (*.xaml)|*.xaml|Xamlx files (*.xamlx)|*.xamlx";
                
                if (openFileDialog.ShowDialog() == true)
                {
                    //clear the designer, no matter what
                    ClearWorkflowDesigner();

                    lblDescription.Content = "Workflow Designer for: <Workflow Name> - <Workflow Version Name - Version Number (Version Type)>";

                    //Update the designer with new XAML
                    UpdateDesigner(null, openFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                String errorMessage = String.Format("The following error occurred while importing.\nError: {0}", ex.Message);
                MessageBox.Show(errorMessage, "MDM.Workflow.Designer", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSaveAsDraft_Click(object sender, RoutedEventArgs e)
        {
            WorkflowWCFUtilities workflowService = null;
            OperationResult operationResult = null;

            if (this._workflowDesigner != null && !String.IsNullOrEmpty(this._workflowDesigner.Text))
            {
                if (!this.IsValid(this._workflowDesigner.Text))
                {
                    MessageBox.Show("Workflow has some errors. Please resolve them before saving.", "MDM.Workflow.Designer", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                try
                {
                    EnterWorkflowDetails enterWorkflowDetailsDialog = new EnterWorkflowDetails();

                    //Check for Workflow Version
                    if (this._workflowVersion != null)
                    {
                        //Version is already been populated..
                        //It means we are opening the already created workflow

                        //Pass the workflow details to EnterWorkflowDetails Dialog
                        enterWorkflowDetailsDialog.tbWorkflowName.Text = this._workflowVersion.WorkflowShortName;
                        enterWorkflowDetailsDialog.tbWorkflowLongName.Text = this._workflowVersion.WorkflowLongName;

                        if (this._workflowVersion.WorkflowId > 0)
                        {
                            //Since the workflow already been published, do not allow user to edit workflow name and short name
                            enterWorkflowDetailsDialog.tbWorkflowName.IsEnabled = false;
                            enterWorkflowDetailsDialog.tbWorkflowLongName.IsEnabled = false;
                        }

                        //Check for IsDraft
                        if (this._workflowVersion.IsDraft)
                        {
                            //This is a drafted version..
                            //Whatever changes are being done, must be committed to this version

                            //Pass the version details to EnterWorkflowDetails Dialog
                            enterWorkflowDetailsDialog.tbWorkflowVersionName.Text = this._workflowVersion.VersionName;
                            enterWorkflowDetailsDialog.tbComments.Text = this._workflowVersion.Comments;
                        }
                        else
                        {
                            //This is a published version
                            //Whatever changes are being done, must go as a new version
                            this._workflowVersion.Id = 0;
                        }
                    }

                    enterWorkflowDetailsDialog.Owner = Application.Current.MainWindow;
                    if (enterWorkflowDetailsDialog.ShowDialog() == true)
                    {
                        this.Cursor = Cursors.Wait;

                        if (this._workflowVersion == null)
                        {
                            //It is a creation of new workflow..
                            this._workflowVersion = new WorkflowVersion();
                        }

                        //Save workflow and workflow version along with Workflow Definition
                        MDM.BusinessObjects.Workflow.Workflow workflow = new BusinessObjects.Workflow.Workflow();
                        workflow.Id = this._workflowVersion.WorkflowId;
                        workflow.Name = enterWorkflowDetailsDialog.tbWorkflowName.Text;
                        workflow.LongName = enterWorkflowDetailsDialog.tbWorkflowLongName.Text;
                        workflow.Action = Core.ObjectAction.Create;

                        this._workflowVersion.WorkflowShortName = enterWorkflowDetailsDialog.tbWorkflowName.Text;
                        this._workflowVersion.WorkflowLongName = enterWorkflowDetailsDialog.tbWorkflowLongName.Text;
                        this._workflowVersion.VersionName = enterWorkflowDetailsDialog.tbWorkflowVersionName.Text;
                        this._workflowVersion.Comments = enterWorkflowDetailsDialog.tbComments.Text;
                        this._workflowVersion.IsDraft = true;
                        this._workflowVersion.Action = Core.ObjectAction.Create;
                        this._workflowVersion.WorkflowDefinition = this._workflowDesigner.Text;

                        //TO DO :: If user is not providing Tracking Profile, assign NULL so that the Base Tracking Config defined in the web.config will be fetched.
                        //Populate TrackingProfile
                        this._workflowVersion.TrackingProfile = @"<trackingProfile name=""MDMTrackingProfile"" >
                                                            <workflow>
                                                            <workflowInstanceQueries>
                                                                <workflowInstanceQuery>
                                                                <states>
                                                                    <state name=""*""/>
                                                                </states>
                                                                </workflowInstanceQuery>
                                                            </workflowInstanceQueries>
                                                            <activityStateQueries>
                                                                <activityStateQuery>
                                                                <states>
                                                                    <state name=""*""/>
                                                                </states>
                                                                <arguments>
                                                                    <argument name=""*""/>
                                                                </arguments>
                                                                <variables>
                                                                    <variable name=""*""/>
                                                                </variables>
                                                                </activityStateQuery>
                                                            </activityStateQueries>
                                                            <customTrackingQueries>
                                                                <customTrackingQuery name=""*"" activityName=""*""/>
                                                            </customTrackingQueries>
                                                            <faultPropagationQueries>
                                                                <faultPropagationQuery faultSourceActivityName=""*"" faultHandlerActivityName=""*""/>
                                                            </faultPropagationQueries>
                                                            </workflow>
                                                        </trackingProfile>";

                        workflow.WorkflowVersions.Add(this._workflowVersion);

                        Collection<MDM.BusinessObjects.Workflow.Workflow> workflowCollection = new Collection<BusinessObjects.Workflow.Workflow>();
                        workflowCollection.Add(workflow);

                        workflowService = new WorkflowWCFUtilities();

                        operationResult = new OperationResult();
                        CallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.MDMAdvanceWorkflow);

                        WorkflowVersion workflowVersion = workflowService.ProcessWorkflows(workflowCollection, App.LoginUser, ref operationResult,callerContext);

                        if (workflowVersion != null && workflowVersion.Id > 0)
                        {
                            this._workflowVersion.Id = workflowVersion.Id;
                            this._workflowVersion.WorkflowId = workflowVersion.WorkflowId;
                            this._workflowVersion.VersionNumber = workflowVersion.VersionNumber;
                        }

                        this.Cursor = Cursors.Arrow;

                        if (operationResult.HasError)
                        {
                            String error = operationResult.Errors[0].ErrorMessage;
                            throw new Exception(error);
                        }

						//Update Workflow Breadcrumb
                        String descriptionString = String.Format("Workflow Designer for: {0} - {1} - {2} (Draft Version)", _workflowVersion.WorkflowLongName, _workflowVersion.VersionName, _workflowVersion.VersionNumber);
                        lblDescription.Content = descriptionString;

                        MessageBox.Show("Workflow saved successfully.", "MDM.Workflow.Designer", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    this.Cursor = Cursors.Arrow;

                    String errorMessage = String.Format("The following error occurred while saving the workflow.\nError: {0}", ex.Message);
                    MessageBox.Show(errorMessage, "MDM.Workflow.Designer", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please Create/Open a Workflow.", "MDM.Workflow.Designer", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void btnPublish_Click(object sender, RoutedEventArgs e)
        {
            WorkflowWCFUtilities workflowService = null;
            OperationResult operationResult = null;

            if (this._workflowDesigner != null && !String.IsNullOrEmpty(this._workflowDesigner.Text))
            {
                if (!this.IsValid(this._workflowDesigner.Text))
                {
                    MessageBox.Show("Workflow has some errors. Please resolve them before publishing.", "MDM.Workflow.Designer", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                try
                {
                    EnterWorkflowDetails enterWorkflowDetailsDialog = new EnterWorkflowDetails();

                    //Check for Workflow Version
                    if (this._workflowVersion != null)
                    {
                        //Version is already been populated..
                        //It means we are opening the already created workflow

                        //Pass the workflow details to EnterWorkflowDetails Dialog
                        enterWorkflowDetailsDialog.tbWorkflowName.Text = this._workflowVersion.WorkflowShortName;
                        enterWorkflowDetailsDialog.tbWorkflowLongName.Text = this._workflowVersion.WorkflowLongName;

                        if (this._workflowVersion.WorkflowId > 0)
                        {
                            //Since the workflow already been published, do not allow user to edit workflow name and short name
                            enterWorkflowDetailsDialog.tbWorkflowName.IsEnabled = false;
                            enterWorkflowDetailsDialog.tbWorkflowLongName.IsEnabled = false;
                        }

                        //Check for IsDraft
                        if (this._workflowVersion.IsDraft)
                        {
                            //This is a draft version..
                            //Whatever changes are being done, must be committed to this version

                            //Pass the workflow details to EnterWorkflowDetails Dialog
                            enterWorkflowDetailsDialog.tbWorkflowVersionName.Text = this._workflowVersion.VersionName;
                            enterWorkflowDetailsDialog.tbComments.Text = this._workflowVersion.Comments;
                        }
                        else
                        {
                            //This is a published version
                            //Whatever changes are being done, must go as a new version
                            this._workflowVersion.Id = 0;
                        }
                    }

                    enterWorkflowDetailsDialog.Owner = Application.Current.MainWindow;
                    if (enterWorkflowDetailsDialog.ShowDialog() == true)
                    {
                        this.Cursor = Cursors.Wait;

                        if (this._workflowVersion == null)
                        {
                            //It is a creation of new workflow..
                            this._workflowVersion = new WorkflowVersion();
                        }

                        //Save workflow and workflow version along with Workflow Definition
                        MDM.BusinessObjects.Workflow.Workflow workflow = new BusinessObjects.Workflow.Workflow();
                        workflow.Id = this._workflowVersion.WorkflowId;
                        workflow.Name = enterWorkflowDetailsDialog.tbWorkflowName.Text;
                        workflow.LongName = enterWorkflowDetailsDialog.tbWorkflowLongName.Text;
                        workflow.Action = Core.ObjectAction.Create;

                        this._workflowVersion.WorkflowShortName = enterWorkflowDetailsDialog.tbWorkflowName.Text;
                        this._workflowVersion.WorkflowLongName = enterWorkflowDetailsDialog.tbWorkflowLongName.Text;
                        this._workflowVersion.VersionName = enterWorkflowDetailsDialog.tbWorkflowVersionName.Text;
                        this._workflowVersion.Comments = enterWorkflowDetailsDialog.tbComments.Text;
                        this._workflowVersion.IsDraft = false;
                        this._workflowVersion.Action = Core.ObjectAction.Create;
                        this._workflowVersion.WorkflowDefinition = this._workflowDesigner.Text;

                        //TO DO :: If user is not providing Tracking Profile, assign NULL so that the Base Tracking Config defined in the web.config will be fetched.
                        //Populate TrackingProfile
                        this._workflowVersion.TrackingProfile = @"<trackingProfile name=""MDMTrackingProfile"" >
                                                            <workflow>
                                                            <workflowInstanceQueries>
                                                                <workflowInstanceQuery>
                                                                <states>
                                                                    <state name=""*""/>
                                                                </states>
                                                                </workflowInstanceQuery>
                                                            </workflowInstanceQueries>
                                                            <activityStateQueries>
                                                                <activityStateQuery>
                                                                <states>
                                                                    <state name=""*""/>
                                                                </states>
                                                                <arguments>
                                                                    <argument name=""*""/>
                                                                </arguments>
                                                                <variables>
                                                                    <variable name=""*""/>
                                                                </variables>
                                                                </activityStateQuery>
                                                            </activityStateQueries>
                                                            <customTrackingQueries>
                                                                <customTrackingQuery name=""*"" activityName=""*""/>
                                                            </customTrackingQueries>
                                                            <faultPropagationQueries>
                                                                <faultPropagationQuery faultSourceActivityName=""*"" faultHandlerActivityName=""*""/>
                                                            </faultPropagationQueries>
                                                            </workflow>
                                                        </trackingProfile>";

                        workflow.WorkflowVersions.Add(this._workflowVersion);

                        Collection<MDM.BusinessObjects.Workflow.Workflow> workflowCollection = new Collection<BusinessObjects.Workflow.Workflow>();
                        workflowCollection.Add(workflow);

                        workflowService = new WorkflowWCFUtilities();

                        operationResult = new OperationResult();
                        CallerContext callerContext = new CallerContext(MDMCenterApplication.PIM, MDMCenterModules.MDMAdvanceWorkflow);

                        WorkflowVersion workflowVersion = workflowService.ProcessWorkflows(workflowCollection, App.LoginUser, ref operationResult,callerContext);

                        if (workflowVersion != null && workflowVersion.Id > 0)
                        {
                            this._workflowVersion.Id = workflowVersion.Id;
                            this._workflowVersion.WorkflowId = workflowVersion.WorkflowId;
                            this._workflowVersion.VersionNumber = workflowVersion.VersionNumber;

                            //Turn off GenerateNewGUIDForActivity flag.. 
                            //So that activity extraction will not create new GUIDs
                            WorkflowHelper.GenerateNewGUIDForActivity = false;

                            //Extract activities
                            ActivityExtractor activityExtractor = new ActivityExtractor(workflowVersion.Id);
                            Collection<WorkflowActivity> activityCollection = activityExtractor.GetActivities(this._workflowDesigner.Text);
                            workflowService.ProcessActivities(activityCollection, App.LoginUser, ref operationResult,callerContext);

                            //Turn on GenerateNewGUIDForActivity flag.. 
                            //So that user can continue to work on activity Drag Drop and Copy paste
                            WorkflowHelper.GenerateNewGUIDForActivity = true;
                        }

                        this.Cursor = Cursors.Arrow;

                        if (operationResult.HasError)
                        {
                            String error = operationResult.Errors[0].ErrorMessage;
                            throw new Exception(error);
                        }
                        else
                        {
                            //Update Workflow Breadcrumb
                            String descriptionString = String.Format("Workflow Designer for: {0} - {1} - {2} (Published Version)", _workflowVersion.WorkflowLongName, _workflowVersion.VersionName, _workflowVersion.VersionNumber);
                            lblDescription.Content = descriptionString;

                            MessageBox.Show("Workflow published successfully.", "MDM.Workflow.Designer", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.Cursor = Cursors.Arrow;

                    String errorMessage = String.Format("The following error occurred while publishing the workflow.\nError: {0}", ex.Message);
                    MessageBox.Show(errorMessage, "MDM.Workflow.Designer", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please Create/Open a Workflow.", "MDM.Workflow.Designer", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void dockManager_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                //if (this._docWindow != null)
                //{
                //    this._docWindow.Show();
                //}

                if (this._propertyWindow != null)
                {
                    this._propertyWindow.Hide();
                    this._propertyWindow.Show();
                }

                if (this._xamlWindow != null)
                {
                    this._xamlWindow.Hide();
                    this._xamlWindow.Show();
                }

                if (this._toolBoxWindow != null)
                {
                    this._toolBoxWindow.Hide();
                    this._toolBoxWindow.Show();
                }

                //since this task has to be done only once after loading, removing the event handler
                this.dockManager.MouseEnter -= new MouseEventHandler(dockManager_MouseEnter);
            }
            catch
            {
                //Bug # 26294
                //Ignore the exception..
            }
        }

        #endregion
    }
}
