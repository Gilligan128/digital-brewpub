using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digital.BrewPub.Infrastructure.Notification
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
