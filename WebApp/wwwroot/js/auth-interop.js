// Returns a persistent device identifier stored in localStorage.
// Preconditions:
// - `window.localStorage` and `crypto.randomUUID` must be available in the browser.
// - Caller must be running in a secure browser context (e.g., HTTPS).
//
// Postconditions:
// - If a device ID already exists in localStorage under the key `corelayer-device-id`, it is returned.
// - If not, a new UUID is generated, stored, and returned.

window.getDeviceIdentifier = () => {
    // Example logic: you can replace this with actual fingerprinting or device ID strategy
    const key = "corelayer-device-id";

    let id = localStorage.getItem(key);
    if (!id) {
        id = crypto.randomUUID();
        localStorage.setItem(key, id);
    }

    return id;
};
