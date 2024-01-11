import React from 'react';
import {
  Home,
  UserDashboard,
  TestScreen,
  Checkout,
  PaymentStatus,
  FormCompleted,
  EditorDemo,
  Login,
  AdminDashboard,
  AdminJobPortal,
  CreateEditJob,
  CreateEditResource,
  JobDetail,
  AdminResourceCenter,
  AdminOneToOne,
  ResourceCenter,
  ResourceDetail,
  ForgotPassword,
  ResetPassword,
  RegisterUser,
  ConfirmEmail,
  AdminLogin,
  UserProfile,
  LinkedInRedirect,
  PricingPage,
} from '../screens/index';
import ResendConfirmation from '../screens/resendConfirmation';

const routesObj = {
  Root: {
    path: '/',
    component: <UserDashboard />,
    roles: [],
    fallback: null,
  },
  home: {
    path: '/jobs-board',
    component: <Home />,
    roles: [],
    fallback: null,
  },
  userDashboard: {
    path: '/userDashboard',
    component: <UserDashboard />,
    roles: [],
    fallback: null,
  },
  userProfile: {
    path: '/account',
    component: <UserProfile />,
    roles: [],
    fallback: null,
  },
  pricingPage: {
    path: '/pricing',
    component: <PricingPage />,
    roles: [],
    fallback: null,
  },
  jobDetail: {
    path: '/job/:jobId',
    component: <JobDetail />,
    roles: [],
    fallback: null,
  },
  testScreen: {
    path: '/test',
    component: <TestScreen />,
    roles: [],
    fallback: null,
  },
  Health: {
    path: '/health',
    component: <h3>Hey There!!! The App is Healthy</h3>,
    roles: [],
    fallback: null,
  },
  checkOut: {
    path: '/checkout',
    component: <Checkout />,
    roles: [],
    fallback: null,
  },
  paymentStatus: {
    path: '/paymentstatus',
    component: <PaymentStatus />,
    roles: [],
    fallback: null,
  },
  formCompleted: {
    path: '/courses/form-complete',
    component: <FormCompleted />,
    roles: [],
    fallback: null,
  },
  editorDemo: {
    path: '/editor',
    component: <EditorDemo />,
    roles: [],
    fallback: null,
  },
  login: {
    path: '/login',
    component: <Login />,
    roles: [],
    fallback: null,
  },
  linkedInRedirect: {
    path: '/linkedin-redirect',
    component: <LinkedInRedirect />,
    roles: [],
    fallback: null,
  },
  adminLogin: {
    path: '/admin/login',
    component: <AdminLogin />,
    roles: [],
    fallback: null,
  },
  adminDashboard: {
    path: '/admindashboard',
    component: <AdminDashboard />,
    roles: [],
    fallback: null,
  },
  adminJobPortal: {
    path: '/adminjobportal',
    component: <AdminJobPortal />,
    roles: [],
    fallback: null,
  },
  createEditJob: {
    path: '/adminjob',
    component: <CreateEditJob />,
    roles: [],
    fallback: null,
  },
  adminResourceCenter: {
    path: '/adminresourcecenter',
    component: <AdminResourceCenter />,
    roles: [],
    fallback: null,
  },
  adminOneToOne: {
    path: '/one2one',
    component: <AdminOneToOne />,
    roles: [],
    fallback: null,
  },
  createEditResource: {
    path: '/adminresource',
    component: <CreateEditResource />,
    roles: [],
    fallback: null,
  },
  resourceCenter: {
    path: '/resourcecenter',
    component: <ResourceCenter />,
    roles: [],
    fallback: null,
  },
  resourceDetail: {
    path: '/resourcecenter/:url',
    component: <ResourceDetail />,
    roles: [],
    fallback: null,
  },
  forgotPassword: {
    path: '/forgotpassword',
    component: <ForgotPassword />,
    roles: [],
    fallback: null,
  },
  resendConfirmation: {
    path: '/resendConfirmation',
    component: <ResendConfirmation />,
    roles: [],
    fallback: null,
  },
  resetPassword: {
    path: '/reset-password',
    component: <ResetPassword />,
    roles: [],
    fallback: null,
  },
  confirmEmail: {
    path: '/confirm-email',
    component: <ConfirmEmail />,
    roles: [],
    fallback: null,
  },
  registerUser: {
    path: '/registerUser',
    component: <RegisterUser />,
    roles: [],
    fallback: null,
  },
};
export default routesObj;