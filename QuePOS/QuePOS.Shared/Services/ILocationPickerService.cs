using QuePOS.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuePOS.Shared.Services
{
    public interface ILocationPickerService
    {
        Task<LocationViewModel> GetAddress();
    }
}
