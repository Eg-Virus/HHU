using System.Diagnostics;
using Windows.ApplicationModel.Background;
using Windows.Devices.Enumeration;
using Windows.Storage;

namespace AirObservationSystem.HHU.UWP.Tasks
{
    public sealed class BackgroundDeviceWatcher : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            Debug.WriteLine("BackgroundDeviceWatcher.Run called");

            uint eventCount = 0;
            ApplicationDataContainer settings = ApplicationData.Current.LocalSettings;
            DeviceWatcherTriggerDetails triggerDetails = (DeviceWatcherTriggerDetails)taskInstance.TriggerDetails;

            Debug.WriteLine("Trigger contains {0} events", triggerDetails.DeviceWatcherEvents.Count);

            foreach (DeviceWatcherEvent e in triggerDetails.DeviceWatcherEvents)
            {
                switch (e.Kind)
                {
                    case DeviceWatcherEventKind.Add:
                        Debug.WriteLine("Add: " + e.DeviceInformation.Id);
                        break;

                    case DeviceWatcherEventKind.Update:
                        Debug.WriteLine("Update: " + e.DeviceInformationUpdate.Id);
                        break;

                    case DeviceWatcherEventKind.Remove:
                        Debug.WriteLine("Remove: " + e.DeviceInformationUpdate.Id);
                        break;
                }
            }

            if (settings.Values["eventCount"] != null)
            {
                if (settings.Values["eventCount"] is uint)
                {
                    eventCount = (uint)settings.Values["eventCount"];
                }
            }

            // Add the number of events for this trigger to the number of events received in the past
            eventCount += (uint)triggerDetails.DeviceWatcherEvents.Count;

            Debug.WriteLine(eventCount + " events processed for lifetime of trigger");

            settings.Values["eventCount"] = eventCount;
        }
    }
}
