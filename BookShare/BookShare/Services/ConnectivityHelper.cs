using Xamarin.Essentials;

public static class ConnectivityHelper
{
    public static bool IsConnected()
    {
        var current = Connectivity.NetworkAccess;

        if (current == NetworkAccess.Internet)
        {
            return true;
        }

        return false;
    }
}