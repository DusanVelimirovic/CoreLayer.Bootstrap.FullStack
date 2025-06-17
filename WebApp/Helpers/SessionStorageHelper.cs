using Microsoft.JSInterop;
using System.Text.Json;

namespace WebApp.Helpers
{
    /// <summary>
    /// Provides helper methods to interact with browser session storage using JavaScript interop.
    /// </summary>
    /// <remarks>
    /// This class allows storing, retrieving, and removing typed values in session storage.
    /// </remarks>
    public class SessionStorageHelper
    {
        private readonly IJSRuntime _jsRuntime;

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionStorageHelper"/> class.
        /// </summary>
        /// <param name="jsRuntime">JavaScript runtime for invoking session storage methods.</param>
        public SessionStorageHelper(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        /// <summary>
        /// Stores an item in session storage.
        /// </summary>
        /// <typeparam name="T">The type of the value to store.</typeparam>
        /// <param name="key">The session storage key.</param>
        /// <param name="value">The value to be stored.</param>
        public async Task SetItemAsync<T>(string key, T value)
        {
            var json = JsonSerializer.Serialize(value);
            await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", key, json);
        }

        /// <summary>
        /// Retrieves an item from session storage.
        /// </summary>
        /// <typeparam name="T">The expected type of the stored value.</typeparam>
        /// <param name="key">The session storage key.</param>
        /// <returns>The deserialized value if found; otherwise, the default value of <typeparamref name="T"/>.</returns>
        public async Task<T?> GetItemAsync<T>(string key)
        {
            var json = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", key);
            return json == null ? default : JsonSerializer.Deserialize<T>(json);
        }

        /// <summary>
        /// Removes an item from session storage by key.
        /// </summary>
        /// <param name="key">The session storage key to remove.</param>
        public async Task RemoveItemAsync(string key)
        {
            await _jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", key);
        }
    }
}
