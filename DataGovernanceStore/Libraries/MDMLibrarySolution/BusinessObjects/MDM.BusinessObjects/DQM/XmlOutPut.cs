using System.Collections.Generic;
using System.Xml;

namespace MDM.BusinessObjects.DQM
{
    /// <summary>
    /// This class is obtained from Mark S. Rasmussen
    /// http://improve.dk/xmldocument-fluent-interface/
    /// </summary>
    public class XmlOutput
    {
        // The internal XmlDocument that holds the complete structure.
        readonly XmlDocument _xmlDocument = new XmlDocument();

        // A stack representing the hierarchy of nodes added. nodeStack.Peek() will always be the current node scope.
        readonly Stack<XmlNode> _nodeStack;

        // Whether the next node should be created in the scope of the current node.
        bool _nextNodeWithin;

        // The current node. If null, the current node is the XmlDocument itself.
        XmlNode _currentNode;

        /// <summary>
        /// Retuns the xml node in the form of stack object
        /// </summary>
        public XmlOutput()
        {
            _nodeStack = new Stack<XmlNode>();
        }

        /// <summary>
        /// Returns the string representation of the XmlDocument.
        /// </summary>
        /// <returns>A string representation of the XmlDocument.</returns>
        public string GetOuterXml()
        {
            return _xmlDocument.OuterXml;
        }

        /// <summary>
        /// Returns the XmlDocument
        /// </summary>
        /// <returns></returns>
        public XmlDocument GetXmlDocument()
        {
            return _xmlDocument;
        }

        /// <summary>
        /// Changes the scope to the current node.
        /// </summary>
        /// <returns>this</returns>
        public XmlOutput Within()
        {
            _nextNodeWithin = true;

            return this;
        }

        /// <summary>
        /// Changes the scope to the parent node.
        /// </summary>
        /// <returns>this</returns>
        public XmlOutput EndWithin()
        {
            if (_nextNodeWithin)
                _nextNodeWithin = false;
            else
                _nodeStack.Pop();

            return this;
        }

        /// <summary>
        /// Adds an XML declaration with the most common values.
        /// </summary>
        /// <returns>this</returns>
        public XmlOutput XmlDeclaration()
        {
            return XmlDeclaration("1.0", "utf-8", "");
        }

        /// <summary>
        /// Adds an XML declaration to the document.
        /// </summary>
        /// <param name="version">The version of the XML document.</param>
        /// <param name="encoding">The encoding of the XML document.</param>
        /// <param name="standalone">Whether the document is standalone or not. Can be yes/no/(null || "").</param>
        /// <returns>this</returns>
        public XmlOutput XmlDeclaration(string version, string encoding, string standalone)
        {
            XmlDeclaration xdec = _xmlDocument.CreateXmlDeclaration(version, encoding, standalone);
            _xmlDocument.AppendChild(xdec);

            return this;
        }

        /// <summary>
        /// Creates a node. If no nodes have been added before, it'll be the root node, otherwise it'll be appended as a child of the current node.
        /// </summary>
        /// <param name="name">The name of the node to create.</param>
        /// <returns>this</returns>
        public XmlOutput Node(string name)
        {
            XmlNode xn = _xmlDocument.CreateElement(name);

            // If nodeStack.Count == 0, no nodes have been added, thus the scope is the XmlDocument itself.
            if (_nodeStack.Count == 0)
            {
                _xmlDocument.AppendChild(xn);

                // Automatically change scope to the root DocumentElement.
                _nodeStack.Push(xn);
            }
            else
            {
                // If this node should be created within the scope of the current node, change scope to the current node before adding the node to the scope element.
                if (_nextNodeWithin)
                {
                    _nodeStack.Push(_currentNode);

                    _nextNodeWithin = false;
                }

                _nodeStack.Peek().AppendChild(xn);
            }

            _currentNode = xn;

            return this;
        }

        /// <summary>
        /// Sets the InnerText of the current node without using CData.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public XmlOutput InnerText(string text)
        {
            return InnerText(text, false);
        }

        /// <summary>
        /// Sets the InnerText of the current node.
        /// </summary>
        /// <param name="text">The text to set.</param>
        /// <param name="useCData"></param>
        /// <returns>this</returns>
        public XmlOutput InnerText(string text, bool useCData)
        {
            if (useCData)
                _currentNode.AppendChild(_xmlDocument.CreateCDataSection(text));
            else
                _currentNode.AppendChild(_xmlDocument.CreateTextNode(text));

            return this;
        }

        /// <summary>
        /// Adds an attribute to the current node.
        /// </summary>
        /// <param name="name">The name of the attribute.</param>
        /// <param name="value">The value of the attribute.</param>
        /// <returns>this</returns>
        public XmlOutput Attribute(string name, string value)
        {
            XmlAttribute xa = _xmlDocument.CreateAttribute(name);
            xa.Value = value;

            if (_currentNode.Attributes != null) 
                _currentNode.Attributes.Append(xa);

            return this;
        }
    }
}
