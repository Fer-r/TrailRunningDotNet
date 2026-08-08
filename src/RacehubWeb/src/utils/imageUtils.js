const API_BASE = import.meta.env.VITE_URL_API || "http://localhost:5000";

/**
 * Resolves an image path to a full URL.
 * - Relative paths like "/uploads/xxx.jpg" get prefixed with the API base URL.
 * - Absolute URLs (http/https) are returned as-is.
 * - Null/undefined returns null (so the component can fall back to default).
 */
export const resolveImageUrl = (imagePath) => {
  if (!imagePath) return null;
  if (imagePath.startsWith("http://") || imagePath.startsWith("https://")) {
    return imagePath;
  }
  return `${API_BASE}${imagePath}`;
};
