using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace AzureEmbodiedAISamples
{
    [UnitTitle("Sample Listen")]
    [UnitCategory("AzureEmbodiedAISamples")]
    public class SampleListen : Unit
    {
        [DoNotSerialize]
        public ControlInput inputTrigger;

        [DoNotSerialize]
        public ControlOutput outputTrigger;

        [DoNotSerialize]
        public ControlOutput emptyTrigger;

        [DoNotSerialize]
        public ValueOutput outputText;

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
            inputTrigger = ControlInputCoroutine("inputTrigger", flow => outputTrigger, ListenAsync);
            outputTrigger = ControlOutput("outputTrigger");
            emptyTrigger = ControlOutput("emptyTrigger");
            outputText = ValueOutput<string>("outputText");
        }

        public IEnumerator ListenAsync(Flow flow)
        {
            var result = Manager.ListenAsync();
            yield return new WaitUntil(() => result.IsCompleted);
            flow.SetValue(outputText, result.Result);
            yield return result.Result != string.Empty ? outputTrigger : emptyTrigger;
        }
    }
}