using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace RS.MDM.Diagnostics
{
    /// <summary>
    /// Provides functionality to trap the trace messages and displays in an output windows (richtextbox control)
    /// </summary>
    public class OutputListener : TraceListener
    {

        #region Fields

        /// <summary>
        /// field for the information window
        /// </summary>
        private RichTextBox _infoWindow;

        /// <summary>
        /// field for the eror window
        /// </summary>
        private RichTextBox _errorWindow;

        #endregion

        #region Delegates

        /// <summary>
        /// Delegate for setting the text in the richtextbox
        /// </summary>
        /// <param name="text">The text that needs to be set in the richtextbox</param>
        public delegate void SetTextCallback(string text);

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor with single RichTextBox as input parameter
        /// </summary>
        /// <param name="outputWindow">Indicates a RichTextBox that will be used as output window</param>
        public OutputListener(RichTextBox outputWindow)
        {
            this._infoWindow = outputWindow;
            this._errorWindow = outputWindow;
        }

        /// <summary>
        /// Constructor with two RichTextBox as input parameter
        /// </summary>
        /// <param name="infoWindow">Indicates a RichTextBox that will be used as infor window</param>
        /// <param name="errorWindow">Indicates a RichTextBox that will be used as error window</param>
        public OutputListener(RichTextBox infoWindow, RichTextBox errorWindow)
        {
            this._infoWindow = infoWindow;
            this._errorWindow = errorWindow;
        }

        #endregion

        #region Delegates

        /// <summary>
        /// Sets a text in the Info RichTextBox control
        /// </summary>
        /// <param name="text">Indicates the text that needs to be set in the RichTextBox control</param>
        private void SetInfoText(string text)
        {
            this.SetText(this._infoWindow, text);
        }

        /// <summary>
        /// Sets a text in the Error RichTextBox control
        /// </summary>
        /// <param name="text">Indicates the text that needs to be set in the RichTextBox control</param>
        private void SetErrorText(string text)
        {
            this.SetText(this._errorWindow, text);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="outputWindow"></param>
        /// <param name="text"></param>
        private void SetText(RichTextBox outputWindow, string text)
        {
            try
            {
                outputWindow.SuspendLayout();
                if (outputWindow.Text.Length > 10000)
                {
                    outputWindow.Text = text;
                }
                else
                {
                    outputWindow.AppendText(text);
                }
                outputWindow.SelectionStart = outputWindow.Text.Length;
                outputWindow.ScrollToCaret();
                outputWindow.ResumeLayout();
            }
            catch
            {
            }
        }

        /// <summary>
        /// Writes a message to output window
        /// </summary>
        /// <param name="message">Indicates the message that needs to be written to the output window</param>
        public override void Write(string message)
        {
            this.WriteLine(message, string.Empty);
        }

        /// <summary>
        /// Writes a line of message to output window
        /// </summary>
        /// <param name="message">Indicates the message that needs to be written to the output window</param>
        /// <param name="category">Indicates the category of the message</param>
        public override void Write(string message, string category)
        {
            this.WriteLine(message, category);
        }

        /// <summary>
        /// Writes a line of message to output window
        /// </summary>
        /// <param name="message">Indicates the message that needs to be written to the output window</param>
        public override void WriteLine(string message)
        {
            this.WriteLine(message, string.Empty);
        }

        /// <summary>
        /// Writes a line of message to output window
        /// </summary>
        /// <param name="message">Indicates the message that needs to be written to the output window</param>
        /// <param name="category">Indicates the category of the message</param>
        public override void WriteLine(string message, string category)
        {
            if (!string.IsNullOrEmpty(message))
            {
                try
                {
                    if (message.IndexOf("\r\n") != 1)
                    {
                        message = "\r\n" + message;
                    }
                    if (string.IsNullOrEmpty(category) || category != "ERROR")
                    {
                        if (this._infoWindow.IsHandleCreated)
                        {
                            this._infoWindow.Invoke(new SetTextCallback(this.SetInfoText), new object[] { message });
                        }
                        else
                        {
                            this.SetText(this._infoWindow, message);
                        }
                    }
                    else
                    {
                        if (this._errorWindow.IsHandleCreated)
                        {
                            this._errorWindow.Invoke(new SetTextCallback(this.SetErrorText), new object[] { message });
                        }
                        else
                        {
                            this.SetText(this._errorWindow, message);
                        }
                    }
                }
                catch
                {
                }
            }
        }

        #endregion

    }
}
