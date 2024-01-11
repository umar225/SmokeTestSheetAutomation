import React from 'react';
import ReactDOM from 'react-dom';
import './index.css';
import '../src/styles/styles.css';
import '../src/styles/userStyle.css';
import App from './App';
import { Provider } from 'react-redux';
import store from './store';
import reportWebVitals from './reportWebVitals';
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import { GoogleOAuthProvider } from '@react-oauth/google';

ReactDOM.render(
  <GoogleOAuthProvider clientId="863465078019-g6c62vk2k0svdslutl5nfndevdhdmnjf.apps.googleusercontent.com">
  <React.StrictMode>
    <Provider store={store}>
      <App />
      <ToastContainer
        position="top-center"
        autoClose={5000}
        hideProgressBar={false}
        newestOnTop={false}
        closeOnClick
        rtl={false}
        pauseOnFocusLoss
        draggable
        pauseOnHover
        icon={false}
        theme="colored"
      />
    </Provider>
  </React.StrictMode>
  </GoogleOAuthProvider>,
  document.getElementById('root')
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
