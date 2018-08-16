using System.Threading.Tasks;
using Inventory.Services;
using Quartz;

namespace Inventory.Quartz
{
    public class ExpiredNotifyJob : IJob
    {
        private readonly INotifyService _notifyService;

        public ExpiredNotifyJob(INotifyService notifyService)
        {
            _notifyService = notifyService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _notifyService.Expired();
        }
    }
}
