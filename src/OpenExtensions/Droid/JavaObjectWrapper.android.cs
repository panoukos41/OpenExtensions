using Java.Lang;

namespace OpenExtensions.Droid
{
    /// <summary>
    /// A class to wrap your object into a java object.
    /// </summary>
    /// <typeparam name="T">Your object</typeparam>
    public class JavaObjectWrapper<T> : Object
    {
        /// <summary>
        /// Your object
        /// </summary>
        public T Obj { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="JavaObjectWrapper{T}"/>
        /// </summary>
        /// <param name="obj">The object to wrap as a java object</param>
        public JavaObjectWrapper(T obj) => Obj = obj;
    }
}