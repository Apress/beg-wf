using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Reflection;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;

namespace TestQC
{
    partial class Workflow1
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
            this.qcPolicy1 = new QCPolicy.QCPolicy();
            // 
            // qcPolicy1
            // 
            this.qcPolicy1.QueueName = "ProcessRequest";
            this.qcPolicy1.ConnectionString = "Data Source=localhost;Initial Catalog=Appendix;Integrated Security=True";
            this.qcPolicy1.Name = "qcPolicy1";
            this.qcPolicy1.OperatorKey = new System.Guid("cef8af08-748f-4ff0-9229-0a16537c3be1");
            this.qcPolicy1.Priority = 0;
            this.qcPolicy1.SubQueueName = "Service";
            this.qcPolicy1.Review = false;
            // 
            // Workflow1
            // 
            this.Activities.Add(this.qcPolicy1);
            this.Name = "Workflow1";
            this.CanModifyActivities = false;

        }

        #endregion

        private QCPolicy.QCPolicy qcPolicy1;

    }
}
