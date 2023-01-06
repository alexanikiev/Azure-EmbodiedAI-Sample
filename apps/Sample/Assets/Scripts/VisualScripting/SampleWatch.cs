using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace AzureEmbodiedAISamples
{
    [UnitTitle("Sample Watch")]
    [UnitCategory("AzureEmbodiedAISamples")]
    public class SampleWatch : Unit
    {
        [DoNotSerialize]
        public ControlInput inputTrigger;

        [DoNotSerialize]
        public ControlOutput outputTrigger;

        [DoNotSerialize]
        public ValueInput timeOut;

        [DoNotSerialize]
        public ValueOutput outputFlag;

        private SampleManager _manager;
        private SampleManager Manager
        {
            get
            {
                if (!_manager)
                    _manager = (SampleManager)Variables.ActiveScene["SampleManager"];
                return _manager;
            }
        }

        protected override void Definition()
        {
            inputTrigger = ControlInputCoroutine("inputTrigger", flow => outputTrigger, WatchAsync);
            outputTrigger = ControlOutput("outputTrigger");
            timeOut = ValueInput<float>("timeOut", 0);
            outputFlag = ValueOutput<bool>("outputFlag");
        }

        public IEnumerator WatchAsync(Flow flow)
        {
            var result = Manager.WatchAsync((float)flow.GetValue(timeOut));
            yield return new WaitUntil(() => result.IsCompleted);
            flow.SetValue(outputFlag, result.Result);
            yield return outputTrigger;
        }
    }
}