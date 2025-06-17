// Stores the authentication cookie into the browser's cookie storage and localStorage.
// Preconditions:
// - `cookieString` must be in standard format (e.g., ".AspNetCore.Identity.Application=xyz; Path=/; ...").
// - Must be called in a browser environment with access to `document.cookie` and `localStorage`.
// - Caller must ensure this runs after authentication response.
// Postconditions:
// - Cookie is set in the browser with appropriate flags (SameSite, Secure).
// - The key-value pair is stored in localStorage under the cookie name.
window.storeCookie = function (cookieString) {
    console.log("[storeCookie] Called with:", cookieString);

    const parts = cookieString.split(';');
    const cookieValue = parts[0];
    const separatorIndex = cookieValue.indexOf('=');
    const key = cookieValue.substring(0, separatorIndex);
    const value = cookieValue.substring(separatorIndex + 1);

    const isHttps = window.location.protocol === "https:";
    const extraFlags = isHttps
        ? "; SameSite=None; Secure"
        : "; SameSite=Lax"; // fallback for local HTTP dev

    document.cookie = cookieValue + "; path=/" + extraFlags;

    try {
        localStorage.setItem(key, value);
        console.log("[storeCookie] Cookie injected and saved to localStorage:", key, value);
    } catch (err) {
        console.error("[storeCookie] Failed to save to localStorage:", err);
    }
};

// Clear the cookie manually from browser
window.clearAuthCookie = function () {
    const isHttps = window.location.protocol === "https:";
    const extraFlags = isHttps
        ? "; SameSite=None; Secure"
        : "; SameSite=Lax";

    document.cookie = ".AspNetCore.Identity.Application=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/" + extraFlags;

    console.log("[clearAuthCookie] .AspNetCore.Identity.Application manually cleared");
};
