import axios from 'axios';

// Create an axios instance with default config
const apiClient = axios.create({
  baseURL: process.env.NEXT_PUBLIC_API_URL || 'https://localhost:7202',
  headers: {
    'Content-Type': 'application/json',
  },
});

export default apiClient;
