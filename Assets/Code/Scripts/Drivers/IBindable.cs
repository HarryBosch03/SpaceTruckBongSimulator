namespace SpaceTruckBongSimulator.Drivers
{
    public interface IBindable
    {
        bool CanBind { get; }
        Binding Bind(IBindingTarget target);
        void BindingDeactivated(IBindingTarget oldBinding);
    }
}