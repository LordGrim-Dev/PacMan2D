
namespace PacMan
{
    public class PMPowerPallet : PMConsumable
    {
        public override void OnPacManEncountered()
        {
            GameEventManager.Instance.TriggerItemConsumed(Game.Common.ItemType.ePowerPallet);
        }
    }
}
