namespace Rimba
{
    namespace Survival
    {
        public interface IInteractable
        {
            string ItemName { get; }
            string ItemDescription { get; }
            void Interact(PlayerController player);
        }
    }
}
