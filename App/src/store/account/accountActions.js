import { accountApi } from './accountApi';
import { accountTypes } from './accountTypes';

const onChange = (prop, value) => {
  return (dispatch) => {
    dispatch({
      type: accountTypes.ACCOUNT_ONCHANGE,
      payload: { prop: prop, value: value },
    });
  };
};

const onUserLogin = (email, password) => async () => {
    return accountApi.loginUser(email, password);
};

const onExternalLogin = (provider, access_token) => async () => {
  return accountApi.externalLogin(provider, access_token);
};


const onLinkedInUserLogin = (grant_type, code, redirect_uri) => async () => {
  return accountApi.loginUserLinkedIn(grant_type, code, redirect_uri);
};

const onAdminLogin = (email, password) => async () => {
  return accountApi.adminLogin(email, password);
};

const onForgotPassword = (email) => async () => {
  return accountApi.forgotPassword(email);
};
const onResendConfirmation = (email) => async () => {
  return accountApi.resendConfirmation(email);
};
const onResetPassword = (userId, token, password) => async () => {
  return accountApi.resetPassword(userId, token, password);
};

const onRegisterUser = (email, firstName, lastName, password,reCaptcha) => async () => {
  return accountApi.registerUser(email, firstName, lastName, password, reCaptcha);
};

const onConfirmEmail = (userId, token) => async () => {
  return accountApi.confirmEmail(userId, token);
};

const onChangePassword = (currentPassword, newPassword) => async () => {
  return accountApi.changePassword(currentPassword, newPassword);
};

const onCreateCheckout = (type) => async () => {
  return accountApi.createCheckout(type);
};

const getPricingDetails = () => async () => {
  return accountApi.pricingDetails();
};

const onUpdateNotifications = (jobNotification, resourceNotification) => async () => {
  return accountApi.updateNotifications(jobNotification, resourceNotification);
};

const onGetUserInfo = () => async () => {
  return accountApi.userInfo();
};

const onCancelSubscription = (subscriptionType, isCancel) => async () => {
  return accountApi.cancelSubscription(subscriptionType, isCancel);
};

export const accountActions = {
  onChange: onChange,
  onUserLogin: onUserLogin,
  onExternalLogin: onExternalLogin,
  onLinkedInUserLogin: onLinkedInUserLogin,
  onAdminLogin: onAdminLogin,
  onForgotPassword: onForgotPassword,
  onResendConfirmation: onResendConfirmation,
  onResetPassword: onResetPassword,
  onRegisterUser: onRegisterUser,
  onConfirmEmail: onConfirmEmail,
  onChangePassword: onChangePassword,
  onGetUserInfo: onGetUserInfo,
  onUpdateNotifications: onUpdateNotifications,
  onCreateCheckout: onCreateCheckout,
  getPricingDetails: getPricingDetails,
  onCancelSubscription: onCancelSubscription,
};
