using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Reflection;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;

namespace Workflow35
{
    public partial class CustomActivity
    {
        #region Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCode]
        private void InitializeComponent()
        {
            this.CanModifyActivities = true;
            this.throwActivity1 = new System.Workflow.ComponentModel.ThrowActivity();
            this.codeActivity1 = new System.Workflow.Activities.CodeActivity();
            // 
            // throwActivity1
            // 
            this.throwActivity1.FaultType = typeof(System.InvalidProgramException);
            this.throwActivity1.Name = "throwActivity1";
            // 
            // codeActivity1
            // 
            this.codeActivity1.Name = "codeActivity1";
            this.codeActivity1.ExecuteCode += new System.EventHandler(this.codeActivity1_ExecuteCode);
            // 
            // CustomActivity
            // 
            this.Activities.Add(this.codeActivity1);
            this.Activities.Add(this.throwActivity1);
            this.Name = "CustomActivity";
            this.CanModifyActivities = false;

        }

        #endregion

        private ThrowActivity throwActivity1;
        private CodeActivity codeActivity1;

    }
}
