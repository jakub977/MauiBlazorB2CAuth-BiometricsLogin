
namespace Principal.Telemedicine.Shared.Utils;
public static class DateTimeUtils
{
    public static int ToEpoch(DateTime time)
    {
        var span = time - new DateTime(1970, 1, 1);
        return Convert.ToInt32(span.TotalSeconds);
    }
}

