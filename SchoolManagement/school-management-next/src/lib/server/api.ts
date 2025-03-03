import axios, { AxiosInstance, AxiosRequestConfig } from 'axios';
import https from 'https';
import { API_CONFIG } from '@/config';

// Extend AxiosRequestConfig to include retry property
interface ExtendedAxiosRequestConfig extends AxiosRequestConfig {
  retry?: number;
}

class ServerAPI {
  private client: AxiosInstance;

  constructor() {
    this.client = axios.create({
      baseURL: API_CONFIG.baseURL,
      headers: {
        'Content-Type': 'application/json',
      },
      // Allow self-signed certificates in development
      httpsAgent: new https.Agent({
        rejectUnauthorized: API_CONFIG.useSecureConnection
      }),
      // Add timeout from config
      timeout: API_CONFIG.timeout,
    });

    // Add retry interceptor
    this.client.interceptors.response.use(null, async (error) => {
      const config = error.config as ExtendedAxiosRequestConfig;
      if (!config || !config.retry) {
        return Promise.reject(error);
      }
      config.retry -= 1;
      const delayRetry = new Promise(resolve => setTimeout(resolve, 1000));
      await delayRetry;
      return this.client(config);
    });

    // Log the API configuration in development
    if (process.env.NODE_ENV !== 'production') {
      console.log('API Client Configuration:', {
        baseURL: API_CONFIG.baseURL,
        useSecureConnection: API_CONFIG.useSecureConnection,
        timeout: API_CONFIG.timeout
      });
    }
  }

  // Generic GET method
  async get<T>(url: string, params?: Record<string, string>) {
    try {
      const config: ExtendedAxiosRequestConfig = {
        params,
        retry: 3, // Allow 3 retries
      };
      const response = await this.client.get<T>(url, config);
      return response.data;
    } catch (error: any) {
      console.error(`Error fetching from ${url}:`, error);
      if (error.code === 'ECONNREFUSED') {
        throw new Error(`Unable to connect to the API server at ${API_CONFIG.baseURL}. Please ensure the API is running.`);
      }
      throw error;
    }
  }

  // Generic POST method
  async post<T>(url: string, data: unknown) {
    try {
      const config: ExtendedAxiosRequestConfig = {
        retry: 3, // Allow 3 retries
      };
      const response = await this.client.post<T>(url, data, config);
      return response.data;
    } catch (error: any) {
      console.error(`Error posting to ${url}:`, error);
      if (error.code === 'ECONNREFUSED') {
        throw new Error(`Unable to connect to the API server at ${API_CONFIG.baseURL}. Please ensure the API is running.`);
      }
      throw error;
    }
  }
}

// Create a singleton instance
const serverAPI = new ServerAPI();

export default serverAPI;
