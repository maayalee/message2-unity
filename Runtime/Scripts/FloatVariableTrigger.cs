using System;
using System.Collections.Generic;
using System.ComponentModel;
using PlayGem.JawRed.Core.Variables;
using Subsets.Message2;
using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace
{
    public class FloatVariableTrigger : MonoBehaviour 
    {
        public FloatVariable Variable;
        public float CompareEpsilon = 0.01f;
        public ResponseConditionOperator ConditionOperator;
        [NonReorderable]
        public List<FloatCondition> Conditions;
        public UnityEvent<FloatVariable> Listeners;
        
        private void Awake()
        {
            Variable.PropertyChanged += delegate(object sender, PropertyChangedEventArgs args)
            {
                ConditionCompareResult result = new ConditionCompareResult();
                foreach (FloatCondition condition in Conditions)
                {
                    if (condition.Compare == FloatCompare.Equal)
                    {
                        result.Add(IsEqual(Variable.Value, condition.Value));
                    }
                    else if (condition.Compare == FloatCompare.IsNot)
                    {
                        result.Add(!IsEqual(Variable.Value, condition.Value));
                    }
                }
                
                if (result.CheckConditionOperator(ConditionOperator))
                {
                    Listeners?.Invoke(Variable);
                }
            };
        }
        
        private bool IsEqual(float a, float b)
        {
            return Math.Abs(a - b) < CompareEpsilon;
        }
    }
}