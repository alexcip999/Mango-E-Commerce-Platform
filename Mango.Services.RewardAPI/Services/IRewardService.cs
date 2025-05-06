using Mango.Services.RewardAPI.Message;

namespace Mango.Service.RewardAPI.Services
{
    public interface IRewardService
    {
        Task UpdateRewards(RewardsMessage rewardsMessage);
    }
}
