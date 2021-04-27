namespace GpsNotepad.Services.Settings
{
    interface ISettingsManager
    {
        int UserId { get; set; }
        string Culture { get; set; }
        public string Theme { get; set; }
        double Latitude { get; set; }
        double Longitude { get; set; }
        double Zoom { get; set; }

        void ClearSettings();

    }
}
