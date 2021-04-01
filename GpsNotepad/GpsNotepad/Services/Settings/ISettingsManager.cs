using System;
using System.Collections.Generic;
using System.Text;

namespace GpsNotepad.Services.Settings
{
    interface ISettingsManager
    {
        int UserId { get; set; }
        string Culture { get; set; }
    }
}
