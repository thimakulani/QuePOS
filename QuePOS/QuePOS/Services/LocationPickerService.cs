using CommunityToolkit.Maui.Alerts;
using QuePOS.Shared.Services;
using QuePOS.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuePOS.Services
{
    public class LocationPickerService : ILocationPickerService
    {
        public async Task<LocationViewModel> GetAddress()
        {
            var location = await Geolocation.Default.GetLastKnownLocationAsync();
            string address = string.Empty;
            try
            {

                if (location != null)
                {


                    var placemarks = await Geocoding.GetPlacemarksAsync(location);
                    if (placemarks != null && placemarks.Any())
                    {
                        var placemark = placemarks.FirstOrDefault();
                        if (placemark != null)
                        {
                            address = string.Join(", ", new[]
                            {
                            placemark.Thoroughfare,
                            placemark.SubThoroughfare,
                            placemark.SubLocality,
                            placemark.Locality,
                            placemark.SubAdminArea,
                            placemark.AdminArea,
                            placemark.PostalCode,
                            placemark.CountryName
                        }.Where(part => !string.IsNullOrWhiteSpace(part)));

                            // Example: display or assign the address
                        }
                    }
                }

            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature not supported on device
                await Toast.Make("This feature is not supported on your device.", CommunityToolkit.Maui.Core.ToastDuration.Long).Show();
                // Optional: Log the exception
                Console.WriteLine(fnsEx);
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Feature not enabled (e.g., GPS is off)
                await Toast.Make("Location feature is not enabled. Please enable it in your settings.", CommunityToolkit.Maui.Core.ToastDuration.Long).Show();
                Console.WriteLine(fneEx);
            }
            catch (PermissionException pEx)
            {
                // Permission to access location was denied
                await Toast.Make("Location permission was denied. Please allow it in your settings.", CommunityToolkit.Maui.Core.ToastDuration.Long).Show();
                var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                if (status != PermissionStatus.Granted)
                {
                    await Toast.Make("Location permission not granted.", CommunityToolkit.Maui.Core.ToastDuration.Long).Show();
                }
                Console.WriteLine(pEx);
            }
            catch (Exception ex)
            {
                // General exception (e.g., no last known location)
                await Toast.Make("Unable to get location. Please try again.", CommunityToolkit.Maui.Core.ToastDuration.Long).Show();
                Console.WriteLine(ex);
            }
            return new LocationViewModel()
            {
                Longitude = location.Longitude,
                Latitude = location.Latitude,
                Address = address
            };
        }
    }

}
