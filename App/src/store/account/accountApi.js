import { axios } from '../../config/axiosConfig';
import { endpoints } from '../../config/apiConfig';

async function loginUser(email, pass) {
  return axios
    .post(endpoints.account.login, {
      email: email,
      password: pass,
    })
    .then((response) => {
      let data = response.data;
      if (data?.success && data.data?.accessToken) {
        localStorage.setItem('access_token', data.data.accessToken.token);
        localStorage.setItem('user', data.data.role);
        localStorage.setItem('displayName', data.data.name);
      }
      return data;
    })
    .catch((error) => {
      return error;
    });
}
async function loginUserLinkedIn(grant_type, code, redirect_uri) {
  const config = {
    params: {
      form: true,
    },
}
  let body = new FormData();
  body.append('grant_type', grant_type);
  body.append('code', code);
  body.append('redirect_uri', redirect_uri);

  return axios
    .post(endpoints.account.linkedInAccessToken, body,config)
    .then((response) => {
      let data = response.data;
      return data;
    })
    .catch((error) => {
      return error;
    });
}
async function adminLogin(email, pass) {
  return axios
    .post(endpoints.account.adminLogin, {
      email: email,
      password: pass,
    })
    .then((response) => {
      let data = response.data;
      if (data?.success && data.data?.accessToken) {
        localStorage.setItem('access_token', data.data.accessToken.token);
        localStorage.setItem('user', data.data.role);
      }
      return data;
    })
    .catch((error) => {
      return error;
    });
}
async function externalLogin(provider, access_token) {
  return axios
    .post(endpoints.account.userLogin, {
      provider: provider,
      accessToken: access_token
    })
    .then((response) => {
      let data = response.data;
      if (data?.success && data.data?.accessToken) {
        localStorage.setItem('access_token', data.data.accessToken.token);
        localStorage.setItem('user', data.data.role);
        localStorage.setItem('userName', data.data.name);
        localStorage.setItem('displayName', data.data.name);
      }
      return data;
    })
    .catch((error) => {
      return error;
    });
}
async function forgotPassword(email) {
  return axios
    .post(endpoints.account.forgotPassword, {
      email: email,
    })
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      return error;
    });
}
async function resendConfirmation(email) {
  return axios
    .post(endpoints.account.resendConfirmation, {
      email: email,
    })
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      return error;
    });
}
async function resetPassword(userId, token, password) {
  return axios
    .post(endpoints.account.resetPassword, {
      userId: userId,
      token: token,
      password: password,
    })
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      return error;
    });
}

async function registerUser(email, firstName, lastName, password,reCaptcha) {
  return axios
    .post(endpoints.account.registerUser, {
      email: email,
      firstName: firstName,
      lastName: lastName,
      password: password,
      reCaptcha:reCaptcha
    })
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      return error;
    });
}

async function confirmEmail(userId, token) {
  return axios
    .post(endpoints.account.confirmEmail, {
      userId: userId,
      token: token,
    })
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      return error;
    });
}

async function changePassword(currentPassword, newPassword) {
  return axios
    .post(endpoints.account.changePassword, {
      currentPassword: currentPassword,
      newPassword: newPassword,
    })
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      return error;
    });
}

async function createCheckout(type) {
  return axios
    .get(`${endpoints.account.createCheckout}?subscriptionType=${type}`)
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      return error;
    });
}

async function pricingDetails() {
  return axios
    .get(endpoints.account.pricingDetails)
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      return error;
    });
}

async function updateNotifications(jobNotification, resourceNotification) {
  const config = {
    params: {
      form: true,
    },
}
  let body = new FormData();
  body.append('jobNotification', jobNotification);
  body.append('resourceNotification', resourceNotification);
  return axios
    .post(endpoints.account.updateNotifications, body, config)
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      return error;
    });
}

async function userInfo() {
  return axios
    .get(endpoints.account.userProfile)
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      return error;
    });
}

async function cancelSubscription(subscriptionType, isCancel) {
  const config = {
    params: {
      form: true,
    },
}
  let body = new FormData();
  body.append('subscriptionType', subscriptionType);
  body.append('isCancel', isCancel);
  return axios
    .post(endpoints.account.cancelSubscription, body, config)
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      return error;
    });
}

export const accountApi = {
  loginUser: loginUser,
  loginUserLinkedIn: loginUserLinkedIn,
  adminLogin: adminLogin,
  externalLogin: externalLogin,
  forgotPassword: forgotPassword,
  resendConfirmation: resendConfirmation,
  resetPassword: resetPassword,
  registerUser: registerUser,
  confirmEmail: confirmEmail,
  changePassword: changePassword,
  userInfo: userInfo,
  updateNotifications: updateNotifications,
  createCheckout: createCheckout,
  pricingDetails: pricingDetails,
  cancelSubscription: cancelSubscription,
};
