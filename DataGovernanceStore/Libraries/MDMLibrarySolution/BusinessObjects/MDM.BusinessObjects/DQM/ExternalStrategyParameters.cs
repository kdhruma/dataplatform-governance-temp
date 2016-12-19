using System;
using System.Runtime.Serialization;
using ProtoBuf;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Interfaces;

    /// <summary>
    /// Specify parameters for external merging process
    /// </summary>
    [ProtoContract]
    [DataContract]
    public class ExternalStrategyParameters : IExternalStrategyParameters
    {
        #region Fields

        /// <summary>
        /// Name of external strategy
        /// </summary>
        private string _externalStrategyName;

        /// <summary>
        /// Source attribute
        /// </summary>
        private Attribute _sourceAttribute;

        /// <summary>
        /// Source Entity for mergin process
        /// </summary>
        private Entity _sourceEntity;

        /// <summary>
        /// Target Entity for mergin process
        /// </summary>
        private Entity _targetEntity;

        /// <summary>
        /// The Target attribute
        /// </summary>
        private Attribute _targetAttribute;

        #endregion Fields

        /// <summary>
        /// Default constructor
        /// </summary>
        public ExternalStrategyParameters() { }

        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="externalStrategyName">Name of external strategy</param>
        /// <param name="sourceAttribute">Source attribute</param>
        /// <param name="targetAttribute">Target attribute</param>
        /// <param name="sourceEntity">Source Entity</param>
        /// <param name="targetEntity">Target Entity</param>
        public ExternalStrategyParameters(String externalStrategyName, Attribute sourceAttribute, Attribute targetAttribute, Entity sourceEntity, Entity targetEntity)
        {
            this.ExternalStrategyName = externalStrategyName;
            this.SourceAttribute = sourceAttribute;
            this.TargetAttribute = targetAttribute;
            this.SourceEntity = sourceEntity;
            this.TargetEntity = targetEntity;
        }


        /// <summary>
        /// Name of external strategy
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
        public String ExternalStrategyName
        {
            get { return _externalStrategyName; }
            set { _externalStrategyName = value; }
        }

        /// <summary>
        /// Source attribute
        /// </summary>
        [DataMember]
        [ProtoMember(2)]
        public Attribute SourceAttribute
        {
            get { return _sourceAttribute; }
            set { _sourceAttribute = value; }
        }

        /// <summary>
        /// Source Entity for mergin process
        /// </summary>
        [DataMember]
        [ProtoMember(3)]
        public Entity SourceEntity
        {
            get { return _sourceEntity; }
            set { _sourceEntity = value; }
        }

        /// <summary>
        /// Target Entity for mergin process
        /// </summary>
        [DataMember]
        [ProtoMember(4)]
        public Entity TargetEntity
        {
            get { return _targetEntity; }
            set { _targetEntity = value; }
        }

        /// <summary>
        /// The Target attribute
        /// </summary>
        [DataMember]
        [ProtoMember(5)]
        public Attribute TargetAttribute
        {
            get { return _targetAttribute; }
            set { _targetAttribute = value; }
        }
    }
}
