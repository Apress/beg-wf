using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;

namespace Workflow35
{
    public partial class CustomActivity : SequenceActivity
    {
        public CustomActivity()
        {
            InitializeComponent();
        }

        public static DependencyProperty MessageProperty =
    DependencyProperty.Register("Message", typeof(string), typeof(CustomActivity));

        [DescriptionAttribute("Message")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string Message
        {
            get
            {
                return ((string)(base.GetValue(MessageProperty)));
            }
            set
            {
                base.SetValue(MessageProperty, value);
            }
        }

        private void codeActivity1_ExecuteCode(object sender, EventArgs e)
        {
            Console.WriteLine(this.Message);
        }
    }
}
