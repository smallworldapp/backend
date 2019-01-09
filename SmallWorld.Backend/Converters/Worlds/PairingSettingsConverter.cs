using System;
using SmallWorld.Database.Entities;

// ReSharper disable ArrangeAccessorOwnerBody
// ReSharper disable InconsistentNaming

namespace SmallWorld.Converters.Worlds
{
    public class PairingSettingsConverter : EntityConverter<PairingSettings>
    {
        public DateTime start
        {
            get => Value.Start;
            set => Value.Start = value;
        }

        public long period
        {
            get => Value.Period;
            set => Value.Period = value;
        }

        public bool enabled
        {
            get => Value.Enabled;
            set => Value.Enabled = value;
        }
    }
    //public class PairingSettingsConverter : QuickJsonConverter<PairingSettings>
    //{
    //    public PairingSettingsConverter()
    //    {
    //        Default("start", a => a.Start);
    //        Default("period", a => a.Period);
    //        Default("enabled", a => a.Enabled);
    //    }
    //}
}