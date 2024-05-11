namespace BookingApp.Service
{
    public class HangfireSetting
    {
        public string SchedulerSync { get; set; }
        public bool EnableSync { get; set; }
        public string TimeZone { get; set; }
        public string SchedulerPushNotify { get; set; }
        public bool EnablePushNotify { get; set; }
        public string SchedulerDeletePushNotify { get; set; }
        public int DateToDeletePushNotify { get; set; }
        public bool EnableDeletePushNotify { get; set; }
        public string PythonPath { get; set; }
    }
}