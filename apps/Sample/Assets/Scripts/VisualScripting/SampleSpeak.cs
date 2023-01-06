using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace AzureEmbodiedAISamples
{
    [UnitTitle("Sample Speak")]
    [UnitCategory("AzureEmbodiedAISamples")]
    public class SampleSpeak : Unit
    {
        [DoNotSerialize]
        public ControlInput inputTrigger;

        [DoNotSerialize]
        public ControlOutput outputTrigger;

        [DoNotSerialize]
        public ValueInput inputText;

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
            inputTrigger = ControlInputCoroutine("inputTrigger", flow => outputTrigger, SpeakAsync);
            outputTrigger = ControlOutput("outputTrigger");
            inputText = ValueInput<string>("inputText", string.Empty);
        }

        public IEnumerator SpeakAsync(Flow flow)
        {
            var result = Manager.SpeakAsyncUnity(flow.GetValue(inputText).ToString());
            yield return new WaitUntil(() => result.IsCompleted);
            yield return outputTrigger;
        }
    }
}