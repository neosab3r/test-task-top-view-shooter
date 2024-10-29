using BeeGood.Systems;

namespace BeeGood.View
{
    public class BotView : BaseView
    {
        private void Start()
        {
            BaseEntrySystems.SubscribeOnAllSystemsInitialized(() =>
            {
                EntrySystem.Instance.Get<BotSystem>().AddView(this);
            });
        }
    }
}