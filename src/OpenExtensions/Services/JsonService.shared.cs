using Newtonsoft.Json;
using System;

namespace OpenExtensions.Services
{
    /// <summary>
    /// A wrapper class around the <see cref="Newtonsoft.Json"/> methods.
    /// </summary>
    public static class JsonService
    {
        /// <summary>
        /// Serializes the value.
        /// </summary>
        public static string Serialize(object value)
        {
            if (value == null)
                return null;

            if (value as string == string.Empty)
                return string.Empty;

            // Serialize to json
            return JsonConvert.SerializeObject(value);
        }

        /// <summary>
        /// Try to serialize the valye.
        /// </summary>
        /// <param name="parameter">Object to serialize.</param>
        /// <param name="result">If serialization succeds the value will be copied here.</param>
        /// <returns>True if serialization succeeds</returns>
        public static bool TrySerialize(object parameter, out string result)
        {
            try
            {
                result = Serialize(parameter);
                return true;
            }
            catch (Exception)
            {
                result = default;
                return false;
            }
        }

        /// <summary>
        /// Deserializes the value.
        /// </summary>
        public static object Deserialize(string value)
        {
            if (value == null)
                return null;

            if (value == string.Empty)
                return string.Empty;

            // Deserialize from json
            return JsonConvert.DeserializeObject(value);
        }

        /// <summary>
        /// Deserializes the value.
        /// </summary>
        public static T Deserialize<T>(string value)
        {
            if (value == null)
                return default;

            if (value == string.Empty)
                return default;

            // Deserialize from json
            return JsonConvert.DeserializeObject<T>(value);
        }

        /// <summary>
        /// Try to deserialize the value.
        /// </summary>
        /// <param name="value">String to deserialize.</param>
        /// <param name="result">If deserialization succeds the value will be copied here.</param>
        /// <returns>True if deserialization succeeds.</returns> 
        public static bool TryDeserialize<T>(string value, out T result)
        {
            try
            {
                var r = Deserialize<T>(value);
                if (r == null)
                {
                    result = default;
                    return false;
                }
                result = r;
                return true;
            }
            catch
            {
                result = default;
                return false;
            }
        }
    }
}
