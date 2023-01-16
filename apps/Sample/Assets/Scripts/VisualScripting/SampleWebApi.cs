using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace AzureEmbodiedAISamples
{
    [UnitTitle("Sample WebApi")]
    [UnitCategory("AzureEmbodiedAISamples")]
    public class SampleWebApi : Unit
    {
        [DoNotSerialize]
        public ControlInput inputTrigger;

        [DoNotSerialize]
        public ControlOutput outputTrigger;

        [DoNotSerialize]
        public ValueInput webApiType;

        [DoNotSerialize]
        public ValueInput inputContent;

        [DoNotSerialize]
        public ValueOutput outputContent;

        private SampleManager _manager;
        private SampleManager Manager
        {
            get
            {
                if (!_manager)
                {
                    _manager = (SampleManager)Variables.ActiveScene["SampleManager"];
                }

                return _manager;
            }
        }

        protected override void Definition()
        {
            inputTrigger = ControlInputCoroutine("inputTrigger", flow => outputTrigger, WebApiAsync);
            outputTrigger = ControlOutput("outputTrigger");
            webApiType = ValueInput<SampleWebApiType>("webApiType", SampleWebApiType.None);
            inputContent = ValueInput<string>("inputContent", string.Empty);
            outputContent = ValueOutput<string>("outputContent");
        }

        public IEnumerator WebApiAsync(Flow flow)
        {
            var result = Manager.WebApiAsync((SampleWebApiType)flow.GetValue(webApiType), flow.GetValue(inputContent).ToString());
            yield return new WaitUntil(() => result.IsCompleted);
            flow.SetValue(outputContent, result.Result);
            yield return outputTrigger;
        }
    }
}