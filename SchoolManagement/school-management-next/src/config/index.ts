const requiredEnvVar = (name: string): string => {
  const value = process.env[name];
  if (!value) {
    throw new Error(`Environment variable ${name} is required but not set.`);
  }
  return value;
};

export const API_CONFIG = {
  // Use API_URL for server-side calls (not exposed to client)
  baseURL: process.env.API_URL || process.env.NEXT_PUBLIC_API_URL || 'https://localhost:7370',
  // For development with self-signed certificates
  useSecureConnection: process.env.NODE_ENV === 'production',
  // Additional API configuration
  timeout: 5000,
} as const;

// Validate environment variables during build/startup
if (process.env.NODE_ENV !== 'production') {
  // Verify that we have our API URL
  if (!process.env.API_URL && !process.env.NEXT_PUBLIC_API_URL) {
    console.warn('Warning: Neither API_URL nor NEXT_PUBLIC_API_URL is set. Using default: https://localhost:7370');
  }
}
