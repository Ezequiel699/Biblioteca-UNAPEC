// src/services/api.js
import axios from 'axios';

const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL, // ver paso 3
  timeout: 5000,
});

export default api;