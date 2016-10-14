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
        private ConfigDataContext _dc = null;
        private SubQueue _queue = null;
        private OperatorConfig _operator = null;

        public QCPolicy()
        {
            InitializeComponent();
        }


        public static DependencyProperty ConnectionStringProperty =
        DependencyProperty.Register("ConnectionString", typeof(string), typeof(QCPolicy));

        [DescriptionAttribute("ConnectionString")]
        [CategoryAttribute("Input Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string ConnectionString
        {
            get
            {
                return ((string)(base.GetValue(QCPolicy.ConnectionStringProperty)));
            }
            set
            {
                base.SetValue(QCPolicy.ConnectionStringProperty, value);
            }
        }

        public static DependencyProperty QueueNameProperty =
            DependencyProperty.Register("QueueName", typeof(string), typeof(QCPolicy));

        [DescriptionAttribute("QueueName")]
        [CategoryAttribute("Input Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string QueueName
        {
            get
            {
                return ((string)(base.GetValue(QCPolicy.QueueNameProperty)));
            }
            set
            {
                base.SetValue(QCPolicy.QueueNameProperty, value);
            }
        }

        public static DependencyProperty SubQueueNameProperty =
            DependencyProperty.Register("SubQueueName", typeof(string), typeof(QCPolicy));

        [DescriptionAttribute("SubQueueName")]
        [CategoryAttribute("Input Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string SubQueueName
        {
            get
            {
                return ((string)(base.GetValue(QCPolicy.SubQueueNameProperty)));
            }
            set
            {
                base.SetValue(QCPolicy.SubQueueNameProperty, value);
            }
        }

        public static DependencyProperty OperatorKeyProperty =
            DependencyProperty.Register("OperatorKey", typeof(Guid), typeof(QCPolicy));

        [DescriptionAttribute("OperatorKey")]
        [CategoryAttribute("Input Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public Guid OperatorKey
        {
            get
            {
                return ((Guid)(base.GetValue(QCPolicy.OperatorKeyProperty)));
            }
            set
            {
                base.SetValue(QCPolicy.OperatorKeyProperty, value);
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
            DependencyProperty.Register("Priority", typeof(int), typeof(QCPolicy));

        [DescriptionAttribute("Priority")]
        [CategoryAttribute("Output Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public int Priority
        {
            get
            {
                return ((int)(base.GetValue(QCPolicy.PriorityProperty)));
            }
            set
            {
                base.SetValue(QCPolicy.PriorityProperty, value);
            }
        }

        private void ReadConfig_ExecuteCode(object sender, EventArgs e)
        {
            if (_dc == null)
                _dc = new ConfigDataContext(this.ConnectionString);

            _dc.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, _dc.SubQueues);
            _dc.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, _dc.OperatorConfigs);

            _queue = _dc.SubQueues.SingleOrDefault
                (x => x.SubQueueName == this.SubQueueName &&
                      x.Queue.QueueName == this.QueueName);

            _operator = _dc.OperatorConfigs.SingleOrDefault
                (x => x.OperatorKey == this.OperatorKey);

            if (!this.Review)
            {
                if (_queue != null)
                    _queue.NumberSinceLastEval++;

                if (_operator != null)
                    _operator.NumberSinceLastEval++;
            }
        }

        private void UpdateConfig_ExecuteCode(object sender, EventArgs e)
        {
            if (this.Review)
            {
                if (_queue != null)
                    _queue.NumberSinceLastEval = 0;

                if (_operator != null)
                    _operator.NumberSinceLastEval = 0;
            }

            _dc.SubmitChanges();
        }

        private void Hardcode_ExecuteCode(object sender, EventArgs e)
        {
            this.Review = true;
            this.Priority = 5;
        }
    }
}
