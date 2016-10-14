using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Linq;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;

namespace QCPolicy
{
    public partial class QCPolicy : SequenceActivity
    {
        public QCPolicy()
        {
            InitializeComponent();
        }

        public static DependencyProperty ActivityDataProperty =
        DependencyProperty.Register("ActivityData", typeof(ActivityConfig),
            typeof(QCPolicy));

        [DescriptionAttribute("ActivityData")]
        [CategoryAttribute("Input Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public ActivityConfig ActivityData
        {
            get
            {
                return ((ActivityConfig)(base.GetValue(QCPolicy.ActivityDataProperty)));
            }
            set
            {
                base.SetValue(QCPolicy.ActivityDataProperty, value);
            }
        }

        public static DependencyProperty OperatorDataProperty =
            DependencyProperty.Register("OperatorData", typeof(OperatorConfig),
                typeof(QCPolicy));

        [DescriptionAttribute("OperatorData")]
        [CategoryAttribute("Input Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public OperatorConfig OperatorData
        {
            get
            {
                return ((OperatorConfig)(base.GetValue(QCPolicy.OperatorDataProperty)));
            }
            set
            {
                base.SetValue(QCPolicy.OperatorDataProperty, value);
            }
        }

        public static DependencyProperty CustomerDataProperty =
            DependencyProperty.Register("CustomerData", typeof(CustomerConfig),
                typeof(QCPolicy));

        [DescriptionAttribute("CustomerData")]
        [CategoryAttribute("Input Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public CustomerConfig CustomerData
        {
            get
            {
                return ((CustomerConfig)(base.GetValue(QCPolicy.CustomerDataProperty)));
            }
            set
            {
                base.SetValue(QCPolicy.CustomerDataProperty, value);
            }
        }

        public static DependencyProperty TransactionDataProperty =
            DependencyProperty.Register("TransactionData", typeof(TransactionConfig),
                typeof(QCPolicy));

        [DescriptionAttribute("TransactionData")]
        [CategoryAttribute("Input Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public TransactionConfig TransactionData
        {
            get
            {
                return ((TransactionConfig)
                    (base.GetValue(QCPolicy.TransactionDataProperty)));
            }
            set
            {
                base.SetValue(QCPolicy.TransactionDataProperty, value);
            }
        }

        public static DependencyProperty ReviewProperty =
            DependencyProperty.Register("Review", typeof(bool), typeof(QCPolicy));

        [DescriptionAttribute("Review")]
        [CategoryAttribute("Output Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool Review
        {
            get
            {
                return ((bool)(base.GetValue(QCPolicy.ReviewProperty)));
            }
            set
            {
                base.SetValue(QCPolicy.ReviewProperty, value);
            }
        }

        public static DependencyProperty PriorityProperty =
            DependencyProperty.Register("Priority", typeof(string), typeof(QCPolicy));

        [DescriptionAttribute("Priority")]
        [CategoryAttribute("Output Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string Priority
        {
            get
            {
                return ((string)(base.GetValue(QCPolicy.PriorityProperty)));
            }
            set
            {
                base.SetValue(QCPolicy.PriorityProperty, value);
            }
        }
    }
}
