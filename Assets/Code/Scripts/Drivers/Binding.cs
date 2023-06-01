namespace SpaceTruckBongSimulator.Drivers
{
    public class Binding
    {
        public readonly IBindingTarget target;
        private bool active = true;

        public Binding(IBindingTarget target)
        {
            this.target = target;
        }
        
        public void Deactivate()
        {
            active = false;
        }
        
        public static implicit operator bool(Binding binding)
        {
            if (binding == null) return false;
            if (!binding.active) return false;

            return true;
        }
    }
}