import axios from 'axios';
require('dotenv').config();


const api = axios.create({
    baseURL: process.env.REACT_APP_NARUTO_API,
})

export default api;