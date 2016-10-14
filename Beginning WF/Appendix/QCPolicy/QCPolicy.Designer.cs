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

namespace QCPolicy
{
    public partial class QCPolicy
    {
        #region Activity Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCode]
        private void InitializeComponent()
        {
            this.CanModifyActivities = true;
            System.Workflow.Activities.Rules.RuleSetReference rulesetreference1 = new System.Workflow.Activities.Rules.RuleSetReference();
            System.Workflow.Activities.Rules.RuleConditionReference ruleconditionreference1 = new System.Workflow.Activities.Rules.RuleConditionReference();
            System.Workflow.Activities.Rules.RuleSetReference rulesetreference2 = new System.Workflow.Activities.Rules.RuleSetReference();
            this.PriorityPolicy = new System.Workflow.Activities.PolicyActivity();
            this.ifElseBranchActivity2 = new System.Workflow.Activities.IfElseBranchActivity();
            this.ifReview = new System.Workflow.Activities.IfElseBranchActivity();
            this.UpdateConfig = new System.Workflow.Activities.CodeActivity();
            this.Hardcode = new System.Workflow.Activities.CodeActivity();
            this.ifElseActivity1 = new System.Workflow.Activities.IfElseActivity();
            this.ReviewPolicy = new System.Workflow.Activities.PolicyActivity();
            this.ReadConfig = new System.Workflow.Activities.CodeActivity();
            // 
            // PriorityPolicy
            // 
            this.PriorityPolicy.Name = "PriorityPolicy";
            rulesetreference1.RuleSetName = "Priority Policy";
            this.PriorityPolicy.RuleSetReference = rulesetreference1;
            // 
            // ifElseBranchActivity2
            // 
            this.ifElseBranchActivity2.Name = "ifElseBranchActivity2";
            // 
            // ifReview
            // 
            this.ifReview.Activities.Add(this.PriorityPolicy);
            ruleconditionreference1.ConditionName = "Review";
            this.ifReview.Condition = ruleconditionreference1;
            this.ifReview.Name = "ifReview";
            // 
            // UpdateConfig
            // 
            this.UpdateConfig.Name = "UpdateConfig";
            this.UpdateConfig.ExecuteCode += new System.EventHandler(this.UpdateConfig_ExecuteCode);
            // 
            // Hardcode
            // 
            this.Hardcode.Enabled = false;
            this.Hardcode.Name = "Hardcode";
            this.Hardcode.ExecuteCode += new System.EventHandler(this.Hardcode_ExecuteCode);
            // 
            // ifElseActivity1
            // 
            this.ifElseActivity1.Activities.Add(this.ifReview);
            this.ifElseActivity1.Activities.Add(this.ifElseBranchActivity2);
            this.ifElseActivity1.Name = "ifElseActivity1";
            // 
            // ReviewPolicy
            // 
            this.ReviewPolicy.Name = "ReviewPolicy";
            rulesetreference2.RuleSetName = "Review Policy";
            this.ReviewPolicy.RuleSetReference = rulesetreference2;
            // 
            // ReadConfig
            // 
            this.ReadConfig.Name = "ReadConfig";
            this.ReadConfig.ExecuteCode += new System.EventHandler(this.ReadConfig_ExecuteCode);
            // 
            // QCPolicy
            // 
            this.Activities.Add(this.ReadConfig);
            this.Activities.Add(this.ReviewPolicy);
            this.Activities.Add(this.ifElseActivity1);
            this.Activities.Add(this.Hardcode);
            this.Activities.Add(this.UpdateConfig);
            this.Name = "QCPolicy";
            this.CanModifyActivities = false;

        }

        #endregion

        private CodeActivity Hardcode;
        private CodeActivity UpdateConfig;
        private CodeActivity ReadConfig;
        private PolicyActivity PriorityPolicy;
        private IfElseBranchActivity ifElseBranchActivity2;
        private IfElseBranchActivity ifReview;
        private IfElseActivity ifElseActivity1;
        private PolicyActivity ReviewPolicy;
















    }
}
