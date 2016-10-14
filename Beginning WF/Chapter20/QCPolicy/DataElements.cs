using System;
using System.Collections.Generic;

namespace QCPolicy
{
    /*****************************************************/
    // Configuration options for a specific activity
    /*****************************************************/
    public class ActivityConfig
    {
        public decimal MinimumAmount { get; set; }
        public decimal ThresholdAmount { get; set; }
        public int Frequency { get; set; }
        public int NumberSinceLastEval { get; set; }

        public ActivityConfig(decimal min, decimal max, int freq)
        {
            this.MinimumAmount = min;
            this.ThresholdAmount = max;
            this.Frequency = freq;
            this.NumberSinceLastEval = 0;
        }

        public int IncrementEvalCount()
        {
            this.NumberSinceLastEval++;
            return this.NumberSinceLastEval;
        }

        public void ResetEval()
        {
            this.NumberSinceLastEval = 0;
        }
    }

    /*****************************************************/
    // Details about the operator
    /*****************************************************/
    public class OperatorConfig
    {
        public bool UnderEvaluation { get; set; }
        public int Frequency { get; set; }
        public int NumberSinceLastEval { get; set; }

        public OperatorConfig(bool eval, int freq)
        {
            this.UnderEvaluation = eval;
            this.Frequency = freq;
            this.NumberSinceLastEval = 0;
        }

        public int IncrementEvalCount()
        {
            this.NumberSinceLastEval++;
            return this.NumberSinceLastEval;
        }

        public void ResetEval()
        {
            this.NumberSinceLastEval = 0;
        }
    }

    /*****************************************************/
    // Properties of the customer needed fot processing
    /*****************************************************/
    public class CustomerConfig
    {
        public string Category { get; set; }

        public CustomerConfig(string cat)
        {
            this.Category = cat;
        }
    }

    /*****************************************************/
    // Details needed to process a transaction
    /*****************************************************/
    public class TransactionConfig
    {
        public decimal Amount { get; set; }

        public TransactionConfig(decimal amt)
        {
            this.Amount = amt;
        }
    }

    /*****************************************************/
    // Contains a list of transaction to be processed
    /*****************************************************/
    public class TransactionList
    {
        public List<TransactionConfig> List { get; set; }

        public TransactionList()
        {
            this.List = new List<TransactionConfig>();
        }
    }
}
