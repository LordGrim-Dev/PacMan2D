
namespace PacMan
{
    public class PMPallet : PMConsumable
    {
        public override void OnPacManEncountered()
        {
            GameEventManager.Instance.TriggerItemConsumed(Game.Common.ItemType.ePallet);
        }
    }
}