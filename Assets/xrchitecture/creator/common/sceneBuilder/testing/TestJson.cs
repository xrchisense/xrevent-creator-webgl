using UnityEngine;

namespace Xrchitecture.Creator.Common.Data
{
    internal class TestJson : MonoBehaviour
    {
        public void LoadTestRoom()
        {
            CreatorNetworkUtility.LoadEventFromAddress(TestConfigHelper.ProjectDataFolder + TestConfigHelper.ProjectId + "/ExampleEvent.json",TestConfigHelper.ProjectId);
        }


        public void SaveTestRoom()
        {
            CreatorNetworkUtility.SaveCurrentEventToAddress(TestConfigHelper.ProjectDataFolder + TestConfigHelper.ProjectId + "/ExampleEvent.json");
        }
    }
}
