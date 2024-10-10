import axios from 'axios';

export const BASE_URL = import.meta.env.VITE_API_URL;

export const httpService = axios.create({
    baseURL: BASE_URL,
    headers: {
        "Content-type": "application/json"
    }
});