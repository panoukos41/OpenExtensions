using Android.Support.V4.App;
using GalaSoft.MvvmLight.Helpers;
using System.Collections.Generic;

namespace OpenExtensions.Droid.App
{
    /// <summary>
    /// A base fragment class with properties to save and delete MVVMLights bindings
    /// in a Bindings dictionary.
    /// </summary>
    public class FragmentBase : Fragment
    {
        /// <summary>
        /// A dictionary to store all your bindings.
        /// </summary>
        public readonly Dictionary<object, Binding> Bindings = new Dictionary<object, Binding>();

        /// <summary>
        /// Save a binding in the <see cref="Bindings"/> dictionary to keep it alive.
        /// it is recommended to use the <see cref="ReplaceBinding(object, Binding)"/>
        /// to ensure no other binding exists for this object.
        /// </summary>
        /// <param name="key">The object the binding was made for.</param>
        /// <param name="binding">The binding</param>
        public void SaveBinding(object key, Binding binding)
        {
            lock (Bindings)
            {
                if (Bindings.ContainsKey(key))
                {
                    Bindings[key].Detach();
                    Bindings[key] = binding;
                }
                else
                {
                    Bindings.Add(key, binding);
                }
            }
        }

        /// <summary>
        /// Delete a binding from the <see cref="Bindings"/> dictionary.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>The binding for further proccesing or null if no binding exists.</returns>
        public Binding DeleteBinding(object key)
        {
            lock (Bindings)
            {
                if (Bindings.ContainsKey(key))
                {
                    var binding = Bindings[key];
                    binding.Detach();
                    Bindings.Remove(key);
                    return binding;
                }
                return null;
            }
        }

        /// <summary>
        /// It executs <see cref="DeleteBinding(object)"/>
        /// then <see cref="SaveBinding(object, Binding)"/>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="binding"></param>
        public void ReplaceBinding(object key, Binding binding)
        {
            DeleteBinding(key);
            SaveBinding(key, binding);
        }

        /// <summary>
        /// Returns the binding for the specified key(view) 
        /// or null if no binding exists.
        /// </summary>
        /// <param name="key"></param>
        public Binding GetBinding(object key)
        {
            lock (Bindings)
            {
                if (Bindings.ContainsKey(key))
                {
                    return Bindings[key];
                }
                return null;
            }
        }
    }
}
