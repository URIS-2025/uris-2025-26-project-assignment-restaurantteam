

namespace ReservationService.Mappers
{
    public static class ReservationStatusParser
    {
        public static Entities.Enums.ReservationStatus? ToEnum(string status)
        {
            switch (status)
            {
                case "ACTIVE": return  Entities.Enums.ReservationStatus.ACTIVE;break;
                case "CANCELED": return  Entities.Enums.ReservationStatus.CANCELED;break;
                default: return null;break;
            }

        }

        public static string? ToString(Entities.Enums.ReservationStatus status)
        {
            switch (status)
            {
                case Entities.Enums.ReservationStatus.ACTIVE: return "ACTIVE"; break;
                case Entities.Enums.ReservationStatus.CANCELED: return "CANCELED"; break;
                default: return null; break;
            }

        }
    }
}
