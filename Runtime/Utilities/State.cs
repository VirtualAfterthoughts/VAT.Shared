namespace VAT.Shared.Utilities
{
    /// <summary>
    /// Holder for a single value with a callback for when its changed.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class State<T>
    {
        public delegate void StateCallback(T previous, T current);

        private T _value = default;

        /// <summary>
        /// The current value of this state.
        /// </summary>
        public T Value
        {
            get
            {
                return _value;
            }
            set
            {
                var previous = _value;
                var current = value;

                _value = current;

                OnValueChanged?.Invoke(previous, current);
            }
        }

        /// <summary>
        /// Callback invoked any time the Value is set, even if the same value is provided.
        /// </summary>
        public event StateCallback OnValueChanged;

        public State()
        {
            _value = default;
        }

        public State(T value) 
        {
            _value = value;
        }
    }

    /// <summary>
    /// Holder for a bool with a callback for when its changed.
    /// </summary>
    public sealed class BoolState : State<bool> 
    {
        /// <summary>
        /// Flips the state's current value.
        /// </summary>
        public void Toggle() => Value = !Value;

        /// <summary>
        /// Sets the bool to True and then back to False once.
        /// </summary>
        public void Trigger()
        {
            Value = false;
            Toggle();
        }

        public BoolState() : base() { }

        public BoolState(bool value) : base(value) { }
    }

    /// <summary>
    /// Holder for a float with a callback for when its changed.
    /// </summary>
    public sealed class FloatState : State<float> 
    {
        public FloatState() : base() { }

        public FloatState(float value) : base(value) { }

        /// <summary>
        /// Sets the float to 1 and then back to 0 once.
        /// </summary>
        public void Trigger()
        {
            Value = 0f;
            Value = 1f;
            Value = 0f;
        }
    }
}
