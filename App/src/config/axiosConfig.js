import {default as ax} from 'axios';

let cancelToken = ax.CancelToken;
let source = cancelToken.source();
const axiosInstance = ax.create({
  cancelToken: source.token,
});

axiosInstance.interceptors.request.use((config) => {
  let token = {
    access_token: localStorage.getItem('access_token'),
  };

  let defaultHeaders = {
    'api-version': '1.0',
    'content-type': config.params?.form ? ' multipart/form-data': 'application/json',
    'Access-Control-Allow-Origin': '*',
  };

  if (token.access_token) {
    config.headers = {
      ...defaultHeaders,
      Authorization: `Bearer ${token.access_token}`,
    };
  } else {
    config.headers = defaultHeaders;
  }
  return config;
});

axiosInstance.interceptors.response.use(
  (response) => response,
  (error) => {
    const { status } = error.response;

    if (status === 401) {
      localStorage.removeItem('access_token');
      localStorage.removeItem('user');
      if (status === 401) {
        window.location = '/login?isError';
      } else {
        window.location = '/login';
      }
    }
    if (status === 403) {
      return {
        status: 200,
        data: { success: false, message: 'You can not perform this action.' },
      };
    } else {
      return error.response;
    }
  }
);

export const axios = axiosInstance;
